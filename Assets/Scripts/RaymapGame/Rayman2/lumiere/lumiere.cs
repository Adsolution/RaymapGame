//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Checklist
    /// </summary>
    public partial class lumiere : PersoController {
        public override bool isAlways => true;
        public override bool resetOnRayDeath => false;
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            Kill();
        }

        protected override void OnStart() {
            GetDsgVar<PersoController>("Perso_1").AddDeathEvent(() => GetChannel(0).visible = false);
            GetDsgVar<PersoController>("Perso_2").AddDeathEvent(() => GetChannel(1).visible = false);
            GetDsgVar<PersoController>("Perso_3").AddDeathEvent(() => GetChannel(2).visible = false);
            GetDsgVar<PersoController>("Perso_4").AddDeathEvent(() => GetChannel(3).visible = false);
        }
    }
}