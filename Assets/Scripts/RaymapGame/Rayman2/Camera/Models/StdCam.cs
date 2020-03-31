//================================
//  By: Adsolution
//================================
using UnityEngine;
using static RaymapGame.InputEx;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Camera
    /// </summary>
    public partial class StdCam : Camera {

        Timer t_land_orbSpd = new Timer();
        Timer t_tY = new Timer();

        float tY { get => _tY; set { _tY = Mathf.Lerp(_tY, value, dt * 5); } }
        float _tY;



        protected override void OnStart() {
            if (Main.main.emptyLevel)
                pos = Vector3.zero;
            col.top = 0.5f;
            col.wallEnabled = true;
            SetRule("Follow");
        }


        protected override void OnInput() {
            if (Input.GetKeyDown(KeyCode.Y)) {
                if (rule == "Default") {
                    SetRule("Free");
                }
            }
        }

        float shInt, shTime;
        Timer t_shake = new Timer();
        public void Shake(float intensity, float time, Vector3 origin, float innerRadius = 8)
            => Shake(intensity / Mathf.Clamp(DistTo(origin), innerRadius, 999) * innerRadius, time);
        public void Shake(float intensity, float time) {
            shInt = intensity;
            t_shake.Start(shTime = time);
        }

        protected override void OnUpdate() {
            targ = mainActor;

            if (t_shake.active)
                rot = Quaternion.Slerp(rot, rot * Random.rotation, dt * shInt * (t_shake.remaining / shTime));

            cam.transform.position = pos;
            cam.transform.LookAt(pos + forward, Vector3.up);
        }

        Vector3 cen;
        float steepOff => 4 * -targ.apprVel.y;

        protected void Rule_Follow() {
            col.wallEnabled = true;

            // Manual rotate
            if (!(targIsRay && rayman.strafing))
                SetOrbitRot(oAngleY - (rStick.x * 135), 1);


            // Lower auto orbit speed when in air for any actor with matching rule names
            if (targ.rule == "Air") {
                t_land_orbSpd.Abort(); orbSpd = 0.08f;
            }
            else if (targ.newRule) {
                t_land_orbSpd.Start(0.3f, () => orbSpd = defaultOrbitSpeed, false);
            }


            // Auto rotate while moving
            if (!((targIsRay && rayman.strafing) || targ.rule == "Climbing")
                && lStickPress && lStickAngle > -160 && lStickAngle < 160) {

                orbVel = Mathf.Lerp(orbVel, Mathf.Clamp(-lStickAngle * new Vector3(targ.apprVel.x, 0, targ.apprVel.z).magnitude
                    * orbSpd, -120, 120), 15 * dt);
            }
            else orbVel = Mathf.Lerp(orbVel, 0, 20 * dt);
            oAngleY += orbVel * dt;



            // ------- Orbit dist/pitch offsets -------


            // ---- For everyone but Rayman ---
            if (!targIsRay) {
                tY = 20;
                cen = targ.pos;
                SetOrbitOffset(defaultDist, defaultAngleX);
            }



            // --------- For Rayman -----------
            else {

                if (targ.rule == "Falling") {
                    tY = 0.1f;
                }
                else if (rayman.helic) {
                    if (rayman.hasSuperHelic)
                        SetOrbitOffset(8, 3, 2);
                    else SetOrbitOffset(11, 35, 2);
                }
                else if (targ.rule == "Sliding" || rayman.slideJump) {
                    SetOrbitOffset(9, 30 + -targ.apprVel.normalized.y, 1);
                    SetOrbitRot(90 + Mathf.Atan2(-targ.apprVel.z, targ.apprVel.x) * Mathf.Rad2Deg, targ.velXZ.magnitude);
                    tY = 3.5f;
                }
                else if (targ.rule == "Ground") {
                    tY = 9;
                    xLook = 10;
                    if (targ.velXZ.magnitude < targ.moveSpeed / 2)
                        SetOrbitOffset(9, 25, 2);
                    else SetOrbitOffset(9.5f, 25 + steepOff, 3);
                }
                else if (targ.rule == "Air") {
                    if (rayman.jumping && targ.velY > 0) {
                        tY = 1.5f;
                        SetOrbitOffset(9, 15, 2);

                    }
                    else if (targ.col.groundFar.hit.distance > 4) {
                        t_tY.Start(0.3f, () => tY = 3);
                    }
                }
                else if (targ.rule == "Hanging") {
                    SetOrbitOffset(9.5f, 0, 2);
                    tY = 1;
                }
                else if (targ.rule == "Swinging") {
                    SetOrbitOffset(14, 20, 2);
                    SetOrbitRot(targ.oAngleY, 6);
                    tY = 4;
                }

                if (targ.rule == "Hanging")
                    xLook = Mathf.Lerp(xLook, -5, 5 * dt);
                else if (targ.velY > 0)
                    xLook = Mathf.Lerp(xLook, -2, 8 * dt);
                else xLook = Mathf.Lerp(xLook, 8, 8 * dt);


                if (targ.col.groundFar.AnyGround && targ.col.groundFar.hit.distance < 2.5f) {
                    cen = targ.col.groundFar.hit.point;
                }
                else
                    cen = targ.pos;

                if (rayman.strafing) {
                    SetOrbitOffset(6.5f, 30, 50);
                    SetOrbitRot(targ.rot.eulerAngles.y + 180, 20);
                    xLook = 13;
                }
            }

            // Huge low-ceiling override
            var lowCeil = targ.Raycast(Vector3.up * col.top, Vector3.up, 5);
            if (lowCeil.Any)
                SetOrbitOffset(6, lowCeil.hit.distance * 5  + steepOff, 2);


            // Transform
            oT_v = tY;
            oT_h = 8;
            oTarget = cen;
            LookAtY(targ.pos, 0);
            LookAtX(targ.pos, xLook, 8);
            Orbit();

            /*
            // Ensure line of sight
            var los = targ.Raycast(pos - targ.center, DistTo(targ.center));
            if (los.Any)
                pos = los.hit.point + los.hit.normal * col.radius;*/
        }
    }
}