//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Magic Sphere Target
    /// </summary>
    public partial class SUN_magie_simple : magie {
        public SUN_magic_key orb;
        public SUN_magic_key.OrbColor color;

        protected override void OnStart() {
            color = (SUN_magic_key.OrbColor)GetDsgVar<byte>("UByte_0");
            SetRule("Default");
        }

        void Rule_Default() {
            if (orb == null) {
                foreach (SUN_magic_key k in GetPersos(typeof(SUN_magic_key)))
                    if (DistTo(k) < 1)
                        orb = k;
                anim.Set(Anim.Off);
            }

            if (orb != null) {
                orb.pos = pos;
                orb.placed = true;
                anim.Set(Anim.MagicStart);
                Kill();
            }
        }
    }
}