//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Camera
    /// </summary>
    public partial class Camera : PersoController {
        public override bool isAlways => true;
        public UnityEngine.Camera cam => UnityEngine.Camera.main;
        public override bool resetOnRayDeath => false;
    }
}