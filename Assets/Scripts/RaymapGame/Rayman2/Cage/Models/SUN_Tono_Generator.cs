//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Keg dispenser
    /// </summary>
    public partial class SUN_Tono_Generator : Cage {
        public override bool isAlways => onlyActiveSector;
        public override bool hasLinkedDeath => true;
        public BNT_TonneauFusee keg => (BNT_TonneauFusee)deathLinks[0];

        protected override void OnLinksDead() {
            Timers("Dump").Start(1.5f + Random.value, DumpBarrel);
        }

        public void DumpBarrel() {
            DumpBarrelAnim();
            keg.Restart();
            keg.pos = pos + forward - upward;
        }
    }
}