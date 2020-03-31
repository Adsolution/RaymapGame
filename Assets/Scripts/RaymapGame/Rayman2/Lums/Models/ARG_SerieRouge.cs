//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// Red Lums Timed Sequence
    /// </summary>
    public partial class ARG_SerieRouge : Alw_Lums_Model {
        protected override void OnStart() {
            if (!all.Contains(this))
                all.Add(this);
            type = LumType.Red;
            attractRadius = 0;
            base.OnStart();
        }

        static List<ARG_SerieRouge> all = new List<ARG_SerieRouge>();
        static Timer t_poof = new Timer();

        public override void OnCollect(PersoController collector) {
            collector.Heal(20);
            if (t_poof.active) return;
            float time = GetDsgVar<float>("Float_0");
            time = 6.5f;

            t_poof.Start(time, () => {
                foreach (var l in all) l.Remove();
            }, false);
        }
    }
}