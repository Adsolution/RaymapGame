//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    public enum LumType { NotSet = -1, Red = 0, Yellow, SuperBlue, Blue, Green }
    /// <summary>
    /// Collectible Lums base
    /// </summary>
    public partial class Alw_Lums_Model : Lums {
        public override bool resetOnRayDeath => false;
        public LumType type = LumType.NotSet;
        public float attractRadius = -1;
        public virtual void OnCollect(PersoController collector) { }

        public void CollectBy(PersoController collector) {
            switch (type) {
                case LumType.Red: collector.Heal(20); break;
                case LumType.Green:
                    if (collector.checkpoint != null)
                        collector.checkpoint.lum.Restart();
                    collector.checkpoint = (CHR_CheckP)creator; break;
            }
            OnCollect(collector);
        }


        protected override void OnStart() {
            if (type == LumType.NotSet) {
                if (HasDsgVar("Int_0")) type = (LumType)GetDsgVar<int>("Int_0");
                else type = (LumType)GetDsgVar<byte>("UByte_0");
            }
            if (attractRadius == -1)
                attractRadius = GetDsgVar<float>("Float_1");

            switch (type) {
                case LumType.Red: anim.Set(Anim.Red); break;
                case LumType.Yellow: anim.Set(Anim.Yellow); break;
                case LumType.SuperBlue:
                case LumType.Blue: anim.Set(Anim.Blue); break;
                case LumType.Green: anim.Set(Anim.Green); break;
            }

            SetShadow(true);
            SetRule("WaitCollect");
        }

        protected override void OnUpdate() {
            if (DistTo(mainActor) < 2) {
                vel = Vector3.zero;
                pos = new Vector3(0, -10000, 0);
                SetRule("");
                CollectBy(mainActor);
                OnCollect(mainActor);
            }
        }

        public static float wanderSpeed = 1;
        protected void Rule_WaitCollect() {
            if (newRule)
                SetFriction(wanderSpeed, wanderSpeed);

            // Wander very slightly
            moveSpeed = Mathf.Clamp(DistTo(startPos), 0.2f, 10) * 0.2f + wanderSpeed;
            vel += Random.onUnitSphere * 7 * dt;
            NavTowards(startPos, false);

            if (attractRadius > 0 && DistTo(mainActor.center) < attractRadius)
                SetRule("Attracted");
        }

        Timer t_speedup = new Timer();
        float speedup = 1;
        protected void Rule_Attracted() {
            if (newRule) {
                SetFriction(6, 6);

                t_speedup.Start(1.5f, () => {
                    speedup = 4;
                    SetFriction(9, 9);
                });
            }

            vel += Random.onUnitSphere * moveSpeed * 7 * dt * 2 / speedup;
            moveSpeed = speedup * 6 * Mathf.Clamp(DistTo(mainActor), 2, 100);
            NavTowards(mainActor.center, false);
        }
    }
}