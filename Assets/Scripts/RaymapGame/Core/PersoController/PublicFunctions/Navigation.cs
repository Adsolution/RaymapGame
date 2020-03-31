//================================
//  By: Adsolution
//================================
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenSpace.Collide;
using static RaymapGame.InputEx;
using System.Reflection;
using System.Linq;

namespace RaymapGame {
    public partial class PersoController {
        //========================================
        //  Navigation
        //========================================
        public void NavDirection(Vector3 dir, bool tank = false) {
            if (navRotSpeed > 0) FaceDir2D(dir, navRotSpeed);
            var vec = tank ? forward : dir.normalized;
            velXZ += vec * fricXZ * globalFricMult * moveSpeed * dt;
            velY += vec.y * fricY * globalFricMult * moveSpeed * dt;
        }
        public void NavDirection2D(Vector3 dir, bool tank = true)
            => NavDirection(new Vector3(dir.x, 0, dir.z), tank);
        public void NavDirectionRel(Vector3 dir, bool tank = false)
            => NavDirection(Matrix4x4.Rotate(rot).MultiplyVector(new Vector3(-dir.x, dir.y, -dir.z)), tank);
        public void NavDirectionCam(Vector3 dir, bool tank = true)
            => NavDirection(Matrix4x4.Rotate(Camera.main.transform.rotation).MultiplyVector(dir));
        public bool NavTowards(Target target, bool tank = true, bool arriveAccurate = false) {
            var vec = target - pos;
            if (!arriveAccurate || vec.magnitude > moveSpeed * 0.5f / fricMag)
                NavDirection(vec, tank);
            return vec.magnitude < 0.25f;
        }
        public bool NavTowards2D(Target target, bool tank = true, bool arriveAccurate = false)
            => NavTowards(new Vector3(target.x, pos.y, target.z), tank, arriveAccurate);
        public void NavForwards()
            => NavTowards(pos + forward);


        public bool NavArcFromTo(Target from, Target to, float height, float time) {
            var vec = to.pos - from.pos;
            pos += vec * dt / time;
            float prog = DistTo2D(from) / vec.magnitude;

            pos.y = Mathf.Lerp(from.y, to.y, prog) + height - height * (prog < 0.5f
                ? Mathf.Pow(1 + -Mathf.Abs(2 * prog), 2)
                : Mathf.Pow(1 + -Mathf.Abs(2 * -prog + 2), 2));

            if (DistTo(to) < 1)
                return true;
            return false;
        }



        // Input navigation
        public void RotateToStick(float t = 10) {
            if (lStickPress)
                SetRotY(lStickAngleCam, t * Mathf.Clamp(lStick_s.sqrMagnitude, 0.2f, 50) * 2);
        }
        public void InputMovement(bool strafe = false, float factor = 1) {
            if (!lStickPress) return;
            float hold = moveSpeed;
            moveSpeed *= Mathf.Clamp(lStick_s.magnitude, 0, 1) * factor;
            if (strafe)
                NavDirection(lStickCam_s, false);
            else NavForwards();
            moveSpeed = hold;
        }



        // Waypoint navigation
        public Waypoint GetNearestWaypoint()
            => Waypoint.GetNearest(pos);
        public bool InWaypointRadius(Waypoint wp) {
            if (wp.radius > 0)
                return DistTo(wp.pos) < wp.radius + waypointLenience;
            return DistTo(wp.pos) < (moveSpeed / 2) / fricXZ + waypointLenience;
        }
        public void NavToWaypoint(Waypoint wp) {
            NavTowards(wp.pos, false);
            waypoint = wp;
        }

        Waypoint wp { get => waypoint; set { waypoint = value; } }
        Waypoint wpNext(bool r2) => r2 ? wp.nextR2 : wp.next;
        public bool NavNearestWaypointGraph(bool r2 = false)
            => NavWaypointGraph(wp == null ? GetNearestWaypoint()?.graph : wp?.graph);
        public bool NavWaypointGraph(WaypointGraph graph, bool r2 = false) {
            if (graph == null) return true;
            if (wp == null) {
                if ((wp = GetNearestWaypoint()) == null)
                    return false;
                if (wp.next != null && DistTo(wpNext(r2).pos) < Vector3.Distance(wp.pos, wpNext(r2).pos))
                    wp = wp.next;
            }
            if (InWaypointRadius(wp))
                wp = wpNext(r2);
            if (wp == null)
                return true;

            NavToWaypoint(wp);
            return false;
        }

    }
}