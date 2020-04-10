//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Base for cages, a gate, and barrel dispensers.
    /// </summary>
    public partial class Cage : PersoController {
        public override bool resetOnRayDeath => false;


        protected void DumpBarrelAnim() {
            anim.Set(0);
            anim.Set(Anim.BarrelDispenserFlap);
            Timers("Flap Stop").Start(2, () => anim.Set(Anim.BarrelDispenserIdle));
        }


        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
            public const int
                CageHanging = 0,
                CageHangingSwing = 10,
                CageHangingHit = 9,
                CageHangingBreak = 1,
                CageHangingBroken = 2,
                CageGround = 6,
                CageGroundHit = 8,
                CageGroundBreak = 7,

                Gate1 = 3,

                BarrelDispenserIdle = 4,
                BarrelDispenserFlap = 5;
		}
    }
}