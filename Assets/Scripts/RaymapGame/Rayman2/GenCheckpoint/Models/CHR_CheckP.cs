//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Green Lum spawn point
    /// </summary>
    public partial class CHR_CheckP : GenCheckpoint {
        public override bool resetOnRayDeath => false;
        public Alw_Lums_Model lum;

        protected override void OnStart() {
            lum = Clone<Alw_Lums_Model>(pos + Vector3.up * 2);
            lum.type = LumType.Green;
            lum.attractRadius = 8;
        }
    }
}