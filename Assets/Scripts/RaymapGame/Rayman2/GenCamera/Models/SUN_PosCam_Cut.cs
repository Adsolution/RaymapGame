//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Locked camera position (instant cut)
    /// </summary>
    public partial class SUN_PosCam_Cut : GenCamera {
        public override void OnEnter() {
            cam.SetRule("PosCam", pos, -1);
        }
    }
}