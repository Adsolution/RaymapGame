//================================
//  By: Adsolution
//================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RaymapGame.EditorUI {
    public class PersoListItem : MonoBehaviour, IPointerDownHandler {
        public PersoController perso;
        public Text persoName;
        public Image bg;
        public Image dot;
        PersoList list;

        public void OnPointerDown(PointerEventData eventData) {
            Main.SetMainActor(perso);
        }

        void Start() {
            list = GetComponentInParent<PersoList>();
            perso = perso.GetComponent<PersoController>();

            persoName.text = perso.perso.perso.namePerso;
        }

        void Update() {
            if (perso == Main.mainActor)
                dot.color = list.colorControllerDot;
            else dot.color = list.colorGeneric;

            if (perso == Main.rayman)
                bg.color = list.colorController;
            else bg.color = list.colorGeneric;
        }
    }
}
