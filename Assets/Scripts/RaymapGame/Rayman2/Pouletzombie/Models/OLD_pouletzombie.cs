//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Zombie Chicken
    /// </summary>
    public partial class OLD_pouletzombie : Pouletzombie {
        protected override void OnStart() {
            SetFriction(5, 5);
            moveSpeed = 3;
            SetRule("Waiting");
        }

        protected void Rule_Waiting() {
            if (rayman.sector == sector)
                SetRule("Rising");
        }

        float spawnAngle;
        protected void Rule_Rising() {
            if (newRule) {
                spawnAngle = Random.value * 360;
                Orbit(new Vector3(rayman.pos.x, pos.y, rayman.pos.z) + rayman.apprVelXZ * 5, 7, spawnAngle, 0);
            }
            LookAt2D(rayman.pos);
            NavDirection(upward);

            if (pos.y > rayman.pos.y + 3)
                SetRule("Chasing");
        }

        Vector3 dir;
        Timer t_restart = new Timer();
        Timer t_speedup = new Timer();
        protected void Rule_Chasing() {
            if (newRule) {
                dir = rayman.pos - pos;
                t_speedup.Start(0.3f, () => moveSpeed = 8);
                t_restart.Start(4, Restart);
            }

            NavDirection2D(dir);
        }
    }
}