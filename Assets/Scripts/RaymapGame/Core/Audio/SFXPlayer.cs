//================================
//  By: Adsolution
//================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace RaymapGame
{
    public class SFXPlayer : MonoBehaviour
    {
        public enum Mode { RandomNoRepeat, Consecutive, Random }
        public enum Polyphony { Poly, Mono, Loop }
        public enum Space { Point = 1, Global = 0 }
        public class Info
        {
            public Info() { }
            public Info(string path) => this.path = path;
            public string path;
            public Polyphony polyphony;
            public Mode mode;
            public Space space = Space.Point;
            public float volume = 1;
            public float pointMinRadius = 12;
            public float pointMaxRadius = 75;
        }

        public Mode mode;
        public Polyphony polyphony;

        public void SetSpace(Space space) => asrc.spatialBlend = (float)space;
        public void SetVolume(float volume) => asrc.volume = volume;
        public void SetMinRadius(float minRadius) => asrc.minDistance = minRadius;
        public void SetMaxRadius(float maxRadius) => asrc.maxDistance = maxRadius;

        public List<AudioClip> queued = new List<AudioClip>();
        public List<AudioClip> done = new List<AudioClip>();
        public AudioSource asrc;

        public void ResetQueue()
        {
            queued.AddRange(done);
            done.Clear();
        }

        string sfxPath;
        public void SetSFX(string path)
        {
            if (asrc == null) {
                bool newAsrc = true;
                foreach (var pl2 in GetComponents<SFXPlayer>())
                    if (pl2.asrc != null && pl2.sfxPath == sfxPath) {
                        asrc = pl2.asrc; newAsrc = false; break;
                    }
                if (newAsrc) asrc = gameObject.AddComponent<AudioSource>();
            }
            done.Clear();
            queued = ResManager.GetAll<AudioClip>("Sounds/" + path).ToList();
            sfxPath = path;
        }

        AudioClip DequeueClip(AudioClip clip)
        {
            if (queued.Contains(clip))
            {
                done.Add(clip);
                queued.Remove(clip);
            }
            return clip;
        }

        Timer t_hyst = new Timer();
        public void Play()
        {
            if (queued.Count == 0 || t_hyst.active || asrc == null) return;
            switch (mode)
            {
                case Mode.RandomNoRepeat:
                    PlayClip(DequeueClip(queued[Random.Range(0, queued.Count - 1)]));
                    break;
                case Mode.Consecutive:
                    PlayClip(DequeueClip(queued[0]));
                    break;
                case Mode.Random:
                    PlayClip(queued[Random.Range(0, queued.Count - 1)]);
                    break;
            }
            if (queued.Count == 0) ResetQueue();
            t_hyst.Start(1f / 8);
        }
        public void Stop()
        {
            if (asrc != null) asrc.Stop();
        }

        Timer t_fade = new Timer();
        public void FadeOut(float seconds) {
            t_fade.Start(seconds, ()
                => asrc.volume = Mathf.Lerp(asrc.volume, 0, Time.fixedDeltaTime / seconds), ()
                => asrc.volume = 0);
        }
        public void FadeIn(float seconds, float volume = 1) {
            t_fade.Start(seconds, ()
                => asrc.volume = Mathf.Lerp(0, volume, Time.fixedDeltaTime / seconds), ()
                => asrc.volume = volume);
        }

        void PlayClip(AudioClip clip)
        {
            switch (polyphony)
            {
                case Polyphony.Mono:
                    asrc.clip = clip;
                    asrc.Play(); break;

                case Polyphony.Poly:
                    asrc.PlayOneShot(clip, asrc.volume); break;

                case Polyphony.Loop:
                    if (asrc.isPlaying) break;
                    asrc.loop = true;
                    asrc.clip = clip;
                    asrc.Play(); break;
            }
        }

        public void SetFromInfo(Info i)
        {
            if (i == null) i = new Info();
            SetSFX(i.path);
            SetSpace(i.space);
            SetVolume(i.volume);
            SetMinRadius(i.pointMinRadius);
            SetMaxRadius(i.pointMaxRadius);
            mode = i.mode;
            polyphony = i.polyphony;
        }
        
        public static SFXPlayer CreateOn(MonoBehaviour source, Info properties)
        {
            var pl = source.gameObject.AddComponent<SFXPlayer>();
            pl.SetFromInfo(properties);

            return pl;
        }

        void Start()
        {
            asrc.outputAudioMixerGroup = Main.env.envGroup;
        }
    }
}