//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Blastable door
    /// </summary>
    public partial class porte_bois : PersoController {
        public override bool resetOnRayDeath => false;
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            Kill();
        }
        protected override void OnDeath() {
            SetRule("Broken");
        }

        protected void Rule_Broken() {
            if (newRule) {
                SFX("Rayman2/doorbreak").Play();
                velY = 10;
                velXZ = -forward * 20;
                rotVel = Random.rotationUniform.eulerAngles * 3;
                SetFriction(0, 0);
                Timer.StartNew(1, SetNullPos);
            }

            ApplyGravity();
        }
    }
}