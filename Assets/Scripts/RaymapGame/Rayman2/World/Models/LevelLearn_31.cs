//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelLearn_31 : World {
        protected override void Events() {
            mainActor.SpawnParticle(true, "Env/Leaves");
        }

        public override void Music() {
            if (start)
                QueueMusic(1, 0, 0);

            else if (activeSector == 1) {
                if (mainActor.rule == "Climbing")
                    QueueMusic(1, 1, 0);
                else
                    QueueMusic(1, 0, 0);
            }

            else if (activeSector == 13)
                QueueMusic(0, 2, 16);
        }
    }
}