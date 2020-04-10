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

        void BounceFX() {
            SFX("Rayman2/Rayman/shoot/RICOCHET").Play(0.2f);
            SpawnParticle("RayRicochet", LumType.Yellow);
        }

        protected override void OnDeath() {
            BounceFX();
            SetRule("");
            SetVisibility(false);
            Timers("Remove").Start(0.125f, Remove);
        }

        void Rule_Shot() {
            if (newRule) {
                SFX("Rayman2/Rayman/shoot/simple").Play(0.05f);
                SpawnParticle(true, "FistTrail1", LumType.Yellow);
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
                    BounceFX();

                    // Optional find homing target
                    var t = FindTarget(20, 45);
                    if (t != null)
                        vel = (t.pos - pos).normalized * vel.magnitude;
                }
                else
                    Kill();
            }

            if (DistTo(rayman) > 40) {
                SetRule("Fizzle");
            }
        }

        protected void Rule_Fizzle() {
            if (newRule) {
                Timers("Remove").Start(0.35f, Remove);
                anim.Set(1);
            }
        }
    }
}