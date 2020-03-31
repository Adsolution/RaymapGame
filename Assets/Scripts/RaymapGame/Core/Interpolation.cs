//================================
//  By: Adsolution
//================================

using UnityEngine;

namespace RaymapGame {
    public interface IInterpolate {
        bool interpolate { get; }
        Vector3 interpolPos { get; }
        Quaternion interpolRot { get; }
    }

    public class Interpolation : MonoBehaviour {
        public static float deltaPosThreshold = 8;

        Vector3 pprev;
        Quaternion rprev;
        public MonoBehaviour fixedTimeController;
        IInterpolate ipl;

        void Update() {
            if (fixedTimeController == null) return;
            if (ipl == null) {
                if (fixedTimeController is IInterpolate i)
                    ipl = i;
                else return;
            }

            if (ipl.interpolate) {
                if ((ipl.interpolPos - pprev).magnitude < deltaPosThreshold)
                    transform.position = Vector3.Slerp(pprev, ipl.interpolPos, Time.deltaTime / Time.fixedDeltaTime);
                else transform.position = ipl.interpolPos;
                pprev = transform.position;


                transform.rotation = Quaternion.Slerp(rprev, ipl.interpolRot, Time.deltaTime / Time.fixedDeltaTime);
                rprev = transform.rotation;
            }
            else {
                transform.position = ipl.interpolPos;
                transform.rotation = ipl.interpolRot;
            }
        }
    }
}