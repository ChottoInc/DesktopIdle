using UnityEngine;

public static class UtilsCombatMap
{
    private static CombatMapSO[] maps;


    public static void Initialize()
    {
        maps = LoadMaps();
    }

    private static CombatMapSO[] LoadMaps()
    {
        return Resources.LoadAll<CombatMapSO>("Data/CombatMaps");
    }


    public static CombatMapSO[] GetAllMaps()
    {
        return maps;
    }

    public static CombatMapSO GetMapById(int id)
    {
        foreach (var map in maps)
        {
            if(map.IdMap == id)
                return map;
        }
        return null;
    }





    public enum MapDifficulty
    {
        VeryEasy,   // 0
        Easy,       // 1
        Normal,     // 2
        Hard,       // 3
        VeryHard    // 4
    }

    // Used to calculate from base exp given
    public static float[] DifficultyExpMultiplier = 
    {
        1.0f,   // VeryEasy
        1.5f,  // Easy
        2.0f,   // Normal
        2.5f,   // Hard
        3.0f    // VeryHard
    };

    // Used to calculate actual stats
    public static float[] DifficultyStatMultiplier = 
    {
        0.7f,
        0.85f,
        1.0f,
        1.3f,
        1.7f
    };


    /// <summary>
    /// Exp given by the dead monsters
    /// </summary>
    public static int GetEnemyExp(int enemyLevel, MapDifficulty difficulty)
    {
        float baseExp = 4.5f;
        float exp = baseExp
                    * Mathf.Pow(enemyLevel, 1.1f)
                    * DifficultyExpMultiplier[(int)difficulty];

        return Mathf.FloorToInt(exp);
        //return Mathf.FloorToInt(exp * 100);
    }
}
