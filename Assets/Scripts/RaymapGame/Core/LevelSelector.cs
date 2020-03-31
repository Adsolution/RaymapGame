//================================
//  By: Adsolution
//================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static string selectedLevel = "Ski_10";
    void Start()
    {
        var ops = new Dropdown.OptionDataList();
        foreach (var cn in levelNames)
            ops.options.Add(new Dropdown.OptionData(CodeToGameName(cn)));
        dropLevels.ClearOptions();
        dropLevels.AddOptions(ops.options);
        dropLevels.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropLevels);
        });
    }

    bool changed;
    void DropdownValueChanged(Dropdown change)
    {
        selectedLevel = levelNames[change.value];
        changed = true;
    }
    void Update()
    {
        if (changed)
        {/*
            foreach (var g in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (var m in g.GetComponentsInChildren<MonoBehaviour>())
                    Destroy(m);
                Destroy(g);
            }
            Application.LoadLevel(0);*/
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
            Application.Quit();
        }
    }

    public Dropdown dropLevels;

    public static string[] levelNames = new string[]
   {
            "Menu",
            "Mapmonde",
            "Jail_10",
            "Jail_20",
            "Learn_10",
            "Learn_30",
            "Learn_31",
            "Bast_20",
            "Bast_22",
            "Learn_60",
            "Ski_10",
            "Ski_60",
            "Chase_10",
            "Chase_22",
            "Water_10",
            "Water_20",
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
            "Helic_10",
            "Helic_20",
            "Helic_30",
            "Morb_00",
            "Morb_10",
            "Morb_20",
            "Learn_40",
            "Ball",
            "Ile_10",
            "Mine_10",
            "Boat01",
            "Boat02",
            "Astro_00",
            "Astro_10",
            "Rhop_10",
            "Ly_10",
            "Ly_20",
            "Bonux",
            "Nego_10",
            "Poloc_10",
            "Poloc_20",
            "Poloc_30",
            "Poloc_40",
            "Batam_10",
            "Batam_20",
            "Raycap",
            "End_10",
            "Staff_10"
   };

    public static string CodeToGameName(string codeName)
    {
        switch (codeName)
        {
            case "Menu":
                return "Main Menu";
            case "Mapmonde":
                return "The Hall of Doors";
            case "Jail_10":
                return "Introduction Cutscene";
            case "Jail_20":
                return "Introduction - Jail";
            case "Learn_10":
                return "The Woods of Light";
            case "Learn_30":
                return "The Fairy Glade #1";
            case "Learn_31":
                return "The Fairy Glade #2";
            case "Bast_20":
                return "The Fairy Glade #3";
            case "Bast_22":
                return "The Fairy Glade #4";
            case "Learn_60":
                return "The Fairy Glade #5";
            case "Ski_10":
                return "The Marshes of Awakening #1";
            case "Ski_60":
                return "The Marshes of Awakening #2";
            case "Chase_10":
                return "The Bayou #1";
            case "Chase_22":
                return "The Bayou #2";
            case "Water_10":
                return "The Sanctuary of Water and Ice #1";
            case "Water_20":
                return "The Sanctuary of Water and Ice #2";
            case "Rodeo_10":
                return "The Menhir Hills #1";
            case "Rodeo_40":
                return "The Menhir Hills #2";
            case "Rodeo_60":
                return "The Menhir Hills #3";
            case "Vulca_10":
                return "The Cave of Bad Dreams #1";
            case "Vulca_20":
                return "The Cave of Bad Dreams #2";
            case "GLob_30":
                return "The Canopy #1";
            case "GLob_10":
                return "The Canopy #2";
            case "GLob_20":
                return "The Canopy #3";
            case "Whale_00":
                return "Whale Bay #1";
            case "Whale_05":
                return "Whale Bay #2";
            case "Whale_10":
                return "Whale Bay #3";
            case "Plum_00":
                return "The Sanctuary of Stone and Fire #1";
            case "Plum_20":
                return "The Sanctuary of Stone and Fire - temple";
            case "Plum_10":
                return "The Sanctuary of Stone and Fire #2";
            case "Bast_09":
                return "The Echoing Caves - opening cutscene";
            case "Bast_10":
                return "The Echoing Caves #1";
            case "Cask_10":
                return "The Echoing Caves #2";
            case "Cask_30":
                return "The Echoing Caves #3";
            case "Nave_10":
                return "The Precipice #1";
            case "Nave_15":
                return "The Precipice #2";
            case "Nave_20":
                return "The Precipice #3";
            case "Seat_10":
                return "The Top of the World #1";
            case "Seat_11":
                return "The Top of the World #2";
            case "Earth_10":
                return "The Sanctuary of Rock and Lava #1";
            case "Earth_20":
                return "The Sanctuary of Rock and Lava #2";
            case "Earth_30":
                return "The Sanctuary of Rock and Lava #3";
            case "Helic_10":
                return "Beneath the Sanctuary of Rock and Lava #1";
            case "Helic_20":
                return "Beneath the Sanctuary of Rock and Lava #2";
            case "Helic_30":
                return "Beneath the Sanctuary of Rock and Lava #3";
            case "Morb_00":
                return "Tomb of the Ancients #1";
            case "Morb_10":
                return "Tomb of the Ancients #2";
            case "Morb_20":
                return "Tomb of the Ancients #3";
            case "Learn_40":
                return "The Iron Mountains #1";
            case "Ball":
                return "The Iron Mountains - balloon cutscene";
            case "Ile_10":
                return "The Iron Mountains #2";
            case "Mine_10":
                return "The Iron Mountains part 3";
            case "Boat01":
                return "The Prison Ship #1";
            case "Boat02":
                return "The Prison Ship #2";
            case "Astro_00":
                return "The Prison Ship #3";
            case "Astro_10":
                return "The Prison Ship #4";
            case "Rhop_10":
                return "The Crow's Nest";
            case "Ly_10":
                return "The Walk of Life";
            case "Ly_20":
                return "The Walk of Power";
            case "Bonux":
                return "Bonus game";
            case "Nego_10":
                return "Cine - Council Chamber of the Teensies";
            case "Poloc_10":
                return "Cine - Polokus #1";
            case "Poloc_20":
                return "Cine - Polokus #2";
            case "Poloc_30":
                return "Cine - Polokus #3";
            case "Poloc_40":
                return "Cine - Polokus #4";
            case "Batam_10":
                return "Cine - Meanwhile on The Prison Ship";
            case "Batam_20":
                return "Cine - Razorbeard buys the Grolgoth";
            case "Raycap":
                return "Level scores";
            case "End_10":
                return "Cine - Ending";
            case "Staff_10":
                return "Cine - Credits";
            default:
                return codeName;
        }
    }
}
