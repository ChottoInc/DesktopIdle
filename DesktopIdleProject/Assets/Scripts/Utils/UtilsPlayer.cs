using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsPlayer
{
    // ----- IDS -------

    public const int ID_MAXHP = 0;
    public const int ID_ATK = 1;
    public const int ID_DEF = 2;
    public const int ID_ATKSPD = 3;
    public const int ID_CRITRATE = 4;
    public const int ID_CRITDMG = 5;
    public const int ID_LUCK = 6;

    // ------ STATS ------

    public const int PER_LEVEL_MAX_MAXHP = 50;
    public const int PER_LEVEL_MAX_ATK = 50;
    public const int PER_LEVEL_MAX_DEF = 50;
    public const int PER_LEVEL_MAX_ATK_SPEED = 30;
    public const int PER_LEVEL_MAX_CRIT_RATE = 20;
    public const int PER_LEVEL_MAX_CRIT_DMG = 30;
    public const int PER_LEVEL_MAX_LUCK = 40;



    public static int RequiredExpForLevel(int level)
    {
        return Mathf.FloorToInt(20 * Mathf.Pow(level, 1.85f) + 50 * level);
    }

    public static int GetStatMaxLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case ID_MAXHP: return PER_LEVEL_MAX_MAXHP;
            case ID_ATK:  return PER_LEVEL_MAX_ATK;
            case ID_DEF: return PER_LEVEL_MAX_DEF;
            case ID_ATKSPD: return PER_LEVEL_MAX_ATK_SPEED;
            case ID_CRITRATE: return PER_LEVEL_MAX_CRIT_RATE;
            case ID_CRITDMG: return PER_LEVEL_MAX_CRIT_DMG;
            case ID_LUCK: return PER_LEVEL_MAX_LUCK;
        }
    }
}
