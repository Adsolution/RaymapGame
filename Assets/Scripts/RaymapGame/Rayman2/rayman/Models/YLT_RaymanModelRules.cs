//================================
//  By: Adsolution
//================================

using UnityEngine;
using static RaymapGame.InputEx;
using OpenSpace.Collide;

namespace RaymapGame.Rayman2.Persos {
    public partial class YLT_RaymanModel {

        void Rule_Ground() {
            #region Rule
            col.groundDepth = groundDepth;
            col.UpdateGroundCollision();

            if (newRule && lStick.magnitude < deadZone)
                velXZ = Vector3.zero;

            slideJump = false;
            selfJump = false;

            if (col.ground.AnyGround && col.ground.hit.distance < 1.5f) {
                col.StickToGround();
            }
            else if (col.ground.Slide) {
                SetRule("Sliding"); return;
            }
            else if (col.water.Water && !col.waterIsShallow) {
                SetRule("Swimming"); return;
            }
            else {
                SetRule("Air"); return;
            }

            AlignY(10);
            SetFriction(30, 0);
            MoveOrStrafe(10);
            DetectCarriable();

            #endregion
            #region Animation
            if (!strafing) {
                if (velXZ.magnitude < 0.05f) {
                    t_runStart.Start(0.033f);
                    if (newRule && prevRule == "Air") {
                        if (helic)
                            anim.Set(Anim.HelicLandIdle, 1);
                        else
                            anim.Set(Anim.LandIdle, 1);
                    }
                    else {
                        anim.SetSpeed(5);
                        anim.Set(Anim.Idle, 0);
                    }
                    if (anim.currAnim == Anim.RunStop)
                        anim.SetSpeed(40);
                    else anim.SetSpeed(25);
                }
                else if (velXZ.magnitude < 5) {
                    if (newRule && prevRule == "Air") {
                        if (helic)
                            anim.Set(Anim.HelicLandWalk, 1);
                        else
                            anim.Set(Anim.LandWalk, 1);
                    }
                    else
                        anim.Set(Anim.Walk, 0);
                    float spd = velXZ.magnitude * moveSpeed * 1.5f;

                    if (anim.currAnim == Anim.HelicLandWalk)
                        anim.SetSpeed(spd / 2);
                    else
                        anim.SetSpeed(spd);
                }
                else {
                    if (newRule && prevRule == "Air") {
                        if (helic)
                            anim.Set(Anim.HelicLandWalk, 1);
                        else
                            anim.Set(Anim.LandRun, 1);
                    }
                    else {
                        if (anim.currAnim == Anim.RunStart)
                            anim.SetSpeed(200);
                        else if (anim.IsSet(Anim.SlideToRun))
                            anim.SetSpeed(30);
                        else {
                            anim.Set(Anim.Run, 0);
                            anim.SetSpeed(velXZ.magnitude * moveSpeed * 0.46f);
                        }
                    }

                    if (anim.currAnim == Anim.HelicLandWalk || anim.currAnim == Anim.LandRun)
                        anim.SetSpeed(60);
                }

                if (!forceNav) {
                    if ((anim.currAnim == Anim.RunStop || velXZ.magnitude < 0.05f) && lStick.magnitude >= deadZone) {
                        anim.Set(Anim.RunStart, 1);
                    }
                    else if (velXZ.magnitude > 5 && lStick.magnitude < deadZone) {
                        anim.Set(Anim.RunStop, 1);
                    }
                }
            }

            // Strafing
            else {
                if (velXZ.magnitude > 0.05f && lStick_s.magnitude > deadZone) {
                    float velAngle = Mathf.Atan2(velRel.z, velRel.x);

                    if (velAngle >= -45 && velAngle < 45)
                        anim.Set(Anim.StrafeForward);
                    else if (velAngle >= 45 && velAngle < 135)
                        anim.Set(Anim.StrafeRight);
                    else if (velAngle >= 135 && velAngle < -135)
                        anim.Set(Anim.StrafeBackward);
                    else if (velAngle >= -135 && velAngle < -45)
                        anim.Set(Anim.StrafeLeft);
                }
                else {
                    anim.Set(Anim.Idle);
                }
            }
            #endregion

            helic = false;
        }


