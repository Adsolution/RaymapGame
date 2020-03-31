//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame {
    public class Channel {
        public Channel(Transform chTr) {
            tr = chTr;
            pos = startPos = chTr.position;
            rot = startRot = chTr.rotation;
        }

        public readonly Transform tr;
        public readonly Vector3 startPos;
        public readonly Quaternion startRot;
        public readonly bool startVisible = true;
        public Vector3 pos;
        public Quaternion rot;
        public bool visible = true;
    }
}
