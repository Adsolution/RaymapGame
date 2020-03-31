//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Learn_30 Gate
    /// </summary>
    public partial class SUN_Grille_Cage : Cage {
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Open");
        }

        protected void Rule_Open() {
            vel = -right * 2;
            if (DistTo(startPos) > 5 * scale) {
                vel = Vector3.zero;
                SetRule("");
            }
        }
    }
}