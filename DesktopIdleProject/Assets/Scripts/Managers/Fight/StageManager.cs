using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //public const int MAX_ENEMY_INDEX = 100;
    public const int MAX_ENEMY_INDEX = 10;

    [SerializeField] private int startingEnemies = 20;

    [Header("UI")]
    [SerializeField] TMP_Text textStage;

    private int currentPrestige;
    private int currentStage;
    private int currentEnemyIndex;


    private int currentStageKilled;



    private float offsetSpawn = 200f;
    private float ySpawn = 2f;
    private float minXSpawn, maxXSpawn;

    private List<Enemy> currentEnemies;


    public int CurrentPrestige => currentPrestige;
    public int CurrentStage => currentStage;
    public int CurrentEnemyIndex => currentEnemyIndex;


    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Setup()
    {
        currentEnemies = new List<Enemy>();

        currentPrestige = 0;
        currentStage = 1;
        currentEnemyIndex = 1;

        minXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(0f + offsetSpawn, 0)).x; 
        maxXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(Screen.currentResolution.width - offsetSpawn, 0)).x;

        StartCoroutine(CoSpawnStartingEnemies());

        UpdateStageUI();
    }

    #region ENEMY FUNCTIONS

    /// <summary>
    /// Called every time the game requires the next enemy
    /// </summary>
    private EnemyData GenerateEnemy()
    {
        // generate data
        EnemyData result = new EnemyData(CombatManager.Instance.TempMap);

        // increase index
        currentEnemyIndex++;

        return result;
    }

    private IEnumerator CoSpawnStartingEnemies(float timer = 0f)
    {
        yield return new WaitForSeconds(timer);

        for (int i = 0; i < startingEnemies; i++)
        {
            float randX = Random.Range(minXSpawn, maxXSpawn);
            Vector2 spawnPos = new Vector2(randX, ySpawn);

            EnemyData data = GenerateEnemy();
            SpawnEnemy(data, spawnPos);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnEnemy(EnemyData data, Vector2 spawnPos)
    {
        GameObject enemyObj = PoolManager.Instance.Pull("Enemy");
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        currentEnemies.Add(enemy);

        enemy.Setup(data, currentEnemyIndex - 1);

        enemyObj.transform.position = spawnPos;
    }

    public void SpawnNextEnemy()
    {
        float randX = Random.Range(minXSpawn, maxXSpawn);
        Vector2 spawnPos = new Vector2(randX, ySpawn);

        EnemyData data = GenerateEnemy();
        SpawnEnemy(data, spawnPos);
    }

    public void RemoveFromCurrentEnemiesList(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
    }

    #endregion

    public void AddKill(int amount)
    {
        currentStageKilled += amount;
    }

    

    /// <summary>
    /// If true resets player and move to next stage
    /// </summary>
    public bool NextStage()
    {
        /*
         * stop player
         * */
        if (currentStageKilled != currentEnemyIndex - 1) return false;

        if(currentStage <= CombatManager.Instance.TempMap.Stages && SettingsManager.Instance.IsAutoBattleOn)
        {
            currentStage++;

            Resets();

            StartCoroutine(CoSpawnStartingEnemies());

            UpdateStageUI();

            return true;
        }
        else
        {
            //Debug.Log("Is auto attle on: " + SettingsManager.Instance.IsAutoBattleOn);

            Resets();

            StartCoroutine(CoSpawnStartingEnemies());

            UpdateStageUI();

            return true;
        }
    }

    public void RestartCurrentStage()
    {
        foreach (var enemy in currentEnemies)
        {
            enemy.PlayDeath(true);
        }

        Resets();

        StartCoroutine(CoSpawnStartingEnemies(2f));
    }

    private void Resets()
    {
        currentEnemyIndex = 1;
        currentStageKilled = 0;
    }

    #region UI

    private void UpdateStageUI()
    {
        textStage.text = $"Forest - {CurrentStage}";
    }

    #endregion
}
