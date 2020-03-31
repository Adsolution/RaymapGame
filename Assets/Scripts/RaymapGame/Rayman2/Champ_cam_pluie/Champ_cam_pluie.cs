//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Camera Rain
    /// </summary>
    public partial class Champ_cam_pluie : PersoController {
        public override bool isAlways => true;
        public override bool resetOnRayDeath => false;
        SFXPlayer sfx;
        protected override void OnStart() {
            sfx = SFX("Rayman2/Ambience/rain");
            sfx.polyphony = SFXPlayer.Polyphony.Loop;
            sfx.SetSpace(SFXPlayer.Space.Global);
            SetRule("Drout");
        }

        void Rule_Raining() {
            if (newRule) {
                sfx.Play();
                sfx.FadeIn(0, 0.3f);
            }

            pos = stdCam.pos;

            if (stdCam.sector != startSector) {
                sfx.FadeOut(0);
                SetRule("Drout");
            }
        }

        void Rule_Drout() {
            if (stdCam.sector == startSector)
                SetRule("Raining");
        }
    }
}