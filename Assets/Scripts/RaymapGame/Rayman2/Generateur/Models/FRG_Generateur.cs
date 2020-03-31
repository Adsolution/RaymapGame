//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Perso generator
    /// </summary>
    public partial class FRG_Generateur : Generateur {
        public override bool hasLinkedDeath => true;
        public PersoController genPerso;
        protected override void OnLinksDead() {
            genPerso.Enable();
        }
    }
}