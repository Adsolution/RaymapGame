//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelLearn_30 : World {
        public override void Music() {
            if (start)
                QueueMusic(0, 0, 0, () => SetMusic(0, 1, 0));

            else if (activeSector == 8)
                QueueMusic(0, 2, 0);

            else if (activeSector == 11)
                QueueMusic(0, 3, 0);

            else if (activeSector == 0)
                QueueMusic(0, 4, 0);

            else if (activeSector == 9 && mainActor.pos.y < 0)
                QueueMusic(0, 5, 8);

            else if (activeSector == 9)
                QueueMusic(0, 2, 16);
        }
    }
}