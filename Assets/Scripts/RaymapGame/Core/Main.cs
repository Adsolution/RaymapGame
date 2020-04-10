//================================
//  By: Adsolution
//================================
using System;
using RaymapGame.Rayman2.Persos;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using OpenSpace;
using OpenSpace.Collide;
using RaymapGame.EditorUI;
using System.Diagnostics;

namespace RaymapGame
{
    public class Main : MonoBehaviour {
        public PersoController _mainActor; // Inspector display only
        public bool alwaysControlRayman;
        public bool controlAnyMainActor;
        public bool showLiveScripts;
        public static bool HDShaders = false;
        public bool emptyLevel;
        public string emptyLevelPersoTest;

        public static bool useFixedTimeWithInterpolation = true;
        public static Main main;
        public static MapEditor editor;
        public static PersoController mainActor;
        public static YLT_RaymanModel rayman;
        public static Type[] persoScripts = new Type[0];
        public static List<PersoController> persos = new List<PersoController>();
        public static StdCam cam;
        public static EnvHandler env;
        public static Controller controller;
        public static AudioSource music;
        public static bool showMainActorDebug;
        public static bool loaded;
        public static event EventHandler onLoad;
        public static bool isRom;
        public static GameUI ui;
        void Main_onLoad(object sender, EventArgs e) { }

        public static string gameName = "Rayman2";
        public static string lvlName => controller?.loader?.lvlName;

        public static string GetArgsGameDir() {

            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
                switch (args[i]) {
                    case "--folder":
                    case "--directory":
                    case "-d":
                    case "-f":
                        return args[i + 1];
                }
            return "";
        }

        public static void LoadLevel(string lvl)
            => PersoController.LoadLevel(lvl);

        public static PersoController SetMainActor(PersoController perso) {
            return main._mainActor = mainActor = perso;
        }

        public static Type[] GetPersoScripts() {
            return persoScripts = (from t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
            where t.IsClass && t.Namespace == $"{nameof(RaymapGame)}.{gameName}.Persos" && !t.IsAbstract
            select t).ToArray();
        }


        void Awake() {
            main = this;
            editor = FindObjectOfType<MapEditor>();
            GetPersoScripts();
            controller = FindObjectOfType<Controller>();
            music = GetComponent<AudioSource>();
            env = gameObject.AddComponent<EnvHandler>();
            onLoad += Main_onLoad;
        }

        void LateUpdate() {
            if (!loaded && controller.LoadState == Controller.State.Finished)
                Load();

            // Debug
            if (Input.GetKeyDown(KeyCode.D))
                showMainActorDebug = !showMainActorDebug;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
                if (UnityEditor.Selection.gameObjects.Length != 0)
                    mainActor.pos = UnityEditor.Selection.gameObjects[0].transform.position;
            if (Input.GetMouseButton(0)) {
                var ms = RayCollider.RaycastMouse();
                if (ms.hitPerso == null) ms = RayCollider.RaycastMouse(true);
                if (ms.hitPerso != null)
                    UnityEditor.Selection.activeObject = ms.hitPerso.gameObject;
            }
#endif

            //if (Input.GetKeyDown(KeyCode.J))
                //SceneManager.LoadScene(0);
        }


        public void ApplyPersoScript(PersoBehaviour pb, ref List<Type> list, ref int iterator) {
            pb.gameObject.AddComponent(list[iterator]);
            list.Remove(list[iterator--]);
        }

