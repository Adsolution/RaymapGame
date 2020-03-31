//================================
//  By: Adsolution
//================================

using System.Collections.Generic;
using System.Linq;

namespace RaymapGame
{
    public class AnimSFX
    {
        public SFXPlayer.Info info;
        public SFXPlayer player;
        public int anim;
        public List<int> frames;

        void Init(int anim, SFXPlayer.Info sfxInfo, params int[] frames)
        {
            info = sfxInfo;
            this.anim = anim;
            this.frames = frames.ToList();
        }

        public AnimSFX(SFXPlayer player, int anim, string sfxPath) { Init(anim, new SFXPlayer.Info(sfxPath), 2); this.player = player; }
        public AnimSFX(int anim, string sfxPath) => Init(anim, new SFXPlayer.Info(sfxPath), 2);
        public AnimSFX(int anim, string sfxPath, SFXPlayer.Polyphony polyphony) => Init(anim, new SFXPlayer.Info { path = sfxPath, polyphony = polyphony }, 2);
        public AnimSFX(int anim, string sfxPath, params int[] frames) => Init(anim, new SFXPlayer.Info(sfxPath), frames);

        public AnimSFX(int anim, SFXPlayer.Info sfxInfo) => Init(anim, sfxInfo, 2);
        public AnimSFX(int anim, SFXPlayer.Info sfxInfo, params int[] frames) => Init(anim, sfxInfo, frames);
    }
}