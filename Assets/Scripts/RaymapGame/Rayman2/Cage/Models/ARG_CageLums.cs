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
                anim.Set(Anim.CageGround);
            else {
                anim.Set(Anim.CageHanging);
                SetShadow(true);
                shadow.size = 2;
                shadow.fadeDistance = 30;
            }

            SetRule("DesiringFreedom");
        }

        Timer t_hit = new Timer();
        protected void Rule_DesiringFreedom() {
            if (ReceiveProjectiles() != null)
                if (ground) {
                    anim.Set(Anim.CageGroundHit, 0);
                    t_hit.Start(0.3f, () => anim.Set(Anim.CageGround));
                }
                else anim.Set(Anim.CageHangingHit, 0);
        }

        protected override void OnDeath() {
            t_hit.Abort();
            if (ground) {
                anim.Set(Anim.CageGroundBreak, 1);
                t_hit.Start(1, () => SetVisibility(false));
            }
            else anim.Set(Anim.CageHangingBreak, 1);
            SetShadow(false);
            SetRule("");
        }
    }
}