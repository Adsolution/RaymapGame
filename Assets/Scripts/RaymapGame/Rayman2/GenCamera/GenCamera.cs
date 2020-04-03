//================================
//  By: Adsolution
//================================
using OpenSpace.Collide;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Forced camera orientation
    /// </summary>
    public partial class GenCamera : PersoController {
        public static bool areasEnabled = true;
        public override bool isAlways => true;
        public static GenCamera curr;
    }
}