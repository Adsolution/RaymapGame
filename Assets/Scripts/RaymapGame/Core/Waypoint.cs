using UnityEngine;
using System.Collections.Generic;

namespace RaymapGame {
    public class Waypoint : MonoBehaviour {
        public WaypointGraph graph;
        public int index;
        public Waypoint prev, next, nextR2;
        public Vector3 pos;
        public float radius;

        public static List<Waypoint> all = new List<Waypoint>();

        public static Waypoint GetNearest(Vector3 pos) {
            Waypoint closest = null;
            float cdist = 1000000;
            foreach (var wp in all) {
                float dist = PersoController.Dist(pos, wp.pos);
                if (dist < cdist) {
                    cdist = dist;
                    closest = wp;
                }
            }
            return closest;
        }

        void Awake() {
            pos = transform.position;
            if (transform.childCount > 1)
                radius = transform.GetChild(0).localScale.x / 2;

            all.Add(this);
        }

        void FixedUpdate() {
            pos = transform.position;
        }

        void Start() {
            graph = GetComponentInParent<WaypointGraph>();
            if (graph == null) return;

            index = graph.waypoints.IndexOf(this);

            var bh = GetComponent<WayPointBehaviour>();

            if (index - 1 >= 0)
                prev = graph.waypoints[index - 1];

            if (index + 1 < graph.waypoints.Count)
                next = graph.waypoints[index + 1];

            if (bh.targets.Count != 0)
                nextR2 = bh.targets[0].GetComponent<Waypoint>();
        }
    }
}
