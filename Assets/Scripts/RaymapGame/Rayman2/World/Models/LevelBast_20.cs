//================================
//  By: Adsolution
//================================
using static RaymapGame.MusicHandler;

namespace RaymapGame.Rayman2.Persos {
    public partial class LevelBast_20 : World {
        protected override void Events() {
            GetPerso("OLP_Swithc_Porte_Trampoline").AddDeathEvent(() => GetPerso<porte>().SetRule("Open"));
            GetPerso("OLP_Sbire_Garde_Telescope").AddDeathEvent(() => GetPerso<telescop>().SetRule("Expand"));
            GetPerso("OLP_switch_PorteLaser").AddDeathEvent(() => GetPerso<porte_laser>().SetRule("Open"));
        }

        public override void Music() {
        }
    }
}