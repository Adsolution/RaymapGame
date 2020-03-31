//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Pirate gorilla
    /// </summary>
    public partial class pirate_gorille : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
            public const int
                Idle = 0,
                IdleEx = 6,
                Hobble = 1,
                Hit = 2,
                Run = 3,
                RunStop = 5,
                Fall = 4,
                Jump = 15,
                PlumHit = 27,
                PlumIdle = 16,
                PlumWalk = 17,
                Electrocute = 19;
        }
    }
}