//================================
//  By: Adsolution
//================================
using UnityEngine;
using OpenSpace.Collide;

namespace RaymapGame.Rayman2.Persos {
    public class PHL_Actor_Model2 : champ_devi {
        protected override void OnStart() {
            SetRule("Default");
        }

        void Rule_Default() {
            if (StoodOnBy(mainActor)) {
                rayman.pos = pos + upward;
                rayman.velXZ = Vector3.zero;
                rayman.Jump(14, true);

                anim.Set(0);
                anim.Set(1);
                SFX("Rayman2/bounce").Play();
            }
        }
    }
}