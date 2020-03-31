//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class Levelhelic_20 : World {
        protected override void Events() {
            rayman.hasSuperHelic = true;
        }

        public bool danger1;
        public override void Music() {
            if (activeSector == 8) {
                QueueMusic(0, 0, 0);
                danger1 = false;
            }

            else if (!danger1 && activeSector == 9) {
                QueueMusic(0, 1, 2, () => SetMusic(0, 2, 0));
                danger1 = true;
            }

            else if (activeSector == 4)
                QueueMusic(0, 2, 0);

            else if (activeSector == 0)
                QueueMusic(0, 3, 0);
        }
    }
}