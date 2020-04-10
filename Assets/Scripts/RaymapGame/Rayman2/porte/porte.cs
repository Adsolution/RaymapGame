//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Pirate double sliding door
    /// </summary>
    public partial class porte : PersoController {
        public static class Anim {
            public const int
                Closed = 1,
                Closing = 2,
                Open = 3,
                Opening = 4;
        }
        public override bool resetOnRayDeath => false;
        public override bool hasLinkedDeath => true;
        protected override void OnLinksDead() {
            SetRule("Open");
        }

        protected override void OnStart() {
            anim.Set(Anim.Closed);
        }

        Timer t_opening = new Timer();
        protected void Rule_Open() {
            if (newRule) {
                SFX("Rayman2/PorteOpen").Play();
                anim.Set(Anim.Opening);
                t_opening.Start(0.6f, () => anim.Set(Anim.Open));
            }
            if (!deathLinks[0].dead)
                SetRule("Closed");
        }

        protected void Rule_Closed() {
            if (newRule) {
                SFX("Rayman2/PorteClose").Play();
                anim.Set(Anim.Closing);
                t_opening.Start(0.6f, () => anim.Set(Anim.Closed));
            }
        }
    }
}