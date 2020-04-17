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

        public static bool start { get; private set; } = true;
        private void Main_onLoad(object sender, EventArgs e) {
            if (tunes == null) return;
            foreach (var t in tunes)
                t.clip = t.GetDownloadedClip();
        }

        const int RATE = 22050;

        public static void SetSection(int section, int bar, bool loop = true, Action onFinishAction = null) {
            if (tune.clip == null) return;
            var cl = tune.clip;
            int s1 = tune.sectBars[section];
            int s2 = section + 1 >= tune.sectBars.Length ? tune.clip.samples - s1 : tune.sectBars[section + 1];
            int newSmpStart = (int)(RATE * 2 * s1 * tune.barLength);
            int newSmpLength = (int)((RATE * 2 * s2 * tune.barLength) - (s1 * tune.barLength * RATE * 2));
            
            var newCl = AudioClip.Create($"{cl.name}_sec{section}", newSmpLength / 2, 2, RATE, false);
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
            if (!Main.loaded || tunes == null)
                return;
            
            foreach (var t in tunes.Where((x) => x.clip == null))
                t.clip = t.GetDownloadedClip();

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
            inst = this;
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
                    new RayTune(100, "http://www.mediafire.com/file/6w8rcztlvarorh1/003_-_The_Woods_of_Light_%257EFirst_Dawn%257E.ogg/file",
                    0, 8),
                    new RayTune(100, "http://www.mediafire.com/file/zfaobqvg5u7ky3p/004_-_The_Woods_of_Light.ogg/file",
                    0, 4, 12, 22, 38),
                    new RayTune(100, "http://www.mediafire.com/file/3scnkfcjpurqifm/005_-_The_Woods_of_Light_%2528Reprise%2529.ogg/file",
                    0, 8),
                }; break;

                case "learn_30": tunes = new RayTune[] {
                    new RayTune(129, "http://www.mediafire.com/file/v06te9ql1aij3y7/013_-_The_Fairy_Glade.ogg/file",
                    0, 8, 24, 48, 72, 80, 96),
                    new RayTune(129, "http://www.mediafire.com/file/jgpqssh77rx61z9/014_-_The_Fairy_Glade_%2528Reprise%2529.ogg/file",
                    0, 16, 24),
                }; break;

                case "learn_31": tunes = new RayTune[] {
                    new RayTune(129, "http://www.mediafire.com/file/v06te9ql1aij3y7/013_-_The_Fairy_Glade.ogg/file",
                    0, 8, 24, 48, 72, 80, 96),
                    new RayTune(129, "http://www.mediafire.com/file/jgpqssh77rx61z9/014_-_The_Fairy_Glade_%2528Reprise%2529.ogg/file",
                    0, 16, 24),
                }; break;

                case "chase_10": tunes = new RayTune[] {
                    new RayTune(84, "http://www.mediafire.com/file/aqaefmr4r11gvu5/034_-_The_Bayou_%257EThe_Warship%257E.ogg/file",
                    0, 24, 32, 48),
                    new RayTune(65, "http://www.mediafire.com/file/ukppyoolqaj4zwf/035_-_The_Bayou_%257EDark_Swamp%257E.ogg/file",
                    0, 16, 32),
                    new RayTune(103, "https://raytunes.raymanpc.com/music/R2/020%20-%20A%20Small%20Skirmish%20(Reprise).mp3",
                    0, 16),
                }; break;

                case "chase_22": tunes = new RayTune[] {
                    new RayTune(110, "http://www.mediafire.com/file/isgv4x9b4qawhr7/036_-_The_Bayou_%257EThe_Pirate_Base%257E.ogg/file",
                    0, 8, 24),
                    new RayTune(110, "http://www.mediafire.com/file/p3tp1rz707w5kj2/037_-_The_Bayou_%257EThe_Pirate_Base%257E_%2528Reprise%2529.ogg/file",
                    0, 8, 24),
                    new RayTune(103, "https://raytunes.raymanpc.com/music/R2/020%20-%20A%20Small%20Skirmish%20(Reprise).mp3",
                    0, 16),
                }; break;

                case "water_10": tunes = new RayTune[] {
                    new RayTune(136, "http://download1514.mediafire.com/dkgjdnxrvqqg/3loqk8q3kvir3ei/040+-+The+Sanctuary+of+Water+and+Ice+%28Reprise+2%29.ogg",
                    0, 8),
                    new RayTune(136, "http://download1649.mediafire.com/1pf5d7mjwuqg/jt9j1yatmttajxf/039+-+The+Sanctuary+of+Water+and+Ice+%28Reprise+1%29.ogg",
                    0, 8, 12, 28),
                    new RayTune(136, "http://download850.mediafire.com/k5ollidh0rfg/vkrorlbd79qiry1/038+-+The+Sanctuary+of+Water+and+Ice.ogg",
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

                case "whale_00":
                case "whale_05": tunes = new RayTune[] {
                    new RayTune(75, "http://www.mediafire.com/file/kbnbbz0cr6veyj9/070_-_The_Whale_Bay_%257EShallow_Harbour%257E.ogg/file",
                    0, 12),
                    new RayTune(78, "http://www.mediafire.com/file/wyuinjqcjjg2ans/071_-_The_Whale_Bay_%257EBubble_Trail%257E.ogg/file",
                    0, 2),
                }; break;

                case "whale_10": tunes = new RayTune[] {
                    new RayTune(78, "http://www.mediafire.com/file/wyuinjqcjjg2ans/071_-_The_Whale_Bay_%257EBubble_Trail%257E.ogg/file",
                    0, 16, 36, 56),
                }; break;

                case "plum_10": tunes = new RayTune[] {
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/079%20-%20The%20Lava%20Stream.mp3",
                    0, 12),
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/080%20-%20The%20Lava%20Stream%20(Reprise%201).mp3",
                    0, 12),
                    new RayTune(95, "https://raytunes.raymanpc.com/music/R2/081%20-%20The%20Lava%20Stream%20(Reprise%202).mp3",
                    0, 12),
                }; break;

                case "plum_20": tunes = new RayTune[] {
                    new RayTune(122, "https://raytunes.raymanpc.com/music/R2/082%20-%20The%20Ancient%20Lava%20Temple.mp3",
                    0, 12, 16),
                }; break;

                case "earth_10": tunes = new RayTune[] {
                    new RayTune(75, "https://www.mediafire.com/file/nq2vyt710v2rmaq/Occluded_Woods-wip5.ogg/file",
                    0, 18, 20, 24, 48, 56, 72, 80, 88, 96),
                }; break;

                case "helic_10": tunes = new RayTune[] {
                    new RayTune(120, "http://www.mediafire.com/file/q2vg4fw28r01xvh/109_-_Hot_Air_Flight.ogg/file",
                    0, 8, 12, 20, 28),
                    new RayTune(120, "http://www.mediafire.com/file/1lma0fk342c1np5/110_-_Hot_Air_Flight_%2528Reprise%2529.ogg/file",
                    0, 9, 12),
                }; break;

                case "helic_20": tunes = new RayTune[] {
                    new RayTune(120, "http://www.mediafire.com/file/q2vg4fw28r01xvh/109_-_Hot_Air_Flight.ogg/file",
                    0, 8, 12, 20, 28),
                }; break;

                case "learn_40": tunes = new RayTune[] {
                    new RayTune(100, "https://www.mediafire.com/file/kotwq7mf4wxda4t/021_-_Pirate_Machinery.ogg/file",
                    0, 8, 16),
                    new RayTune(100, "https://www.mediafire.com/file/16h9kx1kejpl16d/022_-_Pirate_Machinery_%28Reprise%29.ogg/file",
                    0, 8, 16),
                }; break;

            }
        }
    }
}
