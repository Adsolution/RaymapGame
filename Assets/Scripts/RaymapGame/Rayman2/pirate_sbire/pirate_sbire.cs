//================================
//  By: Shrooblord & Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Henchman 800
    /// </summary>
    public partial class pirate_sbire : PersoController {
        public int shootClipSize = 4;
        public float shootDelay = 0.4f;
        public float shootReloadTime = 1;
        int shotNo;

        public void PirateShoot() {
            if (Timers("Shoot").active) return;

            Shoot(typeof(Alw_Projectile_Rayman_Model), 20, rayman.center);
            anim.Set(0);
            anim.Set(Anim.Shoot);
            Timers("ShootAnim").Start(0.3f, () => anim.Set(Anim.Aim));

            if (++shotNo < shootClipSize)
                Timers("Shoot").Start(shootDelay);
            else {
                Timers("Shoot").Start(shootReloadTime);
                shotNo = 0;
            }
        }

        public void WakeUp() {
            if (newRule) EnterCombat();
            anim.Set(Anim.WakeUp);
            SetRule("");
            Timers("WakeUp").Start(0.9f, () => SetRule("Ground"));
        }






        protected void Rule_Dead() {
            if (newRule) {
                anim.Set(Anim.Die);
                Timers("Die").Start(0.55f, () => SetVisibility(false));
            }
        }

        protected void Rule_Shooting() {
            if (newRule)
                anim.Set(Anim.AimDraw);
            LookAt2D(rayman.pos, 10);
            PirateShoot();
        }


        protected void Rule_Sleeping() {
            if (newRule)
                anim.Set(Anim.Sleeping);

            if (rayman.velXZ.magnitude > 0.3f && DistTo(rayman) < 15)
                WakeUp();
        }

        protected void Rule_Ground() {
            if (newRule)
                anim.Set(Anim.Idle);
            SetRule("Shooting");
        }





        protected override void OnUpdate() {
            if (rule == "Dead") return;
            col.top = rule == "Sleeping" ? 1.75f : 3;
            ReceiveProjectiles(1.5f);
        }


        protected override void OnHit() {
            if (rule == "Sleeping")
                WakeUp();
            else {
                Timers("ShootAnim").Abort();
                Timers("Shoot").Start(1);
                anim.Set(Anim.Hit);
                Timers("DrawAnim").Start(0.6f, () => anim.Set(Anim.AimDraw));
            }
        }


        protected override void OnDeath() {
            Timers("DrawAnim").Abort();
            Timers("ShootAnim").Abort();
            SetRule("Dead");
        }



        public static class Anim {
            public const int
                Idle = 0,
                Run = 2,
                AimDraw = 8,
                Aim = 7,
                Shoot = 3,
                Hit = 9,
                Die = 45,
                Dead = 33,

                Sleeping = 48,
                WakeUp = 49,


                BarrelGrab = 35,
                BarrelTakeAim = 36,
                BarrelAim = 41,
                BarrelThrowStart = 40,
                BarrelThrowEnd = 37,
                BarrelJuggle = 38;
        }

        public override AnimSFX[] animSfx => new AnimSFX[] {
            //running animation footstep plants
            new AnimSFX(2, new SFXPlayer.Info {
                path = "Rayman2/Henchman/Footstep/Walk",
                space = SFXPlayer.Space.Point,          //make the sound originate from specifically the Henchman
                volume = 0.60f,
            }, 1, 10),

            //wake up in surprise
            //surprise sound
            new AnimSFX(49, new SFXPlayer.Info {
                path = "Rayman2/Henchman/General/pimoteur",
                space = SFXPlayer.Space.Point,
            }, 1),
            //heavy foot plant sound
            new AnimSFX(49, new SFXPlayer.Info {
                path = "Rayman2/Henchman/Footstep/Land/",
                space = SFXPlayer.Space.Point,
            }, 16),

            //swivel head in surprise
            new AnimSFX(6, new SFXPlayer.Info {
                path = "Rayman2/Henchman/General/surpris",
                space = SFXPlayer.Space.Point,
            }, 1),

            //Drilling
            //Submerging
            new AnimSFX(39, new SFXPlayer.Info {
                path = "Rayman2/Henchman/Weapon/Drill/ELEC6",
                space = SFXPlayer.Space.Point,
            }, 17),

            //Emerging
            //drill
            new AnimSFX(23, new SFXPlayer.Info {
                path = "Rayman2/Henchman/Weapon/Drill/ELEC5",
                space = SFXPlayer.Space.Point,
            }, 1),

            //heavy foot plant sound
            new AnimSFX(23, new SFXPlayer.Info {
                path = "Rayman2/Henchman/Footstep/Land/",
                space = SFXPlayer.Space.Point,
            }, 21),
        };
    }
}