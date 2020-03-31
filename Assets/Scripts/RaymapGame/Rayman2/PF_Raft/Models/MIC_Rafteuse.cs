//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Stone and Fire temple lava floe
    /// </summary>
    public partial class MIC_Rafteuse : PF_Raft {
        protected override void OnStart() {
            navRotSpeed = 0;
            SetFriction(1, 1);
            SetRule("Waiting");
        }

        void Rule_Waiting() {
            if (StoodOnBy(mainActor))
                SetRule("GraphFlow");
        }

        void Rule_GraphFlow() {
            if (NavNearestWaypointGraph())
                SetRule("Bumped");
        }

        void Rule_Bumped() {
            if (newRule) {
                vel = -vel.normalized * 3;
                SetFriction(2, 2, 0.2f);
                rotVel.y = 45;
                Timers("Stun").Start(2, () => SetRule("FreeFlow"));
            }

            rot = Quaternion.Slerp(rot, rot * Random.rotation, dt / (1 + Timers("Stun").elapsed));
            AlignY(5);
        }

        void Rule_FreeFlow() {
            if (col.groundFar.DeathWarp) {
                SetFriction(0.2f, 1, 0.05f);
                vel += (col.groundFar.hit.normal + Random.insideUnitSphere) * dt * 5;
                pos.y = Mathf.Lerp(pos.y, col.groundFar.hit.point.y, dt);
            }
            if (col.wall.AnyWall && velXZ.magnitude > 1) {
                BounceXZ(col.wall.hit.normal);
                rotVel.y = rndAngle / 15;
            }
            col.wallEnabled = true;
            col.radius = 2.5f;
            col.bottom = -0.5f;
            col.top = 0;
            gravity = -5;
            ApplyGravity();
        }
    }
}