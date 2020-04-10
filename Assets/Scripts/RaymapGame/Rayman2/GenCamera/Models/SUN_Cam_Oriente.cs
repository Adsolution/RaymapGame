//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Forced camera orientation
    /// </summary>
    public partial class SUN_Cam_Oriente : GenCamera {
        public float rotY, rotX, dist, xAddDeg;
        protected override void OnStart() {
            base.OnStart();
            rotY = -GetDsgVar<float>("Float_1") + rot.y;
            rotX = GetDsgVar<float>("Float_2") + rot.x;
            dist = GetDsgVar<float>("Float_3");
        }
    }
}