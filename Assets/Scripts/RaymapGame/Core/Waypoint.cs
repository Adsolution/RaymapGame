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

        void FixedUpdate() {
            pos = transform.position;
        }

        public void Init() {

            pos = transform.position;
            if (transform.childCount > 1)
                radius = transform.GetChild(0).localScale.x / 2;

            all.Add(this);

            var bh = GetComponent<WayPointBehaviour>();
            if (bh == null) return;
            var graphs = bh.graphs;
            if (graphs.Count == 0) {
                return;
            }

            graph = graphs[0].GetComponent<WaypointGraph>();
            if (graph == null) return;

            index = graph.waypoints.IndexOf(this);

            if (index - 1 >= 0)
                prev = graph.waypoints[index - 1];

            if (index + 1 < graph.waypoints.Count)
                next = graph.waypoints[index + 1];

            if (bh.Targets.Count != 0)
                nextR2 = bh.Targets[0].GetComponent<Waypoint>();
        }
    }
}
