//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Locked camera position (smooth transition)
    /// </summary>
    public partial class SUN_PosCam_Smooth : GenCamera {
        public override void OnEnter() {
            cam.SetRule("PosCam", pos, 2);
        }
    }
}