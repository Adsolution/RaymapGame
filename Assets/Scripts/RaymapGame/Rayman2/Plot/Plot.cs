//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rising stone pillar (Echoing Caves)
    /// </summary>
    public partial class Plot : PersoController {
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Rise");
        }
        protected override void OnStart() {
            SetFriction(20, 20);
            moveSpeed = 2.5f;
            navRotSpeed = 0;
            SetRule("Unrisen");
        }

        protected void Rule_Unrisen() {
            if (!NavTowards(startPos, false))
                pos += Random.onUnitSphere * 0.03f;
        }

        protected void Rule_Rise() {
            if (!NavTowards(startPos - upward * GetDsgVar<float>("Float_3"), false))
                pos += Random.onUnitSphere * 0.03f;

            if (!deathLinks[0].dead)
                SetRule("Unrisen");
        }
    }
}