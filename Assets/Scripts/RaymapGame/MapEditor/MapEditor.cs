//================================
//  By: Adsolution
//================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RaymapGame.EditorUI {
    public class MapEditor : MonoBehaviour {
        public PersoList persoList;
        public InputField projectileText;

        void Start() => Main.onLoad += Main_onLoad;

        void Main_onLoad(object sender, System.EventArgs e) {
            persoList.Load();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.E))
                persoList.gameObject.SetActive(!persoList.gameObject.activeSelf);

            if (Input.GetMouseButtonDown(0)) {
                Camera.main.ScreenPointToRay(Input.mousePosition);
            }
        }
    }
}
