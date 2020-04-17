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
        CollideInfo r;

        void BounceFX() {
            SFX("Rayman2/Rayman/shoot/RICOCHET").Play(0.2f);
            var p = SpawnParticle("RayRicochet", LumType.Yellow);
            p.transform.position = r.hit.point;
        }

        protected override void OnDeath() {
            BounceFX();
            SetRule("");
            SetVisibility(false);
            Timers("Remove").Start(0.125f, Remove);
        }

        void Rule_Fizzle() {
            if (newRule) {
                Timers("Remove").Start(0.35f, Remove);
                anim.Set(1);
            }
        }

        void Rule_Shot() {
            if (newRule) {
                SFX("Rayman2/Rayman/shoot/simple").SetVolume(0.625f);
                SFX("Rayman2/Rayman/shoot/simple").Play(0.05f);
                SpawnParticle(true, "FistTrail1", LumType.Yellow);
                bounces = 0;
            }

            if (DistTo(rayman) > 40) {
                SetRule("Fizzle");
                return;
            }

            r = Raycast(vel, 1);
            if (r.Any) {
                if (++bounces >= 3)
                    Kill();
                else {
                    if (r.hitPerso != null) {
                        r.hitPerso.Damage(damage);
                        Remove();
                        return;
                    }

                    Bounce3D(r.hit.normal);
                    FaceVel2D(false);
                    BounceFX();

                    var t = FindTarget(20, 45);
                    if (t != null)
                        vel = vel.magnitude * Vec(t);
                }
            }
        }
    }
}