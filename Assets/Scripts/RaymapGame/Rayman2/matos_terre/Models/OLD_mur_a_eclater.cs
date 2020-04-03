//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Breakable temple wall
    /// </summary>
    public partial class OLD_mur_a_eclater : matos_terre {
        protected override void OnStart() {
            SetHealth(30);
            SetRule("Default");
        }

        protected override void OnDeath() {
            anim.Set(Anim.BreakableWallBreak);
            Timers("Remove").Start(1, SetNullPos);
        }

        protected void Rule_Default() {
            ReceiveProjectiles();

            if (hitPoints > 29)
                anim.Set(Anim.BreakableWallNormal);
            else if (hitPoints > 9)
                anim.Set(Anim.BreakableWallDamage1);
            else 
                anim.Set(Anim.BreakableWallDamage2);
        }
    }
}