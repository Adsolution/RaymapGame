//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Base for all Lums
    /// </summary>
    public partial class Lums : PersoController {
        public override float activeRadius => 30;
        public override bool resetOnRayDeath => false;
        public static class Anim {
            public const int
                Purple = 1,
                Red = 2,
                Blue = 3,
                Yellow = 4,
                SuperYellow = 13,
                Green = 14,
                Bubble = 6;
		}
    }
}