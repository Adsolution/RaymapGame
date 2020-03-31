using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace RaymapGame {
    public class ControlPanel : EditorWindow {
        [MenuItem("Raymap/RaymapGame/Control Panel")]
        public static void ShowWindow() {
            GetWindow(typeof(ControlPanel));
        }

        public static string persoPath => $"Assets/Scripts/RaymapGame/{Main.gameName}";
        public static string persoLvlPath => $"Assets/Scripts/RaymapGame/{Main.gameName}/PersoLevelScripts/{Main.lvlName}";



        public static void CreatePersoScript(PersoBehaviour pb, string description, string author, string rank) {
            string baseName = "";
            string newName = "";
            string newDir = "";
            switch (rank) {
                case "Family":
                    newName = pb.perso.nameFamily;
                    newDir = $"{persoPath}/{pb.perso.nameFamily}";
                    break;
                case "Model":
                    baseName = pb.perso.nameFamily;
                    newName = pb.perso.nameModel;
                    newDir = $"{persoPath}/{pb.perso.nameFamily}/Models";
                    break;
                case "Instance":
                    baseName = pb.perso.nameModel;
                    newName = pb.perso.namePerso;
                    newDir = $"{persoLvlPath}";
                    break;
            }
            
            if (!Directory.Exists(newDir))
                Directory.CreateDirectory(newDir);
            string newPath = $"{newDir}/{newName}.cs";
            if (File.Exists(newPath)) return;

            var scr = new StreamReader($"Assets/Scripts/RaymapGame/PersoEditor/Objects/NewScript_{rank}.txt");
            var outs = scr.ReadToEnd()
                .Replace("Author", author)
                .Replace("NewScript", newName)
                .Replace("DerivedScript", baseName)
                .Replace("Description", description).Split(new string[] { "~~" }, System.StringSplitOptions.RemoveEmptyEntries);

            var newScr = new StreamWriter(newPath);
            if (author != "") newScr.Write(outs[0]);
            newScr.Write(outs[1]);

            scr.Close();
            newScr.Close();
        }



        public static void ExportOBJ(int sector, int meshIndex) {
            var mesh = GameObject.Find("Father Sector").transform
                .GetChild(meshIndex)?.GetComponentsInChildren<MeshRenderer>();

            string dir = $"Data/World/{Main.lvlName}/Sector{sector.ToString("00")}";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var obj = new StreamWriter($"{dir}/Mesh{meshIndex.ToString("00")}.obj");
            obj.WriteLine($"# {Main.lvlName} -- Sector {sector} -- Mesh {meshIndex}");

            for (int s = 0, i = 0; s < mesh.Length; s++) {
                obj.WriteLine($"o SubMesh{s.ToString("00")}");
                var m = mesh[s].GetComponent<MeshFilter>().sharedMesh;

                foreach (var v in m.vertices)
                    obj.WriteLine($"v {v.x} {v.z} {v.y}");
                for (int t = 0; t < m.triangles.Length; t += 3)
                    obj.WriteLine($"f {i++} {i++} {i++}");
            }
            obj.Close();
        }







        static Timer t_error = new Timer();

        public static PersoBehaviour GetSelectedPersoBehaviour() {
            return Selection.activeGameObject.GetComponentInParent<PersoBehaviour>();
        }


        public static void Header(string text) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(text, EditorStyles.toolbarButton);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }


        static PersoController[] persos = new PersoController[0];
        public void RescanLevel() {
            persos = FindObjectsOfType<PersoController>();
        }


        static string author = System.Environment.UserName;
        static string description = "";

        static float nameWidth = 160;
        public void OnGUI() {
            Header("Create Perso Script");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Author:", GUILayout.Width(nameWidth));
            author = GUILayout.TextField(author);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("English description:", GUILayout.Width(nameWidth));
            description = GUILayout.TextField(description);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Create from Selected Perso", GUILayout.Width(nameWidth));
            if (GUILayout.Button("Family")) {
                var pb = GetSelectedPersoBehaviour();
                CreatePersoScript(pb, description, author, "Family");
            }
            if (GUILayout.Button("Model")) {
                var pb = GetSelectedPersoBehaviour();
                CreatePersoScript(pb, description, author, "Family");
                CreatePersoScript(pb, description, author, "Model");

            }
            if (GUILayout.Button("Instance")) {
                var pb = GetSelectedPersoBehaviour();
                CreatePersoScript(pb, description, author, "Family");
                CreatePersoScript(pb, description, author, "Model");
                CreatePersoScript(pb, description, author, "Instance");

            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(nameWidth);
            if (t_error.active) {
                GUILayout.Label("Perso script with this name already exists");
            }
            GUILayout.EndHorizontal();


            Header("Geometry");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Export Selected to OBJ (not working)")) {
                if (Selection.activeObject.name.Contains("IPO "))
                    ExportOBJ(
                        Selection.activeGameObject.transform.parent.GetSiblingIndex(),
                        Selection.activeGameObject.transform.GetSiblingIndex());
                else
                    Debug.LogError("Selection is not a sector mesh (\"IPO @ ...\").");
            }
            GUILayout.EndHorizontal();
        }
    }
}