//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Door-switch toggler
    /// </summary>
    public partial class OLP_Inverseur : Gen_OLP {
        public PersoController interrupteur;
        protected override void OnStart() {
            interrupteur = GetDsgVar<PersoController>("Perso_0");
            SetRule("Default");
        }

        protected void Rule_Default() {
			
        }
    }
}