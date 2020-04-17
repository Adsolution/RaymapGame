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
            foreach (var s in scripts["ARG_Teleport_Tableau"]) {
                bool found = false;
                if (s.TranslatedScript.Contains($"UByte_5 == {UByte_5}")) {

                    foreach (var l in s.TranslatedScript.Split(new char[] { '\n', '\r', }, System.StringSplitOptions.RemoveEmptyEntries))
                        if (!found && l.Contains($"UByte_5 == {UByte_5}"))
                            found = true;
                        else if (found && l.Contains("Proc_ChangeMap"))
                            return l.Split('\"')[1];
                }
            }
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
                    rayman.ForceNav(graph, () => ChangeMap(level));
                else ChangeMap(level);
            }
        }
    }
}