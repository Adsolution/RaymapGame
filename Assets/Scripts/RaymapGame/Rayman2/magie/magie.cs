//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Magic Sphere Target
    /// </summary>
    public partial class magie : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
            public const int
                Off = 1,
                MagicStart = 2,
                MagicLoop = 0;
		}
    }
}