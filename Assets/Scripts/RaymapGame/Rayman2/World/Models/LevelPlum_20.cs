//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class Levelplum_20 : World {
        public override void Music() {
            if (start) QueueMusic(0, 0, 0);
        }
    }
}