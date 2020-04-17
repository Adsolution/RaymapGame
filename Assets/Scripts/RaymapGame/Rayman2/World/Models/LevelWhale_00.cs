//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Whale_00 level script
    /// </summary>
    public partial class LevelWhale_00 : World {
        protected override void Events() {
        }

        bool swimStart;
        public override void Music() {
            var secCheck = activeSector >= 2 && activeSector <= 5;

            if (swimStart && secCheck) {
                swimStart = false;
                QueueMusic(0, 0, 0, () => QueueMusic(1, 0, 0));
            }
            else if (!secCheck) {
                swimStart = true;
                FadeOut(6);
            }
        }
    }
}