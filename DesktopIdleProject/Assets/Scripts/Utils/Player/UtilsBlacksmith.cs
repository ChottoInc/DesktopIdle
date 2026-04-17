using UnityEngine;

public static class UtilsBlacksmith
{
    public static float PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED = 0.02f;
    public static float PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY = 0.01f;
    public static float PER_LEVEL_BLACKSMITH_GAIN_LUCK = 0.01f;
           
    public static int PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED = 50;
    public static int PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY = 30;
    public static int PER_LEVEL_BLACKSMITH_MAX_LUCK = 70;


    public static int MAX_LEVEL_BLACKSMITH;



    private static float BASE_BLACKSMITH_EXP_GROWTH = 50f;
    private static float EXPO_BLACKSMITH_EXP_GROWTH = 1.08f;
    private static float FLAT_BLACKSMITH_EXP_GROWTH = 10f;
            
    //Helmet
    private static float BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH = 0.20f;
    private static float BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH = 0.05f;
            
    // Armor
    private static float BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH = 0.04f;
            
    // Gloves
    private static float BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH = 0.04f;
            
    private static float BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH = 0.05f;
            
    // Boots
    private static float BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH = 0.15f;
    private static float BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH = 0.038f;
            
    private static float BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH = 0.05f;
            
            
    public static int BLACKSMITH_HELMET_MAX_LEVEL = 10;
    public static int BLACKSMITH_ARMOR_MAX_LEVEL = 10;
    public static int BLACKSMITH_GLOVES_MAX_LEVEL = 10;
    public static int BLACKSMITH_BOOTS_MAX_LEVEL = 10;


    private static PlayerJobBlacksmithSO jobDataSO;


    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Blacksmith) as PlayerJobBlacksmithSO;

        PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED = jobDataSO.PerLevelGainCraftSpeed;
        PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY = jobDataSO.PerLevelGainEfficiency;
        PER_LEVEL_BLACKSMITH_GAIN_LUCK = jobDataSO.PerLevelGainLuck;


        PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED = jobDataSO.MaxLevelCraftSpeed;
        PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY = jobDataSO.MaxLevelEfficiency;
        PER_LEVEL_BLACKSMITH_MAX_LUCK = jobDataSO.MaxLevelLuck;


        MAX_LEVEL_BLACKSMITH =
            PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED +
            PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY +
            PER_LEVEL_BLACKSMITH_MAX_LUCK;



        BASE_BLACKSMITH_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_BLACKSMITH_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_BLACKSMITH_EXP_GROWTH = jobDataSO.FlatExpGrowth;



        BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH = jobDataSO.HelmetMaxHpLinearGrowth;
        BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH = jobDataSO.HelmetMaxHpQuadraticGrowth;

        BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH = jobDataSO.ArmorDefLinearGrowth;
        BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH = jobDataSO.ArmorDefQuadraticGrowth;


        BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH = jobDataSO.GlovesAtkSpdLinearGrowth;
        BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH = jobDataSO.GlovesAtkSpdQuadraticGrowth;

        BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH = jobDataSO.GlovesCritDmgLinearGrowth;
        BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH = jobDataSO.GlovesCritDmgQuadraticGrowth;



        BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH = jobDataSO.BootsDefLinearGrowth;
        BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH = jobDataSO.BootsDefQuadraticGrowth;

        BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH = jobDataSO.BootsCritRateLinearGrowth;
        BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH = jobDataSO.BootsCritRateQuadraticGrowth;
    }


    public static long RequiredExpForBlacksmithLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_BLACKSMITH_EXP_GROWTH * Mathf.Pow(EXPO_BLACKSMITH_EXP_GROWTH, level) + FLAT_BLACKSMITH_EXP_GROWTH * level);
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
