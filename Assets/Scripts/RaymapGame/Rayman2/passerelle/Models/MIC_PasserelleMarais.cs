//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Bayou 1 switch bridge
    /// </summary>
    public partial class MIC_PasserelleMarais : passerelle {
        public override bool resetOnRayDeath => false;
        float rvel;
        float rx;

        protected void Rule_Tip() {
            rvel += 110 * dt;
            rx += rvel * dt;

            if (rx > 87) {
                rx = 87;
                if (Mathf.Abs(rvel) < 2)
                    SetRule("");
                else
                    rvel *= -0.3f;
            }

            rot = startRot + new Vector3(-rx, 0, 0);
        }
    }
}