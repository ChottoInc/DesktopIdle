using System.Collections.Generic;
using UnityEngine;
using static UtilsCombatMap;

public static class UtilsGather
{
    private static RockSO[] rocks;


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

        return result;
    }

    #endregion
}
