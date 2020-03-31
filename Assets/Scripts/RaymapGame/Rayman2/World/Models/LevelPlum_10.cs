//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelPlum_10 : World {
        public override void Music() {
            if (rayman.rule == "Mounted")
                QueueMusic(0, 0, 0);
        }
    }
}