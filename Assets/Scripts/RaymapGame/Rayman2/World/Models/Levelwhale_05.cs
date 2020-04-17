//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Whale_00 level script
    /// </summary>
    public partial class Levelwhale_05 : World {
        bool swimStart;
        public override void Music() {
            var secCheck = activeSector == 11 || activeSector == 8;

            if (swimStart && secCheck) {
                swimStart = false;
                QueueMusic(0, 0, 0, () => QueueMusic(0, 0, 4, () => QueueMusic(1, 0, 0)));
            }
            else if (!secCheck) {
                swimStart = true;
                FadeOut(6);
            }
        }
    }
}