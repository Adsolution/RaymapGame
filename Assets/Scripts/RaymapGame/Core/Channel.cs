//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame {
    public class Channel {
        public Channel(Transform chTr) {
            tr = chTr;
            pos = startPos = chTr.position;
            rot = startRot = chTr.rotation.eulerAngles - new Vector3(0, 180, 0);
        }

        public readonly Transform tr;
        public readonly Vector3 startPos;
        public readonly Vector3 startRot;
        public readonly bool startVisible = true;
        public Vector3 pos;
        public Vector3 rot;
        public bool visible = true;
    }
}
