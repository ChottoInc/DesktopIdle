using UnityEngine;

public static class UtilsWarrior
{
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

    public static int RequiredExpForWarriorLevel(int level)
    {
        return Mathf.FloorToInt(BASE_FIGHT_EXP_GROWTH * Mathf.Pow(level, EXPO_FIGHT_EXP_GROWTH) + FLAT_FIGHT_EXP_GROWTH * level);
    }
}
