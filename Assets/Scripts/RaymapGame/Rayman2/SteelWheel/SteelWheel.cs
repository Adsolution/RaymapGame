//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Spinning doughnut platform (Iron Mountains)
    /// </summary>
    public partial class SteelWheel : PersoController {
        protected override void OnStart() {
            SetRule("Default");
        }

        protected void Rule_Default() {
            RotateLocalY(-45, 1);
        }
    }
}