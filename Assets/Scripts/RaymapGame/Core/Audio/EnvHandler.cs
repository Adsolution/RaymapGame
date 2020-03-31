//================================
//  By: Adsolution
//================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RaymapGame
{
    public class EnvHandler : MonoBehaviour
    {
        AudioSource asrc;
        public static AudioMixer mixer;
        public AudioMixerGroup envGroup;

        void Awake() {
            mixer = ResManager.Get<AudioMixer>("Environment");
        }

        public void Enable() {
            asrc = gameObject.AddComponent<AudioSource>();
            asrc.loop = true;
            asrc.playOnAwake = false;

            // Ambience track
            switch (Main.lvlName.ToLower()) {
                case "learn_10":
                case "rodeo_10":
                    asrc.clip = GetAmbience("forest1");
                    asrc.volume = 0.3f; break;

                case "learn_30":
                case "rodeo_60":
                case "ly_10":
                case "ly_20":
                    asrc.clip = GetAmbience("glade");
                    asrc.volume = 0.2f; break;

                case "learn_31":
                case "rodeo_40":
                case "glob_10":
                case "glob_20":
                    asrc.clip = GetAmbience("forest2");
                    asrc.volume = 0.3f; break;

                case "ski_10":
                case "chase_10":
                case "chase_22":
                case "earth_10":
                case "glob_30":
                    asrc.clip = GetAmbience("swamp");
                    asrc.volume = 0.3f; break;

                case "earth_20":
                case "helic_20":
                    asrc.clip = GetAmbience("temple2");
                    asrc.volume = 0.4f; break;

                case "earth_30":
                    asrc.clip = GetAmbience("temple1");
                    asrc.volume = 0.8f; break;



                case "water_10":
                case "whale_05":
                case "ile_10":
                    asrc.clip = GetAmbience("beach");
                    asrc.volume = 0.7f; break;

                case "ski_20":
                case "mine_10":
                case "bast_20":
                case "bast_10":
                case "poloc_10":
                case "poloc_20":
                case "poloc_30":
                case "poloc_40":
                    asrc.clip = GetAmbience("night");
                    asrc.volume = 0.15f; break;

                case "learn_60":
                case "cask_10":
                    asrc.clip = GetAmbience("cellar");
                    asrc.volume = 0.35f; break;

                case "cask_30":
                case "morb_10":
                    asrc.clip = GetAmbience("sewer");
                    asrc.volume = 0.35f; break;

                case "morb_00":
                case "morb_20":
                    asrc.clip = GetAmbience("tomb");
                    asrc.volume = 0.5f; break;

                case "bast_22":
                case "whale_00":
                case "astro_00":
                    asrc.clip = GetAmbience("electronics");
                    asrc.volume = 0.3f; break;

                case "whale_10":
                    asrc.clip = GetAmbience("underwater");
                    asrc.volume = 0.5f; break;

                case "plum_00":
                case "astro_10":
                    asrc.clip = GetAmbience("wind");
                    asrc.volume = 0.5f; break;

                case "plum_10":
                case "plum_20":
                case "helic_10":
                case "helic_30":
                    asrc.clip = GetAmbience("lava");
                    asrc.volume = 0.5f; break;

                case "boat01":
                case "boat02":
                    asrc.clip = GetAmbience("factory");
                    asrc.volume = 0.5f; break;

                case "vulca_10":
                case "vulca_20":
                    asrc.clip = GetAmbience("cave");
                    asrc.volume = 0.35f; break;
                    /*
                case "learn_40":
                case "bast_09":
                case "bast_10":
                    asrc.clip = GetAmbience("rain0");
                    asrc.volume = 0.3f; break;*/

            }

            // Audio Env Effects
            switch (Main.lvlName.ToLower()) {
                default: envGroup = mixer.FindMatchingGroups("EnvDefault")[0]; break;

                /*
            case "Earth_20":
            case "Helic_20":
                asrc.clip = GetAmbience("temple2");
                asrc.volume = 0.4f; break;

            case "Earth_30":
                asrc.clip = GetAmbience("temple1");
                asrc.volume = 0.8f; break;*/


                /*
            case "Water_10":
            case "Whale_05":
            case "ile_10":
                asrc.clip = GetAmbience("beach");
                asrc.volume = 0.7f; break;*/

                case "vulca_10":
                case "vulca_20":
                case "morb_00":
                case "morb_20":
                case "cask_30":
                case "morb_10":
                case "bast_22":
                case "learn_60":
                case "cask_10":
                    envGroup = mixer.FindMatchingGroups("EnvCave")[0]; break;
                    /*
                case "Whale_00":
                case "Astro_00":
                    asrc.clip = GetAmbience("electronics");
                    asrc.volume = 0.3f; break;

                case "Whale_10":
                    asrc.clip = GetAmbience("underwater");
                    asrc.volume = 0.5f; break;

                case "Plum_10":
                case "Plum_20":
                case "Helic_10":
                case "Helic_30":
                    asrc.clip = GetAmbience("lava");*/

            }


            asrc.Stop();
            FadeIn(2);
        }

        AudioClip GetAmbience(string name) => ResManager.Get<AudioClip>("Sounds/Rayman2/ambience/" + name);


        public void FadeIn(float seconds) { StopAllCoroutines(); StartCoroutine(cr_FadeIn(seconds)); }
        IEnumerator cr_FadeIn(float seconds) {
            asrc.Play();
            float oldV = asrc.volume;
            for (float v = 0; v < oldV; v += Time.deltaTime * oldV * seconds)
            {
                asrc.volume = v;
                yield return new WaitForEndOfFrame();
            }
            asrc.volume = oldV;
            yield return null;
        }
    }
}