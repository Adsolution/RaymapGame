//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelWater_10 : World {
        public override void Music() {
            if (start) {
                QueueMusic(0, 0, 0);

                GetPerso<pirate_sbire>().AddCombatEvent(()
                    => QueueMusic(1, 1, 0));
                GetPerso<pirate_sbire>().AddDeathEvent(()
                    => QueueMusic(2, 0, 0));
                foreach (var p in GetPersos(typeof(porte_bois))) p.AddDeathEvent(()
                    => QueueMusic(2, 2, 0));
            }

            if (!GetPersos(typeof(pirate_sbire))[1].dead && activeSector == 4)
                QueueMusic(2, 1, 0);
        }
    }
}