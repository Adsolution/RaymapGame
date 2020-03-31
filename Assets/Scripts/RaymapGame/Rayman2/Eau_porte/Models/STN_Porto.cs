//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Water_10 Big Door
    /// </summary>
    public partial class STN_Porto : Eau_porte {
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Rumbling");
        }

        protected override void OnUpdate() {
            if (Input.GetKeyDown(KeyCode.V))
                SetRule("Rumbling");
        }

        protected void Rule_Rumbling() {
            if (newRule) Timers("Open Delay").Start(3, () => SetRule("Opening"));

            foreach (var c in channels)
                c.pos = Vector3.Lerp(c.pos, c.startPos + Random.onUnitSphere * 0.2f, dt * 30);
        }

        void Rule_Opening() {
            if (newRule) Timers("Open Finish").Start(4, () => SetRule(""));

            float m = dt * 2.5f;
            channels[0].pos += m * -upward;
            channels[1].pos += m * (upward - right).normalized;
            channels[2].pos += m * (upward + right).normalized;
        }
    }
}