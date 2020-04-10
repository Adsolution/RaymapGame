//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelLearn_10 : World {
        protected override void Events() {
            mainActor.SpawnParticle(true, "Env/ForestDust");
        }

        public override void Music() {
            if (start) {
                QueueMusic(1, 0, 0);
                GetPerso("JCP_ARG_CageLums_I2").AddDeathEvent(() => QueueMusic(1, 2, 0));
            }

            else if (activeSector == 4)
                QueueMusic(0, 0, 0);

            else if (activeSector == 11)
                QueueMusic(1, 1, 0);

            else if (activeSector == 14)
                QueueMusic(1, 2, 0);
        }
    }
}