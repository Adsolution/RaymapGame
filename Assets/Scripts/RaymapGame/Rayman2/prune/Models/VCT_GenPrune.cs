//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Plum tree sprout
    /// </summary>
    public partial class VCT_GenPrune : prune {
        public override float activeRadius => 20;
        public BNT_ThePrune prune;

        protected override void OnStart() {
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (prune == null) {
                prune = Clone<BNT_ThePrune>(pos);
                prune.rot = SwapYZ(GetDsgVar<Vector3>("Vector_5"));
            }
            else if (ReceiveProjectiles()) {
                prune.SetRule("Bouncing");
            }
        }
    }
}