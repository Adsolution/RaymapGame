//================================
//  By: Adsolution
//================================
namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Forced camera orientation
    /// </summary>
    public partial class GenCamera : PersoController {
        public static bool areasEnabled = true;

        public StdCam cam;
        Timer t_follow = new Timer();
        public override bool isAlways => true;
        public bool inside { get; private set; }
        public static GenCamera curr { get; private set; }

        protected override void OnStart() {
            if (!areasEnabled) return;
            cam = GetPerso<StdCam>();
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (!inside && CheckCollisionZone(mainActor, OpenSpace.Collide.CollideType.ZDD)) {
                inside = true;
                curr = this;
                OnEnter();
            }
            else if (inside && !CheckCollisionZone(mainActor, OpenSpace.Collide.CollideType.ZDD)) {
                inside = false;
                if (curr == this) {
                    curr = null;
                    cam.SetRule("Follow");
                }
                OnExit();
            }
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}