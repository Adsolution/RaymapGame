using UnityEditor;
using UnityEngine;

namespace RaymapGame.PersoEditor {
    [CustomEditor(typeof(Main))]
    public class MainEditor : Editor {
        static float nameWidth = 160;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if (!((Main)target).showLiveScripts) return;

            ControlPanel.Header("");


            GUILayout.BeginHorizontal();
            GUILayout.Label("Live Preview", GUILayout.Width(nameWidth));
            GUILayout.EndHorizontal();

            float ind = 25;
            int columns = Mathf.CeilToInt((Screen.width - ind) / 100);
            float oWidth = Screen.width / columns;

            foreach (var s in Main.persoScripts) {
                int i = 0;
                EditorGUILayout.ToggleLeft(s.Name, true);
                foreach (var p in Main.persos) {
                    if (p.GetType() == s) {
                        if (i == 0) {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(ind);
                        }
                        EditorGUILayout.ObjectField(p, typeof(PersoController), true, GUILayout.MaxWidth(oWidth));
                        i++;
                    }
                    if (i > 0 && (i == columns || Main.persos.IndexOf(p) == Main.persos.Count - 1)) {
                        GUILayout.EndHorizontal();
                        i = 0;
                    }
                }
            }

        }
        void OnGUI() {

        }
    }
}
