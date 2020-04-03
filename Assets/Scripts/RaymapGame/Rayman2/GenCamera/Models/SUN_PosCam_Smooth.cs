//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Locked camera position (smooth transition)
    /// </summary>
    public partial class SUN_PosCam_Smooth : GenCamera {
        public float speed;
        protected override void OnStart() {
            base.OnStart();
            speed = GetDsgVar<float>("Float_3") / 10;
        }
    }
}