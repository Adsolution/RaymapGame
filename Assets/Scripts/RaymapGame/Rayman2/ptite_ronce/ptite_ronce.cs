//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Nettle (thorn tentacle enemy)
    /// </summary>
    public partial class ptite_ronce : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
			public const int
				Retracted = 0,
				Breakout = 1,
                Lashing = 2,
                Retract = 3;
		}
    }
}