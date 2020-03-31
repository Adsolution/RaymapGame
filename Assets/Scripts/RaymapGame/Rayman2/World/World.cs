//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Learn_31
    /// </summary>
    public partial class World : PersoController {
        public override float activeRadius => 9999999;
        public override bool isAlways => true;
        public override bool resetOnRayDeath => false;

        protected virtual void Events() { }
        public virtual void Music() { }

        protected override void OnStart() {
            Events();
        }
    }
}