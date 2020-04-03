//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Nettle (thorn tentacle)
    /// </summary>
    public partial class tterre_morceaux : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
			public const int
				Unbroken = 1,
				Break = 3,
                Broken = 2;
		}
    }
}