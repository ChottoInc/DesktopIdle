using UnityEngine;

public static class UtilsFarmer
{
    public static float PER_LEVEL_FARMER_GAIN_GREENTHUMB = 0.01f;    // max 25%, base 1, multiplier

    // for now max 5 companions, so need to unlock just few plants, if companion different from crop, 
    // 4 more seeds are needed, so max 4f, gain 0.2 for level, every 5 level increase seeds, so 5 levels times max 4, 20 max cap
    public static float PER_LEVEL_FARMER_GAIN_AGRONOMY = 0.2f;       
    public static float PER_LEVEL_FARMER_GAIN_KINDNESS = 0.01f;      // max 25%, decrease max cooldown lure timer, multiplier
    public static float PER_LEVEL_FARMER_GAIN_LUCK = 0.01f;          // max 25%, base 10%
           
    public static int PER_LEVEL_FARMER_MAX_GREENTHUMB = 25;
    public static int PER_LEVEL_FARMER_MAX_AGRONOMY = 20;
    public static int PER_LEVEL_FARMER_MAX_KINDNESS = 25;
    public static int PER_LEVEL_FARMER_MAX_LUCK = 15;



    private static float BASE_FARMER_EXP_GROWTH = 50f;
    private static float EXPO_FARMER_EXP_GROWTH = 1.15f;


    private static PlayerJobFarmerSO jobDataSO;


    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Farmer) as PlayerJobFarmerSO;

        PER_LEVEL_FARMER_GAIN_GREENTHUMB = jobDataSO.PerLevelGainGreenthumb;
        PER_LEVEL_FARMER_GAIN_AGRONOMY = jobDataSO.PerLevelGainAgronomy;
        PER_LEVEL_FARMER_GAIN_KINDNESS = jobDataSO.PerLevelGainKindness;
        PER_LEVEL_FARMER_GAIN_LUCK = jobDataSO.PerLevelGainLuck;

        PER_LEVEL_FARMER_MAX_GREENTHUMB = jobDataSO.MaxLevelGreenthumb;
        PER_LEVEL_FARMER_MAX_AGRONOMY = jobDataSO.MaxLevelAgronomy;
        PER_LEVEL_FARMER_MAX_KINDNESS = jobDataSO.MaxLevelKindness;
        PER_LEVEL_FARMER_MAX_LUCK = jobDataSO.MaxLevelLuck;

        BASE_FARMER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_FARMER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
    }




    public static int RequiredExpForFarmerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_FARMER_EXP_GROWTH * Mathf.Pow(EXPO_FARMER_EXP_GROWTH, level - 1));
    }
}
