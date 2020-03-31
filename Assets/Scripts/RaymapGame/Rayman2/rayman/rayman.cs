//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rayman player
    /// </summary>
    public partial class rayman : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
            new AnimSFX(Anim.Run, "Rayman2/Rayman/footsteps/grass", 11, 22),

            new AnimSFX(Anim.LandRun, "Rayman2/Rayman/roll", SFXPlayer.Polyphony.Mono),
            new AnimSFX(Anim.LandRun, "Rayman2/Rayman/footsteps/grass", 40, 51),

            new AnimSFX(Anim.JumpIdleStart, new SFXPlayer.Info { path = "Rayman2/Rayman/jump", volume = 0.75f }),
            new AnimSFX(Anim.JumpRunStart, new SFXPlayer.Info { path = "Rayman2/Rayman/jump", volume = 0.75f }),
            new AnimSFX(Anim.FallRunStart, "Rayman2/Rayman/flip", SFXPlayer.Polyphony.Mono),

            new AnimSFX(Anim.HelicEnable, "Rayman2/Rayman/helic", SFXPlayer.Polyphony.Loop),
            new AnimSFX(Anim.HelicIdle, "Rayman2/Rayman/helic", SFXPlayer.Polyphony.Loop),
            new AnimSFX(Anim.HelicDisable, "Rayman2/Rayman/helicstop"),
            new AnimSFX(Anim.HelicLandIdle, "Rayman2/Rayman/helicstop"),
            new AnimSFX(Anim.HelicLandWalk, "Rayman2/Rayman/helicstop"),
            new AnimSFX(Anim.HelicLandWalk, "Rayman2/Rayman/footsteps/grass", 10),
            new AnimSFX(Anim.RunStart, "Rayman2/Rayman/footsteps/grass", 30),

            new AnimSFX(Anim.Despawn, new SFXPlayer.Info { path = "Rayman2/Rayman/despawn", volume = 0.6f }),
            new AnimSFX(Anim.Respawn, new SFXPlayer.Info { path = "Rayman2/Rayman/respawn", volume = 0.85f }),
        };

        public static class Anim {
            public const int
    None = -1,
    Idle = 0,
    Tiptoe = 177,
    Walk = 1,
    Jog = 178,
    Run = 2,
    RunStart = 179,
    RunStop = 180,
    LandIdle = 3,
    LandWalk = 93,
    LandRun = 282,

    JumpIdleStart = 4,
    FallIdleStart = 6,
    JumpIdleLoop = 7,
    FallIdleLoop = 8,

    JumpRunStart = 89,
    FallRunStart = 90,
    JumpRunLoop = 91,
    FallRunLoop = 92,

                JumpBackStart = 112,
                JumpBackLoop = 111,
                FallBackStart = 109,
                FallBack = 110,

                CreviceClimbIdle = 119,
                CreviceClimbFwd = 119,
                CreviceClimbBack = 119,

    Despawn = 161,
    Respawn = 162,
    HelicEnable = 22,
    HelicIdle = 23,
    HelicDisable = 24,

    //Joy
    YeahRayman = 182,
    Yahoo = 183,    //186 dupe
    Yeah = 185,

    //Hurt
    GreatShock = 138,
    PainPiroutte = 144,
    HurtRecoil = 145,
    PancakeStart = 169,
    Pancake = 168,
    PancakeRebound = 146,
    HurtFlyingStart = 150,
    HurtFlying = 149,
    HurtHeadache = 153,
    HurtHanging = 166,

    //**SHOOTING**//
    ShootLoopChuckAndJump = 193, //-->194
    ShootLoopChuckAndJumpFollowThrough = 194, //-->192
    ShootLoopApex = 189, //--> 193
    ShootLoopBackUpTop = 192, //-->189

    ChargeStart = 190,
    ChargeHold = 191,


    //STRAFING
    //216
    //...
    //259
    //278 charge while strafing backwards loop
    //279 charge strafe right
    //280 charge strafe left
    //281 charge strafe backwards
    //STRAFING END


    AirShootLoopRecharge = 212,
    AirShootLoopApex = 215,
    AirShootLoopRelease = 213,
    AirShootLoopFollowThrough = 214,

    HelicShootWindUp = 26,
    HelicShootChargeWindUp = 204,
    HelicShootCharge = 201,
    HelicShootChargeHold = 202,
    HelicShootRecharge = 203,
    HelicShoot = 25,

    HangingShootWindup = 195,
    HangingShootCharge = 196,
    HangingShoot = 197,
    HangingShootFollowThrough = 198,
    HangingShootRecharge = 199,
    HangingShootAnticipation = 200,

    SwimShootWindup = 117,
    SwimShoot = 116,
    SwimShootWindDown = 210,
    SwimShootWindDown2 = 211,
    SwimShootLoopStart = 206,
    SwimShootLoopApex = 208,
    SwimShootLoopRelease = 207,
    SwimShootLoopFollowThrough = 205,
    SwimShootLoopRecharge = 209,

    StrafeEndShootWindUp = 128,
    StrafeEndShoot = 129,

    //264-277   DUPES??
    //290-293   DUPES??

    //Teensie Dance
    Prisyadka = 184,

    //Basketball Idle
    BasketballStart = 305,
    BasketballDribbleLeft = 297,
    BasketballPassLeftToRight = 295,
    BasketballDribbleRight = 305,

    BasketballJuggleLeftRigthStart = 307, //starts from the left side
    BasketballJuggleLeftRight = 299,

    BasketballSpinStart = 296,
    BasketballSpin = 298,
    BasketballSpinStop = 304,

    BasketballJuggleRightToLeft = 300,
    BaskeballJuggleBackForthStart = 303,
    BasketballJuggleBackForthTransition = 301,
    BasketballJuggleBackForthLoop = 302,

    BasketballStop = 308,

    //Philosophy
    RethinkLifeDecisions = 309,
    PsyduckCosplay = 310,
    Eureka = 311,

    //Strafing//
    StrafeRight = 58,

    StrafeLeft = 60,
    StrafeEndSides = 59,

    StrafeStartForward = 65, //66 is a dupe?
    StrafeForward = 61,
    StrafeEndForward = 63,

    StrafeBackward = 62,
    StrafeEndBackward = 64,

    StrafeTurnRight = 70,
    StrafeTurnLeft = 71,

    //Helico
    HelicLandIdle = 139,
    HelicLandWalk = 140,

    ClimbWallStart = 45,
    ClimbWallIdle = 32,

    ClimbWallUp = 33,
    ClimbWallDown = 34,
    ClimbWallLeft = 35,
    ClimbWallRight = 36,

    ClimbWallUpEnd = 37,
    ClimbWallDownEnd = 38,
    ClimbWallLeftEnd = 39,
    ClimbWallRightEnd = 40,

    ClimbWallUpStart = 41,
    ClimbWallDownStart = 42,
    ClimbWallLeftStart = 43,
    ClimbWallRightStart = 44,

                HangMove = 10,
                HangIdle = 11,
                HangMoveStop = 14,


    DeathFall = 141,

    RunToSlide1 = 96,
    SlideToRun = 94,
    SlideToIdle = 97,
    SlideFwd = 95,
    SlideLeft = 171,
    SlideRight = 172,
    SlideMidToLeft = 173,
    SlideMidToRight = 174,
    SlideRightToMid = 175,
    SlideLeftToMid = 176,

    SwimEnter = 76,
    SwimIdle = 52,
    SwimStartMove = 51,
    SwimMove = 53,
    SwimStopMove = 48,

    SwimLookFwdToLeft = 55,
    SwimLookLeft = 57,
    SwimLookLeftToFwd = 49,

    //SwimLookFwdToRight,
    SwimLookRight = 54,
    SwimLookRightToFwd = 50,

            SwingBack = 72,
                SwingFwd = 74,

                CarryPickUp = 151,
                CarryCatch = 152,
                CarryIdle = 99,
                CarryWalk = 100,
                CarryThrow = 101,
                CarryThrowUp = 107,
                CarryFlyStart = 106,
                CarryFly = 102,
                CarryFlyDrop = 108,
                CarryWalkStop = 103,
                CarryWalkStart = 104;


        }

    }
}