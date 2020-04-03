//================================
//  By: Adsolution
//================================

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Linq;
using OpenSpace.AI;
using OpenSpace.Object;      

namespace RaymapGame {
    public partial class PersoController : MonoBehaviour, IInterpolate {
        public static Dictionary<string, PersoController> persos = new Dictionary<string, PersoController>();

        //========================================
        //  Variables
        //========================================
        public PersoBehaviour perso;
        public ROMPersoBehaviour persoRom;
        public Type persoType;
        public DsgVarComponent dsg;
        public AnimHandler anim;
        public RayCollider col = new RayCollider();
        public Channel[] channels;

        public string persoFamily;
        public string persoModel;
        public string persoName;

        public Dictionary<string, MethodBase> rules = new Dictionary<string, MethodBase>();
        public object[] ruleParams;

        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale3;
        public float scale { get => scale3.x; set { scale3 = Vector3.one * value; } }

        public Vector3 velXZ;
        public float velY;
        public Vector3 rotVel;
        public Vector3 vel {
            get => new Vector3(velXZ.x, velY, velXZ.z);
            set {
                velXZ = new Vector3(value.x, 0, value.z);
                velY = value.y;
            }
        }
        public Vector3 velRel {
            get => Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0) * rot).MultiplyPoint3x4(vel);
            set {
                vel = Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0) * rot).MultiplyPoint3x4(value);
            }
        }

        public float fricXZ = 50, fricY = 0;
        public float rotFric;
        public float moveSpeed = 10;
        public float navRotSpeed = 1;
        public float gravity = -25;
        public static float globalGravityMult = 1;
        public static float globalFricMult = 1;
        public float waypointLenience;

        public float hitPoints = 1, maxHitPoints = 1, startHitPoints = 1;
        public Rayman2.Persos.CHR_CheckP checkpoint;

        public int handChannel;
        public Vector3 handChannelRot;

        public PersoController target;
        public Type projectileType;
        public float projectileVel = 25;
        public Vector3 projectileOffset = new Vector3(0, 1.2f, 1);

        public bool HD = false;

        public Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();
        public List<Action>
            deathEvents = new List<Action>(),
            combatEvents = new List<Action>();

        public virtual AnimSFX[] animSfx => new AnimSFX[0];
        public virtual bool isAlways => false;
        public virtual float activeRadius => 75;
        public virtual int maxAllowedNearMainActor => 1000;
        public virtual bool resetOnRayDeath => true;
        public virtual bool hasLinkedDeath => false;
        public virtual bool carriable => false;
        public virtual bool updateCollision => true;

        protected virtual void OnDebug() { }
        protected virtual void OnStart() { }
        protected virtual void OnInput() { }
        protected virtual void OnInputMainActor() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnUpdateAlways() { }
        protected virtual void OnEnterCombat() { }
        protected virtual void OnThrown() { }
        protected virtual void OnHit() { }
        protected virtual void OnDeath() { }
        void OnEveryLinkedDeath() {
            foreach (var l in deathLinks) if (!l.dead) return;
            OnLinksDead();
        }
        protected virtual void OnLinksDead() { }


        // Only get allowed in scripts
        public string rule { get; private set; } = NO_RULE_SET;
        const string NO_RULE_SET = "[no rule set]";
        public Vector3 startPos { get; private set; }
        public Quaternion startRot { get; private set; }
        public Vector3 startScale { get; private set; }
        public int startSector { get; private set; }
        protected Vector3 posFrame { get; private set; }
        protected Quaternion rotFrame { get; private set; }
        protected Vector3 posPrev { get; private set; }
        public Vector3 deltaPos { get; private set; }
        public Quaternion deltaRot { get; private set; }
        public Vector3 apprVel { get; private set; }
        protected string prevRule { get; private set; }
        protected string prevRuleIdk { get; private set; }
        public bool newRule { get; private set; } = true;
        public bool hasShadow { get; private set; }
        public Waypoint waypoint { get; private set; }
        public PersoController mount { get; private set; }
        public PersoController carryPerso { get; private set; }
        public bool inThrowArc { get; private set; }
        public PersoController creator { get; private set; }
        public PersoController lastDmgSrc { get; private set; }
        public List<PersoController> deathLinks { get; private set; } = new List<PersoController>();
        public bool hasEnableTrigger { get; private set; }
        public bool persoEnabled { get; private set; } = true;
        public PersoController enableLink { get; private set; }
        public bool visible { get; private set; }
        bool visChanged;


        // Interpolate fixed time movement
        public virtual bool interpolate => Main.useFixedTimeWithInterpolation;
        public Vector3 interpolPos => pos;
        public Quaternion interpolRot => rot;


        static void InitPersoCoreAndScripts(Perso p, bool forceAlways = false, int i = 0) {
            PersoBehaviour unityBehaviour = p.Gao.AddComponent<PersoBehaviour>();
            unityBehaviour.controller = Main.controller;

            if (forceAlways)
                unityBehaviour.IsAlways = true;

            if (Main.controller.loader.globals != null && Main.controller.loader.globals.spawnablePersos != null) {
                if ((Main.controller.loader.globals.spawnablePersos.IndexOf(p) > -1)) {
                    unityBehaviour.IsAlways = true;
                    unityBehaviour.transform.position = new Vector3(i * 10, -1000, 0);
                }
            }
            if (!unityBehaviour.IsAlways) {
                if (p.sectInfo != null && p.sectInfo.off_sector != null) {
                    unityBehaviour.sector = Main.controller.sectorManager.sectors.FirstOrDefault(s => s.sector != null && s.sector.SuperObject.offset == p.sectInfo.off_sector);
                }
                else {
                    SectorComponent sc = Main.controller.sectorManager.GetActiveSectorWrapper(p.Gao.transform.position);
                    unityBehaviour.sector = sc;
                }
            }
            else unityBehaviour.sector = null;
            unityBehaviour.perso = p;
            unityBehaviour.Init();

            // Scripts
            if (p.Gao) {
                if (p.brain != null && p.brain.mind != null && p.brain.mind.AI_model != null) {
                    if (p.brain.mind.AI_model.behaviors_normal != null) {
                        GameObject intelParent = new GameObject("Rule behaviours");
                        intelParent.transform.parent = p.Gao.transform;
                        Behavior[] normalBehaviors = p.brain.mind.AI_model.behaviors_normal;
                        int iter = 0;
                        foreach (Behavior behavior in normalBehaviors) {
                            string shortName = behavior.GetShortName(p.brain.mind.AI_model, Behavior.BehaviorType.Intelligence, iter);
                            GameObject behaviorGao = new GameObject(shortName);
                            behaviorGao.transform.parent = intelParent.transform;
                            foreach (Script script in behavior.scripts) {
                                GameObject scriptGao = new GameObject("Script");
                                scriptGao.transform.parent = behaviorGao.transform;
                                ScriptComponent scriptComponent = scriptGao.AddComponent<ScriptComponent>();
                                scriptComponent.SetScript(script, p);
                            }
                            if (behavior.firstScript != null) {
                                ScriptComponent scriptComponent = behaviorGao.AddComponent<ScriptComponent>();
                                scriptComponent.SetScript(behavior.firstScript, p);
                            }
                            if (iter == 0) {
                                behaviorGao.name += " (Init)";
                            }
                            if ((behavior.scripts == null || behavior.scripts.Length == 0) && behavior.firstScript == null) {
                                behaviorGao.name += " (Empty)";
                            }
                            iter++;
                        }
                    }
                    if (p.brain.mind.AI_model.behaviors_reflex != null) {
                        GameObject reflexParent = new GameObject("Reflex behaviours");
                        reflexParent.transform.parent = p.Gao.transform;
                        Behavior[] reflexBehaviors = p.brain.mind.AI_model.behaviors_reflex;
                        int iter = 0;
                        foreach (Behavior behavior in reflexBehaviors) {
                            string shortName = behavior.GetShortName(p.brain.mind.AI_model, Behavior.BehaviorType.Reflex, iter);
                            GameObject behaviorGao = new GameObject(shortName);
                            behaviorGao.transform.parent = reflexParent.transform;
                            foreach (Script script in behavior.scripts) {
                                GameObject scriptGao = new GameObject("Script");
                                scriptGao.transform.parent = behaviorGao.transform;
                                ScriptComponent scriptComponent = scriptGao.AddComponent<ScriptComponent>();
                                scriptComponent.SetScript(script, p);
                            }
                            if (behavior.firstScript != null) {
                                ScriptComponent scriptComponent = behaviorGao.AddComponent<ScriptComponent>();
                                scriptComponent.SetScript(behavior.firstScript, p);
                            }
                            if ((behavior.scripts == null || behavior.scripts.Length == 0) && behavior.firstScript == null) {
                                behaviorGao.name += " (Empty)";
                            }
                            iter++;
                        }
                    }
                    if (p.brain.mind.AI_model.macros != null) {
                        GameObject macroParent = new GameObject("Macros");
                        macroParent.transform.parent = p.Gao.transform;
                        Macro[] macros = p.brain.mind.AI_model.macros;
                        int iter = 0;

                        foreach (Macro macro in macros) {
                            GameObject behaviorGao = new GameObject(macro.GetShortName(p.brain.mind.AI_model, iter));
                            behaviorGao.transform.parent = macroParent.transform;
                            ScriptComponent scriptComponent = behaviorGao.AddComponent<ScriptComponent>();
                            scriptComponent.SetScript(macro.script, p);
                            iter++;
                        }
                    }
                }
            }
        }
    }
}