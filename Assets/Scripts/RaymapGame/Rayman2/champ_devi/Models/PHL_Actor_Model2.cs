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

        Timer t_hyst = new Timer();
        void Rule_Default() {
            if (!t_hyst.active && CheckCollisionZone(rayman, CollideType.ZDR)) {

                rayman.pos = pos + Vector3.up;
                rayman.velXZ = Vector3.zero;
                rayman.Jump(14, true);

                anim.Set(0);
                anim.Set(1);
                t_hyst.Start(0.2f);
            }
        }
    }
}