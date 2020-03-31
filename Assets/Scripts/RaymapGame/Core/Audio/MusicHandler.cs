//================================
//  By: Adsolution
//================================

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Reflection;

namespace RaymapGame {
    partial class MusicHandler : MonoBehaviour {
        public AudioMixerGroup musicMixerGroup;
        public float _volume = 0.7f;
        static MusicHandler inst;
        public static float volume { get => inst._volume; set { inst._volume = value; } }
        public static RayTune[] tunes;
        public static RayTune tune;
        public static AudioSource asrc;

        public static bool start { get; private set; } = true;
        private void Main_onLoad(object sender, EventArgs e) {
            if (tunes == null) return;
            foreach (var t in tunes)
                t.clip = t.GetDownloadedClip();
        }

        public static void SetSection(int section, int bar, bool loop = true, Action onFinishAction = null) {
            if (tune.clip == null) return;
            var cl = tune.clip;
            int s1 = tune.sectBars[section];
            int s2 = section + 1 >= tune.sectBars.Length ? tune.clip.samples - s1 : tune.sectBars[section + 1];
            int newSmpStart = (int)(44100 * 2 * s1 * tune.barLength);
            int newSmpLength = (int)((44100 * 2 * s2 * tune.barLength) - (s1 * tune.barLength * 44100 * 2));
            
            var newCl = AudioClip.Create($"{cl.name}_sec{section}", newSmpLength / 2, 2, 44100, false);
            float[] data = new float[newSmpLength];
            tune.clip.GetData(data, Mathf.Clamp(newSmpStart / 2, 0, int.MaxValue));
            newCl.SetData(data.Take(newSmpLength).ToArray(), 0);
            
            fading = false;
            asrc.volume = volume;
            asrc.time = 0;
            asrc.clip = newCl;
            asrc.loop = MusicHandler.loop = loop;
            action = onFinishAction;
            MusicHandler.section = section;
            tr = false;
            asrc.time = bar * tune.barLength;
            asrc.Play();
        }

        public static int bar => tune == null ? 0 : Mathf.FloorToInt((asrc.time + 0.05f) / tune.barLength);
        public static int section = -1, tuneIdx = -1;
        static int trBar = -1;
        static int newBar = -1;
        static bool tr;
        static bool loop;
        static Action action, fadeAction;
        static bool fading;
        static float fadeBars;

        public static void FadeOut()
            => FadeOut(4, null);
        public static void FadeOut(float bars, Action onFadeAction = null) {
            fadeAction = onFadeAction;
            fadeBars = bars;
            fading = true;
        }

        public static void SetMusic(int song, int section, int bar, Action onFinishAction)
            => SetMusic(song, section, bar, false, onFinishAction);
        public static void SetMusic(int song, int section, int bar, bool loop = true, Action onFinishAction = null) {
            if (tune == null || tuneIdx != song || tr)
                tune = tunes[tuneIdx = song];
            if (MusicHandler.section != section || newBar != bar || tr)
                SetSection(section, bar, loop, onFinishAction);
        }

        public static void QueueMusic(int song, int section, int bar, Action onFinishAction)
            => QueueMusic(song, section, bar, false, onFinishAction);
        public static void QueueMusic(int song, int section, int bar, bool loop = true, Action onFinishAction = null) {
            if (section == MusicHandler.section && song == tuneIdx && bar == newBar)
                return;
            MusicHandler.section = section;
            MusicHandler.loop = loop;
            newBar = bar;
            action = onFinishAction;
            tuneIdx = song;
            trBar = MusicHandler.bar + (MusicHandler.bar % 2 == 0 ? 2 : 1);
            tr = true;
        }

        void FixedUpdate() {
            if (!Main.loaded) return;
            /*
            foreach (var t in tunes)
                t.clip = t.GetDownloadedClip();*/

            if (!loop && !asrc.isPlaying && action != null) {
                var ac = action;
                action = null;
                ac.Invoke();
            }

            ((Rayman2.Persos.World)PersoController.GetPerso("World")).Music();

            if (tr && (tune == null || bar >= trBar))
                SetMusic(tuneIdx, section, newBar, loop, action);

            if (fading) {
                asrc.volume = Mathf.Clamp01(asrc.volume - Time.fixedDeltaTime / (fadeBars * tune.barLength * volume));
                if (asrc.volume == 0) {
                    fading = false;
                    if (fadeAction != null)
                        fadeAction.Invoke();
                }
            }

            start = false;
        }


        void Awake() {
            enabled = false;
            return;
            inst = this;
            asrc = gameObject.AddComponent<AudioSource>();
            asrc.loop = true;
            asrc.outputAudioMixerGroup = musicMixerGroup;
            asrc.volume = 0.7f;
            asrc.playOnAwake = false;
            Main.onLoad += Main_onLoad;
        }

