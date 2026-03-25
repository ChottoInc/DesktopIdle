using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsGather
{
    // Miner
    private static RockSO[] rocks;

    public enum RockType { Copper, Iron, Bronze, Silver, Gold }

    // Fisher
    private static FishGroupSO[] fishGroups;

    // Max hp, Atk, Def, Atk Spd, Crit rate, Crit dmg, Luck, Exp gain, Move spd warrior
    public enum FishGroupType { Life, Predator, Guardian, Dart, Sharp, Piercing, Golden, Elder, Quick }


    // Fisher
    private static CropSO[] crops;
    private static CompanionSO[] companions;



    private static Sprite[] minerWeaponSprites;

    private static Sprite[] blacksmithHelmetSprites;
    private static Sprite[] blacksmithArmorSprites;
    private static Sprite[] blacksmithGlovesSprites;
    private static Sprite[] blacksmithBootsSprites;

    public const int ID_MINER_WEAPON = 0;
    public const int ID_BLACKSMITH_HELMET = 0;
    public const int ID_BLACKSMITH_ARMOR = 1;
    public const int ID_BLACKSMITH_GLOVES = 2;
    public const int ID_BLACKSMITH_BOOTS = 3;

    // ------------------ MINER -----------------------

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

    private const float BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 1f;
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

    // -------------------- BLACKSMITH -----------------------
    /*
     * The first requirements for level up the miner weapon are manually set, 
     * but after 5 levels, the weapon needs at least every item on the list, so can cycle throught them and use const values to determine
     * how many you should use to level up.
     * 
     * if expanding the game you need more than 5 items to level up, make manual checks. and add const values to arrays
     * 
     * */

    private const int MAX_ITEM_REQUIREMENTS_FOR_BLACKSMITH_GEARS = 5;

    private const float BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER = 1f;
    private const float BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON = 1.2f;
    private const float BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE = 0.8f;
    private const float BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER = 0.6f;
    private const float BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD = 0.3f;

    private static readonly float[] BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL =
    {
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD
    };


    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_COPPER = 2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_IRON = 1.7f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_BRONZE = 1.4f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_SILVER = 1.2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_GOLD = 1.05f;

    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_COPPER = 2.1f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_IRON = 1.75f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_BRONZE = 1.5f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_SILVER = 1.3f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_GOLD = 1.2f;

    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_COPPER = 1.5f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_IRON = 1.4f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_BRONZE = 1.3f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_SILVER = 1.2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_GOLD = 1.1f;

    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_COPPER = 1.5f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_IRON = 1.4f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_BRONZE = 1.3f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_SILVER = 1.2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_GOLD = 1.1f;


    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_GOLD
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
        rocks = LoadRocks();

        // Fisher
        fishGroups = LoadFishGroups();

        // Farmer
        crops = LoadCrops();
        companions = LoadCompanions();

        minerWeaponSprites = LoadMinerWeaponSprites();

        blacksmithHelmetSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Helmets");
        blacksmithArmorSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Armors");
        blacksmithGlovesSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Gloves");
        blacksmithBootsSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Boots");
    }

    #region ROCKS

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
    public static long GetRockExp(RockType rockType)
    {
        switch (rockType)
        {
            default:
            case RockType.Copper: return 2;
            case RockType.Iron: return 6;
            case RockType.Bronze: return 15;
            case RockType.Silver: return 38;
            case RockType.Gold: return 60;
        }
    }

    #endregion

    #region MINER WEAPON

    public const int CHANGE_MINER_WEAPON_EVERY = 5;

    private static Sprite[] LoadMinerWeaponSprites()
    {
        return Resources.LoadAll<Sprite>("Sprites/Miner/Weapon");
    }


    public static Sprite[] GetAllMinerWeaponSprites()
    {
        return minerWeaponSprites;
    }

    public static Sprite GetMinerWeaponSpriteByIndex(int index)
    {
        if (index < minerWeaponSprites.Length)
            return minerWeaponSprites[index];
        return null;
    }

    public static List<ItemGroup> GetRequirementsForMinerWeaponLevel(int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        if (level <= 5)
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
            List<int> ids = new List<int> { 0, 1, 2, 3, 4 };
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON; i++)
            {
                result.Add(new ItemGroup(i, RequiredMinerItemAmount(level, ids[i])));
            }
        }

        return result;
    }

    private static int RequiredMinerItemAmount(int level, int itemIndex)
    {
        return Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex]));
    }

    #endregion

    #region BLACKSMITH GEARS

    public const int CHANGE_BLACKSMITH_GEARS_EVERY = 5;

    private static Sprite[] LoadBlacksmithGearSprites(string path)
    {
        return Resources.LoadAll<Sprite>(path);
    }


    public static Sprite[] GetAllBlacksmithGearSprites(int idGear)
    {
        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET: return blacksmithHelmetSprites;
            case ID_BLACKSMITH_ARMOR: return blacksmithArmorSprites;
            case ID_BLACKSMITH_GLOVES: return blacksmithGlovesSprites;
            case ID_BLACKSMITH_BOOTS: return blacksmithBootsSprites;
        }
        return null;
        
    }

    public static Sprite GetBlacksmithGearSpriteByIndex(int idGear, int index)
    {
        Sprite[] sprites = null;

        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET: sprites = blacksmithHelmetSprites; break;
            case ID_BLACKSMITH_ARMOR: sprites = blacksmithArmorSprites; break;
            case ID_BLACKSMITH_GLOVES: sprites = blacksmithGlovesSprites; break;
            case ID_BLACKSMITH_BOOTS: sprites = blacksmithBootsSprites; break;
        }

        if(sprites != null)
        {
            if (index < sprites.Length)
                return sprites[index];
        }

        return null;
    }

    public static List<ItemGroup> GetRequirementsForBlacksmithGearLevel(int idGear, int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        // For simplicity, the first 5 levels are shared between gears
        if (level <= 5)
        {
            switch (level)
            {
                default:
                case 2:
                    result.Add(new ItemGroup(150, 20));
                    result.Add(new ItemGroup(151, 1));
                    break;

                case 3:
                    result.Add(new ItemGroup(150, 70));
                    result.Add(new ItemGroup(151, 3));
                    result.Add(new ItemGroup(152, 1));
                    break;

                case 4:
                    result.Add(new ItemGroup(150, 200));
                    result.Add(new ItemGroup(151, 15));
                    result.Add(new ItemGroup(152, 7));
                    result.Add(new ItemGroup(153, 1));
                    break;

                case 5:
                    result.Add(new ItemGroup(150, 750));
                    result.Add(new ItemGroup(151, 70));
                    result.Add(new ItemGroup(152, 20));
                    result.Add(new ItemGroup(153, 5));
                    result.Add(new ItemGroup(154, 1));
                    break;
            }
        }
        else
        {

            List<int> ids = new List<int> { 150, 151, 152, 153, 154 };
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_BLACKSMITH_GEARS; i++)
            {
                int amount = RequiredBlacksmithItemAmount(idGear, level, ids[i]);

                if(amount != -1)
                    result.Add(new ItemGroup(ids[i], amount));
            }
        }

        return result;
    }

    private static int RequiredBlacksmithItemAmount(int idGear, int level, int itemIndex)
    {
        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET: return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_METAL[itemIndex]));
            case ID_BLACKSMITH_ARMOR: return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_METAL[itemIndex]));
            case ID_BLACKSMITH_GLOVES: return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_METAL[itemIndex]));
            case ID_BLACKSMITH_BOOTS: return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_METAL[itemIndex]));
        }

        return -1;
    }

    #endregion

    #region FISH GROUPS

    private static FishGroupSO[] LoadFishGroups()
    {
        return Resources.LoadAll<FishGroupSO>("Data/Gatherer/Fisher");
    }


    public static FishGroupSO[] GetAllFishGroups()
    {
        return fishGroups;
    }
    
    public static FishGroupSO GetFishGroupByType(FishGroupType type)
    {
        foreach (var group in fishGroups)
        {
            if (group.GroupType == type)
                return group;
        }
        return null;
    }

    public static FishGroupSO GetFishGroupByFish(FishSO fish)
    {
        foreach (var group in fishGroups)
        {
            if (group.Fishes.Contains(fish))
                return group;
        }
        return null;
    }

    #endregion

    #region FARMER 

    private static CropSO[] LoadCrops()
    {
        return Resources.LoadAll<CropSO>("Data/Player/Farmer");
    }


    public static CropSO[] GetAllCrops()
    {
        return crops;
    }

    public static CropSO GetCropById(int id)
    {
        foreach (var crop in crops)
        {
            if (crop.Id == id)
                return crop;
        }
        return null;
    }

    private static CompanionSO[] LoadCompanions()
    {
        return Resources.LoadAll<CompanionSO>("Data/Player/Farmer");
    }


    public static CompanionSO[] GetAllCompanions()
    {
        return companions;
    }

    public static CompanionSO GetCompanionById(int id)
    {
        foreach (var companion in companions)
        {
            if (companion.Id == id)
                return companion;
        }
        return null;
    }

    #endregion
}
