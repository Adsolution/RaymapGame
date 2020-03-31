//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Wind Area
    /// </summary>
    public partial class SUN_GenWind_Mod : GenWind {
        public override float activeRadius => 999999;

        protected override void OnStart() {
            SetRule("Active");
        }

        protected void Rule_Active() {
            if (rayman.col.ground.None && CheckCollisionZone(rayman, OpenSpace.Collide.CollideType.ZDD)) {
                rayman.GiveForce(
                    SwapYZ(GetDsgVar<Vector3>("Vector_3"))
                    * GetDsgVar<float>("Float_4")
                    * (rayman.helic ? 1 : 0.1f));
            }
        }
    }
}