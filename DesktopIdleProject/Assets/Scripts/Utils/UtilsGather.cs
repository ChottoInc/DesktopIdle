using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsGather
{
    private const float BASE_ROCK_DURABILITY = 20f;
    private const float ROCK_DURABILITY_SCALE = 1.8f;

    public enum RockType
    {
        Copper,
        Iron,
        Bronze,
        Silver,
        Gold
    }

    public static float GetRockDurabilityByType(RockType rockType)
    {
        return BASE_ROCK_DURABILITY * Mathf.Pow(ROCK_DURABILITY_SCALE, (int)rockType);
    }
}
