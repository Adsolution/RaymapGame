﻿using OpenSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace RaymapGame.Rayman2 {
	public static class MapNames {
        private static string[] MapNamesPC = new string[] {
            "Menu",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "Mapmonde",
            "Learn_30",
            "Learn_31",
            "Bast_20",
            "Bast_22",
            "Learn_60",
            "Ski_10",
            "Ski_60",
            "Batam_10",
            "Chase_10",
            "Chase_22",
            "Ly_10",
            "nego_10",
            "Water_10",
            "Water_20",
            "poloc_10",
            "Rodeo_10",
            "Rodeo_40",
            "Rodeo_60",
            "Vulca_10",
            "Vulca_20",
            "GLob_30",
            "GLob_10",
            "GLob_20",
            "Whale_00",
            "Whale_05",
            "Whale_10",
            "Plum_00",
            "Plum_20",
            "Plum_10",
            "poloc_20",
            "Bast_09",
            "Bast_10",
            "Cask_10",
            "Cask_30",
            "Nave_10",
            "Nave_15",
            "Nave_20",
            "Seat_10",
            "Seat_11",
            "Earth_10",
            "Earth_20",
            "Earth_30",
            "Ly_20",
            "Helic_10",
            "Helic_20",
            "Helic_30",
            "poloc_30",
            "Morb_00",
            "Morb_10",
            "Morb_20",
            "Learn_40",
            "Ball",
            "Ile_10",
            "Mine_10",
            "poloc_40",
            "Batam_20",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_10",
            "Rhop_10",
            "End_10",
            "Staff_10",
            "Bonux",
            "Raycap"
        };

        private static string[] MapNamesN64 = new string[] {
            "menu",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "mapmonde",
            "Learn_30",
            "Learn_31",
            "Bast_20",
            "Bast_22",
            "Learn_60",
            "Ski_10",
            "Ski_60",
            "Batam_10",
            "Chase_10",
            "Chase_22",
            "ly_10",
            "Nego_10",
            "Water_10",
            "Water_20",
            "Poloc_10",
            "Rodeo_10",
            "Rodeo_40",
            "Rodeo_60",
            "Vulca_10",
            "Vulca_20",
            "glob_30",
            "glob_10",
            "glob_20",
            "Whale_00",
            "Whale_05",
            "Whale_10",
            "Plum_00",
            "Plum_20",
            "Plum_10",
            "Poloc_20",
            "Bast_09",
            "Bast_10",
            "Cask_10",
            "Cask_30",
            "nave_10",
            "nave_15",
            "nave_20",
            "Seat_10",
            "Seat_11",
            "Earth_10",
            "Earth_20",
            "Earth_30",
            "ly_20",
            "helic_10",
            "helic_20",
            "helic_30",
            "Poloc_30",
            "morb_00",
            "morb_10",
            "morb_20",
            "Learn_40",
            "Ball",
            "Ile_10",
            "mine_10",
            "Poloc_40",
            "Batam_20",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_10",
            "Rhop_10",
            "End_10",
            "Staff_10",
            "Bonux",
            "RayCap"
        };

        private static string[] MapNamesPS1 = new string[] {
            "menu_st",
            "jail_10",
            "jail_20",
            "learn_10",
            "mapmonde",
            "learn_30",
            "bast_20",
            "bast_22",
            "ski_10",
            "ski_60",
            "batam_10",
            "chase_10",
            "chase_22",
            "ly_10",
            "whale_00",
            "whale_10",
            "water_20",
            "poloc",
            "rodeo_40",
            "rodeo_60",
            "vulca_10",
            "vulca_15",
            "vulca_20",
            "vulca_25",
            "glob_30",
            "glob_10",
            "glob_20",
            "plum_00",
            "plum_20",
            "plum_30",
            "plum_10",
            "plum_15",
            "bast_10",
            "cask_10",
            "cask_30",
            "nave_10",
            "nave_15",
            "nave_20",
            "earth_20",
            "earth_30",
            "ly_20",
            "helic_10",
            "helic_20",
            "helic_30",
            "morb_10",
            "morb_15",
            "morb_20",
            "learn_40",
            "ball",
            "isle_10",
            "batam_20",
            "boat01",
            "boat02",
            "astro_10",
            "fan_10",
            "boss_10",
            "end"
        };

        private static string[] MapNamesPS2 = new string[] {
            "Menu",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "Mc_10",
            "MC_10__GRP_Revisite2",
            "MC_10__GRP_Revisite3",
            "MC_15",
            "Learn_30",
            "Learn_30_Revisite1__DS1_Son",
            "Learn_31",
            "Learn_31__DS1_Son",
            "Learn_31_Revisite1__DS1_Son",
            "Bast_20",
            "Bast_20__DS1_Son",
            "Bast_20_Revisite1",
            "Bast_20_Revisite1__DS1_Son",
            "Bast_22",
            "Bast_22__DS1_Son",
            "Learn_60",
            "Learn_60__DS1_Son",
            "Learn_60_Revisite1__DS1_Son",
            "Batam_10",
            "Chase_10",
            "Chase_10__DS1_Son",
            "Chase_22",
            "Chase_22__DS1_Son",
            "Water_10",
            "Water_10__DS1_Son",
            "Water_20",
            "Water_20__DS1_Son",
            "poloc_10",
            "MC_20",
            "MC_20__XAP_Revisite2",
            "MC_20__XAP_Revisite3",
            "MC_25",
            "Rodeo_10",
            "Rodeo_40",
            "Rodeo_60",
            "Ski_10",
            "Ski_10__DS1_Son",
            "Ski_60",
            "Ski_60__DS1_Son",
            "Vulca_10",
            "Vulca_20",
            "GLob_30",
            "GLob_10",
            "Glob_10_Revisite1__DS1_Son",
            "GLob_20",
            "Glob_20_Revisite1__DS1_Son",
            "Whale_00",
            "Whale_00__DS1_Son",
            "Whale_05",
            "Whale_05__DS1_Son",
            "Whale_10",
            "Whale_10__DS1_Son",
            "Plum_00",
            "Plum_00__DS1_Son",
            "Plum_20",
            "Plum_20__DS1_Son",
            "Plum_05",
            "Plum_05__DS1_Son",
            "Plum_10",
            "Plum_10__DS1_Son",
            "Plum_15",
            "poloc_20",
            "Mc_30",
            "MC_30__POC_Revisite2",
            "MC_30__POC_Revisite3",
            "MC_31",
            "Nave_10",
            "Nave_20",
            "Cask_10",
            "Cask_10__DS1_Son",
            "Cask_30",
            "Cask_30__DS1_Son",
            "Mine_10",
            "Nave_15",
            "Bast_10",
            "Bast_10__DS1_Son",
            "Seat_11",
            "Earth_10",
            "Earth_10_Revisite1__DS1_Son",
            "Earth_20",
            "Earth_20__DS1_Son",
            "Earth_30",
            "Earth_30_Revisite1__DS1_Son",
            "Helic_10",
            "Helic_20",
            "Helic_30",
            "poloc_30",
            "Morb_00",
            "Morb_00__DS1_Son",
            "Morb_10",
            "Morb_10__DS1_Son",
            "Morb_20",
            "Learn_40",
            "Learn_40_Revisite1__DS1_Son",
            "Ball",
            "ile_10",
            "Ile_10__DS1_Son",
            "Air_00",
            "poloc_40",
            "Batam_20",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_00__DS1_Son",
            "Astro_10",
            "Astro_10__DS1_Son",
            "Liber_10",
            "Rhop_10",
            "end_10",
            "Fiesta",
            "Bonux",
            "Seat_10",
            "B_pyram",
            "B_Ski",
            "Ly_10",
            "B_Lift",
            "Ly_20",
            "B_toile",
            "RType",
            "B_disc",
            "DogFight",
            "Stade_00"
        };

        private static string[] MapNamesIOS = new string[] {
            "MENU",
            "JAIL_10",
            "JAIL_20",
            "LEARN_10",
            "MAPMONDE2",
            "LEARN_30",
            "LEARN_31",
            "BAST_20",
            "BAST_22",
            "LEARN_60",
            "SKI_10",
            "SKI_60",
            "BATAM_10",
            "CHASE_10",
            "CHASE_22",
            "LY_10",
            "NEGO_10",
            "WATER_10",
            "WATER_20",
            "POLOC_10",
            "RODEO_10",
            "RODEO_40",
            "RODEO_60",
            "VULCA_10",
            "VULCA_20",
            "GLOB_30",
            "GLOB_10",
            "GLOB_20",
            "WHALE_00",
            "WHALE_05",
            "WHALE_10",
            "PLUM_00",
            "PLUM_20",
            "PLUM_10",
            "POLOC_20",
            "BAST_09",
            "BAST_10",
            "CASK_10",
            "CASK_30",
            "NAVE_10",
            "NAVE_15",
            "NAVE_20",
            "SEAT_10",
            "SEAT_11",
            "EARTH_10",
            "EARTH_20",
            "EARTH_30",
            "LY_20",
            "HELIC_10",
            "HELIC_20",
            "HELIC_30",
            "POLOC_30",
            "MORB_00",
            "MORB_10",
            "MORB_20",
            "LEARN_40",
            "BALL",
            "ILE_10",
            "MINE_10",
            "POLOC_40",
            "BATAM_20",
            "BOAT01",
            "BOAT02",
            "ASTRO_00",
            "ASTRO_10",
            "LIBER_10",
            "RHOP_10",
            "END_10",
            "STAFF_10",
            "BONUX",
            "RAYCAP"
        };

        private static string[] MapNamesDC = new string[] {
            "MENU",
            "JAIL_10",
            "JAIL_20",
            "LEARN_10",
            "MAPMONDE2",
            "LEARN_30",
            "LEARN_31",
            "BAST_20",
            "BAST_22",
            "LEARN_60",
            "SKI_10",
            "SKI_60",
            "BATAM_10",
            "CHASE_10",
            "CHASE_22",
            "LY_10",
            "NEGO_10",
            "WATER_10",
            "WATER_20",
            "POLOC_10",
            "RODEO_10",
            "RODEO_40",
            "RODEO_60",
            "VULCA_10",
            "VULCA_20",
            "GLOB_30",
            "GLOB_10",
            "GLOB_20",
            "WHALE_00",
            "WHALE_05",
            "WHALE_10",
            "PLUM_00",
            "PLUM_20",
            "PLUM_10",
            "POLOC_20",
            "BAST_09",
            "BAST_10",
            "CASK_10",
            "CASK_30",
            "NAVE_10",
            "NAVE_15",
            "NAVE_20",
            "SEAT_10",
            "SEAT_11",
            "EARTH_10",
            "EARTH_20",
            "EARTH_30",
            "LY_20",
            "HELIC_10",
            "HELIC_20",
            "HELIC_30",
            "POLOC_30",
            "MORB_00",
            "MORB_10",
            "MORB_20",
            "LEARN_40",
            "BALL",
            "ILE_10",
            "MINE_10",
            "POLOC_40",
            "BATAM_20",
            "BOAT01",
            "BOAT02",
            "ASTRO_00",
            "ASTRO_10",
            "LIBER_10",
            "RHOP_10",
            "END_10",
            "STAFF_10",
            "BONUX",
            "GLOBVILL",
            "B_PYRAM",
            "B_TOILE",
            "GLODISC_CINE",
            "B_DISC",
            "B_LIFT",
            "B_INVADE",
            "RAYCAP"
        };

        private static string[] MapNamesDS = new string[] {
            "menu",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "mapmonde",
            "Learn_30",
            "Learn_31",
            "Bast_20",
            "Bast_22",
            "Learn_60",
            "Ski_10",
            "Ski_60",
            "Batam_10",
            "Chase_10",
            "Chase_22",
            "ly_10",
            "Nego_10",
            "Water_10",
            "Water_20",
            "Poloc_10",
            "Rodeo_10",
            "Rodeo_40",
            "Rodeo_60",
            "Vulca_10",
            "Vulca_20",
            "glob_30",
            "glob_10",
            "glob_20",
            "Whale_00",
            "Whale_05",
            "Whale_10",
            "Plum_00",
            "Plum_20",
            "Plum_10",
            "Poloc_20",
            "Bast_09",
            "Bast_10",
            "Cask_10",
            "Cask_30",
            "nave_10",
            "nave_15",
            "nave_20",
            "Seat_10",
            "Seat_11",
            "Earth_10",
            "Earth_20",
            "Earth_30",
            "ly_20",
            "helic_10",
            "helic_20",
            "helic_30",
            "Poloc_30",
            "morb_00",
            "morb_10",
            "morb_20",
            "Learn_40",
            "Ball",
            "Ile_10",
            "mine_10",
            "Poloc_40",
            "Batam_20",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_10",
            "Rhop_10",
            "End_10",
            "Staff_10",
            "Bonux",
            "RayCap"
        };

        private static string[] MapNames3DS = new string[] {
            "Menu",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "Mapmonde2",
            "Learn_30",
            "Learn_31",
            "Bast_20",
            "Bast_22",
            "Learn_60",
            "Ski_10",
            "Ski_60",
            "Batam_10",
            "Chase_10",
            "Chase_22",
            "Ly_10",
            "nego_10",
            "Water_10",
            "Water_20",
            "poloc_10",
            "Rodeo_10",
            "Rodeo_40",
            "Rodeo_60",
            "Vulca_10",
            "Vulca_20",
            "GLob_30",
            "GLob_10",
            "GLob_20",
            "Whale_00",
            "Whale_05",
            "Whale_10",
            "Plum_00",
            "Plum_20",
            "Plum_10",
            "poloc_20",
            "Bast_09",
            "Bast_10",
            "Cask_10",
            "Cask_30",
            "Nave_10",
            "Nave_15",
            "Nave_20",
            "Seat_10",
            "Seat_11",
            "Earth_10",
            "Earth_20",
            "Earth_30",
            "Ly_20",
            "Helic_10",
            "Helic_20",
            "Helic_30",
            "poloc_30",
            "Morb_00",
            "Morb_10",
            "Morb_20",
            "Learn_40",
            "Ball",
            "ile_10",
            "Mine_10",
            "poloc_40",
            "Batam_20",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_10",
            "Liber_10",
            "Rhop_10",
            "end_10",
            "staff_10",
            "Bonux",
            "Raycap"
        };

        public static string GetCorrectCapsMapName(string mapname) {
            string mapname0 = mapname.Split('$')[0];
            switch (Settings.s?.platform) {
                case Settings.Platform.N64:
                    return MapNamesN64.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.PS1:
                    return MapNamesPS1.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.PS2:
                    return MapNamesPS2.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.iOS:
                    return MapNamesIOS.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.DC:
                    return MapNamesDC.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.DS:
                    return MapNamesDS.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform._3DS:
                    return MapNames3DS.FirstOrDefault(m => m.ToLower() == mapname0);
                case Settings.Platform.PC:
                default:
                    return MapNamesPC.FirstOrDefault(m => m.ToLower() == mapname0);
            }
        }
    }
}