//================================
//  By: Author
//================================
using OpenSpace.Collide;
using UnityEngine;
using System.Linq;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Description
    /// </summary>
    public partial class MIC_PiranhaSauteurVisible : piranha {
        public Waypoint wpCurr, wpStart, wp1, wp2;
        public float wait;
        public float height;

        protected override void OnStart() {
            wait = (float)GetDsgVar<int>("Int_1") / 1000;
            height = GetDsgVar<float>("Float_3");
            wpStart = GetDsgVar<Waypoint>("WayPoint_5");
            wp1 = GetDsgVar<Waypoint>("WayPoint_7");
            wp2 = GetDsgVar<Waypoint>("WayPoint_8");
            SetRule("Waiting");
        }

        protected override void OnDeath() {
            SetRule("Dead");
        }

        void Rule_Waiting() {
            if (newRule) {
                wpCurr = DistTo(wp1) < DistTo(wp2) ? wp1 : wp2;
                vel = apprVel;
            }

            navRotSpeed = 3;
            moveSpeed = 8;
            SetFriction(8, 6);
            NavTowards(wpCurr);

            if (!Timers("Wait").active) {
                SetRule("Leaping");
            }

            anim.Set(Anim.Swim);
        }

        Vector3 leapPos;
        void Rule_Leaping() {
            if (newRule) {
                leapPos = pos;
                Timers("Wait").Start(wait);
            }
            if (leapPos == Vector3.zero) return;

            ReceiveProjectiles();

            var wpTarg = wpCurr == wp1 ? wp2 : wp1;
            if (NavArcFromTo(leapPos, wpTarg, height, 0.5f + height * 0.125f))
                SetRule("Waiting");

            FaceVel2D(true);
            anim.Set(DistTo(wpCurr) < DistTo(wpTarg) ? Anim.Aaah : Anim.ChompOnce);
        }

        void Rule_Dead() {
            anim.Set(Anim.Die);
            vel = Vector3.zero;
            Timers("Death").Start(0.9f, SetNullPos, false);
        }
    }
}