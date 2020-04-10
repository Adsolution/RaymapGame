//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Sinking pillar platform
    /// </summary>
    public partial class SUN_PLF_Pilier : matos_terre {
        Vector3 sinkRot;
        protected override void OnStart() {
            sinkRot = rndRot / 60;
            gravity = -1.6f;
            SetFriction(1, 0.05f);
            SetRule("Wait");
        }

        protected void Rule_Wait() {
			if (StoodOnBy(rayman)) {
                Timers("Sink Wait").Start(0.5f);
                SetRule("Sinking");
            }
        }
        protected void Rule_Sinking() {
            if (Timers("Sink Wait").active) return;
            ApplyGravity();
            rot += sinkRot * dt * Mathf.Clamp01(-velY);
            if (pos.y < startPos.y - 10)
                SetRule("");
        }
    }
}