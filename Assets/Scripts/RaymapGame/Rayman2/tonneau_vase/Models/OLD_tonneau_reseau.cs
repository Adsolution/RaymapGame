//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Floating Barrel
    /// </summary>
    public partial class OLD_tonneau_reseau : tonneau_vase {
        protected override void OnStart() {
            switch (Main.lvlName) {
                case "Morb_00":
                    anim.Set(Anim.BarrelTomb); break;
            }

            moveSpeed = GetDsgVar<float>("Float_3");
            gravity = -1;
            navRotSpeed = 0;
            SetFriction(1, 5);
            SetRule("Wait");
        }

        protected void Rule_Wait() {
            if (StoodOnBy(rayman))
                SetRule("Navigate");
        }

        protected void Rule_Navigate() {
            if (NavNearestWaypointGraph()) {
                SetRule("Sink");
                sinkY = pos.y;
            }
            var hit = Raycast(Vector3.down * 3, Vector3.up, 4);
            if (hit.HurtTrigger)
                pos.y = Mathf.Lerp(pos.y, hit.hit.point.y + 1.5f, dt * 2);
        }

        public virtual void OnSink() { }

        float sinkY;
        protected void Rule_Sink() {
            SetFriction(2, 3);
            if (pos.y - sinkY < -2) {
                Restart();
                OnSink();
            }
            else ApplyGravity();
        }
    }
}