using UnityEngine;

public static class UtilsFisher
{
    public static float PER_LEVEL_FISHER_GAIN_CALMNESS = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_REFLEX = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_KNOWLEDGE = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_LUCK = 0.01f;
            
    public static int PER_LEVEL_FISHER_MAX_CALMNESS = 50;
    public static int PER_LEVEL_FISHER_MAX_REFLEX = 25;
    public static int PER_LEVEL_FISHER_MAX_KNOWLEDGE = 30;
    public static int PER_LEVEL_FISHER_MAX_LUCK = 40;
           
           
           
    private static float BASE_FISHER_EXP_GROWTH = 50f;
    private static float EXPO_FISHER_EXP_GROWTH = 1.15f;


    private static PlayerJobFisherSO jobDataSO;


    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Fisher) as PlayerJobFisherSO;

        PER_LEVEL_FISHER_GAIN_CALMNESS = jobDataSO.PerLevelGainCalmness;
        PER_LEVEL_FISHER_GAIN_REFLEX = jobDataSO.PerLevelGainReflex;
        PER_LEVEL_FISHER_GAIN_KNOWLEDGE = jobDataSO.PerLevelGainKnowledge;
        PER_LEVEL_FISHER_GAIN_LUCK = jobDataSO.PerLevelGainLuck;

        PER_LEVEL_FISHER_MAX_CALMNESS = jobDataSO.MaxLevelCalmness;
        PER_LEVEL_FISHER_MAX_REFLEX = jobDataSO.MaxLevelReflex;
        PER_LEVEL_FISHER_MAX_KNOWLEDGE = jobDataSO.MaxLevelKnowledge;
        PER_LEVEL_FISHER_MAX_LUCK = jobDataSO.MaxLevelLuck;

        BASE_FISHER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_FISHER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
    }



    public static int RequiredExpForFisherLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_FISHER_EXP_GROWTH * Mathf.Pow(EXPO_FISHER_EXP_GROWTH, level - 1));
    }
}
