//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Walking Shell
    /// </summary>
    public partial class obus : PersoController {
        public override AnimSFX[] animSfx => new AnimSFX[] {
			
		};

		public static class Anim {
            public const int
                Idle = 0,
                Sleep = 21,
                WakeUp = 10,
                RunStart = 8,
                RunLoop = 9,
                RunLoopB = 1;
        }
    }
}