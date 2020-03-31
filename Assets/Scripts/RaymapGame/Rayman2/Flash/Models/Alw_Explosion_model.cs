//================================
//  By: Adsolution
//================================
using System.Linq;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Explosion
    /// </summary>
    public partial class Alw_Explosion_model : Flash {
        public float radius = 6;

        public void Explode() {
            SetRotY(rndAngle);
            scale = radius / 6;
            anim.frame = 0;
            Timer.StartNew(0.4f, Remove);
            SFX("Rayman2/Explosions/Generic").Play();

            foreach (var p in GetPersos<PersoController>()
                .Where((x) => DistTo(x) < radius && x != this && x != creator))
                p.Damage(60);
        }
    }
}