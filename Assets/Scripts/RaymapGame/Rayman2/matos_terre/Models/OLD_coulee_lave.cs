//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Lava fall
    /// </summary>
    public partial class OLD_coulee_lave : matos_terre {
        public override float activeRadius => 100;
        protected override void OnStart() {
            anim.Set(Anim.LavaFall);
            scale3.y *= 1.15f;
            SetRule("Default");
        }

        protected void Rule_Default() {
        }
    }
}