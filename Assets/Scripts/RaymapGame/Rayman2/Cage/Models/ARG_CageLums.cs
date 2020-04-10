//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Cage
    /// </summary>
    public partial class ARG_CageLums : Cage {
        public bool ground;
        protected override void OnStart() {
            col.bottom = -1;
            maxHitPoints = 20;
            hitPoints = 20;

            if (ground = GetDsgVar<byte>("UByte_1") == 1)
                anim.Set(Anim.CageGround, 0);
            else {
                anim.Set(Anim.CageHanging, 0);
                SetShadow(true);
                shadow.size = 2;
                shadow.fadeDistance = 30;
            }

            SetRule("DesiringFreedom");
        }

        protected void Rule_DesiringFreedom() {
            if (ReceiveProjectiles() != null) {
                if (ground) {
                    anim.Set(Anim.CageGroundHit, 0);
                    Timers("Hit").Start(0.3f, () => anim.Set(Anim.CageGround, 0));
                }
                else anim.Set(Anim.CageHangingHit, 0);
                SFX("Rayman2/Cage/Hit").Play();
            }

            Timers("Help").Start(Random.Range(8, 18), ()
                => SFX("Rayman2/Cage/Help").Play(), false);
        }

        protected override void OnDeath() {
            GetPerso<World>().cages++;
            Timers("Hit").Abort();
            Timers("Help").Abort();
            if (ground) {
                anim.Set(Anim.CageGroundBreak, 1);
                Timers("Hide").Start(1.1f, () => SetVisibility(false));
            }
            else anim.Set(Anim.CageHangingBreak, 1);
            SFX("Rayman2/Cage/Break").Play();
            SetShadow(false);
            SetRule("");
        }
    }
}