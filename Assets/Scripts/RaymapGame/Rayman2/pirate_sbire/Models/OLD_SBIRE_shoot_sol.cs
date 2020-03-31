//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Henchman 800
    /// </summary>
    public partial class OLD_SBIRE_shoot_sol : pirate_sbire {
        protected override void OnStart() {
            projectileOffset.z = 2;
            projectileOffset.y = 2.3f;
            maxHitPoints = 30;
            hitPoints = 30;
            SetRule("Sleeping");
        }
    }
}