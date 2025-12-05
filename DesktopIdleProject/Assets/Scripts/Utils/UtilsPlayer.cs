using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsPlayer
{
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

    // ------ FIGHT STATS ------

    public const float PER_LEVEL_WARRIOR_GAIN_MAXHP = 20;
    public const float PER_LEVEL_WARRIOR_GAIN_ATK = 2;
    public const float PER_LEVEL_WARRIOR_GAIN_DEF = 1;
    public const float PER_LEVEL_WARRIOR_GAIN_ATK_SPEED = 0.01f;
    public const float PER_LEVEL_WARRIOR_GAIN_CRIT_RATE = 0.001f;
    public const float PER_LEVEL_WARRIOR_GAIN_CRIT_DMG = 0.02f;
    public const float PER_LEVEL_WARRIOR_GAIN_LUCK = 0.1f;

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
    public const float PER_LEVEL_MINER_GAIN_PRECISION = 0.1f;
    public const float PER_LEVEL_MINER_GAIN_LUCK = 0.1f;

    public const int PER_LEVEL_MINER_MAX_POWER = 50;
    public const int PER_LEVEL_MINER_MAX_SMASHSPEED = 40;
    public const int PER_LEVEL_MINER_MAX_PRECISION = 30;
    public const int PER_LEVEL_MINER_MAX_LUCK = 40;



    private const float BASE_MINER_EXP_GROWTH = 50f;
    private const float EXPO_MINER_EXP_GROWTH = 1.15f;

    private const float MINER_WEAPON_LINEAR_GROWTH = 0.35f;
    private const float MINER_WEAPON_QUADRATIC_GROWTH = 0.05f;

    private const int MINER_WEAPON_MAX_LEVLE = 5;





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

    public static int GetStatMaxLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;

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
        }
    }


    public static float GetMinerWeaponMultiplier(int weaponLevel)
    {
        float baseMultiplier = 1f;                 // 1× damage at level 1

        int lv = weaponLevel - 1;

        return baseMultiplier
               + MINER_WEAPON_LINEAR_GROWTH * lv
               + MINER_WEAPON_QUADRATIC_GROWTH * lv * lv;
    }
}
