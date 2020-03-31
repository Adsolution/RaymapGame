//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelChase_22 : World {
        protected override void Events() {
            rayman.ForceNav();
        }

        public override void Music() {
            if (start) {
                QueueMusic(0, 0, 0);
                GetPerso<pirate_sbire>().AddDeathEvent(FadeOut);
            }

            else if (GetPerso<pirate_sbire>().rule == "Shooting")
                QueueMusic(2, 0, 0);

            else if (activeSector == 7 || activeSector == 9)
                QueueMusic(0, 1, 0);

            else if (activeSector == 23)
                FadeOut();

            else if (activeSector == 17)
                QueueMusic(1, 0, 0);

            else if (activeSector == 19)
                QueueMusic(1, 1, 0);

            else if (activeSector == 22)
                FadeOut();
        }
    }
}