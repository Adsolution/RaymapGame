//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Metal expanding bridge
    /// </summary>
    public partial class STN_pontlevis : telescop {
        Timer t_tel = new Timer();
        Channel tel1, tel2;

        protected override void OnStart() {
            tel1 = GetChannel(1);
            tel2 = GetChannel(2);
        }

        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Expand");
        }

        protected void Rule_Expand() {
            if (newRule)
                t_tel.Start(3, () => SetRule(""));

            if (t_tel.elapsed < 1.5f)
                tel1.pos += right * dt * 4;
            tel2.pos += right * dt * 4;
        }
    }
}