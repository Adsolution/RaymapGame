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
            rotY = -GetDsgVar<float>("Float_1") + rot.eulerAngles.y + 180;
            rotX = GetDsgVar<float>("Float_2") + rot.eulerAngles.x;
            dist = GetDsgVar<float>("Float_3");
        }
        public override void OnEnter() {
            cam.SetRule("Oriente", rotY, rotX, dist, xAddDeg);
        }
    }
}