//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Flipping helicopter platform
    /// </summary>
    public partial class plate_helice : PersoController {
        public int ascendTime;
        protected override void OnStart() {
            ascendTime = GetDsgVar<int>("Int_1");
            anim.Set(1);
            moveSpeed = 3.5f;
            navRotSpeed = 0;
            SetRule("Wait");
        }

        protected override void OnUpdate() {
            anim.SetSpeed(Mathf.Clamp(velY * 30 + 30, 30, 300));
        }

        void Rule_Wait() {
            if (newRule)
                velY = Mathf.Sign(velY);

            SetFriction(0.25f, 0.2f);
            NavTowards(startPos, false);

            if (StoodOnBy(mainActor))
                SetRule("Ascend");
        }

        void Rule_Ascend() {
            if (newRule)
                Timers("Ascend").Start(ascendTime, () => SetRule("Descend"));

            SetFriction(0.25f, 0.35f);
            NavDirection(worldUpward, false);
        }

        void Rule_Descend() {
            if (newRule)
                Timers("Flip").Start(2, () => RotateLocalX(90 * dt), null);

            SetRotX(180, 0);
            SetFriction(0.25f, 0.5f);

            if (NavTowards(startPos, false)) {
                SetRule("Wait");
                Timers("Flip").Start(2, () => RotateLocalX(90 * dt), null);
            }

            if (upward.y < 0 && StoodOnBy(rayman))
                rayman.Kill();
        }
    }
}