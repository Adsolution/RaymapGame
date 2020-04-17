//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Walking Shell
    /// </summary>
    public class MIC_Obus_Complexe : obus {
        bool inAlertRadius => DistTo(rayman) < 25;

        protected override void OnDeath() {
            CreateExplosion(pos);
            CamShake3D(1, 0.6f);
            SetNullPos();
            Timers("Respawn").Start(1, Restart);
        }

        public void WakeUp() {
            Timers("FallAsleep").Abort();
            anim.Set(Anim.WakeUp);
            Timers("WakeUp").Start(1.6f, () => SetRule("Run"));
            SetRule("");
        }

        protected override void OnStart() {
            if (deathLinks.Count != 0) {
                SetNullPos(); return;
            }
            SetHealth(10);
            SetShadow(true);
            col.wallEnabled = true;
            SetRule("Idle");
        }

        protected override void OnUpdate() {
            ReceiveProjectiles();
        }

        void Rule_Idle() {
            if (newRule) {
                Timers("FallAsleep").Start(8, () => SetRule("Sleep"));
            }
            anim.Set(Anim.Idle);
            if (inAlertRadius) WakeUp();
        }

        void Rule_Sleep() {
            anim.Set(Anim.Sleep);
            if (inAlertRadius) WakeUp();
        }

        void Rule_Run() {
            if (newRule) {
                anim.Set(Anim.RunStart, 0);
                Timers("RunStart").Start(0.3f);
            }

            if (Timers("RunStart").finished) {
                anim.SetSpeed(moveSpeed * 8);
                moveSpeed = 8;
                navRotSpeed = 3;

                if (col.ground.AnyGround) {
                    col.StickToGround();
                    velY = 0;
                    SetFriction(15, 1);

                    if (true || GetDsgVar<WaypointGraph>("Graph_11") == null) {
                        LookAt2D(rayman.pos, navRotSpeed);
                        NavForwards();
                    }
                    else if (NavWaypointGraph(GetDsgVar<WaypointGraph>("Graph_11")))
                        SetRule("Sleep");
                }
                else {
                    SetFriction(1, 1);
                    LookAt(pos + Vector3.down, 0.5f);
                    ApplyGravity();
                }

                if (col.wall.AnyWall) {
                    Damage(vel.magnitude * Vector3.Angle(vel, col.wall.hit.normal) / 720);
                    vel += col.wall.hit.normal * 15;
                    FaceVel(false, 60);
                }
            }

            if (CheckCollisionZone(rayman, OpenSpace.Collide.CollideType.ZDM)) {
                Kill();
            }
        }
    }
}