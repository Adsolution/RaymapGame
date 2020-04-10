//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rolling Barrel
    /// </summary>
    public partial class ARG_Tonneau_DonkeyKong : tonneau_boum {
        public override bool resetOnRayDeath => false;
        public override int maxAllowedNearMainActor => 15;
        protected override void OnStart() {
            RotateY(180);
            SetShadow(true);
            shadow.size = 1.5f;
            shadow.fadeDistance = 20;
            waypointLenience = 2;
            navRotSpeed = -1;
            SetFriction(10, 5);
        }

        protected void Rule_Rolling() {
            if (NavNearestWaypointGraph(false)) {
                CamShake3D(1, 0.5f, pos);
                CreateExplosion(pos, 4);
                Remove();
                return;
            }

            anim.Set(Anim.Rolling);

            if (waypoint != null && DistTo2D(waypoint.pos) > 1.2)
                LookAtY(waypoint.pos, 0, 7);
            if (col.ground.AnyGround) {
                if (velY < -7) {
                    stdCam.Shake(0.5f, 0.3f, pos);
                    BounceY(col.ground.hit.normal, 1.5f);
                }
                else col.StickToGround();
            }
            else
                ApplyGravity();

            if (!Timers("Jump").active && DistTo(rayman.pos + Vector3.down * 1.25f) < 1.25f) {
                rayman.pos = pos + Vector3.up * 1.75f;
                rayman.Jump(6.5f, true);
                Timers("Jump").Start(0.5f);
            }
        }
    }
}