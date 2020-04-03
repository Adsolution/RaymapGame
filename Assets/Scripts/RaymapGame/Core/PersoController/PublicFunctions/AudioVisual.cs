//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;
using RaymapGame.Rayman2.Persos;

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


        public void CamShake(float intensity, float duration)
            => stdCam.Shake(intensity, duration);
        public void CamShake3D(float intensity, float duration, float innerRadius = 8)
            => stdCam.Shake(intensity, duration, pos, innerRadius);
        public void CamShake3D(float intensity, float duration, Vector3 origin, float innerRadius = 8)
            => stdCam.Shake(intensity, duration, origin, innerRadius);


        public ParticleSystem SpawnParticle(string name, object type = null)
            => SpawnParticle(null, pos, name, type);
        public ParticleSystem SpawnParticle(bool attach, string name, object type = null)
            => SpawnParticle(attach ? this : null, pos, name, type);
        public ParticleSystem SpawnParticle(PersoController attachTo, string name, object type = null)
            => SpawnParticle(attachTo, pos, name, type);
        ParticleSystem SpawnParticle(PersoController attachTo, Vector3 pos, string name, object type = null) {
            var p = ResManager.Inst("Particles/" + name, attachTo).GetComponent<ParticleSystem>();
            p.transform.position = attachTo == null ? pos : pos;
            var pr = p.GetComponent<ParticleSystemRenderer>();
            pr.material = Instantiate(pr.material);

            string tex = null;
            switch (type) {
                case LumType.Red:
                    tex = "etincelle_rouge_ad"; break;
                case LumType.Yellow:
                    tex = "etincelle_doree_ad"; break;
                case LumType.Blue:
                case LumType.SuperBlue:
                    tex = "etincelle_bleu_ad"; break;
                case LumType.Green:
                    tex = "etincelle_VERT_ad"; break;
            }
            if (tex != null) pr.material.mainTexture = ResManager.Get<Texture2D>("effets_speciaux/" + tex);

            if (!p.main.loop)
                Timer.StartNew(p.main.startLifetime.constantMax + p.main.startLifetime.constantMax, () => Destroy(p.gameObject));
            return p;
        }
    }
}