        void Rule_Mounted() {
            vel = Vector3.zero;

            DetectCarriable(false);
            RotateToStick(4);

            if (mount == null)
                SetRule("Air");

            if (carryPerso == null)
                anim.Set(Anim.LandIdle);
            else anim.Set(Anim.CarryIdle);
        }


        void Rule_Carrying() {
            moveSpeed = 4;
            handChannelRot = new Vector3(90, 0, 0);
            InputMovement();
            RotateToStick(4);

            if (col.ground.AnyGround)
                col.StickToGround();
            else if (col.ground.None)
                SetRule("Air");


            if (velXZ.magnitude < 0.1f)
                anim.Set(Anim.CarryIdle);
            else {
                anim.Set(Anim.CarryWalkStart);
                anim.SetSpeed(velXZ.magnitude * 10);
            }
        }


        void Rule_BarrelFlight() {
            if (newRule)
                Timers("iBlock").Start(0.5f);

            if (Timers("iBlock").finished) {
                RotateLocalY(lStick_s.x * 45, 1);
                RotateLocalX(-lStick_s.y * 45, 1);
            }

            handChannel = 2;
            handChannelRot = new Vector3(90, 0, 0);
            navRotSpeed = 0;
            moveSpeed = 30;
            SetFriction(5, 5);
            NavForwards();
            anim.Set(Anim.CarryFlyStart);

            if (col.wall.Any) {
                carryPerso.Kill();
                HoldPerso(null);
                SetRule("Air");
                anim.Set(Anim.CarryFlyDrop);
            }
        }

        void MoveOrStrafe(float rot_t, float moveSpeed = -1) {
            if (moveSpeed != -1)
                this.moveSpeed = moveSpeed;

            if (!strafing) {
                if (moveSpeed == -1) this.moveSpeed = 10;
                FindTarget(30, 25);
                InputMovement();
                RotateToStick(rot_t);
            }

            else {
                if (moveSpeed == -1) this.moveSpeed = 6.5f;
                else this.moveSpeed = 6.5f;
                navRotSpeed = 0;
                InputMovement(true);

                FindTarget(30, 90);
                if (target != null) {
                    var targPos = target.GetCollisionSphere(CollideType.ZDE).position;
                    var vec = (targPos - center).normalized;
                    if (Mathf.Abs(vec.y) < 0.707f && !Raycast(centerRel, vec, Dist(center, targPos)).Any)
                        LookAt2D(targPos, 6);
                }

                RotateY(rStick_s.x * -90, 1);
            }
        }

        bool justShot;
        public void ShootMagicFist() {
            justShot = true;
            RayShoot();
            

            //if (shot is Alw_Projectile_Rayman_Model mShot)
              //  mShot.SetShooter(this);

            SetRule("Ground");
        }
        void SpawnChargeParticleFX() {
            //GetPerso<Alw_Projectile_Rayman_Model>().Clone(out RayMagicFistChargingFX shotFX, pos + Vector3.up, typeof(RayMagicFistChargingFX), true);
            //shotFX.pos = perso.channelObjects[2].transform.position + forward * 0.12f + right * 0.28f + upward * 0.15f;
            //shotFX.scale = chargeTime / 10;

            //add random rotation so it looks nice
            //...

            //shotFX.SetShooter(this);
        }

