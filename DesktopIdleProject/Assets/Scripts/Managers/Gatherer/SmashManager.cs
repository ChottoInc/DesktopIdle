using System.Collections;
using UnityEngine;

public class SmashManager : MonoBehaviour
{
    [SerializeField] PlayerMiner player;


    private Rock currentRock;



    public static SmashManager Instance { get; private set; }

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
            player.OnPerformSmash -= OnPlayerSmash;
        }
    }


    public void Setup()
    {
        // setup stage
        RockSpawnManager.Instance.Setup();

        // initialize player
        PlayerMinerData playerData = PlayerManager.Instance.PlayerMinerData;
        player.Setup(playerData);


        if (player != null)
        {
            player.OnPerformSmash += OnPlayerSmash;
        }
    }

    public void StartSmash(Rock rock)
    {
        SetupRock(rock);

        StartCoroutine(CoDelaySmash(0f, 0f));
    }


    private void SetupRock(Rock rock)
    {
        // get rock
        currentRock = rock;
    }


    private IEnumerator CoDelaySmash(float timerIdle, float timerSmash)
    {
        yield return new WaitForSeconds(timerIdle);

        //currentEnemy.Show();

        yield return new WaitForSeconds(timerSmash);

        EnableSmash(true);
    }





    private void OnPlayerSmash()
    {
        currentRock.RockData.TakeDamage(player.PlayerData);

        if (currentRock.IsSmashed)
        {
            HandleRockSmash();
        }
    }

    private void HandleRockSmash()
    {
        //Debug.Log("Rock smash");

        // get exp before starting death for safety
        int rewardedExp = UtilsGather.GetRockExp(currentRock.RockData.RockSO.RockType);

        // kill enemy
        currentRock.PlayDeath(false);
        RockSpawnManager.Instance.AddSmash(1);

        // stop fight after setting death
        EnableSmash(false);

        // give exp to player
        player.PlayerData.AddExp(rewardedExp);

        GiveLoot();

        // always spawn next rock, todo: handle index rock, every now and then reset index
        RockSpawnManager.Instance.SpawnNextRock();
    }

    private void GiveLoot()
    {
        float randPercLoot = Random.value;
        float thresholdLoot = currentRock.RockData.RockSO.BaseLootChance + player.PlayerData.CurrentLuck;

        if (randPercLoot <= 1f)
        //if (randPercLoot <= thresholdLoot)
        {
            Debug.Log("Looted!");
            ItemSO randLoot = UtilsGeneral.GetRandomValueFromGeneralChanches(currentRock.RockData.RockSO.PossibleItems);

            if(randLoot != null)
            {
                PlayerManager.Instance.Inventory.AddItem(randLoot.Id, 1);
                PlayerManager.Instance.SaveInventoryData();
            }
        }
    }


    private void EnableSmash(bool smash)
    {
        player.SetSmashing(smash);
    }


    public void HandleSwitchScene()
    {
        EnableSmash(false);

        RockSpawnManager.Instance.StopSpawns();
        RockSpawnManager.Instance.KillAllRocks();
    }
}
