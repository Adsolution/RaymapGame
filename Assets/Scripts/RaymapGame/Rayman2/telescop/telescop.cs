//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Metal expanding bridge
    /// </summary>
    public partial class telescop : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
			public const int
				StateName1 = 0,
				StateName2 = 1; //...etc
		}
    }
}