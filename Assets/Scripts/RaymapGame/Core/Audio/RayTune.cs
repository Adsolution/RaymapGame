//================================
//  By: Adsolution
//================================
using Cysharp.Threading.Tasks;
using OpenSpace;
using UnityEngine;
using UnityEngine.Networking;

namespace RaymapGame {
    public class RayTune {
        public RayTune(float bpm, string url, params int[] sectBars) {
            this.bpm = bpm;
            this.url = url;
            this.sectBars = sectBars;
            _ = DownloadClip();
        }
        public AudioClip clip;
        public float bpm;
        public string url;
        public int[] sectBars;
        public int tailBars = 1;

        public float barLength => 60 / bpm * 4;
        public const int MP3_DELAY = 528;

        public async UniTaskVoid DownloadClip() {
            using (var dClip = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV)) {

                //((DownloadHandlerAudioClip)dClip.downloadHandler).streamAudio = true;
                try {
                    await dClip.SendWebRequest();
                } catch (UnityWebRequestException) {
                } finally {
                    if (!dClip.isHttpError && !dClip.isNetworkError) {
                        clip = DownloadHandlerAudioClip.GetContent(dClip);
                    } else {
                        UnityEngine.Debug.Log("Web request error for url: " + url);
                    }
                }
            }
        }
    }
}