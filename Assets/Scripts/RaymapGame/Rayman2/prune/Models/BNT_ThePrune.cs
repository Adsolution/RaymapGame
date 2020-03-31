//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Plum
    /// </summary>
    public partial class BNT_ThePrune : prune {
        public override bool carriable => true;
        public float idealMaxSpeed = 13;
        protected override void OnStart() {
            SetShadow(true);
            shadow.size = 2;
            shadow.fadeDistance = 20;

            col.wallEnabled = true;
            col.radius = 0.75f;
            col.bottom = 0.5f;
            col.top = 0.8f;
            col.groundLevel = -0.25f;
            gravity = -25;

            if (creator == null)
                SetRule("Bouncing");
        }

        protected override void OnThrown() {
            SetRule("Bouncing");
        }

        protected override void OnUpdate() {
            if (rayman.carryPerso == this)
                scale = 0.85f;
            else scale = 1;
        }


        protected void Rule_Bouncing() {
            if (DistTo(rayman) > 65) {
                Restart();
                return;
            }

            AlignY(6);
            SetFriction(0, 0);
            ApplyGravity();

            if (col.wall.LavaDeathWarp || col.ground.LavaDeathWarp) {
                rayman.SetMount(null);
                Restart();
            }

            if (col.wall.Any)
                Bounce3D(col.wall.hit.normal, 0.8f);
            if (col.ground.Any) {
                Bounce3D(col.ground.hit.normal, 0.8f);
                velY = 6.5f;
            }

            if (velXZ.magnitude > idealMaxSpeed)
                velXZ = Vector3.Lerp(velXZ, Vector3.ClampMagnitude(velXZ, idealMaxSpeed), dt);

            if (rayman.rule == "Air" && rayman.velY < 0 && CheckCollisionZone(rayman, OpenSpace.Collide.CollideType.ZDM)) {
                rayman.SetMount(this);
            }
        }
    }
}