//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class Levelwater_20 : World {
        public override void Music() {
            if (start)
                QueueMusic(0, 0, 0);

            else if (activeSector == 6)
                QueueMusic(0, 1, 0);

            else if (activeSector == 0)
                FadeOut();

            else if (activeSector == 1)
                QueueMusic(1, 0, 4);

        }
    }
}