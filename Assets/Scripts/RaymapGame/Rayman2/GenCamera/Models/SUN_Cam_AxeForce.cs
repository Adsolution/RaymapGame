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
        public float dist;
        public float xAngle;
        public bool outside;
        protected override void OnStart() {
            dist = GetDsgVar<float>("Float_4");
            xAngle = GetDsgVar<float>("Float_7");
            outside = GetDsgVar<bool>("Boolean_12");
        }
    }
}