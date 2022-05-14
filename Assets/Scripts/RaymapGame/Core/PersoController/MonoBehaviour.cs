//================================
//  By: Adsolution
//================================
using UnityEngine;
using RaymapGame.Rayman2.Persos;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using OpenSpace.Collide;

namespace RaymapGame {
    public partial class PersoController {
        protected void Awake() {
            if (false && mainActor != null) {
                var near = new List<PersoController>();
                foreach (var p in GetPersos(GetType()))
                    if (mainActor.DistTo(p) < activeRadius)
                        near.Add(p);
                if (near.Count > maxAllowedNearMainActor) {
                    Destroy(gameObject);
                    return;
                }
            }
            persoType = GetType();
            HD = Main.HDShaders;
            visible = true;
            if (!Main.isRom) perso = GetComponent<PersoBehaviour>();
            else persoRom = GetComponent<ROMPersoBehaviour>();
            dsg = GetComponent<DsgVarComponent>();
            gameObject.AddComponent<Interpolation>().fixedTimeController = this;
            anim = gameObject.AddComponent<AnimHandler>();
            anim.sfx = animSfx;

            if (!Main.isRom) {
                persoName = perso.NameInstance;
                persoModel = perso.NameModel;
                persoFamily = perso.NameFamily;
            } else {
                persoName = persoRom.NameInstance;
                persoModel = persoRom.NameModel;
                persoFamily = persoRom.NameFamily;
            }


            // Add to perso name cache
            /*if (!persos.ContainsKey(persoName.ToLower()))
                persos.Add(persoName.ToLower(), this);*/
            persos[persoName.ToLower()] = this;

            // Add to perso type cache
            if (getPersosCache.ContainsKey(GetType()))
                getPersosCache[GetType()].Add(this);
            else if (getPersosCache.ContainsKey(GetType().BaseType))
                getPersosCache[GetType().BaseType].Add(this);


            // Make extra sure non-collision colliders are gone
            foreach (var col in GetComponentsInChildren<Collider>()) {
                var comp = col.GetComponent<CollideComponent>();
                var so = comp?.GetComponentInParent<SuperObjectComponent>();
                if (comp == null || (comp.type != CollideType.None && comp.type != CollideType.ZDR))
                    Destroy(col);
            }


            // Collect Rules
            foreach (var m in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => !x.IsPublic && x.Name.StartsWith("Rule_")))
                rules.Add(m.Name.Replace("Rule_", ""), m);

            // Collect Channels
            var c = new List<Channel>();
            foreach (Transform t in transform)
                if (t.name.Contains("Channel"))
                    c.Add(new Channel(t));
            channels = c.ToArray();

            // Collect scripts
            var bhvs = transform.Find("Rule behaviours");
            if (bhvs != null)
                foreach (Transform bh in bhvs) {
                    var scrs = new List<ScriptComponent>();
                    foreach (Transform sc in bh) scrs.Add(sc.GetComponent<ScriptComponent>());
                    var n = bh.name.Split('\"');
                    scripts.Add((n.Length > 1 ? n[1] : n[0]), scrs.ToArray());
                }

            // Position and setup
            col.controller = this;
            startPos = pos = transform.position;
            startRot = rot = transform.rotation.eulerAngles - new Vector3(0, 180, 0);
            startScale = scale3 = transform.localScale;
            startSector = sector;

            // Set Rayman as main actor
            if (this is YLT_RaymanModel r) {
                Main.SetMainActor(this);
                Main.rayman = r;
            }
        }

        protected void Start() {


            // Has in-world enable trigger?
            for (int i = 0; i < 20; i++) {
                var link = GetDsgVar<PersoController>("Perso_" + i.ToString()) as FRG_Generateur;
                if (link != null) {
                    persoEnabled = false;
                    link.genPerso = this; break;
                }
            }


            // Linked death triggers
            if (hasLinkedDeath)
                for (int i = 0; i < 20; i++) {
                    var link = GetDsgVar<PersoController>("Perso_" + i.ToString());
                    if (link != null) {
                        deathLinks.Add(link);
                        link.AddDeathEvent(OnEveryLinkedDeath);
                    }
                }

            if (persoEnabled)
                OnStart();
            else SetNullPos();
        }

