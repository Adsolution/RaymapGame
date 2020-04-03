//================================
//  By: Adsolution
//================================

using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame {
    public enum AnimFlags {
        None = 0b0000,
        Blend = 0b0001,
        Reset = 0b0010
    }

    public class AnimHandler : MonoBehaviour {
        PersoBehaviour perso;
        ROMPersoBehaviour persoRom;

        Dictionary<int, float> prioCache = new Dictionary<int, float>();
        List<OpenSpace.Object.Properties.State> nextStates = new List<OpenSpace.Object.Properties.State>();
        List<OpenSpace.ROM.State> nextStatesRom = new List<OpenSpace.ROM.State>();

        public int currAnim => perso != null ? perso.state.index : 0;
        public int frame { get => (int)perso.currentFrame; set { perso.currentFrame = (uint)value; } }
        public float currPriority {
            get { if (prioCache.ContainsKey(currAnim)) return prioCache[currAnim]; else return 0; }
        }

        public float speed => !Main.isRom ? perso.animationSpeed : persoRom.animationSpeed;

        public void SetSpeed(float speed) {
            if (!Main.isRom) perso.animationSpeed = speed;
            else persoRom.animationSpeed = speed;
        }

        public bool IsSet(int anim) {
            if (!Main.isRom) return perso.state.index == anim;
            else return persoRom.state.Index == anim;
        }

        public bool autoNext { get => perso.autoNextState; set { perso.autoNextState = value; } }

        public void Set(int anim) => Set(anim, currPriority, -1, AnimFlags.None, false);
        public void Set(int anim, bool reset) => Set(anim, currPriority, -1, AnimFlags.None, reset);
        public void Set(int anim, float priority) => Set(anim, priority, -1, AnimFlags.None, false);
        public void Set(int anim, float priority, float speed) => Set(anim, priority, speed, AnimFlags.None, false);
        public void Set(int anim, float priority, float speed, AnimFlags options, bool reset) {
            if (!Main.isRom) {
                if (perso == null || priority < currPriority)
                    return;
                if (anim == currAnim) {
                    if (options.HasFlag(AnimFlags.Reset) || reset)
                        perso.currentFrame = 0;
                    return;
                }

                foreach (var ns in nextStates)
                    if (ns.index == anim)
                        return;

                if (!prioCache.ContainsKey(anim))
                    prioCache.Add(anim, priority);

                var fr = perso.currentFrame;
                perso.SetState(anim);
                if (options.HasFlag(AnimFlags.Blend))
                    perso.currentFrame = fr;

                var next = perso.state;

                nextStates.Clear();
                for (int i = 0; i < 1; i++) {
                    if (next != null) {
                        nextStates.Add(next);
                    }
                }
            }
            else {
                if (persoRom == null || anim == currAnim || priority < currPriority)
                    return;

                foreach (var ns in nextStates)
                    if (ns.index == anim)
                        return;

                if (!prioCache.ContainsKey(anim))
                    prioCache.Add(anim, priority);
                persoRom.autoNextState = true;
                persoRom.SetState(anim);
                var next = persoRom.state;

                nextStatesRom.Clear();
                for (int i = 0; i < 1; i++) {
                    if (next != null) {
                        nextStatesRom.Add(next);
                    }
                }
            }
            if (speed != -1)
                SetSpeed(speed);
        }

        void Awake() {
            if (!Main.isRom) perso = gameObject.GetComponent<PersoBehaviour>();
            else persoRom = gameObject.GetComponent<ROMPersoBehaviour>();
            autoNext = true;
        }


        // SOUND
        public AnimSFX[] sfx;
        void Start() {
            if (sfx == null) return;
            for (int i = 0; i < sfx.Length; i++)
                if (sfx[i].player == null)
                    sfx[i].player = SFXPlayer.CreateOn(this, sfx[i].info);
            ok = true;
        }
        bool ok;

        uint lastFrame;
        void FixedUpdate() {
            if (!ok) return;

            if (!Main.isRom) {
                if (perso.currentFrame <= 2)
                    lastFrame = 0;
                foreach (var s in sfx)
                    if (s.anim == currAnim) {
                        foreach (var f in s.frames)
                            if (f >= lastFrame && f <= perso.currentFrame)
                                s.player.Play();
                    }
                    else if (s.player.polyphony != SFXPlayer.Polyphony.Poly)
                        s.player.Stop();
                lastFrame = perso.currentFrame;
            }

            else {
                if (persoRom.currentFrame <= 2)
                    lastFrame = 0;
                foreach (var s in sfx)
                    if (s.anim == currAnim) {
                        foreach (var f in s.frames)
                            if (f >= lastFrame && f <= persoRom.currentFrame)
                                s.player.Play();
                    }
                    else if (s.player.polyphony != SFXPlayer.Polyphony.Poly)
                        s.player.Stop();
                lastFrame = persoRom.currentFrame;
            }
        }
    }
}