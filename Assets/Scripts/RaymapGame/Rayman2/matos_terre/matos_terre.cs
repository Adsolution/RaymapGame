//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Sinking pillar platform
    /// </summary>
    public partial class matos_terre : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
            public const int
                Pillar = 10,
                LavaFall = 25,
                BreakableWallNormal = 19,
                BreakableWallDamage1 = 18,
                BreakableWallDamage2 = 16,
                BreakableWallBreak = 21;

        }
    }
}