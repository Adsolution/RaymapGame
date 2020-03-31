//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Big Bayou collapsing bridge
    /// </summary>
    public partial class OLD_ponton : ponton {
        protected override void OnStart() {
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (StoodOnBy(mainActor))
                SetRule("Shake");
        }

        float rvel;
        protected void Rule_Shake() {
            if (newRule)
                Timers("Fall").Start(4, () => SetRule("Fall"));

            for (int c = 0; c < 3; c++) {
                channels[c].rot = Quaternion.Slerp(channels[c].rot, channels[c].startRot * (Quaternion.Euler((c % 2 == 0 ? 1 : -1) *
                    new Vector3(Random.value * 20 * Mathf.Sin(rvel += dt * 10 * Random.value), 0, 0)
                + startRot.eulerAngles)), dt * 30);
            }
        }

        float[] chVel = new float[3];
        Vector3[] chRotVel = new Vector3[3];
        protected void Rule_Fall() {
            if (newRule)
                for (int c = 0; c < 3; c++)
                    Timers($"ChFall{c}").Start((float)c / 4, ()
                        => chRotVel[c] = Random.rotation.eulerAngles * 2);

            for (int c = 0; c < 3; c++)
                if (Timers($"ChFall{c}").finished) {
                    chVel[c] += -20 * dt;
                    channels[c].pos.y += chVel[c] * dt;
                    channels[c].rot *= Quaternion.Euler(chRotVel[c] * dt);
                }
        }
    }
}