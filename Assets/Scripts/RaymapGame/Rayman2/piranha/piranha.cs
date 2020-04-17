//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Piranha
    /// </summary>
    public partial class piranha : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
			public const int
                ChompOnce = 1,
				Aaah = 3,
                Swim = 4,
                Die = 5,
                ChompLoop = 10;
		}
    }
}