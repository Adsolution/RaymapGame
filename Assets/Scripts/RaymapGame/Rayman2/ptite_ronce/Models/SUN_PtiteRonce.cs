//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Nettle (thorn tentacle enemy)
    /// </summary>
    public partial class SUN_PtiteRonce : ptite_ronce {
        protected override void OnStart() {
            SetRule("Lashing");
        }

        protected override void OnUpdate() {
            anim.autoNext =
                perso.currentState == Anim.Retract ||
                perso.currentState == Anim.Breakout;
        }

        protected void Rule_Retracted() {
            anim.Set(Anim.Retract);
        }

        protected void Rule_Lashing() {
            anim.Set(Anim.Breakout);
        }
    }
}