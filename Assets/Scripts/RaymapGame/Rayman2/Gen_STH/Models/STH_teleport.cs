//================================
//  By: Adsolution
//================================
using System.Linq;

namespace RaymapGame.Rayman2.Persos {
    /// <summary>
    /// End level sequence/map load
    /// </summary>
    public partial class STH_teleport : Gen_STH {
        public WaypointGraph graph;
        public string level;
        public float radius;

        public string GetLevelFromTable(byte UByte_5) {
            foreach (var s in scripts["ARG_Teleport_Tableau"])
                if (s.TranslatedScript.Contains($"UByte_5 == {UByte_5}"))
                    foreach (var line in s.TranslatedScript.Split(new char[] { '\n', '\r', }, System.StringSplitOptions.RemoveEmptyEntries)
                        .Where((x) => x.Contains("Proc_ChangeMap")))
                        return line.Split('\"')[1];
            return "";
        }

        protected override void OnStart() {
            radius = GetDsgVar<float>("Float_2");
            level = GetLevelFromTable(GetDsgVar<byte>("UByte_5"));
            SetRule("Default");
        }

        protected void Rule_Default() {
            if (level != "" && DistTo(rayman) < radius) {
                SetRule("");

                if ((graph = GetDsgVar<WaypointGraph>("Graph_1")) != null)
                    rayman.ForceNav(graph, () => LoadLevel(level));
                else LoadLevel(level);
            }
        }
    }
}