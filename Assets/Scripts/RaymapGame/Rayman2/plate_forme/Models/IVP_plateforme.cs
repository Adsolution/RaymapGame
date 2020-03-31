//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Floating Flower
    /// </summary>
    public partial class IVP_plateforme : plate_forme {
        protected override void OnStart() {
            SetRule("WaitForRayman");
            navRotSpeed = 0;
            moveSpeed = 4;
            SetFriction(3, 3);
        }

        protected void Rule_WaitForRayman() {
			if (StoodOnBy(rayman)) {
                SetRule("Travel");
            }
        }

        protected void Rule_Travel() {
            NavNearestWaypointGraph();
            if (StoodOnBy(rayman))
                rayman.col.StickToPlatformCentre();
        }
    }
}