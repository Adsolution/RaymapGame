//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Bouncing Eye
    /// </summary>
    public partial class MIC_OctusDiabolus : nenoeil {
        protected override void OnStart() {
            SetRule("Default");
        }

        protected override void OnDeath() {
            SetNullPos();
        }

        protected void Rule_Default() {
            gravity = -8;
            ApplyGravity();
            ReceiveProjectiles();
            if (col.ground.AnyGround)
                velY = 3;
        }
    }
}