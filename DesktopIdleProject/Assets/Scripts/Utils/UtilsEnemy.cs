using UnityEngine;

public static class UtilsEnemy
{
    private static EnemySO[] enemySOs;


    public static void Initialize()
    {
        enemySOs = LoadEnemies();
    }

    private static EnemySO[] LoadEnemies()
    {
        return Resources.LoadAll<EnemySO>("Data/Enemies");
    }


    public static EnemySO[] GetAllEnemies()
    {
        return enemySOs;
    }

    public static EnemySO GetEnemySOById(int id)
    {
        foreach (var enemy in enemySOs)
        {
            if (enemy.Id == id)
                return enemy;
        }
        return null;
    }
}
