//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelPlum_00 : World {
        protected override void Events() {
        }

        public override void Music() {
            if (activeSector == 0)
                QueueMusic(0, 0, 0);

            else if (activeSector == 5)
                QueueMusic(1, 0, 0);

            else if (activeSector == 8)
                FadeOut(2);

            else if (activeSector == 13)
                QueueMusic(1, 0, 0);
        }
    }
}