        float chargeTime;
        Timer chargingTimer = new Timer();
        void Rule_Charging() {
            #region Rule
            if (col.ground.AnyGround)
                col.StickToGround();

            if (strafing) moveSpeed = 7;
            else moveSpeed = 0; //not allowed to move unless strafing
            //InputMovement();
            RotateToStick(5);


            //you can't hold the magic fist charging forever (default R2 behaviour)
            if (!iShootHold || chargeTime > 4.0f) {
                ShootMagicFist();
                chargeTime = 0;
                //SetRule("Ground");
                return;
            }

            //while charging, keep spawning "fake" magic fist particle effects
            //chargingTimer.Set(0.04f, () => { SpawnChargeParticleFX(); }, false);

            chargeTime += dt;

            #endregion
            #region Animation
            anim.Set(191);
            #endregion
        }


        void Rule_Air()
        {
            #region Rule
            col.groundDepth = 0;
            col.UpdateGroundCollision();

            if (newRule)
                liftOffVel = velXZ.magnitude;

            if (col.ground.AnyGround && velY <= 0)
            {
                velY = 0;
                SetRule("Ground");
                return;
            }
            else if (col.ground.Slide)
            {
                SetRule("Sliding");
                return;
            }
            else if (col.ground.Water && velY < 0 && !col.waterIsShallow)
            {
                //SetRule("Swimming");
                //return;
            }
            else if (!t_blockHang.active && col.ceiling.HangableCeiling) {
                SetRule("Hanging");
                return;
            }

            if (jumping)
            {
                gravity = -20;
                if (velY < jumpCutoff)
                    jumping = false;
            }
            else
            {
                gravity = -30;
            }

            ApplyGravity();

            if (helic)
            {
                if (superHelicAscend)
                    superHelicRev = Mathf.Lerp(superHelicRev, 42, dt * 45);
                else superHelicRev = Mathf.Lerp(superHelicRev, 0, dt * 1);

                //GetSFXLayer(Anim.YLT_RaymanModel.HelicIdle).player.asrc.pitch = 1 + superHelicRev / 300;

                SetFriction(6.5f, hasSuperHelic ? 2.6f : 7.5f);
                velY += dt * superHelicRev;
                velY = Mathf.Clamp(velY, hasSuperHelic ? -25 : -5, 5);
                selfJump = false;
            }
            else
            {
                if (slideJump)
                    SetFriction(0.1f, 0);
                else SetFriction(5, 0);
                moveSpeed = 10;
            }


            MoveOrStrafe(6, helic ? 5 : -1);
            AlignY(10);

            if (pos.y < startPos.y - 1100)
                SetRule("Falling");
            #endregion
            #region Animation
            if (helic)
            {
                anim.Set(Anim.HelicEnable, 1);
            }
            else if (liftOffVel < 5 || !selfJump)
            {
                if (velY > 5 + jumpLiftOffVelY)
                {
                    anim.Set(Anim.JumpIdleLoop, 0);
                }
                else
                {
                    if (newRule)
                        anim.Set(Anim.FallIdleLoop, 0);
                    else
                        anim.Set(Anim.FallIdleStart, 1);
                }
            }
            else
            {
                if (velY > 5 + jumpLiftOffVelY)
                {
                    anim.Set(Anim.JumpRunLoop, 0);
                }
                else
                {
                    if (newRule)
                        anim.Set(Anim.FallRunLoop, 0);
                    else
                        anim.Set(Anim.FallRunStart, 1);
                }
            }
            #endregion
        }


