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
        Timer t_fallAsleep = new Timer();

        public void WakeUp() {
            if (!newRule) return;

            SetRule("");
            t_fallAsleep.Abort();
            anim.Set(Anim.WakeUp);
            t_wakeUp.Start(1.75f, () => SetRule("Run"));
        }

        protected override void OnStart() {
            if (deathLinks.Count != 0) {
                SetNullPos(); return;
            }
            SetShadow(true);
            col.wallEnabled = true;
            SetRule("Idle");
        }

        void Rule_Idle() {
            if (newRule) {
                t_fallAsleep.Start(8, () => SetRule("Sleep"));
            }
            anim.Set(Anim.Idle);
            if (inAlertRadius) WakeUp();
        }

        void Rule_Sleep() {
            anim.Set(Anim.Sleep);
            if (inAlertRadius) WakeUp();
        }

        Timer t_wakeUp = new Timer();
        Timer t_runStart = new Timer();

        void Rule_Run() {
            if (newRule) {
                anim.Set(Anim.RunStart, 0);
                t_runStart.Start(0.3f);
            }

            if (t_runStart.finished) {
                anim.SetSpeed(moveSpeed * 8);
                moveSpeed = 8;
                navRotSpeed = 3;

                if (col.ground.AnyGround) {
                    col.StickToGround();
                    velY = 0;
                    SetFriction(15, 1);

                    if (GetDsgVar<WaypointGraph>("Graph_11") == null) {
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

                var h = Raycast(Vector3.up * 0.5f, forward, 1);
                if (h.Any) {
                    Bounce3D(h.hit.normal, 1.2f);
                    FaceVel3D(30);
                }
            }

            if (CheckCollisionZone(rayman, OpenSpace.Collide.CollideType.ZDM)) {
                //Reset();
                //rayman.Despawn();
            }
            if (CheckCollisionZone(GetPerso<Alw_Projectile_Rayman_Model>(), OpenSpace.Collide.CollideType.ZDM)) {
                Restart();
                //rayman.Despawn();
            }
        }
    }
}