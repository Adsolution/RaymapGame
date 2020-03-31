//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Switch base
    /// </summary>
    public partial class interrupteur : PersoController {
        public static class Anim {
            public const int
                TimeSwitchHit = 11,
                TimeSwitchRelease = 12,
                TimeSwitchInactive = 6,
                TimeSwitchActive = 7,

                TrigSwitchHit = 1,
                TrigSwitchActive = 2,
                TrigSwitchInactive = 3;
        }
        public override bool resetOnRayDeath => false;
        public override float activeRadius => 45;
        public bool timed => time != 0;
        public float time;

        protected override void OnStart() {
            time = (float)GetDsgVar<int>("Int_1") / 1000;
            SetRule("Inactive");
        }

        protected override void OnDeath() {
            SetRule("Active");
        }

        protected void Rule_Inactive() {
            if (newRule) HealFull();

            ReceiveProjectiles();
            anim.Set(timed ? Anim.TimeSwitchRelease : Anim.TrigSwitchInactive);
        }

        protected void Rule_Active() {
            if (timed && newRule)
                Timers("0").Start(time, () => SetRule("Inactive"));

            anim.Set(timed ? Anim.TimeSwitchHit : Anim.TrigSwitchHit);
        }
    }
}