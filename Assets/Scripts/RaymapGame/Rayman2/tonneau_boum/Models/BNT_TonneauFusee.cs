//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Explosive Keg
    /// </summary>
    public partial class BNT_TonneauFusee : tonneau_boum {
        public override bool isAlways => true;
        public override bool carriable => true;
        protected override void OnStart() {
            SetShadow(true);
            shadow.size = 1.6f;
            shadow.fadeDistance = 25;
            col.groundLevel = -0.5f;
            gravity = -80;
            SetRule("Air");
        }

        protected override void OnDeath() {
            Timers("Fall").Abort();
            CreateExplosion(pos);
            stdCam.Shake(3, 0.5f, pos);
            if (creator == null)
                pos = worldNullPos;
            else Remove();
        }

        protected override void OnUpdate() {
            if (DistTo(GetClosestPerso(typeof(flamme))) < 2)
                mainActor.SetRule("BarrelFlight");
        }



        protected void Rule_Air() {
            if (newRule) Timers("Fall").Start(4, Kill);

            SetFriction(0, 0.5f);
            ApplyGravity();

            col.wallEnabled = true;

            if (col.ground.Any)
                if (!col.ground.AnyGround || vel.magnitude > 30)
                    Kill();
                else SetRule("Ground");
        }


        protected void Rule_Ground() {
            if (newRule) Timers("Fall").Abort();
            velY = 0;
            SetFriction(10, 0.5f);

            if (col.ground.AnyGround)
                col.StickToGround();
            //else SetRule("Air");
        }


        protected override void OnThrown() {
            SetRule("Thrown");
        }

        void Rule_Thrown() {
            if (newRule) Timers("Fall").Start(4);

            gravity = -25;
            ApplyGravity();
            SetFriction(0, 0);
            RotateLocalX(90, 3);

            if (Raycast(vel, 1).Any)
                Kill();
        }
    }
}