//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Spinning spike trunk
    /// </summary>
    public partial class CHR_tronc_pic : tronc {
        protected override void OnStart() {
            SetRule("Spinz");
        }

        protected void Rule_Spinz() {
            RotateLocalY(GetDsgVar<float>("Float_0") * Mathf.Rad2Deg, 1);
        }
    }
}