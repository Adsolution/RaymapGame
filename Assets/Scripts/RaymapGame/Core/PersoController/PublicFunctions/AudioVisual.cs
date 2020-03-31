//================================
//  By: Adsolution
//================================
using System.Collections.Generic;

namespace RaymapGame {
    public partial class PersoController {
        //========================================
        //  Audio & Visual
        //========================================
        public void SetVisibility(bool visible) {
            if (this.visible == visible) return;
            this.visible = visible;
            visChanged = true;
        }

        public Shadow shadow;
        public void SetShadow(bool enabled) { 
            if (shadow == null && enabled)
                shadow = ResManager.Inst("Shadow", this).GetComponent<Shadow>();
            else if (shadow != null && !enabled) {
                Destroy(shadow.gameObject);
                shadow = null;
            }
            hasShadow = enabled;
        }

        public AnimSFX GetSFXLayer(int anim) {
            foreach (var s in animSfx)
                if (s.anim == anim) return s;
            return null;
        }

        Dictionary<string, SFXPlayer> sfx = new Dictionary<string, SFXPlayer>();
        public SFXPlayer SFX(string path) {
            if (!sfx.ContainsKey(path))
                sfx.Add(path, SFXPlayer.CreateOn(this, new SFXPlayer.Info {
                    path = path,
                    space = SFXPlayer.Space.Point
                }));
            return sfx[path];
        }
    }
}