using UnityEngine;

public static class UtilsBlacksmith
{
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

    public static int RequiredExpForBlacksmithLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_BLACKSMITH_EXP_GROWTH * Mathf.Pow(EXPO_BLACKSMITH_EXP_GROWTH, level - 1));
    }

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
}
