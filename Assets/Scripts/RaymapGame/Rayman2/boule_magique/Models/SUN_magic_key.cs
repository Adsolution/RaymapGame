//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Magic Sphere
    /// </summary>
    public partial class SUN_magic_key : boule_magique {
        public enum OrbColor : byte { Orange = 0, Blue = 2 }
        public OrbColor color;
        public override bool carriable => true;
        public bool placed;
        protected override void OnStart() {
            color = (OrbColor)GetDsgVar<byte>("UByte_1");
            col.groundDepth = 0;
            SetShadow(true);
            shadow.size = 1;
            col.wallEnabled = true;
            col.groundLevel = -0.65f;
            gravity = -30;
            SetRule("Ground");
        }
        protected override void OnThrown() {
            SetRule("Air");
        }

        void Rule_Air() {
            if (placed) {
                vel = Vector3.zero;
                return;
            }

            SetFriction(0, 0);
            ApplyGravity();

            if (col.wall.AnyWall)
                BounceXZ(col.wall.hit.normal, 0.6f);
            if (col.ground.AnyGround)
                if (velY < -2) {
                    BounceY(col.ground.hit.normal, 0.4f);
                    velXZ /= 2;
                }
                else SetRule("Ground");
        }

        void Rule_Ground() {
            if (newRule) {
                SetFriction(8, 0);
                velY = 0;
            }

            if (col.ground.AnyGround)
                col.StickToGround();

            else if (col.ground.DeathWarp)
                Restart(); // not quite

            //else if (col.ground.None)
                //SetRule("Air");

        }
    }
}