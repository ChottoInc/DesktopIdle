using System.Collections.Generic;
using UnityEngine;

public static class UtilsGather
{
    private static RockSO[] rocks;

    // ----- MINER -----
    private const float BASE_ROCK_DURABILITY = 20f;
    private const float ROCK_DURABILITY_SCALE = 1.8f;

    /*
     * The first requirements for level up the miner weapon are manually set, 
     * but after 5 levels, the weapon needs at least every item on the list, so can cycle throught them and use const values to determine
     * how many you should use to level up.
     * 
     * if expanding the game you need more than 5 items to level up, make manual checks. and add const values to arrays
     * 
     * */

    private const int MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON = 5;

    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 10f;
    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 1.2f;
    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 0.8f;
    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 0.6f;
    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 0.3f;

    private static readonly float[] BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };


    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 2.2f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 1.9f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 1.5f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 1.25f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 1.15f;


    private static readonly float[] GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };



    public enum RockType
    {
        Copper,
        Iron,
        Bronze,
        Silver,
        Gold
    }

    #region ROCKS

    public static void Initialize()
    {
        rocks = LoadRocks();
    }

    private static RockSO[] LoadRocks()
    {
        return Resources.LoadAll<RockSO>("Data/Gatherer/Miner");
    }


    public static RockSO[] GetAllRocks()
    {
        return rocks;
    }

    public static RockSO GetRandomRock()
    {
        int rand = Random.Range(0, rocks.Length);
        return rocks[rand];
    }

    public static RockSO GetRockById(int id)
    {
        foreach (var rock in rocks)
        {
            if (rock.Id == id)
                return rock;
        }
        return null;
    }




    public static float GetRockDurabilityByType(RockType rockType)
    {
        return BASE_ROCK_DURABILITY * Mathf.Pow(ROCK_DURABILITY_SCALE, (int)rockType);
    }


    /// <summary>
    /// Exp given by the smashed rocks
    /// </summary>
    public static int GetRockExp(RockType rockType)
    {
        switch (rockType)
        {
            default:
            case RockType.Copper: return 5;
            case RockType.Iron: return 10;
            case RockType.Bronze: return 20;
            case RockType.Silver: return 40;
            case RockType.Gold: return 100;
        }
    }

    public static List<ItemGroup> GetRequirementsForMinerWeaponLevel(int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        if(level <= 5)
        {
            switch (level)
            {
                default:
                case 2:
                    result.Add(new ItemGroup(0, 20));
                    result.Add(new ItemGroup(1, 1));
                    break;

                case 3:
                    result.Add(new ItemGroup(0, 70));
                    result.Add(new ItemGroup(1, 3));
                    result.Add(new ItemGroup(2, 1));
                    break;

                case 4:
                    result.Add(new ItemGroup(0, 200));
                    result.Add(new ItemGroup(1, 15));
                    result.Add(new ItemGroup(2, 7));
                    result.Add(new ItemGroup(3, 1));
                    break;

                case 5:
                    result.Add(new ItemGroup(0, 750));
                    result.Add(new ItemGroup(1, 70));
                    result.Add(new ItemGroup(2, 20));
                    result.Add(new ItemGroup(3, 5));
                    result.Add(new ItemGroup(4, 1));
                    break;
            }
        }
        else
        {
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON; i++)
            {
                result.Add(new ItemGroup(i, RequiredItemAmount(level, i)));
            }
        }

        return result;
    }

    private static int RequiredItemAmount(int level, int itemIndex)
    {
        return Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex]));
    }

    #endregion
}
