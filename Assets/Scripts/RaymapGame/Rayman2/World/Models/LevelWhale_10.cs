//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelWhale_10 : World {
        protected override void Events() {
            rayman.SetRule("Swimming");
        }

        public override void Music() {
            if (start)
                QueueMusic(0, 0, 0, Next);
        }
    }
}