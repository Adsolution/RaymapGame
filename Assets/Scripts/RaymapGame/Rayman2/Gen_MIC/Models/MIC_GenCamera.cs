//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Cutscene
    /// </summary>
    public partial class MIC_GenCamera : Gen_MIC {
        protected override void OnStart() {
            SetRule("Default");
        }

        protected void Rule_Default() {
			
        }
    }
}