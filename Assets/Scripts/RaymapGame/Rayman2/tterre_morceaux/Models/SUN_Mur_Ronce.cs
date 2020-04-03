//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Nettle wall break
    /// </summary>
    public partial class SUN_Mur_Ronce : tterre_morceaux {
        public float alertRadius;
        public float delay;
        public SUN_PtiteRonce nettle;
        protected override void OnStart() {
            alertRadius = GetDsgVar<float>("Float_1");
            delay = (float)GetDsgVar<int>("Int_2") / 1000;

            anim.Set(Anim.Unbroken);
            SetRule("Default");

            if (nettle == null) nettle = Clone<SUN_PtiteRonce>(pos);
            nettle.scale = 0.7f;
            nettle.SetRule("Retracted");
        }

        protected void Rule_Default() {
            nettle.SetRule("Retracted");

            if (DistTo(rayman) < alertRadius)
                SetRule("Broken");
        }

        void Rule_Broken() {
            if (newRule) {
                nettle.SetRule("Lashing");
                Timers("Break").Start(delay, () => anim.Set(Anim.Break));
                Timers("StayBroken").Start(delay + 0.5f, () => anim.Set(Anim.Broken));
            }
        }
    }
}