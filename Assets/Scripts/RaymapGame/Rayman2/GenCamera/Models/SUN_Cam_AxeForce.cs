//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Camera column orbit
    /// </summary>
    public partial class SUN_Cam_AxeForce : GenCamera {
        public override void OnEnter() {
            cam.SetRule("AxeForce", pos, 8);
        }
    }
}