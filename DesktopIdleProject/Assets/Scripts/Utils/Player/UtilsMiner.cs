using UnityEngine;

public static class UtilsMiner
{
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

    public static int RequiredExpForMinerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return Mathf.RoundToInt(BASE_MINER_EXP_GROWTH * Mathf.Pow(EXPO_MINER_EXP_GROWTH, level - 1));
    }

    public static float GetMinerWeaponMultiplier(int weaponLevel)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = weaponLevel - 1;

        return baseMultiplier
               + MINER_WEAPON_LINEAR_GROWTH * lv
               + MINER_WEAPON_QUADRATIC_GROWTH * lv * lv;
    }
}
