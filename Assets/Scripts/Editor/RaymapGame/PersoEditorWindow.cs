using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace RaymapGame.PersoEditor {
    public class PersoEditorWindow : EditorWindow {

        [MenuItem("Raymap/RaymapGame/Perso Editor")]
        public static void ShowWindow() {
            GetWindow(typeof(PersoEditorWindow));
        }


        //public MethodBody body = new MethodBody(/*
            //new MethodInvoke(TypeData.loaded.actions["SetFriction"]),
           // new MethodInvoke(TypeData.loaded.actions["SetRule"])
           // */);

        void OnGUI() {
            //foreach (var t in TypeData.loaded.actions.Values)
              //  GUILayout.Label(t.Name);
            //foreach (var a in body.body) {
                //a.Inspect(EditorMode.UnityInspector);
            //}
        }
    }
}
