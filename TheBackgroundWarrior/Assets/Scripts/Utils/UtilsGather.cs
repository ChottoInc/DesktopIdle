using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsGather
{
    // Miner
    private static Dictionary<int, ListableGameDataSO> dictRocks;
    private static Dictionary<int, ListableGameDataSO> dictWeaponLevelToSprite;

    public enum RockType { Copper, Iron, Bronze, Silver, Gold }

    // Fisher
    private static Dictionary<int, ListableGameDataSO> dictFishGroups;

    // Max hp, Atk, Def, Atk Spd, Crit rate, Crit dmg, Luck, Exp gain, Move spd warrior
    public enum FishGroupType { Life, Predator, Guardian, Dart, Sharp, Piercing, Golden, Elder, Quick }


    // Farmer
    private static Dictionary<int, ListableGameDataSO> dictCompanions;



    public const int ID_MINER_WEAPON = 0;
    

    // ------------------ MINER -----------------------

    private const float BASE_ROCK_DURABILITY = 35f;
    private const float ROCK_DURABILITY_SCALE = 2.5f;

    /*
     * The first requirements for level up the miner weapon are manually set, 
     * but after 5 levels, the weapon needs at least every item on the list, so can cycle throught them and use const values to determine
     * how many you should use to level up.
     * 
     * if expanding the game you need more than 5 items to level up, make manual checks. and add const values to arrays
     * 
     * */

    private const int MAX_MANUAL_WEAPON_REQUIREMENT = 5;

    private const int MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON = 5;

    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 900;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 500;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 200;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 50;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 20;

    private static readonly float[] BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };


    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 1.8f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 1.75f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 1.7f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 1.65f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 1.6f;


    private static readonly float[] GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };


    // -------------------- FISHER -----------------------
    public const float FISHER_LIFE_SERIES_COMPLETE_MULTIPLIER = 2f;         // max hp
    public const float FISHER_PREDATOR_SERIES_COMPLETE_MULTIPLIER = 1.5f;   // atk
    public const float FISHER_GUARDIAN_SERIES_COMPLETE_MULTIPLIER = 1.3f;   // def
    public const float FISHER_DART_SERIES_COMPLETE_MULTIPLIER = 1.2f;       // atk spd
    public const float FISHER_SHARP_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // crit rate
    public const float FISHER_PIERCING_SERIES_COMPLETE_MULTIPLIER = 1.2f;   // crit dmg
    public const float FISHER_GOLDEN_SERIES_COMPLETE_MULTIPLIER = 1.1f;     // luck
    public const float FISHER_ELDER_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // exp mult
    public const float FISHER_QUICK_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // move spd



    public static void Initialize()
    {
        // Miner
        LoadDictRocks();
        LoadDictWeaponLevelToSprites();

        // Fisher
        LoadDictFishGroups();

        // Farmer
        LoadDictCompanions();
    }

    #region ROCKS

    private static void LoadDictRocks()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Miner/ContainerGameData_Rocks");
        dictRocks = container.Entries.ToDictionary(e => e.Id);
    }

    public static float GetRockDurabilityByType(RockType rockType)
    {
        return BASE_ROCK_DURABILITY * Mathf.Pow(ROCK_DURABILITY_SCALE, (int)rockType);
    }


    /// <summary>
    /// Exp given by the smashed rocks
    /// </summary>
    public static long GetRockExp(RockType rockType)
    {
        switch (rockType)
        {
            default:
            case RockType.Copper: return 4;
            case RockType.Iron: return 12;
            case RockType.Bronze: return 30;
            case RockType.Silver: return 75;
            case RockType.Gold: return 120;
        }
    }

    #endregion

    #region MINER WEAPON

    private static void LoadDictWeaponLevelToSprites()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Miner/ContainerGameData_WeaponToSprites");
        dictWeaponLevelToSprite = container.Entries.ToDictionary(e => e.Id);
    }
    
    public static Sprite GetWeaponSpriteByLevel(int id)
    {
        return UtilsGeneral.GetGameDataSO<WeaponToSpriteSO>(id, dictWeaponLevelToSprite).Sprite;
    }

    public static List<ItemGroup> GetRequirementsForMinerWeaponLevel(int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        if (level <= MAX_MANUAL_WEAPON_REQUIREMENT)
        {
            switch (level)
            {
                default:
                case 2:
                    result.Add(new ItemGroup(0, 30));
                    result.Add(new ItemGroup(1, 15));
                    break;

                case 3:
                    result.Add(new ItemGroup(0, 90));
                    result.Add(new ItemGroup(1, 50));
                    result.Add(new ItemGroup(2, 15));
                    break;

                case 4:
                    result.Add(new ItemGroup(0, 350));
                    result.Add(new ItemGroup(1, 200));
                    result.Add(new ItemGroup(2, 50));
                    result.Add(new ItemGroup(3, 15));
                    break;

                case 5:
                    result.Add(new ItemGroup(0, 900));
                    result.Add(new ItemGroup(1, 500));
                    result.Add(new ItemGroup(2, 200));
                    result.Add(new ItemGroup(3, 50));
                    result.Add(new ItemGroup(4, 20));
                    break;
            }
        }
        else
        {
            List<int> ids = new List<int> { 0, 1, 2, 3, 4 };
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON; i++)
            {
                result.Add(new ItemGroup(ids[i], RequiredMinerItemAmount(level, i)));
            }
        }

        return result;
    }

    private static int RequiredMinerItemAmount(int level, int itemIndex)
    {
        //return Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex]));
        return 
            Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] * 
            Mathf.Pow(GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));
    }

    #endregion

    #region FISH GROUPS

    private static void LoadDictFishGroups()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Fisher/ContainerGameData_FishGroups");
        dictFishGroups = container.Entries.ToDictionary(e => e.Id);
    }


    public static FishGroupSO[] GetAllFishGroups()
    {
        return dictFishGroups.Values.OfType<FishGroupSO>().ToArray();
    }
    
    public static FishGroupSO GetFishGroupByType(FishGroupType type)
    {
        return UtilsGeneral.GetGameDataSO<FishGroupSO>((int)type, dictFishGroups);
    }

    public static FishGroupSO GetFishGroupByFish(FishSO fish)
    {
        return GetByPredicate<FishGroupSO>(group => group.Fishes.Contains(fish), dictFishGroups);
    }



    #endregion

    #region FARMER

    private static void LoadDictCompanions()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Farmer/Companions/ContainerGameData_Companions");
        dictCompanions = container.Entries.ToDictionary(e => e.Id);
    }

    public static CompanionSO[] GetAllCompanions()
    {
        return dictCompanions.Values.OfType<CompanionSO>().ToArray();
    }

    public static CompanionSO GetCompanionById(int id)
    {
        return UtilsGeneral.GetGameDataSO<CompanionSO>(id, dictCompanions);
    }

    #endregion

    

    public static T GetByPredicate<T>(Func<T, bool> predicate, Dictionary<int, ListableGameDataSO> dict) where T : ListableGameDataSO
    {
        return dict.Values
            .OfType<T>()
            .FirstOrDefault(predicate);
    }
}
