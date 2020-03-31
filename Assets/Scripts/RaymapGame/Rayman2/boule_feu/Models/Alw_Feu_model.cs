//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Fireball
    /// </summary>
    public partial class Alw_Feu_model : boule_feu {
        protected override void OnStart() {
            radius = GetCollisionSphere(OpenSpace.Collide.CollideType.ZDM).radius;
        }
        protected void Rule_Shot() {
            if (newRule) {
                anim.SetSpeed(30);
                Timers("Remove").Start(0.9f, Remove);
                gravity = 4;
                scale = Random.Range(0.75f, 1.25f);
                rot = Random.rotation;
            }
            radius = scale += dt;
            ApplyGravity();
        }
    }
}