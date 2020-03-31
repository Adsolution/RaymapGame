//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelChase_10 : World {
        protected override void Events() {
            GetPerso("FRG_Interrupteur_Passerelle").AddDeathEvent(() => GetPerso<passerelle>().SetRule("Tip"));
        }

        public override void Music() {
            if (start) {
                QueueMusic(0, 0, 0);
                GetPerso<interrupteur>().AddDeathEvent(() => QueueMusic(0, 2, 0));
                GetPerso<pirate_sbire>().AddDeathEvent(FadeOut);
            }

            else if (GetPerso<pirate_sbire>().rule == "Shooting")
                FadeOut(2, () => SetMusic(2, 0, 0));

            else if (GetPerso("FRG_OLD_tonneau_reseau_I3").StoodOnBy(mainActor) || activeSector == 8)
                QueueMusic(0, 1, 0);
        }
    }
}