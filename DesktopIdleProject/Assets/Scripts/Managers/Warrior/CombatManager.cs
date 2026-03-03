using System;
using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerFight player;

    [Header("Cheats")]
    [SerializeField] bool enemyHighDamageCheat;
    [SerializeField] bool playerHighDamageCheat;
    [SerializeField] bool playerHighExpCheat;
    [SerializeField] bool cardHighDroprateCheat;

    private CombatMapSO mapSO;

    private Enemy currentEnemy;

    // Trigger used for quests
    public event Action<EnemySO> OnEnemyKill;



    public CombatMapSO MapSO => mapSO;

    public Enemy CurrentEnemy => currentEnemy;






    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPerformAttack -= OnPlayerAttack;
            player.OnResetAfterDeath -= ResetAfterDeath;
        }

        if (currentEnemy != null)
        {
            currentEnemy.OnPerformAttack -= OnEnemyAttack;
        }
    }


    public void Setup(CombatMapSO mapSO)
    {
        this.mapSO = mapSO;

        // setup stage
        StageManager.Instance.Setup();

        // initialize player
        PlayerFightData playerData = PlayerManager.Instance.PlayerFightData;
        player.Setup(playerData);


        if(player != null)
        {
            player.OnPerformAttack += OnPlayerAttack;
            player.OnResetAfterDeath += ResetAfterDeath;
        }
    }

    public void StartFight(Enemy enemy)
    {
        SetupEnemy(enemy);

        StartCoroutine(CoDelayFight(0f, 0f));
    }


    private void SetupEnemy(Enemy enemy)
    {
        // disable prev enemy if available
        if (currentEnemy != null)
        {
            currentEnemy.OnPerformAttack -= OnEnemyAttack;
        }

        // get enemy
        currentEnemy = enemy;
        
        // enable its attack again
        if (currentEnemy != null)
        {
            currentEnemy.OnPerformAttack += OnEnemyAttack;
        }
    }


    private IEnumerator CoDelayFight(float timerIdle, float timerFight)
    {
        yield return new WaitForSeconds(timerIdle);

        //currentEnemy.Show();

        yield return new WaitForSeconds(timerFight);

        EnableFight(true);
    }

    



    private void OnPlayerAttack()
    {
        if (playerHighDamageCheat)
        {
            currentEnemy.EnemyData.TakeDamageCheat(1000f);
        }
        else
        {
            currentEnemy.EnemyData.TakeDamage(player.PlayerData);
        }
            

        if (currentEnemy.IsDead)
        {
            HandleEnemyDeath();

            currentEnemy = null;
        }
    }

    private void OnEnemyAttack()
    {
        if (player == null) return;

        if (player.PlayerData == null) return;

        if (enemyHighDamageCheat)
        {
            player.PlayerData.TakeDamageCheat(1000f);
        }
        else
        {
            player.PlayerData.TakeDamage(currentEnemy.EnemyData);
        }

        if (player.IsDead)
        {
            HandlePlayerDeath();
        }
    }



    private void HandleEnemyDeath()
    {
        //Debug.Log("Enemy dead");

        // Trigger which enemy died, mainly used for quests
        OnEnemyKill?.Invoke(currentEnemy.EnemyData.EnemySO);
        
        // --- get exp before starting death for safety
        int rewardedExp = UtilsCombatMap.GetEnemyExp(currentEnemy.EnemyData.CurrentLevel, mapSO.MapDifficuty);

        // todo: add addictional exp multipliers here
        rewardedExp = Mathf.RoundToInt( 
            (float)rewardedExp *
            PlayerManager.Instance.FisherPredatorSeriesMultiplier
            );

        // kill enemy
        currentEnemy.PlayDeath(false);
        StageManager.Instance.AddKill(1);

        // stop fight after setting death
        EnableFight(false);

        // give exp to player
        if (playerHighExpCheat)
        {
            player.PlayerData.AddExp(2000);
        }
        else
        {
            player.PlayerData.AddExp(rewardedExp);
        }
            
        PlayerManager.Instance.UpdateFightData(player.PlayerData);

        // by default cards drop with 10%, add luck of warrior
        float baseCardDropRate = 0.1f;
        if (cardHighDroprateCheat)
        {
            // get card drop, card can be null, it means no drop
            CardSO randCardSO = UtilsGeneral.GetRandomValueFromGeneralChanches(StageManager.Instance.PossibleCards);
            if (randCardSO != null)
            {
                player.AddItem(randCardSO.Id, 1);
            }
        }
        else
        {
            if (UnityEngine.Random.value <= baseCardDropRate + player.PlayerData.CurrentLuck)
            {
                // get card drop, card can be null, it means no drop
                CardSO randCardSO = UtilsGeneral.GetRandomValueFromGeneralChanches(StageManager.Instance.PossibleCards);
                if (randCardSO != null)
                {
                    player.AddItem(randCardSO.Id, 1);
                }
            }
        }
        
        if (StageManager.Instance.CurrentEnemyIndex < mapSO.EnemiesPerStage)
        {
            StageManager.Instance.SpawnNextEnemy();
        }
        else
        {
            if (StageManager.Instance.NextStage())
            {
                player.PlayerData.ResetAfterStage();

                if(StageManager.Instance.CurrentStage > mapSO.Stages)
                {
                    // If stage > maximum, means I'm in autobattle
                    // Go to next map in this case
                    // Only works if last map, or reset last stage instead
                    if(mapSO.NextMap != null)
                    {
                        LastSceneSettings settings = new LastSceneSettings();
                        settings.lastSceneName = mapSO.NextMap.MapSceneName;
                        settings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
                        settings.lastCombatMapId = mapSO.NextMap.IdMap;

                        // Add next map to availables
                        player.PlayerData.AddAvailableMap(mapSO.NextMap.IdMap);
                        PlayerManager.Instance.UpdateFightData(player.PlayerData);
                        PlayerManager.Instance.SaveFightData();

                        SceneLoaderManager.Instance.LoadScene(settings);
                    }
                }
            }
        }
    }


    private void HandlePlayerDeath()
    {
        //Debug.Log("Player dead");

        EnableFight(false);

        player.SetDeath(true);
    }

    private void ResetAfterDeath()
    {
        StageManager.Instance.RestartCurrentStage();

        player.PlayerData.ResetAfterStage();

        player.SetDeath(false);
    }



    private void EnableFight(bool fight)
    {
        player.SetAttacking(fight);

        if(currentEnemy != null)
        {
            Vector2 playerDir = player.transform.position - currentEnemy.transform.position;
            currentEnemy.SetAttacking(fight, playerDir.normalized);
        }
    }


    public void HandleSwitchScene()
    {
        EnableFight(false);

        StageManager.Instance.StopSpawns();
        StageManager.Instance.KillAllEnemies();

        player.PlayerData.ResetAfterStage();
    }
}
