//================================
//  By: Adsolution
//================================

using UnityEngine;
using UnityEngine.UI;
using System;

namespace RaymapGame.EditorUI {
    public class PersoList : MonoBehaviour {
        public VerticalLayoutGroup famList;
        public VerticalLayoutGroup instList;
        public Color colorGeneric, colorMain, colorController, colorControllerDot;

        public void Load() {
            foreach (var f in Main.persoScripts) {
                if (f.BaseType == typeof(PersoController) && PersoController.GetPerso(f) != null)
                    ResManager.Inst<PersoListFamily>("MapEditor/UI/PersoListFamily", famList).familyType = f;
            }
        }

        public void SetInstList(Type famType) {
            foreach (Transform tr in instList.transform)
                Destroy(tr.gameObject);

            foreach (var p in FindObjectsOfType<PersoController>()) {
                if (p.persoFamily == famType.Name)
                    ResManager.Inst<PersoListItem>("MapEditor/UI/PersoListItem", instList).perso = p;
            }
        }
    }
}