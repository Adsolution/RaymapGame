//================================
//  By: Adsolution
//================================

using OpenSpace.Collide;
using System.Linq;
using UnityEngine;
using static RaymapGame.InputEx;
using static UnityEngine.Input;

namespace RaymapGame.Rayman2.Persos {
    public partial class YLT_RaymanModel : rayman {
        public override bool isAlways => true;
        public override bool resetOnRayDeath => false;

        public bool jumping;
        public bool selfJump;
        public bool slideJump;
        public float groundDepth = 0.5f;
        float jumpCutoff;
        float jumpLiftOffVelY;
        float liftOffVel;

        public bool strafing;
        public bool helic;
        public bool hasSuperHelic;
        bool superHelicAscend;
        float superHelicRev;

        CollideInfo colClimb;

        Timer t_runStart = new Timer();
        Timer t_respawn = new Timer();
        Timer t_blockHang = new Timer();

        protected override void OnDebug() {
            DebugLabel("Helic Active", helic);
            DebugLabel("Has Super Helic", hasSuperHelic);
            if (GetPerso<StdCam>() != null) {
                DebugLabel("Cam rule", GetPerso<StdCam>().rule);
                if (GenCamera.curr != null)
                    DebugLabel("GenCamera", GenCamera.curr.persoName);
            }
        }

        public override float activeRadius => 100000;

        protected override void OnStart() {
            maxHitPoints = 100;
            hitPoints = 50;
            waypointLenience = 3;
            navRotSpeed = 10;

            //projectileType = typeof(Alw_Projectile_Rayman_Model);
            SetShadow(true);
            SetRule("Air");
        }

        protected override void OnDeath() {
            // Temporary
            Despawn();
            HealFull();
        }

        protected override void OnUpdate() {
            projectileType = System.Type.GetType($"{nameof(RaymapGame)}.{Main.gameName}.Persos.{Main.editor.projectileText.text}");
            col.wallEnabled = true;
            col.wallAngle = 0.707f;

            if (hasTarget && !strafing)
                target = null;

            col.ApplyZDRCollision();

            if (forceNav)
                forceNav = !NavNearestWaypointGraph();

            var dmg = ReceiveProjectiles();
            if (dmg != null) {
                velXZ = (center - dmg.center).normalized * 60;
                velY = 0;
            }

            // World collision triggers
            if (col.ground.DeathWarp ||
                col.ground.LavaDeathWarp ||
                col.ground.HurtTrigger)
                Despawn();

            else if (col.ground.FallTrigger)
                SetRule("Falling");

            else if (col.ground.Trampoline)
                Jump(16, true);

            else if (col.wall.ClimbableWall && velY <= 2)
                SetRule("Climbing");
        }



        protected override void OnInputMainActor() {
            if (forceNav) return;

            if (GetKeyDown(KeyCode.R))
                Despawn();

            if (iStrafeDown || iStrafeHold) {
                strafing = true;
            }

            if (iStrafeUp)
                strafing = false;

            if (iShootUp) {
                justShot = false;
            }

            switch (rule)
            {
                case "Ground":
                case "Riding":
                    if (iJumpDown)
                        Jump(4, false, true);
                    if (iShootDown && !lStickPress)
                        SetRule("Charging");
                    else if (iShootDown) {
                        RayShoot();
                    }
                    break;

                case "Climbing":
                    if (iJumpDown)
                        Jump(4, false, true);
                    break;

                case "Hanging":
                    if (iShootDown)
                        RayShoot();
                    if (iJumpDown) {
                        SetRule("Air");
                        t_blockHang.Start(0.35f);
                    }
                    break;

                case "Sliding":
                    if (iJumpDown)
                        Jump(4, false, true, true);
                    if (iShootDown)
                        RayShoot();
                    break;

                case "Swimming":
                    if (iJumpDown && col.atWaterSurface)
                        Jump(4, false); break;

                case "Swinging":
                    if (iJumpDown) {
                        vel = apprVel;
                        Jump(Mathf.Clamp(3 + apprVel.y * 0.35f, 3, 20), false, true);
                    } break;

                case "Mounted":
                    if (iShootDown) {
                        if (carryPerso != null)
                            RayThrowCarriedForward();
                        else RayShoot();
                    }
                    if (iJumpDown) {
                        if (carryPerso != null)
                            RayThrowCarriedUp();
                        else {
                            Timers("MountHyst").Start(0.5f);
                            SetMount(null);
                            Jump(4, false, true);
                        }
                    }
                    break;

                case "Carrying":
                    if (iShootDown)
                        RayThrowCarriedForward();
                    else if (iJumpDown)
                        RayThrowCarriedUp();
                    break;

                case "Air":
                    if (jumping && iJumpUp)
                        jumping = false;

                    if (iJumpDown && !(helic && hasSuperHelic) && !slideJump)
                        ToggleHelic();

                    superHelicAscend = hasSuperHelic && helic && iJumpHold;

                    if (iShootDown)
                        RayShoot();

                    // Moonjump
                    if (GetKeyDown(KeyCode.JoystickButton1) || GetKeyDown(KeyCode.S))
                        Jump(4, false);

                    break;
            }


            // Debug/Cheat stuff
            if (GetKeyDown(KeyCode.H))
                hasSuperHelic = !hasSuperHelic;

            if (GetKeyDown(KeyCode.T)) {
                var c = RayCollider.RaycastMouse();
                if (c.Any) pos = c.hit.point;
            }
        }




