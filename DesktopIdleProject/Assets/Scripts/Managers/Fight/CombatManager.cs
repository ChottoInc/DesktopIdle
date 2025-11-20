using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] CombatMapSO tempMap;
    [SerializeField] PlayerFight player;


    public CombatMapSO TempMap => tempMap;

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


    public void Setup()
    {
        // setup stage
        StageManager.Instance.Setup();

        /*
         * when dead call stage manager to get new enemy
         * set enemy every time you start new fight
         * */

        // temp initialize player
        PlayerFightData playerData = new PlayerFightData();
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
        
        // get exp before starting death for safety
        int rewardedExp = UtilsCombatMap.GetEnemyExp(currentEnemy.EnemyData.CurrentLevel, tempMap.MapDifficuty);

        // kill enemy
        currentEnemy.PlayDeath(false);
        StageManager.Instance.AddKill(1);

        // stop fight after setting death
        EnableFight(false);

        // give exp to player
        player.PlayerData.AddExp(rewardedExp);

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
        currentEnemy.SetAttacking(fight);
    }
}
