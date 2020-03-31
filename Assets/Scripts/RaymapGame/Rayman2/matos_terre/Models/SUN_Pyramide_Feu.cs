//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rotating fire jet pyramid
    /// </summary>
    public partial class SUN_Pyramide_Feu : matos_terre {
        public float speed;
        protected override void OnStart() {
            speed = GetDsgVar<int>("Int_1") * Mathf.Rad2Deg;
            SetRule("Rotating");
        }

        protected void Rule_Rotating() {
            RotateY(speed, 1);
        }
    }
}