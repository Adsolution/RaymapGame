//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Helicopter bomb
    /// </summary>
    public partial class Bombe : PersoController {
        protected override void OnStart() {
            moveSpeed = 3;
            SetFriction(0.1f, 0.5f);
            SetRule("Homing");
        }

        void Rule_Homing() {
            NavTowards(rayman);
        }
    }
}