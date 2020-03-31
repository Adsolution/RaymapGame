//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Laser door
    /// </summary>
    public partial class OLP_PorteLaser_Switch : porte_laser {
        public PersoController inverseur, inverseurSwitch;
        protected override void OnStart() {
            inverseur = GetDsgVar<PersoController>("Perso_1");
            inverseurSwitch = ((OLP_Inverseur)inverseur).interrupteur;
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (inverseurSwitch.rule == "Active")
                SetRule("Open");
            else SetRule("Closed");
        }

        void Rule_Open() {
            anim.Set(1);
        }

        void Rule_Closed() {
            anim.Set(0);
        }
    }
}