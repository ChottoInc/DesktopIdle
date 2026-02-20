using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsPlayer
{
    public enum PlayerJob { None, Warrior, Miner, Blacksmith, Fisher }

    private static PlayerJobSO[] jobs;


    // ----- IDS -------

    public const int ID_WARRIOR_MAXHP = 0;
    public const int ID_WARRIOR_ATK = 1;
    public const int ID_WARRIOR_DEF = 2;
    public const int ID_WARRIOR_ATKSPD = 3;
    public const int ID_WARRIOR_CRITRATE = 4;
    public const int ID_WARRIOR_CRITDMG = 5;
    public const int ID_WARRIOR_LUCK = 6;

    public const int ID_MINER_POWER = 20;
    public const int ID_MINER_SMASHSPEED = 21;
    public const int ID_MINER_PRECISION = 22;
    public const int ID_MINER_LUCK = 23;

    public const int ID_BLACKSMITH_CRAFTSPEED = 30;
    public const int ID_BLACKSMITH_EFFICIENCY = 31;
    public const int ID_BLACKSMITH_LUCK = 32;

    public const int ID_FISHER_CALMNESS = 33;
    public const int ID_FISHER_REFLEX = 34;
    public const int ID_FISHER_KNOWLEDGE = 35;
    public const int ID_FISHER_LUCK = 36;

    // ------ FIGHT STATS ------

    public const float PER_LEVEL_WARRIOR_GAIN_MAXHP = 20;
    public const float PER_LEVEL_WARRIOR_GAIN_ATK = 2;
    public const float PER_LEVEL_WARRIOR_GAIN_DEF = 1;
    public const float PER_LEVEL_WARRIOR_GAIN_ATK_SPEED = 0.01f;
    public const float PER_LEVEL_WARRIOR_GAIN_CRIT_RATE = 0.001f;
    public const float PER_LEVEL_WARRIOR_GAIN_CRIT_DMG = 0.02f;
    public const float PER_LEVEL_WARRIOR_GAIN_LUCK = 0.01f;

    public const int PER_LEVEL_WARRIOR_MAX_MAXHP = 50;
    public const int PER_LEVEL_WARRIOR_MAX_ATK = 50;
    public const int PER_LEVEL_WARRIOR_MAX_DEF = 50;
    public const int PER_LEVEL_WARRIOR_MAX_ATK_SPEED = 30;
    public const int PER_LEVEL_WARRIOR_MAX_CRIT_RATE = 20;
    public const int PER_LEVEL_WARRIOR_MAX_CRIT_DMG = 30;
    public const int PER_LEVEL_WARRIOR_MAX_LUCK = 40;



    private const float BASE_FIGHT_EXP_GROWTH = 20f;
    private const float EXPO_FIGHT_EXP_GROWTH = 1.85f;
    private const float FLAT_FIGHT_EXP_GROWTH = 50f;

    // ------ MINER STATS ------

    public const float PER_LEVEL_MINER_GAIN_POWER = 2;
    public const float PER_LEVEL_MINER_GAIN_SMASHSPEED = 0.02f;
    public const float PER_LEVEL_MINER_GAIN_PRECISION = 0.01f;
    public const float PER_LEVEL_MINER_GAIN_LUCK = 0.01f;

    public const int PER_LEVEL_MINER_MAX_POWER = 50;
    public const int PER_LEVEL_MINER_MAX_SMASHSPEED = 40;
    public const int PER_LEVEL_MINER_MAX_PRECISION = 30;
    public const int PER_LEVEL_MINER_MAX_LUCK = 40;



    private const float BASE_MINER_EXP_GROWTH = 50f;
    private const float EXPO_MINER_EXP_GROWTH = 1.15f;

    private const float MINER_WEAPON_LINEAR_GROWTH = 0.35f;
    private const float MINER_WEAPON_QUADRATIC_GROWTH = 0.05f;

    private const int MINER_WEAPON_MAX_LEVEL = 10;


    // ------ BLACKSMITH STATS ------

    public const float PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED = 0.02f;
    public const float PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY = 0.01f;
    public const float PER_LEVEL_BLACKSMITH_GAIN_LUCK = 0.01f;

    public const int PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED = 50;
    public const int PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY = 30;
    public const int PER_LEVEL_BLACKSMITH_MAX_LUCK = 70;



    private const float BASE_BLACKSMITH_EXP_GROWTH = 10f;
    private const float EXPO_BLACKSMITH_EXP_GROWTH = 1.15f;

    //Helmet
    private const float BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH = 0.30f;
    private const float BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH = 0.05f;

    // Armor
    private const float BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH = 0.25f;
    private const float BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH = 0.04f;

    // Gloves
    private const float BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH = 0.25f;
    private const float BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH = 0.04f;

    private const float BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH = 0.25f;
    private const float BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH = 0.05f;

    // Boots
    private const float BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH = 0.15f;
    private const float BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH = 0.038f;

    private const float BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH = 0.25f;
    private const float BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH = 0.05f;


    private const int BLACKSMITH_HELMET_MAX_LEVEL = 10;
    private const int BLACKSMITH_ARMOR_MAX_LEVEL = 10;
    private const int BLACKSMITH_GLOVES_MAX_LEVEL = 10;
    private const int BLACKSMITH_BOOTS_MAX_LEVEL = 10;


    // ------ FISHER STATS ------

    public const float PER_LEVEL_FISHER_GAIN_CALMNESS = 0.01f;
    public const float PER_LEVEL_FISHER_GAIN_REFLEX = 0.01f;
    public const float PER_LEVEL_FISHER_GAIN_KNOWLEDGE = 0.01f;
    public const float PER_LEVEL_FISHER_GAIN_LUCK = 0.01f;

    public const int PER_LEVEL_FISHER_MAX_CALMNESS = 50;
    public const int PER_LEVEL_FISHER_MAX_REFLEX = 25;
    public const int PER_LEVEL_FISHER_MAX_KNOWLEDGE = 30;
    public const int PER_LEVEL_FISHER_MAX_LUCK = 40;



    private const float BASE_FISHER_EXP_GROWTH = 50f;
    private const float EXPO_FISHER_EXP_GROWTH = 1.15f;


    public static void Initialize()
    {
        jobs = LoadJobs();
    }

    #region JOBS

    private static PlayerJobSO[] LoadJobs()
    {
        return Resources.LoadAll<PlayerJobSO>("Data/Player/Jobs");
    }


    public static PlayerJobSO[] GetAllJobs()
    {
        return jobs;
    }

    public static PlayerJobSO GetJobByType(PlayerJob job)
    {
        foreach (var playerJob in jobs)
        {
            if (playerJob.Job == job)
                return playerJob;
        }
        return null;
    }

    #endregion


    public static int RequiredExpForWarriorLevel(int level)
    {
        return Mathf.FloorToInt(BASE_FIGHT_EXP_GROWTH * Mathf.Pow(level, EXPO_FIGHT_EXP_GROWTH) + FLAT_FIGHT_EXP_GROWTH * level);
    }

    public static int RequiredExpForMinerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_MINER_EXP_GROWTH * Mathf.Pow(EXPO_MINER_EXP_GROWTH, level - 1));
    }

    public static int RequiredExpForBlacksmithLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_BLACKSMITH_EXP_GROWTH * Mathf.Pow(EXPO_BLACKSMITH_EXP_GROWTH, level - 1));
    }

    public static int RequiredExpForFisherLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_FISHER_EXP_GROWTH * Mathf.Pow(EXPO_FISHER_EXP_GROWTH, level - 1));
    }



    public static int GetStatMaxLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("stat id not correct. " + id); return -1;

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return PER_LEVEL_WARRIOR_MAX_MAXHP;
            case ID_WARRIOR_ATK:  return PER_LEVEL_WARRIOR_MAX_ATK;
            case ID_WARRIOR_DEF: return PER_LEVEL_WARRIOR_MAX_DEF;
            case ID_WARRIOR_ATKSPD: return PER_LEVEL_WARRIOR_MAX_ATK_SPEED;
            case ID_WARRIOR_CRITRATE: return PER_LEVEL_WARRIOR_MAX_CRIT_RATE;
            case ID_WARRIOR_CRITDMG: return PER_LEVEL_WARRIOR_MAX_CRIT_DMG;
            case ID_WARRIOR_LUCK: return PER_LEVEL_WARRIOR_MAX_LUCK;

            // MINER DATA
            case ID_MINER_POWER: return PER_LEVEL_MINER_MAX_POWER;
            case ID_MINER_SMASHSPEED: return PER_LEVEL_MINER_MAX_SMASHSPEED;
            case ID_MINER_PRECISION: return PER_LEVEL_MINER_MAX_PRECISION;
            case ID_MINER_LUCK: return PER_LEVEL_MINER_MAX_LUCK;

            case ID_BLACKSMITH_CRAFTSPEED: return PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED;
            case ID_BLACKSMITH_EFFICIENCY: return PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY;
            case ID_BLACKSMITH_LUCK: return PER_LEVEL_BLACKSMITH_MAX_LUCK;

            // FISHER DATA
            case ID_FISHER_CALMNESS: return PER_LEVEL_FISHER_MAX_CALMNESS;
            case ID_FISHER_REFLEX: return PER_LEVEL_FISHER_MAX_REFLEX;
            case ID_FISHER_KNOWLEDGE: return PER_LEVEL_FISHER_MAX_KNOWLEDGE;
            case ID_FISHER_LUCK: return PER_LEVEL_FISHER_MAX_LUCK;
        }
    }

    public static int GetStatCurrentLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("stat id not correct. " + id); return -1;

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return PlayerManager.Instance.PlayerFightData.LevelStatMaxHp;
            case ID_WARRIOR_ATK: return PlayerManager.Instance.PlayerFightData.LevelStatAtk;
            case ID_WARRIOR_DEF: return PlayerManager.Instance.PlayerFightData.LevelStatDef;
            case ID_WARRIOR_ATKSPD: return PlayerManager.Instance.PlayerFightData.LevelStatAtkSpd;
            case ID_WARRIOR_CRITRATE: return PlayerManager.Instance.PlayerFightData.LevelStatCritRate;
            case ID_WARRIOR_CRITDMG: return PlayerManager.Instance.PlayerFightData.LevelStatCritDmg;
            case ID_WARRIOR_LUCK: return PlayerManager.Instance.PlayerFightData.LevelStatLuck;

            // MINER DATA
            case ID_MINER_POWER: return PlayerManager.Instance.PlayerMinerData.LevelStatPower;
            case ID_MINER_SMASHSPEED: return PlayerManager.Instance.PlayerMinerData.LevelStatSmashSpeed;
            case ID_MINER_PRECISION: return PlayerManager.Instance.PlayerMinerData.LevelStatPrecision;
            case ID_MINER_LUCK: return PlayerManager.Instance.PlayerMinerData.LevelStatLuck;

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return PlayerManager.Instance.PlayerBlacksmithData.LevelStatCraftSpeed;
            case ID_BLACKSMITH_EFFICIENCY: return PlayerManager.Instance.PlayerBlacksmithData.LevelEfficiency;
            case ID_BLACKSMITH_LUCK: return PlayerManager.Instance.PlayerBlacksmithData.LevelStatLuck;

            // FISHER DATA
            //case ID_MINER_POWER: return PlayerManager.Instance.PlayerMinerData.LevelStatPower;
            //case ID_MINER_SMASHSPEED: return PlayerManager.Instance.PlayerMinerData.LevelStatSmashSpeed;
            //case ID_MINER_PRECISION: return PlayerManager.Instance.PlayerMinerData.LevelStatPrecision;
            //case ID_MINER_LUCK: return PlayerManager.Instance.PlayerMinerData.LevelStatLuck;
        }
    }

    // ------------ MINER ---------------- //

    public static float GetMinerWeaponMultiplier(int weaponLevel)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = weaponLevel - 1;

        return baseMultiplier
               + MINER_WEAPON_LINEAR_GROWTH * lv
               + MINER_WEAPON_QUADRATIC_GROWTH * lv * lv;
    }

    // ------------ BLACKSMITH ---------------- //

    public static float GetBlacksmithHelmetMaxHpMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH * lv
               + BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithArmorDefMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH * lv
               + BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithGlovesAtkSpdMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH * lv
               + BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithGlovesCritDmgMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH * lv
               + BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithBootsDefMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH * lv
               + BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithBootsCritRateMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH * lv
               + BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH * lv * lv;
    }


    public static string GetStatNameById(int id)
    {
        switch (id)
        {
            default: return "Error";
            case ID_WARRIOR_MAXHP: return "Max HP (Warrior)";
            case ID_WARRIOR_ATK: return "Atk (Warrior)";
            case ID_WARRIOR_DEF: return "Def (Warrior)";
            case ID_WARRIOR_ATKSPD: return "Atk Speed (Warrior)";
            case ID_WARRIOR_CRITRATE: return "Crit Rate (Warrior)";
            case ID_WARRIOR_CRITDMG: return "Crit Dmg (Warrior)";
            case ID_WARRIOR_LUCK: return "Luck (Warrior)";

            case ID_MINER_POWER: return "Power (Miner)";
            case ID_MINER_SMASHSPEED: return "Smash Spd (Miner)";
            case ID_MINER_PRECISION: return "Precision (Miner)";
            case ID_MINER_LUCK: return "Luck (Miner)";

            case ID_BLACKSMITH_CRAFTSPEED: return "Craft Speed (Blacksmith)";
            case ID_BLACKSMITH_EFFICIENCY: return "Efficiency (Blacksmith)";
            case ID_BLACKSMITH_LUCK: return "Luck (Blacksmith)";

            case ID_FISHER_CALMNESS: return "Calmness (Fisher)";
            case ID_FISHER_REFLEX: return "Reflex (Fisher)";
            case ID_FISHER_KNOWLEDGE: return "Knowledge (Fisher)";
            case ID_FISHER_LUCK: return "Luck (Fisher)";
        }
    }
}
