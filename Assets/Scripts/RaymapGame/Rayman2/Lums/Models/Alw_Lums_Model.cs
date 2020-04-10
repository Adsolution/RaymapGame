//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    public enum LumType { NotSet = -1, Red = 0, Yellow, SuperBlue, Blue, Green, Purple }
    /// <summary>
    /// Collectible Lums base
    /// </summary>
    public partial class Alw_Lums_Model : Lums {
        PersoController collector => rayman;
        public override bool updateCollision => false;
        public override bool resetOnRayDeath => false;

        public LumType type = LumType.NotSet;
        public float attractRadius = -1;

        public virtual void OnCollect(PersoController collector) { }

        public void CollectBy(PersoController collector) {
            switch (type) {
                case LumType.Red:
                    collector.Heal(20);
                    break;

                case LumType.Yellow:
                    GetPerso<World>().lums++;
                    break;

                case LumType.Blue:
                    collector.SFX("Rayman2/Lums/Yellow").Play(0.1f, 0.3f);
                    collector.Heal(20);
                    break;

                case LumType.Green:
                    if (collector.checkpoint != null)
                        collector.checkpoint.lum.Restart();
                    collector.checkpoint = creator as CHR_CheckP;
                    break;
            }

            SpawnParticle(collector, "LumCollect", type);
            SetNullPos();
            collector.SFX($"Rayman2/Lums/{type}").Play(0.1f);

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

            SpawnParticle(true, "LumTrail", type);
            SetShadow(true);
            SetRule("WaitCollect");
        }

        protected override void OnUpdate() {
            if (DistTo(collector) < 2) {
                SetRule("");
                CollectBy(collector);
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

            if (attractRadius > 0 && DistTo(collector.center) < attractRadius)
                SetRule("Attracted");
        }

        float speedup = 1;
        protected void Rule_Attracted() {
            if (newRule) {
                SetFriction(6, 6);

                Timers("Speedup").Start(1.5f, () => {
                    speedup = 4;
                    SetFriction(9, 9);
                });
            }

            vel += Random.onUnitSphere * moveSpeed * 7 * dt * 2 / speedup;
            moveSpeed = speedup * 6 * Mathf.Clamp(DistTo(collector), 2, 100);
            NavTowards(collector.center, false);
        }
    }
}