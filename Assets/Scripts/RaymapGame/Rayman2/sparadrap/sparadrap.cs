//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Breakable Plaster (weak)
    /// </summary>
    public partial class sparadrap : PersoController {
        public override bool resetOnRayDeath => false;
        protected override void OnStart() {
            // Unbeknownst to the weaker Sparadrap, their idols also left no legacy
            SetRule("DesiringMetallicity");
        }

        void Rule_DesiringMetallicity() {
            ReceiveProjectiles();
        }

        protected override void OnDeath() {
            SetNullPos();
        }
    }
}