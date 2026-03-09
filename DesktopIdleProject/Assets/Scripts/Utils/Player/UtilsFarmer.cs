using UnityEngine;

public static class UtilsFarmer
{
    public const float PER_LEVEL_FARMER_GAIN_GREENTHUMB = 0.01f;    // max 25%, base 1, multiplier

    // for now max 5 companions, so need to unlock just few plants, if companion different from crop, 
    // 4 more seeds are needed, so max 4f, gain 0.2 for level, every 5 level increase seeds, so 5 levels times max 4, 20 max cap
    public const float PER_LEVEL_FARMER_GAIN_AGRONOMY = 0.2f;       
    public const float PER_LEVEL_FARMER_GAIN_KINDNESS = 0.01f;      // max 35%, base 10%
    public const float PER_LEVEL_FARMER_GAIN_LUCK = 0.01f;          // max 25%, base 10%

    public const int PER_LEVEL_FARMER_MAX_GREENTHUMB = 25;
    public const int PER_LEVEL_FARMER_MAX_AGRONOMY = 20;
    public const int PER_LEVEL_FARMER_MAX_KINDNESS = 25;
    public const int PER_LEVEL_FARMER_MAX_LUCK = 15;



    private const float BASE_FARMER_EXP_GROWTH = 50f;
    private const float EXPO_FARMER_EXP_GROWTH = 1.15f;

    public static int RequiredExpForFarmerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_FARMER_EXP_GROWTH * Mathf.Pow(EXPO_FARMER_EXP_GROWTH, level - 1));
    }
}
