using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RaymapGame {
    [CustomEditor(typeof(PersoController), true)]
    public class PersoControllerEditor : Editor {
        PersoController perso => (PersoController)target;
        static float nameWidth = 100;

        public static void Header(string text, bool first = false) {
            if (!first) GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label(text, EditorStyles.toolbarButton);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        public object Field(string name, object value) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name, GUILayout.Width(nameWidth));
            object field = null;
            if (value is float)
                field = EditorGUILayout.FloatField((float)value);
            else if (value is int)
                field = EditorGUILayout.IntField((int)value);
            else if (value is bool)
                field = EditorGUILayout.Toggle((bool)value);
            else if (value is Vector3)
                field = EditorGUILayout.Vector3Field("", (Vector3)value);
            else if (value is Object)
                field = EditorGUILayout.ObjectField("", (Object)value, value.GetType(), true);
            else if (value is string) {
                GUILayout.Label((string)value, EditorStyles.boldLabel);
                field = value;
            }

            GUILayout.EndHorizontal();
            if (value is string)
                GUILayout.Space(3);
            return field;
        }

        FieldInfo[] fields;


        public override void OnInspectorGUI() {
            perso.transform.hideFlags = HideFlags.HideInInspector;
            var soc = perso.GetComponent<SuperObjectComponent>(); if (soc != null) soc.hideFlags = HideFlags.HideInInspector;
            var cbc = perso.GetComponent<CustomBitsComponent>(); if (cbc != null) cbc.hideFlags = HideFlags.HideInInspector;
            var mnd = perso.GetComponent<MindComponent>(); if (mnd != null) mnd.hideFlags = HideFlags.HideInInspector;
            var mod = perso.GetComponent<Moddable>(); if (mod != null) mod.hideFlags = HideFlags.HideInInspector;
            var dmc = perso.GetComponent<DynamicsMechanicsComponent>(); if (dmc != null) dmc.hideFlags = HideFlags.HideInInspector;

            perso.GetComponent<AnimHandler>().hideFlags = HideFlags.HideInInspector;
            perso.GetComponent<Interpolation>().hideFlags = HideFlags.HideInInspector;
            foreach (var a in perso.GetComponents<AudioSource>())
                a.hideFlags = HideFlags.HideInInspector;
            foreach (var a in perso.GetComponents<SFXPlayer>())
                a.hideFlags = HideFlags.HideInInspector;



            Header("Transform", true);
            perso.pos = (Vector3)Field("Position", perso.pos);
            perso.rot = (Vector3)Field("Rotation", perso.rot);
            perso.scale3 = (Vector3)Field("Scale", perso.scale3);
            Field("Sector", perso.sector);


            if (fields == null) fields = perso.persoType.GetFields().Where((x) => x.DeclaringType == perso.persoType).ToArray();
            if (fields.Length > 0) {
                Header($"Family Variables");
                foreach (var f in fields)
                    Field(f.Name, f.GetValue(perso));
            }

            Header("Management");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rule", GUILayout.Width(nameWidth));
            var keys = perso.rules.Keys.ToArray();
            var pick = EditorGUILayout.Popup(System.Array.IndexOf(keys, perso.rule), keys);
            if (perso.rules.ContainsKey(perso.rule))
                perso.SetRule(keys[pick]);

            if (perso.ruleParams != null && perso.rules.ContainsKey(perso.rule)) {
                var pNames = perso.rules[perso.rule].GetParameters();
                var pVals = perso.ruleParams;
                for (int r = 0; r < perso.ruleParams.Length; r++) {
                    Field(pNames[r].Name.Replace("Rule_", ""), pVals[r].ToString());
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Hit Points", GUILayout.Width(nameWidth));
            EditorGUILayout.FloatField(perso.hitPoints);
            GUILayout.Label("/", GUILayout.Width(12));
            EditorGUILayout.FloatField(perso.maxHitPoints);
            GUILayout.EndHorizontal();

            Field("Is Always", perso.isAlways);
            if (!perso.isAlways) Field("Active Radius", perso.activeRadius);

            Field("Active", perso.active);


            Header("Collision");
            Field("Bottom", perso.col.bottom);
            Field("Top", perso.col.top);
            Field("Radius", perso.col.radius);
            Field("Ground", perso.col.ground.Any);
            Field("Wall", perso.col.wall.Any);

            Header("Physics");
            Field("Gravity", perso.gravity);
            Field("Velocity", perso.vel);
            Field("Angular Vel", perso.rotVel);
            Field("Horizontal Fric", perso.fricXZ);
            Field("Vertical Fric", perso.fricY);
            Field("Angular Fric", perso.rotFric);

            Header("Render");
            Field("Has Shadow", perso.hasShadow);
            if (perso.hasShadow) {
                Field("Shadow Size", perso.shadow.size);
                Field("Shadow Distance", perso.shadow.fadeDistance);
            }

            if (perso._timers.Count != 0) {
                Header("Active Timers");
                foreach (var t in perso._timers) {
                    Field(t.Key, t.Value.active ? t.Value.remaining.ToString("00.0") : "00.0");
                }
            }
        }
    }
}
