//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Keg door
    /// </summary>
    public partial class sparadrap_metal : PersoController {
        public override bool resetOnRayDeath => false;
        protected override void OnStart() {
            SetRule("RelishingInNothing");
        }

        void Rule_RelishingInNothing() {
        }

        protected override void OnDeath() {
            SetNullPos();
        }
    }
}