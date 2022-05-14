//================================
//  By: Adsolution
//================================
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using OpenSpace;
using UnityEngine.SceneManagement;
using RaymapGame.Rayman2;

namespace RaymapGame {
    public partial class PersoController {
        //================================
        //  Management
        //================================
        public float dt => interpolate ? Time.fixedDeltaTime : Time.deltaTime;
        public bool active => persoEnabled && (!(t_disable.active ||
            (outOfSector && outOfActiveRadius && !isAlways && (!onlyActiveSector || sector == activeSector))));

        public static Rayman2.Persos.YLT_RaymanModel rayman => Main.rayman;
        public static Rayman2.Persos.StdCam stdCam => GetPerso<Rayman2.Persos.StdCam>();
        public static PersoController mainActor => Main.mainActor;
        public bool isMainActor => this == mainActor;

        public static void ChangeMap(string lvl) {
            lvl = MapNames.GetCorrectCapsMapName(lvl);
            if (!string.IsNullOrEmpty(lvl)) {
                // RaymapGame: reset
                Main.loadState = Main.LoadState.EndMap;
                Timer.timers.Clear();
                getPersosCache.Clear();
                getPersoCache.Clear();
                persos.Clear();

                // Raymap: change map
                UnitySettings.MapName = lvl;
                MapLoader.Reset();
                SceneManager.LoadScene(0);
            }

            /*Process.Start(new ProcessStartInfo {
                FileName = Application.dataPath.Replace("_Data", ".exe"),
                Arguments = $"-m Rayman2PC -d \"{Main.GetArgsGameDir()}\" -l " + lvl
            });
            Application.Quit();*/
        }

        public static void SetMainActor(PersoController perso)
            => Main.SetMainActor(perso);
        public void SetMainActor()
            => SetMainActor(this);

        public void Restart() {
            if (!persoEnabled) return;
            foreach (var t in _timers)
                t.Value.Abort();
            pos = startPos;
            rot = startRot;
            scale3 = startScale;
            vel = Vector3.zero;
            hitPoints = startHitPoints;
            SetVisibility(true);
            SetRule("");
            OnStart();
        }
        public void Enable() {
            persoEnabled = true;
            Restart();
        }
        public void Disable() {
            persoEnabled = false;
            SetNullPos();
        }

        public bool hasMount => mount != null;
        public void SetMount(PersoController mount) {
            if (mount == null)
                this.mount = null;
            else if (!Timers("MountHyst").active) {
                this.mount = mount;
                SetRule("Mounted");
            }
        }

        public MethodBase SetRule(string rule, params object[] ruleParams) {
            if (rules.ContainsKey(rule)) {
                if (this.rule != rule || this.ruleParams != ruleParams) {
                    prevRule = this.rule;
                    this.rule = rule;
                    this.ruleParams = ruleParams;
                } return rules[rule];
            }
            this.rule = NO_RULE_SET;
            return null;
        }

        public void AddDeathEvent(Action onDeath) {
            deathEvents.Add(onDeath);
        }
        public void AddCombatEvent(Action onEnterCombat) {
            combatEvents.Add(onEnterCombat);
        }

        public Timer Timers(string name) {
            if (!_timers.ContainsKey(name))
                _timers.Add(name, new Timer());
            return _timers[name];
        }

        public void DisableForSeconds(float seconds) {
            velY = 0;
            velXZ = Vector3.zero;
            t_disable.Start(seconds);
        }
        Timer t_disable = new Timer();

        public Channel GetChannel(int channelNo, bool updated = false) {
            if (!updated) {
                foreach (var c in channels)
                    if (c.tr.name == "Channel " + channelNo.ToString())
                        return c;
            }
            else foreach (Transform t in transform)
                    if (t.name == "Channel " + channelNo.ToString())
                        return new Channel(t);
            return null;
        }

        Dictionary<string, object> dsgCache = new Dictionary<string, object>();
        public bool HasDsgVar(string name) {
            if (dsgCache.ContainsKey(name)) return true;
            if (dsg?.dsgVarEntries != null)
                foreach (var dv in dsg.dsgVarEntries.Where((x) => x.NiceVariableName.ToLower() == name.ToLower()))
                    return true;
            return false;
        }
        public T GetDsgVar<T>(string name) {
            if (dsgCache.ContainsKey(name)) return (T)dsgCache[name];
            if (dsg?.dsgVarEntries != null)
                for (int v = 0; v < dsg.dsgVarEntries.Length; v++)
                    if (dsg.dsgVarEntries[v].NiceVariableName.ToLower() == name.ToLower()) {
                        var e = dsg.editableEntries[v]?.valueInitial;
                        if (e == null) e = dsg.editableEntries[v]?.valueModel;
                        if (e != null) {
                            dsgCache.Add(name, Dsg.GetValue<T>(e.val));
                            return (T)dsgCache[name];
                        }
                    }
            dsgCache.Add(name, default);
            return default;
        }

        public PersoController GetLinkedPerso() {
            for (int i = 0; i < 10; i++) {
                var link = GetDsgVar<PersoController>("Perso_" + i.ToString());
                if (link != null)
                    return link;
            }
            return null;
        }