        void Rule_Climbing()
        {
            #region Rule
            if (newRule)
            {
                velXZ = Vector3.zero;
                velY = 0;
                anim.Set(Anim.ClimbWallStart, 1);

                if (col.wall.hit.normal != Vector3.zero && Mathf.Abs(col.wall.hit.normal.y) < 0.707f)
                    pos = col.wall.hit.point + col.wall.hit.normal * 0.5f;
                colClimb = col.wall;
            }

            if ((colClimb = RayCollider.Raycast(pos + colClimb.hit.normal, -colClimb.hit.normal, 3)).ClimbableWall && Mathf.Abs(colClimb.hit.normal.y) < 0.707f)
            {
                rot = Matrix4x4.LookAt(pos, pos + colClimb.hit.normal, Vector3.up).rotation;
                pos = colClimb.hit.point + colClimb.hit.normal * 0.5f;
                if (lStick.magnitude > deadZone)
                    pos += Matrix4x4.Rotate(rot).MultiplyVector(new Vector2(-lStick_s.x, lStick_s.y)) * 6 * dt;
            }

            else if (apprVel.y > 2 && lStickAngle * Mathf.Sign(lStickAngle) < 30)
                Jump(4, false);

            col.wallEnabled = false;
            #endregion
            #region Animation
            float la = 0;
            if (lStick_s.magnitude > deadZone)
            {
                la = lStickAngle;
                anim.SetSpeed(lStick_s.magnitude * 35);
                if (la > -45 && la < 45)
                    anim.Set(Anim.ClimbWallUpStart, 1);
                else if (la >= 45 && la <= 135)
                    anim.Set(Anim.ClimbWallRightStart, 1);
                else if (la > 135 || la < -135)
                    anim.Set(Anim.ClimbWallDownStart, 1);
                else if (la >= -135 && la <= -45)
                    anim.Set(Anim.ClimbWallLeftStart, 1);
            }
            else
            {
                anim.SetSpeed(25);
                if (la > -45 && la < 45)
                    anim.Set(Anim.ClimbWallUpEnd, 1);
                else if (la > 45 && la < 135)
                    anim.Set(Anim.ClimbWallRightEnd, 1);
                else if (la > 135 || la < -135)
                    anim.Set(Anim.ClimbWallDownEnd, 1);
                else if (la > -135 && la < -45)
                    anim.Set(Anim.ClimbWallLeftEnd, 1);
            }
            #endregion
        }

        void Rule_Hanging() {
            var h = Raycast(Vector3.up * col.top, Vector3.up, 3);

            if (!h.HangableCeiling) {
                SetRule("Air");
                return;
            }

            if (newRule) {
                vel = Vector3.zero;
                SetFriction(15, 15);
                moveSpeed = 4;
                anim.Set(Anim.HangMoveStop);
            }
            pos.y = h.hit.point.y - 2.4f;
            RotateToStick(6);
            InputMovement();

            if (velXZ.magnitude > 0.5f) {
                anim.Set(Anim.HangMove);
                anim.SetSpeed(8 * moveSpeed);
            }
            else {
                anim.Set(Anim.HangMoveStop);
                anim.SetSpeed(25);
            }
        }

        Timer t_deathFall = new Timer();
        void Rule_Falling() {
            if (newRule) {
                t_deathFall.Start(2.5f, RespawnRay);
                scale = 1; 
            }
            scale -= dt / 2.5f;

            SetFriction(1, 2);
            ApplyGravity();

            anim.Set(Anim.DeathFall, 1);
        }


        void Rule_Sliding() {
            anim.SetSpeed(20);
            if (newRule)
            {
                anim.Set(Anim.RunToSlide1, 1);
                velXZ += Vector3.ClampMagnitude(col.ground.hit.normal * -velY, 20);
                velY = 0;
            }

            col.wallAngle = 0.5f;
            col.groundDepth = groundDepth;
            col.UpdateGroundCollision();

            moveSpeed = 15 + 10 * Mathf.Clamp(lStick_s.y, deadZone, 1);
            SetFriction(0.2f, 3 * -Mathf.Clamp(lStick_s.y, -1, -deadZone));


            if (col.ground.Slide)
            {
                velXZ += col.ground.hit.normal * moveSpeed * dt;
                /*velXZ = Vector3.ClampMagnitude(velXZ + Matrix4x4.Rotate(Quaternion.Euler(0,
                    rot.y + UnityEngine.Camera.main.transform.rotation.eulerAngles.y, 0)).MultiplyVector(Vector3.right)
                    * lStick_s.x * 15 * dt, velXZ.magnitude);*/
                InputMovement(true);
                InputMovement(true);
                InputMovement(true);

                col.StickToGround();
            }
            else if (col.ground.AnyGround) {
                SetRule("Ground");

                if (velXZ.magnitude > 15)
                    Jump(0.75f, true, false, true);
                else if (lStick_s.magnitude > deadZone)
                    anim.Set(Anim.SlideToRun, 2);
                else
                    anim.Set(Anim.SlideToIdle, 2);

                return;
            }
            else if (col.ground.None)
            {
                // watch out for sqrting negatives lol
                Jump(Mathf.Clamp(Mathf.Sqrt(apprVel.magnitude), 0.5f, 4), true, true, true);
                return;
            }

            FaceVel3D(true);
        }


