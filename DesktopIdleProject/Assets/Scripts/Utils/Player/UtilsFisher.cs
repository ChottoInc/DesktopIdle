using UnityEngine;

public static class UtilsFisher
{
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

    public static int RequiredExpForFisherLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_FISHER_EXP_GROWTH * Mathf.Pow(EXPO_FISHER_EXP_GROWTH, level - 1));
    }
}