        //asrc.clip = R2Audio.APM.DecodeFile(apmPath);
        void Start() {
            switch (Main.lvlName.ToLower()) {
                case "learn_10": tunes = new RayTune[] {
                    new RayTune(100, "https://raytunes.raymanpc.com/music/R2/003%20-%20The%20Woods%20of%20Light%20~First%20Dawn~.mp3",
                    0, 8),
                    new RayTune(100, "https://raytunes.raymanpc.com/music/R2/004%20-%20The%20Woods%20of%20Light.mp3",
                    0, 4, 12, 22, 38),
                    new RayTune(100, "https://raytunes.raymanpc.com/music/R2/005%20-%20The%20Woods%20of%20Light%20(Reprise).mp3",
                    0, 8),
                }; break;

                case "learn_30": tunes = new RayTune[] {
                    new RayTune(129, "https://raytunes.raymanpc.com/music/R2/013%20-%20The%20Fairy%20Glade.mp3",
                    0, 8, 24, 48, 72, 80, 96),
                }; break;

                case "learn_31": tunes = new RayTune[] {
                    new RayTune(129, "https://raytunes.raymanpc.com/music/R2/013%20-%20The%20Fairy%20Glade.mp3",
                    0, 8, 24, 48, 72, 80, 96),
                    new RayTune(129, "https://raytunes.raymanpc.com/music/R2/014%20-%20The%20Fairy%20Glade%20(Reprise).mp3",
                    0, 16, 24),
                }; break;

                case "chase_10": tunes = new RayTune[] {
                    new RayTune(84, "https://raytunes.raymanpc.com/music/R2/034%20-%20The%20Bayou%20~The%20Warship~.mp3",
                    0, 24, 32, 48),
                    new RayTune(65, "https://raytunes.raymanpc.com/music/R2/035%20-%20The%20Bayou%20~Dark%20Swamp~.mp3",
                    0, 16, 32),
                    new RayTune(103, "https://raytunes.raymanpc.com/music/R2/020%20-%20A%20Small%20Skirmish%20(Reprise).mp3",
                    0, 16),
                }; break;

                case "chase_22": tunes = new RayTune[] {
                    new RayTune(110, "https://raytunes.raymanpc.com/music/R2/036%20-%20The%20Bayou%20~The%20Pirate%20Base~.mp3",
                    0, 8, 24),
                    new RayTune(110, "https://raytunes.raymanpc.com/music/R2/037%20-%20The%20Bayou%20~The%20Pirate%20Base~%20(Reprise).mp3",
                    0, 8, 24),
                    new RayTune(103, "https://raytunes.raymanpc.com/music/R2/020%20-%20A%20Small%20Skirmish%20(Reprise).mp3",
                    0, 16),
                }; break;

                case "water_10":
                    tunes = new RayTune[] {
                    new RayTune(136, "https://raytunes.raymanpc.com/music/R2/040%20-%20The%20Sanctuary%20of%20Water%20and%20Ice%20(Reprise%202).mp3",
                    0, 8),
                    new RayTune(136, "https://raytunes.raymanpc.com/music/R2/039%20-%20The%20Sanctuary%20of%20Water%20and%20Ice%20(Reprise%201).mp3",
                    0, 8, 12, 28),
                    new RayTune(136, "https://raytunes.raymanpc.com/music/R2/038%20-%20The%20Sanctuary%20of%20Water%20and%20Ice.mp3",
                    0, 8, 12, 28),
                }; break;

                case "water_20": tunes = new RayTune[] {
                    new RayTune(160, "https://raytunes.raymanpc.com/music/R2/042%20-%20The%20Celestial%20Slide%20(Reprise).mp3",
                    0, 16, 28),
                    new RayTune(105, "https://raytunes.raymanpc.com/music/R2/043%20-%20Guardian%20of%20the%20Mask.mp3",
                    0, 16)
                }; break;

                case "plum_00": tunes = new RayTune[] {
                    new RayTune(105, "https://raytunes.raymanpc.com/music/R2/072%20-%20Into%20the%20Sanctuary%20of%20Stone%20and%20Fire.mp3",
                    0, 15),
                    new RayTune(184, "https://raytunes.raymanpc.com/music/R2/073%20-%20Extreme%20Heat.mp3",
                    0, 16, 24), 
                }; break;

                case "plum_10":
                    tunes = new RayTune[] {
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/079%20-%20The%20Lava%20Stream.mp3",
                    0, 12),
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/080%20-%20The%20Lava%20Stream%20(Reprise%201).mp3",
                    0, 12),
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/081%20-%20The%20Lava%20Stream%20(Reprise%202).mp3",
                    0, 12),
                }; break;

                case "plum_20":
                    tunes = new RayTune[] {
                    new RayTune(122, "https://raytunes.raymanpc.com/music/R2/082%20-%20The%20Ancient%20Lava%20Temple.mp3",
                    0, 12, 16),
                }; break;

                case "helic_10":
                    tunes = new RayTune[] {
                    new RayTune(120, "https://raytunes.raymanpc.com/music/R2/109%20-%20Hot%20Air%20Flight.mp3",
                    0, 8, 12, 20, 28),
                }; break;

                case "helic_20":
                    tunes = new RayTune[] {
                    new RayTune(120, "https://raytunes.raymanpc.com/music/R2/109%20-%20Hot%20Air%20Flight.mp3",
                    0, 8, 12, 20, 28),
                }; break;

                case "learn_40":
                    tunes = new RayTune[] {
                    new RayTune(100, "https://raytunes.raymanpc.com/music/R2/021%20-%20Pirate%20Machinery.mp3",
                    0, 8, 16),
                }; break;
            }
        }
    }
}
