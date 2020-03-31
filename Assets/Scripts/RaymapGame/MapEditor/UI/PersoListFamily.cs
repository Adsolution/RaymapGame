using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RaymapGame.EditorUI {
    public class PersoListFamily : MonoBehaviour, IPointerDownHandler {
        public Type familyType;
        public Text familyText;
        public List<PersoController> persos = new List<PersoController>();
        PersoList list;

        public void OnPointerDown(PointerEventData eventData) {
            list.SetInstList(familyType);
        }

        void Start() {
            list = GetComponentInParent<PersoList>();
            familyText.text = familyType.Name;
        }
    }
}
