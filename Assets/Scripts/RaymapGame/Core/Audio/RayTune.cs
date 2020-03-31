//================================
//  By: Adsolution
//================================
using UnityEngine;
using UnityEngine.Networking;

namespace RaymapGame {
    public class RayTune {
        public RayTune(float bpm, string url, params int[] sectBars) {
            this.bpm = bpm;
            this.url = url;
            this.sectBars = sectBars;
            DownloadClip();
        }

        UnityWebRequest dClip;
        public AudioClip clip;
        public float bpm;
        public string url;
        public int[] sectBars;
        public int tailBars = 1;

        public float barLength => 60 / bpm * 4;
        public const int MP3_DELAY = 528;

        public void DownloadClip() {
            dClip = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            dClip.SendWebRequest();
        }
        public AudioClip GetDownloadedClip() {
            if (dClip.isDone && clip == null)
                return DownloadHandlerAudioClip.GetContent(dClip);
            else return null;
        }
    }
}