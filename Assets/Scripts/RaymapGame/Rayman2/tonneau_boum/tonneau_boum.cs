//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rolling Barrel
    /// </summary>
    public partial class tonneau_boum : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			new AnimSFX(Anim.Rolling, new SFXPlayer.Info { 
                path = "Rayman2/TONROUL",
                polyphony = SFXPlayer.Polyphony.Loop,
                pointMaxRadius = 20,
            })
		};

		public static class Anim {
            public const int
                Rolling = 9;
		}
    }
}