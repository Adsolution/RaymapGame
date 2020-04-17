//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Purple Lum
    /// </summary>
    public partial class BNT_grappinable : Lums {
        public float distMin;
        public float distMax;
        public WaypointGraph flyGraph;

        protected override void OnStart() {
            distMin = GetDsgVar<float>("Float_6");
            distMax = GetDsgVar<float>("Float_7");
            if (distMax < distMin) distMax = distMin;
            targetDist = GetDsgVar<float>("Float_11");

            maxHitPoints = float.PositiveInfinity;
            HealFull();

            anim.Set(Anim.Purple);
            if (flyGraph != null)
                SetRule("Nav");
        }

        protected override void OnUpdate() {
            var prj = ReceiveProjectiles();
            if (prj?.creator != null) {
                prj.creator.SetRule("Swinging", this, 2 + Mathf.Clamp(DistTo(rayman), distMin, distMax));
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