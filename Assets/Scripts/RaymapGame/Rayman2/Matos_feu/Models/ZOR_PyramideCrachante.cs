//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Wall fire spitter
    /// </summary>
    public partial class ZOR_PyramideCrachante : Matos_feu {
        public float onTime;
        public float offTime;
        public float fireFric;
        protected override void OnStart() {
            onTime = (float)GetDsgVar<int>("Int_7") / 1000;
            offTime = (float)GetDsgVar<int>("Int_8") / 1000;
            fireFric = GetDsgVar<float>("Float_3");
            projectileVel = 40;
            projectileType = typeof(Alw_Feu_model);
            SetRule("Off");
        }

        protected void Rule_Off() {
            if (newRule) Timers("Off").Start(offTime, () => SetRule("On"));
        }

        protected void Rule_On() {
            if (newRule) Timers("On").Start(onTime, () => SetRule("Off"));

            Timers("Particle Delay").Start(Random.Range(0.07f, 0.085f), ()
                => Shoot().SetFriction(fireFric / 10, 0.2f), false);
        }
    }
}