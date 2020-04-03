//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Projectiles base
    /// </summary>
    public partial class projectiles : PersoController {
        public override bool resetOnRayDeath => false;

        public float radius;
        public float damage = 10;

        protected override void OnStart() {
            radius = 1;//GetCollisionSphere(OpenSpace.Collide.CollideType.ZDM).radius;
            SetFriction(0, 0);
        }
    }
}