        public void Load() {
            // Set physics clock nice and high and enable the ambience/SFX environment effects
            Time.fixedDeltaTime = 1f / 144;
            FindObjectOfType<EnvHandler>().Enable();


            // Do a few things if Empty Level is ticked
            if (emptyLevel)
                ClearLevel(true, emptyLevelPersoTest);


            // Remove colliders on everything but actual world collision
            GameObject root = null;
            if (!isRom)
                root = GameObject.Find("Actual World");
            else foreach (var r in SceneManager.GetActiveScene().GetRootGameObjects())
                    if (r.name.Contains("Hierarchy Root")) root = r;

            if (root != null)
                foreach (var col in root.GetComponentsInChildren<Collider>()) {
                    var comp = col.GetComponent<CollideComponent>();
                    var so = comp?.GetComponentInParent<SuperObjectComponent>();
                    if (comp == null || (comp.type != CollideType.None && comp.type != CollideType.ZDR))
                        Destroy(col);
                    else if (so != null && so.flagPreview.Contains("NoRayTracing"))
                        comp.gameObject.layer = 2;
                }

            // Find Waypoint graphs
            if (controller.graphManager.transform.childCount > 0)
            foreach (Transform gr in controller.graphManager.transform.GetChild(0))
                gr.gameObject.AddComponent<WaypointGraph>();

            var gameMode = Settings.Mode.Rayman2PC;
#if UNITY_EDITOR
            gameMode = UnitySettings.GameMode;
#endif

            // Perso script loading for different games - applied with (Model > Family) priority
            switch (gameMode) {
                case Settings.Mode.Rayman2PC:
                case Settings.Mode.Rayman2DC:
                case Settings.Mode.Rayman3PC:
                    // The OGs work the best
                    foreach (var pb in FindObjectsOfType<PersoBehaviour>()) {
                        Type matchModel = null, matchFamily = null;
                        foreach (var s in persoScripts) {
                            if (s.Name == pb.perso.nameModel) matchModel = s;
                            if (s.Name == pb.perso.nameFamily) matchFamily = s;
                        }
                        if (matchModel != null) persos.Add((PersoController)pb.gameObject.AddComponent(matchModel));
                        else if (matchFamily != null) persos.Add((PersoController)pb.gameObject.AddComponent(matchFamily));
                    }
                    break;

                case Settings.Mode.Rayman2N64:
                    isRom = true;
                    // ROM method with gameObject name splitting, since idk how names are getting there in the first place
                    foreach (var pb in FindObjectsOfType<ROMPersoBehaviour>()) {
                        var names = pb.name.Replace("[", "").Split(new string[] { "] ", " | " }, StringSplitOptions.None);
                        if (names.Length != 3) continue;
                        Type matchModel = null, matchFamily = null;
                        foreach (var s in persoScripts) {
                            if (s.Name == names[1]) matchModel = s;
                            if (s.Name == names[0]) matchFamily = s;
                        }
                        PersoController pc = null;
                        if (matchModel != null) persos.Add(pc = (PersoController)pb.gameObject.AddComponent(matchModel));
                        else if (matchFamily != null) persos.Add(pc = (PersoController)pb.gameObject.AddComponent(matchFamily));
                        if (pc != null && names.Length >= 3) {
                            pc.persoFamily = names[0];
                            pc.persoModel = names[1];
                            pc.persoName = names[2];
                        }
                    }
                    // Camera is currently applied arbitrarily
                    cam = FindObjectOfType<ROMPersoBehaviour>().gameObject.AddComponent<StdCam>();
                    break;

                case Settings.Mode.Rayman2PS2:
                    // Names don't exist. Rayman is always last in the perso list and StdCam is applied arbitrarily
                    var world = GameObject.Find("Dynamic World").transform;
                    rayman = world.GetChild(world.childCount - 1).gameObject.AddComponent<YLT_RaymanModel>();
                    cam = FindObjectOfType<PersoBehaviour>().gameObject.AddComponent<StdCam>();
                    break;
            }

            // HD Shaders?
            if (HDShaders) {
                GameObject.Find("HD").SetActive(HDShaders);
                foreach (var l in controller.lightManager.GetComponentsInChildren<LightBehaviour>()) {
                    var light = l.gameObject.AddComponent<Light>();
                    light.shadows = LightShadows.None;
                    light.intensity = 2;
                    light.color = l.color;
                    light.range = 20;
                }
                foreach (var sec in controller.sectorManager.sectors)
                    foreach (var mr in sec.GetComponentsInChildren<MeshRenderer>()) {
                        if (mr.material.name == "mat_gouraud (Instance)") {
                            var tex = mr.material.GetTexture("_Tex0");
                            mr.material = new Material(Shader.Find("Standard"));
                            mr.material.mainTexture = tex;
                            mr.receiveShadows = true;
                            mr.material.SetFloat("_Glossiness", 0.1f);
                        }
                    }
            }

            // Done loading
            controller.viewGraphs = true;
            Timer.StartNew(0.4f, () => controller.viewGraphs = false);
            onLoad.Invoke(this, EventArgs.Empty);
            loaded = true;
        }


        public static bool anyCollision;
        Vector3 wldPos = new Vector3(256, 256, 256);
        public void ClearLevel(bool createPlane = false, string spawnPerso = "") {

            GameObject ray = null, cam = null, test = null;
            foreach(Transform t in GameObject.Find("Dynamic World").transform) {
                if (t.name.Contains("YLT_RaymanModel"))
                    ray = t.gameObject;
                else if (t.name.Contains("StdCam"))
                    cam = t.gameObject;
                else if (spawnPerso != "" && t.name.Contains(spawnPerso))
                    test = t.gameObject;
                else
                    Destroy(t.gameObject);
            }
            //ray.transform.position = wldPos;
            wldPos = ray.transform.position;
            if (test != null) test.transform.position = wldPos + new Vector3(0, 0, 10);
            GameObject.Find("Father Sector").SetActive(false);
            anyCollision = true;

            if (createPlane) {
                ResManager.Inst("Test/Plane").transform.position = wldPos;
            }
        }
    }
}