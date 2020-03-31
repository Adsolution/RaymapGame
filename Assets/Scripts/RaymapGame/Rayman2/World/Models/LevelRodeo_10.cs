//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelRodeo_10 : World {
        protected override void Events() {
            foreach (var p in GetPersos(typeof(pirate_sbire)))
                p.AddDeathEvent(() => {

                    foreach (var p2 in GetPersos(typeof(pirate_sbire)))
                        if (!p2.dead) return;
                    GetPerso<porte>().SetRule("Open");
                });
        }

        public override void Music() {
        }
    }
}