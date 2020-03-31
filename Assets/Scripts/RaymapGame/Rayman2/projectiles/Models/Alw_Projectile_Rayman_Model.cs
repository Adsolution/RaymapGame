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
                if (++bounces < 3)
                    FindHoming(r.hit.normal);
                else
                    Remove();
            }

            if (DistTo(rayman) > 40) {
                SetRule("Fizzle");
            }
        }

        void FindHoming(Vector3 newDir) {
            float close = 15;
            PersoController closest = null;
            foreach (var p in FindObjectsOfType<PersoController>()) {
                float dist = rayman.DistTo(p.pos);
                if (!p.isMainActor
                    //&& p.perso.perso.stdGame.ConsideredOnScreen()
                    && p.HasCollisionType(OpenSpace.Collide.CollideType.ZDE)
                    && (dist < close)) {
                    close = dist;
                    closest = p;
                }
            }
            if (closest != null)
                vel = (closest.pos - pos).normalized * vel.magnitude;
            Bounce3D(newDir);
            pos += vel.normalized * radius / 2;
            t_bounce.Start(0.03f);
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