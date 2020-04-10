//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelEarth_10 : World {
        public override void Music() {
            if (start) {
                volume = 1.2f;
                QueueMusic(0, 0, 0, () => SetMusic(0, 1, 0, () => SetMusic(0, 2, 0)));
            }

            else if (activeSector == 4)
                QueueMusic(0, 3, 0);

            else if (activeSector == 8)
                QueueMusic(0, 4, 0, () => SetMusic(0, 5, 0));
        }
    }
}