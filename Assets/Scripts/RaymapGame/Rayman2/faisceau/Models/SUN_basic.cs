//================================
//  By: Adsolution
//================================

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Pirate projectiles/lasers
    /// </summary>
    public partial class SUN_basic : faisceau {
        public override float activeRadius => 150;
        protected void Rule_Shot() {
            if (newRule) {
                radius = 2;
                damage = 15;
                anim.Set(Anim.PirateShotStd);
            }
            if (DistTo(mainActor) > 120 || Raycast(vel, 0.5f).Any)
                Remove();
        }
    }
}