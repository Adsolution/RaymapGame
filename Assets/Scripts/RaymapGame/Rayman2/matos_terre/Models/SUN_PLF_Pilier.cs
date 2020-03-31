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

        Timer t_sinkWait = new Timer();
        Vector3 rndRot;
        protected override void OnStart() {
            rndRot = Random.rotation.eulerAngles / 60;
            gravity = -1.6f;
            SetFriction(1, 0.05f);
            SetRule("Wait");
        }

        protected void Rule_Wait() {
			if (StoodOnBy(rayman)) {
                t_sinkWait.Start(0.5f);
                SetRule("Sinking");
            }
        }
        protected void Rule_Sinking() {
            if (t_sinkWait.active) return;
            ApplyGravity();
            rot.eulerAngles += rndRot * dt * Mathf.Clamp01(-velY);
            if (pos.y < startPos.y - 10)
                SetRule("");
        }
    }
}