//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Magic Fist
    /// </summary>
    public partial class Alw_Projectile_Rayman_Model : projectiles {
        public override float activeRadius => 100;
        int bounces;
        Timer t_bounce = new Timer();

        void Rule_Shot() {
            if (newRule) {
                bounces = 0;
            }

            var r = Raycast(vel, 1);
            if (r.Any && !t_bounce.active) {
                if (++bounces < 3) {

                    if (r.hitPerso != null) {
                        r.hitPerso.Damage(damage);
                        Remove();
                        return;
                    }

                    Bounce3D(r.hit.normal);
                    FaceVel2D(false);

                    // Optional find homing target
                    var t = FindTarget(20, 45);
                    if (t != null)
                        vel = (t.pos - pos).normalized * vel.magnitude;
                }
                else
                    Remove();
            }

            if (DistTo(rayman) > 40) {
                SetRule("Fizzle");
            }
        }

        protected void Rule_Fizzle() {
            if (newRule) {
                Timer.StartNew(1, Remove);
                anim.SetSpeed(5);
                anim.Set(1);
            }
        }
    }
}