//================================
//  By: Adsolution
//================================
using OpenSpace.Collide;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Flying chair
    /// </summary>
    public partial class BNT_SupportDuSiege : new_chaise_russe {
        public float speed;
        public WaypointGraph graph;
        protected override void OnStart() {
            moveSpeed = speed = GetDsgVar<float>("Float_2");
            SetFriction(20, 20);
            SetRule("Default");
        }
    }
}