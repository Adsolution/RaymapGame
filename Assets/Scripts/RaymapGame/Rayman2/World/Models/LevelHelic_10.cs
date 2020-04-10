//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class Levelhelic_10 : World {
        protected override void Events() {
            rayman.hasSuperHelic = true;
        }


        public bool danger1, danger2, danger3, danger4;
        public override void Music() {
            if (start)
                SetMusic(1, 1, 0);

            else if (activeSector == 0 && rayman.helic)
                QueueMusic(0, 0, 0);

            else if (!danger1 && activeSector == 16) {
                QueueMusic(0, 1, 0, () => SetMusic(0, 0, 0));
                danger1 = true;
            }

            else if (!danger2 && activeSector == 11) {
                QueueMusic(0, 1, 0, () => SetMusic(0, 0, 0));
                danger2 = true;
            }

            else if (!danger3 && activeSector == 4) {
                QueueMusic(0, 1, 0, () => SetMusic(1, 0, 0));
                danger3 = true;
            }

            else if (activeSector == 12)
                QueueMusic(0, 3, 0);

            else if (activeSector == 13)
                QueueMusic(0, 0, 0);

            else if (!danger4 && activeSector == 6) {
                QueueMusic(0, 1, 0, () => SetMusic(1, 0, 0));
                danger4 = true;
            }
        }
    }
}