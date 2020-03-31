using UnityEngine;
using System.Collections.Generic;

namespace RaymapGame {
    public class WaypointGraph : MonoBehaviour {
        public static List<WaypointGraph> all = new List<WaypointGraph>();
        void Awake() {
            all.Add(this);
        }

        public List<Waypoint> waypoints = new List<Waypoint>();
        void Start() {
            foreach (var wp in GetComponentsInChildren<WayPointBehaviour>()) {
                waypoints.Add(wp.gameObject.AddComponent<Waypoint>());
            }
        }
    }
}