        //----------------------------------------
        //  Rayman Actions
        //----------------------------------------

        public void RespawnRay() {
            foreach (var perso in GetPersos<PersoController>().Where(p => p.resetOnRayDeath && !(p is YLT_RaymanModel)))
                perso.Restart();

            if (checkpoint == null) {
                pos = startPos + Vector3.up * 0.75f;
                rot = startRot;
            }
            else {
                pos = checkpoint.pos + Vector3.up * 0.75f;
                rot = checkpoint.rot;
            }
            velXZ = Vector3.zero;
            velY = 0;
            scale = 1;
            selfJump = false;
            helic = false;

            GetPerso<StdCam>().Orbit(pos, 6.5f, rot.eulerAngles.y + 180, 20);

            SetRule("Air");
            DisableForSeconds(1.8f);
            anim.Set(Anim.Respawn, 1);
        }
        public void Despawn(bool respawn = true) {
            anim.Set(Anim.Despawn, 2);
            if (respawn)
                t_respawn.Start(2, RespawnRay);
            DisableForSeconds(100000);
        }

        public void Jump(float height, bool forceMaxHeight, bool selfJump = false, bool slideJump = false) {
            this.selfJump = selfJump;
            this.slideJump = slideJump;
            jumping = true;
            helic = false;
            SetRule("Air");

            float am = Mathf.Sqrt((1920f / 97) * height);
            jumpLiftOffVelY = slideJump ? apprVel.y / 2 : 0;
            jumpCutoff = am * 0.5f + jumpLiftOffVelY;
            velY = am * 1.5f + jumpLiftOffVelY;

            if (velXZ.magnitude < moveSpeed / 2 || !selfJump)
                anim.Set(Anim.JumpIdleStart, 1);
            else
                anim.Set(Anim.JumpRunStart, 1);
        }

        public void ToggleHelic() {
            helic = !helic;
            if (!helic) anim.Set(Anim.HelicDisable, 1);
        }


        public void RayShoot() {
            projectileVel = 30;
            projectileType = typeof(Alw_Projectile_Rayman_Model);
            Shoot(hasTarget);

            if (mount is BNT_ThePrune)
                mount.GiveImpulse(-forward * 7);
        }

        bool forceNav;
        public void ForceNav() {
            forceNav = true;
        }


        public void DetectCarriable(bool setCarryRule = true) {
            if (vel.magnitude < 0.5f && !Timers("ThrowBlock").active)
                foreach (var p in GetPersos(typeof(PersoController)).
                    Where((x)=> x.carriable && x != mount && x != carryPerso && CheckCollisionZone(x, CollideType.ZDE))) {
                    
                    if (p.vel.magnitude < 0.5f)
                        Timers("CarryWait").Start(0.75f, () => {
                            RayCarryPerso(p, false);
                            if (setCarryRule) SetRule("Carrying");
                        }, false);
                    
                    else {
                        RayCarryPerso(p, true);
                        if (setCarryRule) SetRule("Carrying");
                    }
                }

            else Timers("CarryWait").Abort();
        }

        public void RayCarryPerso(PersoController p, bool caughtInAir) {
            if (caughtInAir) {
                if (p != null) CarryPerso(p, 14);
                anim.Set(Anim.CarryCatch);
                DisableForSeconds(0.3f);
            }
            else {
                if (p != null) Timers("Pickup").Start(0.25f, () => CarryPerso(p, 14));
                anim.Set(Anim.CarryPickUp);
                DisableForSeconds(0.5f);
            }
        }

        public void RayThrowCarriedForward() {
            PersoController targ = null;

            if (carryPerso is BNT_TonneauFusee)
                targ = FindTarget(30);

            else if (carryPerso is SUN_magic_key k) {
                var t = FindTarget<SUN_magie_simple>(30);
                if (t != null && t.color == k.color)
                    targ = t;
            }

            else if (carryPerso is BNT_ThePrune)
                targ = FindTarget<pirate_gorille>(30);

            anim.Set(Anim.CarryThrow, anim.currPriority, 45);
            Timers("Throw").Start(0.4f, () => ThrowCarriedFoward(targ));
            SetRule("Ground");
            DisableForSeconds(0.7f);
        }

        public void RayThrowCarriedUp() {
            anim.Set(Anim.CarryThrowUp);
            Timers("Throw").Start(0.3f, () => ThrowCarriedUp());
            SetRule("Ground");
            DisableForSeconds(0.7f);
        }
    }
}