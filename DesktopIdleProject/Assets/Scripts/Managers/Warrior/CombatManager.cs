using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerFight player;

    private CombatMapSO mapSO;



    public CombatMapSO MapSO => mapSO;

    private Enemy currentEnemy;



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
        }
    }


    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPerformAttack -= OnPlayerAttack;
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
        currentEnemy.EnemyData.TakeDamage(player.PlayerData);

        if (currentEnemy.IsDead)
        {
            HandleEnemyDeath();
        }
    }

    private void OnEnemyAttack()
    {
        player.PlayerData.TakeDamage(currentEnemy.EnemyData);

        if (player.IsDead)
        {
            HandlePlayerDeath();
        }
    }



    private void HandleEnemyDeath()
    {
        //Debug.Log("Enemy dead");
        
        // --- get exp before starting death for safety
        int rewardedExp = UtilsCombatMap.GetEnemyExp(currentEnemy.EnemyData.CurrentLevel, mapSO.MapDifficuty);

        // kill enemy
        currentEnemy.PlayDeath(false);
        StageManager.Instance.AddKill(1);

        // stop fight after setting death
        EnableFight(false);

        // give exp to player
        player.PlayerData.AddExp(rewardedExp);

        // --- get card drop, card can be null, it means no drop
        CardSO randCardSO = UtilsGeneral.GetRandomValueFromGeneralChanches(StageManager.Instance.PossibleCards);
        if(randCardSO != null)
        {
            player.AddItem(randCardSO.Id, 1);
        }

        if (StageManager.Instance.CurrentEnemyIndex < StageManager.MAX_ENEMY_INDEX)
        {
            StageManager.Instance.SpawnNextEnemy();
        }
        else
        {
            if (StageManager.Instance.NextStage())
            {
                player.PlayerData.ResetAfterStage();
            }
        }
    }


    private void HandlePlayerDeath()
    {
        //Debug.Log("Player dead");

        EnableFight(false);

        StageManager.Instance.RestartCurrentStage();

        player.PlayerData.ResetAfterStage();
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