        void Rule_Swimming()
        {
            if (col.atWaterSurface && col.ground.AnyGround)
            {
                SetRule("Ground");
                return;
            }

            if (newRule)
            {
                anim.Set(Anim.SwimEnter, 1);
                helic = false;
            }

            anim.SetSpeed(25);
            SetFriction(3, 7);
            moveSpeed = 7;

            col.ApplyWaterCollision(ref pos, ref velY);


            #region Correct hair for above/under water

            var ch = transform.Find("Channel 8");
            if (ch == null)
            {
                ch = transform.Find("Channel 0");
                if (ch != null) ch = ch.Find("Channel 8");
            }
            if (ch != null)
            {
                var surfHairLeft = ch.Find("Channel 12");
                var surfHairRight = ch.Find("Channel 13");
                var underHairLeft = ch.Find("Channel 5");
                var underHairRight = ch.Find("Channel 4");

                if (surfHairRight != null) surfHairRight.gameObject.SetActive(col.atWaterSurface);
                if (surfHairLeft != null) surfHairLeft.gameObject.SetActive(col.atWaterSurface);
                if (underHairRight != null) underHairRight.gameObject.SetActive(!col.atWaterSurface);
                if (underHairLeft != null) underHairLeft.gameObject.SetActive(!col.atWaterSurface);
            }
            #endregion


            if (lStick_s.magnitude > deadZone)
            {
                InputMovement();
                RotateToStick(2);
                anim.Set(Anim.SwimStartMove, 0);
                if (!anim.IsSet(Anim.SwimEnter))
                    anim.SetSpeed(lStick_s.magnitude * moveSpeed * 3);
            }
            else
            {
                anim.Set(Anim.SwimStopMove, 0);
                anim.SetSpeed(22);
            }
        }


        Vector3 sVec, sRot;
        float sY, sX, sVel, sDist, hiSpd, dir, prevDir;
        void Rule_Swinging(PersoController grap, float dist) {
            if (newRule) {
                helic = false;
                sDist = DistTo(grap);
                gravity = -30;
                sY = VecAngleY(grap.pos);
                sX = VecAngleX(grap.pos);
                sVel = -100 + velY * 6 + velXZ.magnitude * 5/* - (sDist - dist) * sDist * 0.2f*/;
                velY = 0;
            }

            if (apprVel.magnitude > hiSpd)
                hiSpd = apprVel.magnitude;

            float sin = Mathf.Sin(sX * Mathf.Deg2Rad);

            sVec = (grap.pos - pos).normalized;
            sVel += sin * dt * 100 * gravity / DistTo(grap);
            sX += sVel * dt;
            sY += lStick_s.x * dt * 60;

            prevDir = dir;
            dir = Mathf.Sign(sVel);

            if (dir != prevDir && Mathf.Abs(sX) < 90)
                Timers("SwingBoost").Start(0.55f, () => sVel += dir * dt * 100, null);
            if (Mathf.Abs(sVel) > 250)
                sVel /= 1f + 1 * dt;


            rot.eulerAngles = new Vector3(-sX, sY + 180, 0);

            Orbit(grap.pos, sDist = Mathf.Lerp(sDist, dist - 1, dt * 2), sY, sX - 90, 8, 8);

            anim.Set(sVel < 0 ? Anim.SwingFwd : Anim.SwingBack);
        }
    }
}