//================================
//  By: Adsolution
//================================
using OpenSpace.Collide;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Flying chair (for gorilla)
    /// </summary>
    public partial class YLT_SiegeRusse : new_chaise_russe {
        public WaypointGraph[] graph;

        protected override void OnStart() {
            moveSpeed = 20;
            SetFriction(20, 20);
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (CheckCollisionZone(rayman, CollideType.ZDM)) {
                //SetMainActor();
                SetRule("Moving");
            }
        }

        void Rule_Moving() {
            if (newRule)
                graph = FindObjectsOfType<WaypointGraph>();
            if (graph.Length == 0) return;

            NavWaypointGraph(graph[0]);
            FaceVel(true, 6);
        }
    }
}