using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsSave
{

    public const string ROOT_FOLDER = "Data";

    public const string SETTINGS_FOLDER = "Settings";
    public const string SETTINGS_FILE = "settings.json";

    public const string PLAYER_FOLDER = "Player";
    public const string PLAYER_FIGHT_FILE = "player_fight.json";

    public const string COMBATMAPS_FOLDER = "CombatMaps";
    public const string COMBATMAPS_EXT = ".json";

    public static string GetSettingsFolder()
    {
        return ROOT_FOLDER + "/" + SETTINGS_FOLDER;
    }

    public static string GetSettingsFile()
    {
        return GetSettingsFolder() + "/" + SETTINGS_FILE;
    }

    public static string GetPlayerFolder()
    {
        return ROOT_FOLDER + "/" + PLAYER_FOLDER;
    }

    public static string GetPlayerFightFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_FIGHT_FILE;
    }

    public static string GetCombatMapsFolder()
    {
        return ROOT_FOLDER + "/" + COMBATMAPS_FOLDER;
    }

    public static string GetCombatMapFile(string firstPart)
    {
        return GetCombatMapsFolder() + "/" + firstPart + COMBATMAPS_EXT;
    }
}
