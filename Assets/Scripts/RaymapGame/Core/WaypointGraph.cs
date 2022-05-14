using UnityEngine;
using System.Collections.Generic;

namespace RaymapGame {
    public class WaypointGraph : MonoBehaviour {
        public static List<WaypointGraph> all = new List<WaypointGraph>();

        public void Init() {
            all.Add(this);
            GraphBehaviour gbh = GetComponent<GraphBehaviour>();
            foreach (var node in gbh.nodes) {
                waypoints.Add(node.GetComponent<Waypoint>());
            }
        }

        public List<Waypoint> waypoints = new List<Waypoint>();
    }
}