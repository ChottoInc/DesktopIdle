using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsSave
{

    public const string ROOT_FOLDER = "Data";

    public const string SETTINGS_FOLDER = "Settings";
    public const string SETTINGS_FILE = "settings.json";

    public const string PLAYER_FOLDER = "Player";
    public const string PLAYER_INVENTORY_FILE = "player_inventory.json";
    public const string PLAYER_FIGHT_FILE = "player_fight.json";
    public const string PLAYER_MINER_FILE = "player_miner.json";

    public const string COMBATMAPS_FOLDER = "CombatMaps";
    public const string COMBATMAPS_EXT = ".json";

    public const string QUESTS_FOLDER = "Quests";
    public const string QUESTS_EXT = ".json";

    // ----- SETTINGS

    public static string GetSettingsFolder()
    {
        return ROOT_FOLDER + "/" + SETTINGS_FOLDER;
    }

    public static string GetSettingsFile()
    {
        return GetSettingsFolder() + "/" + SETTINGS_FILE;
    }


    // ----- PLAYER

    public static string GetPlayerFolder()
    {
        return ROOT_FOLDER + "/" + PLAYER_FOLDER;
    }

    public static string GetPlayerInventoryFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_INVENTORY_FILE;
    }

    public static string GetPlayerFightFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_FIGHT_FILE;
    }

    public static string GetPlayerMinerFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_MINER_FILE;
    }

    // ----- MAPS

    public static string GetCombatMapsFolder()
    {
        return ROOT_FOLDER + "/" + COMBATMAPS_FOLDER;
    }

    public static string GetCombatMapFile(string firstPart)
    {
        return GetCombatMapsFolder() + "/" + firstPart + COMBATMAPS_EXT;
    }


    // ----- QUESTS

    public static string GetQuestsFolder()
    {
        return ROOT_FOLDER + "/" + QUESTS_FOLDER;
    }

    public static string GetQuestFile(string firstPart)
    {
        return GetQuestsFolder() + "/" + firstPart + QUESTS_EXT;
    }
}
