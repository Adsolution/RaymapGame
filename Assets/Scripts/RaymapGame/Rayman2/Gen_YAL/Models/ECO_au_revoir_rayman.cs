//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Rayman teleport sphere
    /// </summary>
    public partial class ECO_au_revoir_rayman : Gen_YAL {
        public Vector3 telePos;
        public float radius;
        protected override void OnStart() {
            telePos = GetTelePos(GetDsgVar<int>("Int_0"));
            radius = GetDsgVar<float>("Float_2");
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (DistTo(rayman) < radius) {
                if (rayman.hasMount) rayman.mount.pos = telePos;
                rayman.pos = telePos;
            }
        }


        public static Vector3 GetTelePos(int Int_0) {
            switch (Int_0) {
                case 110: return GetPerso("MIC_CheckP_2eSM").pos;
            }
            return new Vector3();
        }
    }
}