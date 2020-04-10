//================================
//  By: Adsolution
//================================
using System.Linq;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Purple Lum
    /// </summary>
    public partial class BNT_grappinable : Lums {
        public float grabDist;
        public WaypointGraph flyGraph;

        protected override void OnStart() {
            grabDist = GetDsgVar<float>("Float_6");

            maxHitPoints = float.PositiveInfinity;
            HealFull();

            anim.Set(Anim.Purple);
            if (flyGraph != null)
                SetRule("Nav");
        }

        protected override void OnUpdate() {
            var prj = ReceiveProjectiles();
            if (prj?.creator != null) {
                prj.creator.SetRule("Swinging", this, 10);
            }
        }

        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Nav");
        }


        void Rule_Nav() {
            if (NavWaypointGraph(flyGraph = GetDsgVar<WaypointGraph>("Graph_3"), true))
                SetRule("");
        }
    }
}