        public void Remove() {
            persos.Remove(persoName);

            if (getPersosCache.ContainsKey(GetType())) {
                var list = getPersosCache[GetType()];
                if (list != null) {
                    for (int p = 0; p < list.Count; p++)
                        if (list[p].persoName == persoName) {
                            list.Remove(list[p]); break;
                        }
                }
            }
            if (getPersoCache.ContainsKey(GetType()))
                if (getPersoCache[GetType()]?.persoName == persoName)
                    getPersoCache.Remove(GetType());

            if(gameObject != null) Destroy(gameObject);
        }

        public static Type PersoTypeFromString(string s) {
            return Type.GetType($"{nameof(RaymapGame)}.{Main.gameName}.Persos.{Main.editor.projectileText.text}");
        }


		public static PersoController GetPerso(string persoName) {
            if (persoName == null) return null;
            if (persos.ContainsKey(persoName.ToLower()))
                return persos[persoName.ToLower()];
            return null;
        }

        static Dictionary<Type, PersoController> getPersoCache = new Dictionary<Type, PersoController>();
        public static P GetPerso<P>() where P : PersoController
            => (P)GetPerso(typeof(P));
        public static PersoController GetPerso(Type persoType) {
            if (getPersoCache.ContainsKey(persoType)) {
                if (getPersoCache[persoType] != null) {
                    return getPersoCache[persoType];
                }
            }
            var r = (PersoController)FindObjectOfType(persoType);
            if (r != null) {
                getPersoCache[persoType] = r;
            }
            return r;
        }

        static Dictionary<Type, List<PersoController>> getPersosCache = new Dictionary<Type, List<PersoController>>();
        public static P[] GetPersos<P>() where P : PersoController {
            return (P[])GetPersos(typeof(P));
        }
        public static PersoController[] GetPersos(Type persoType) {
            if (getPersosCache.ContainsKey(persoType))
                return getPersosCache[persoType].ToArray();
            var r = (PersoController[])FindObjectsOfType(persoType);
            if (r != null && r.Length != 0) {
                getPersosCache.Add(persoType, r.ToList());
            }
            return r;
        }

        public P GetClosestPerso<P>(float maxDist) where P : PersoController
            => (P)GetClosestPerso(typeof(P), (p) => DistTo(p) < maxDist);
        public P GetClosestPerso<P>(Func<PersoController, bool> condition = null) where P : PersoController
            => (P)GetClosestPerso(typeof(P), condition);
        public PersoController GetClosestPerso(Type persoType, float maxDist)
            => GetClosestPerso(persoType, (p) => DistTo(p) < maxDist);
        public PersoController GetClosestPerso(Type persoType, Func<PersoController, bool> condition = null) {
            PersoController[] ps;
            if (condition == null) ps = GetPersos(persoType);
            else ps = GetPersos(persoType).Where(condition).ToArray();
            PersoController closest = null;
            float dist = 999999999;
            foreach (var p in ps.Where((x) => x != this)) {
                var newDist = DistTo(p);
                if (newDist < dist) {
                    closest = p;
                    dist = newDist;
                }
            }
            return closest;
        }


        //Clone a Perso -- by Shrooblord
        static int counter;
        //public T Clone<T>(Vector3 offset, bool isAlways = false) where T : PersoController {
        //return (T)Clone(offset, typeof(T), isAlways);
        //}

        public P Clone<P>(Vector3 pos) where P : PersoController
            => (P)Clone(typeof(P), out _, pos, Vector3.zero, this);
        public PersoController Clone(Type persoType, out PersoController cl, Vector3 pos, Vector3 rot)
            => Clone(persoType, out cl, pos, rot, this);
        /// <summary>
        /// Create a copy of another Perso in the scene and potentially add custom behaviour to it.
        /// @param Vector3 offset   offset from the to-be-cloned's position
        /// @param bool [isAlways = false] Projectiles and the likes are "isAlways" objects. They do not have level loading and Sector data, but are always loaded.
        /// @param Type [customContoller = null] Whether to add custom behaviour to this clone. Leave null to add the same behaviour as the original.
        /// </summary>
        public static PersoController Clone(Type persoType, out PersoController cl, Vector3 pos, Vector3 rot, PersoController spawner) {
            if (!Main.isRom) {
                var pc = GetPerso(persoType);
                OpenSpace.Object.Perso clone;               //the clone to-be
                OpenSpace.Object.Perso me = pc.perso.perso;    //the template to clone from

                //create perso descriptor
                clone = new OpenSpace.Object.Perso(me.offset, me.SuperObject);  //SO will be null if isAlways is true, but that's actually fine
                clone.nameFamily = me.nameFamily;
                clone.nameModel = me.nameModel;

                string myName;
                /*if (customController != null) {
                    myName = customController.Name;
                } else*/
                {
                    myName = me.namePerso;
                }

                //If I'm a clone of a clone...
                if (myName.Contains("_clone_")) {
                    myName = myName.Remove(myName.LastIndexOf("_clone_"));
                }
                clone.namePerso = myName + "_clone_" + counter++;

                clone.p3dData = me.p3dData;
                clone.stdGame = me.stdGame;

                if (clone.collset != null)
                    clone.collset = me.collset.Clone(clone.Gao.transform);

                InitPersoCoreAndScripts(clone, true);

                var pb = clone.Gao.GetComponent<PersoBehaviour>();

                //pull it in close
                if (pb.IsLoaded)
                    clone.Gao.transform.position = pos;

                cl = pb.gameObject.AddComponent(persoType) as PersoController;
                cl.creator = spawner;

                return cl;
            }
            else {
                return cl = null;
            }
        }
    }
}