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
        public float _volume = 0.75f;
        static MusicHandler inst;
        public static float volume { get => inst._volume; set { inst._volume = value; } }
        public static RayTune[] tunes;
        public static RayTune tune;
        public static AudioSource asrc;

        public static string serverAddress = "https://raym.app/data/raymapgame/music/";

        public static bool start { get; private set; } = true;
        private void Main_onLoad(object sender, EventArgs e) {
            Main.onLoad -= Main_onLoad;
        }

        const int RATE = 22050;

        public static void SetSection(int section, int bar, bool loop = true, Action onFinishAction = null) {
            if (tune.clip == null) return;
            var cl = tune.clip;
            int s1 = tune.sectBars[section];
            int s2 = section + 1 >= tune.sectBars.Length ? tune.clip.samples - s1 : tune.sectBars[section + 1];
            int newSmpStart = (int)(RATE * 2 * s1 * tune.barLength);
            int newSmpLength = (int)((RATE * 2 * s2 * tune.barLength) - (s1 * tune.barLength * RATE * 2));
            int barSmp = (int)(RATE * 2 * bar * tune.barLength);

            /*var newCl = AudioClip.Create($"{cl.name}_sec{section}", newSmpLength / 2, 2, RATE, false);
            float[] data = new float[newSmpLength];
            tune.clip.GetData(data, Mathf.Clamp(newSmpStart / 2, 0, int.MaxValue));
            newCl.SetData(data, 0);*/
            currentSmpStart = newSmpStart;
            currentSmpLength = newSmpLength;
            currentLoop = loop;

            fading = false;
            asrc.volume = volume;
            //asrc.clip = newCl;
            if(asrc.clip != cl) asrc.clip = cl;
            asrc.loop = MusicHandler.loop = loop;
            action = onFinishAction;
            MusicHandler.section = section;
            tr = false;
            asrc.timeSamples = Mathf.Clamp(newSmpStart / 2, 0, int.MaxValue) + barSmp;
            asrc.Play();
        }

        static int currentSmpStart, currentSmpLength;
        static bool currentLoop;
        public static int bar => tune == null ? 0 : Mathf.FloorToInt((((asrc.timeSamples - currentSmpStart) / (float)RATE / 2f) + 0.05f) / tune.barLength);
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

        public static void Next()
            => Next(null);
        public static void Next(Action onFinishAction) {
            QueueMusic(tuneIdx, section + 1, 0, onFinishAction);
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
            if (Main.lvlName == null)
                return;
            if (tunes == null)
                GetTunes();
            if (Main.loadState != Main.LoadState.Loaded || tunes == null)
                return;

            if (asrc.isPlaying) {
                if (asrc.timeSamples >= currentSmpStart + currentSmpLength) {
                    if (currentLoop) {
                        asrc.timeSamples = currentSmpStart;
                    } else {
                        asrc.Stop();
                    }
                }
            }

            if (!asrc.isPlaying && action != null) {
                var ac = action;
                action = null;
                ac.Invoke();
            }

            var world = ((Rayman2.Persos.World)PersoController.GetPerso("World"));
            if (world != null) world.Music();

            if (tr && (tune == null || bar >= trBar || !asrc.isPlaying))
                SetMusic(tuneIdx, section, newBar, loop, action);

            if (tune != null && fading) {
                asrc.volume = Mathf.Clamp01(asrc.volume - Time.fixedDeltaTime / (fadeBars * tune.barLength * volume));
                if (asrc.volume == 0) {
                    fading = false;
                    tune = null;
                    tuneIdx = -1;
                    asrc.Stop();
                    if (fadeAction != null)
                        fadeAction.Invoke();
                }
            }

            start = false;
        }


        void Awake() {
            // Reset static values
            start = true;
            tunes = null;
            tune = null;
            inst = this;
            section = -1;
            trBar = -1;
            newBar = -1;
            fading = false;
            fadeBars = 0f;
            fadeAction = null;
            tr = false;
            tuneIdx = -1;
            loop = false;
            // init
            asrc = gameObject.AddComponent<AudioSource>();
            asrc.loop = true;
            asrc.outputAudioMixerGroup = musicMixerGroup;
            asrc.volume = 0.7f;
            asrc.playOnAwake = false;
            Main.onLoad += Main_onLoad;
        }

        //asrc.clip = R2Audio.APM.DecodeFile(apmPath);

        void GetTunes() {
            switch (Main.lvlName.ToLower()) {
                case "learn_10": tunes = new RayTune[] {
                    new RayTune(100, serverAddress + "003 - The Woods of Light ~First Dawn~.wav",
                    0, 8),
                    new RayTune(100, serverAddress + "004 - The Woods of Light.wav",
                    0, 4, 12, 22, 38),
                    new RayTune(100, serverAddress + "005 - The Woods of Light (Reprise).wav",
                    0, 8),
                }; break;

                case "learn_30": tunes = new RayTune[] {
                    new RayTune(129, serverAddress + "013 - The Fairy Glade.wav",
                    0, 8, 24, 48, 72, 80, 96),
                    new RayTune(129, serverAddress + "014 - The Fairy Glade (Reprise).wav",
                    0, 16, 24),
                }; break;

                case "learn_31": tunes = new RayTune[] {
                    new RayTune(129, serverAddress + "013 - The Fairy Glade.wav",
                    0, 8, 24, 48, 72, 80, 96),
                    new RayTune(129, serverAddress + "014 - The Fairy Glade (Reprise).wav",
                    0, 16, 24),
                }; break;

                case "chase_10": tunes = new RayTune[] {
                    new RayTune(84, serverAddress + "034 - The Bayou ~The Warship~.wav",
                    0, 24, 32, 48),
                    new RayTune(65, serverAddress + "035 - The Bayou ~Dark Swamp~.wav",
                    0, 16, 32),
                    new RayTune(103, serverAddress + "020 - A Small Skirmish (Reprise).wav",
                    0, 16),
                }; break;

                case "chase_22": tunes = new RayTune[] {
                    new RayTune(110, serverAddress + "036 - The_Bayou ~The Pirate Base~.wav",
                    0, 8, 24),
                    new RayTune(110, serverAddress + "037 - The_Bayou ~The Pirate Base~ (Reprise).wav",
                    0, 8, 24),
                    new RayTune(103, serverAddress + "020 - A Small Skirmish (Reprise).wav",
                    0, 16),
                }; break;

                case "water_10": tunes = new RayTune[] {
                    new RayTune(136, serverAddress + "040 - The Sanctuary of Water and Ice (Reprise 2).wav",
                    0, 8),
                    new RayTune(136, serverAddress + "039 - The Sanctuary of Water and Ice (Reprise 1).wav",
                    0, 8, 12, 28),
                    new RayTune(136, serverAddress + "038 - The Sanctuary of Water and Ice.wav",
                    0, 8, 12, 28),
                }; break;

                case "water_20": tunes = new RayTune[] {
                    new RayTune(160, serverAddress + "042 - The Celestial Slide (Reprise).wav",
                    0, 16, 28),
                    new RayTune(105, serverAddress + "043 - Guardian of the Mask.wav",
                    0, 16)
                }; break;

                case "plum_00": tunes = new RayTune[] {
                    new RayTune(105, serverAddress + "072 - Into the Sanctuary of Stone and Fire.wav",
                    0, 15),
                    new RayTune(184, serverAddress + "073 - Extreme Heat.wav",
                    0, 16, 24), 
                }; break;

                case "whale_00":
                case "whale_05": tunes = new RayTune[] {
                    new RayTune(75, serverAddress + "070 - The Whale Bay ~Shallow Harbour~.wav",
                    0, 12),
                    new RayTune(78, serverAddress + "071 - The Whale Bay ~Bubble Trail~.wav",
                    0, 2),
                }; break;

                case "whale_10": tunes = new RayTune[] {
                    new RayTune(78, serverAddress + "071 - The Whale Bay ~Bubble Trail~.wav",
                    0, 16, 36, 56),
                }; break;

                case "plum_10": tunes = new RayTune[] {
                    new RayTune(95, serverAddress + "079 - The Lava Stream.wav",
                    0, 12),
                    new RayTune(95, serverAddress + "080 - The Lava Stream (Reprise 1).wav",
                    0, 12),
                    new RayTune(95, serverAddress + "081 - The Lava Stream (Reprise 2).wav",
                    0, 12),
                }; break;

                case "plum_20": tunes = new RayTune[] {
                    new RayTune(122, serverAddress + "082 - The Ancient Lava Temple.wav",
                    0, 12, 16),
                }; break;

                case "earth_10": tunes = new RayTune[] {
                    new RayTune(75, serverAddress + "Occluded Woods-wip5.wav",
                    0, 18, 20, 24, 48, 56, 72, 80, 88, 96),
                }; break;

                case "helic_10": tunes = new RayTune[] {
                    new RayTune(120, serverAddress + "109 - Hot Air Flight.wav",
                    0, 8, 12, 20, 28),
                    new RayTune(120, serverAddress + "110 - Hot Air Flight (Reprise).wav",
                    0, 9, 12),
                }; break;

                case "helic_20": tunes = new RayTune[] {
                    new RayTune(120, serverAddress + "109 - Hot Air Flight.wav",
                    0, 8, 12, 20, 28),
                }; break;

                case "learn_40": tunes = new RayTune[] {
                    new RayTune(100, serverAddress + "021 - Pirate Machinery.wav",
                    0, 8, 16),
                    new RayTune(100, serverAddress + "022 - Pirate Machinery (Reprise).wav",
                    0, 8, 16),
                }; break;

            }
        }
    }
}
