//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Pirate gorilla
    /// </summary>
    public partial class MIC_GorilleGardien : pirate_gorille {
        protected override void OnDeath() {
            SetNullPos();
        }
        protected override void OnStart() {
            navRotSpeed = 4;
            gravity = -40;
            SetRule("Waiting");
        }

        void Rule_Waiting() {
            if (col.ground.AnyGround)
                col.StickToGround();
            if (DistTo(rayman) < 20)
                SetRule("Chasing");
        }

        void Rule_Chasing() {
            if (col.ground.AnyGround) {
                col.wallEnabled = true;
                moveSpeed = 10;
                SetFriction(50, 0);
                NavTowards2D(rayman);
                col.StickToGround();
            }
            else if (col.ground.None) {
                SetRule("Falling");
            }
            else if (col.ground.DeathWarp) {
                SetRule("Electrocute");
            }
            anim.Set(Anim.Run);
        }

        void Rule_Falling() {
            if (newRule) Timers("FallPause").Start(1.5f);
            SetFriction(0.15f, 0);

            if (Timers("FallPause").finished) {
                ApplyGravity();

                if (col.ground.FallTrigger || pos.y < startPos.y - 100) {
                    Kill();
                }
                else if (col.ground.DeathWarp) {
                    SetRule("Electrocute");
                }
            }
            anim.Set(Anim.Fall);
        }

        void Rule_Electrocute() {
            if (newRule) Timers("ElecDeath").Start(3, Kill);
            anim.Set(Anim.Electrocute);
        }
    }
}