        protected void Update() {
            if (!Main.isRom) perso.sector = Main.controller.sectorManager.GetActiveSectorWrapper(pos);
            else persoRom.sector = Main.controller.sectorManager.GetActiveSectorWrapper(pos);

            if (active) {
                OnInput();

                if (isMainActor || (Main.main.alwaysControlRayman && this is YLT_RaymanModel))
                    OnInputMainActor();
            }

            if (!interpolate) LogicLoop();
        }

        protected virtual void LateUpdate() {
            if (visChanged) {
                foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                    mr.enabled = visible;
                visChanged = false;
            }
            else if (!visible) {
                foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                    mr.enabled = false;
            }

            if (HD)
                foreach (var mr in GetComponentsInChildren<MeshRenderer>()) {
                    if (mr.material.name == "mat_gouraud (Instance)") {
                        var tex = mr.material.GetTexture("_Tex0");
                        mr.material = new Material(Shader.Find("Standard"));
                        mr.material.mainTexture = tex;
                        mr.receiveShadows = true;
                        mr.material.SetFloat("_Glossiness", 0);
                    }
                }

            foreach (var c in channels) {
                if (c.startPos != c.pos && c.pos != c.tr.position)
                    c.tr.position = c.pos;
                if (c.startRot != c.rot && c.rot != c.tr.rotation.eulerAngles)
                    c.tr.rotation = Quaternion.Euler(c.rot);
                if (c.visible != c.startVisible)
                    foreach (var mr in c.tr.GetComponentsInChildren<MeshRenderer>())
                        mr.enabled = c.visible;
            }
        }

        protected void FixedUpdate() {
            if (interpolate) LogicLoop();
        }


        //========================================
        //  Logic loop
        //========================================
        void InvokeRule(string rule) {
            if (rules.ContainsKey(rule))
                rules[rule].Invoke(this, ruleParams);
        }

        void LogicLoop() {
            if (Main.loadState != Main.LoadState.Loaded) return;
            if (active && updateCollision) {
                col.UpdateGroundCollision();
                col.UpdateWaterCollision();
            } else col.ClearAll();

            OnUpdateAlways();
            if (Main.loadState != Main.LoadState.Loaded) return;
            if (active) {
                OnUpdate();
                if (Main.loadState != Main.LoadState.Loaded) return;

                // Rule
                if (rule != NO_RULE_SET) {
                    InvokeRule(rule);
                    newRule = rule != prevRuleIdk;
                    prevRuleIdk = rule;
                }
                if (Main.loadState != Main.LoadState.Loaded) return;

                // Mount (Plum, Shell, etc)
                if (hasMount) {
                    pos = mount.pos + mount.upward * mount.col.top;
                }
                else {
                    // VELOCITY / FRICTION

                    // Movement
                    if (vel.magnitude > 0) {
                        if (fricY != 0) velY /= 1f + fricY * globalFricMult * dt;
                        if (fricXZ != 0) velXZ /= 1f + fricXZ * globalFricMult * dt;
                        pos += vel * dt;
                    }

                    // Rotational
                    if (rotVel.magnitude > 0) {
                        if (rotFric > 0) rotVel /= 1f + rotFric * globalFricMult * dt;
                        rot += rotVel * dt;
                    }
                }

                if (inThrowArc) {
                    col.wallEnabled = false;
                    var dist = (thrTarg - thrStart).magnitude;
                    if (NavArcFromTo(thrStart, thrTarg, dist / 6, dist / 15)) {
                        inThrowArc = false;
                        vel = apprVel;
                        OnThrown();
                    }
                }

                if (updateCollision) {
                    col.UpdateWallCollision();
                    col.ApplyWallCollision();
                }

            }

            rot = WrapAngle3(Vector3.zero, rot);
            posPrev = pos;
            deltaPos = pos - posFrame;
            deltaRot = rot - rotFrame;
            apprVel = deltaPos / dt;
            if (scale <= 0) scale = 0.0001f;
            transform.localScale = scale3;

            // If carrying another perso, attach it to the "hand channel"
            if (carryPerso != null) {
                carryPerso.SetRule("");
                var ch = GetChannel(handChannel, true);
                if (ch != null) {
                    carryPerso.pos = ch.pos;
                    carryPerso.rot = (Quaternion.Euler(ch.rot) * Quaternion.Euler(handChannelRot)).eulerAngles;
                }
            }

            posFrame = pos;
            rotFrame = rot;
        }
    }
}