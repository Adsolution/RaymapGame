//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelLearn_40 : World {

        public override void Music() {
            if (activeSector == 3)
                QueueMusic(1, 0, 0);

            else if (activeSector == 6)
                QueueMusic(0, 1, 0);
        }
    }
}