using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawnManager : MonoBehaviour
{
    private const int MAX_FISHES_IN_POOL = 10;

    [SerializeField] float minHookTime = 30f;

    // default max spawn time
    [SerializeField] float maxHookTime = 60f;

    private float timerHook;

    private float CurrentMaxHookTime => Mathf.Max(maxHookTime * player.PlayerData.CurrentCalmness, minHookTime);

    [Space(10)]
    [SerializeField] UtilsGeneral.GeneralChances<UtilsItem.FishRarity>[] rarityProbabilities;


    [Space(10)]
    [SerializeField] PlayerFisher player;

    private List<FishSO> currentPool;


    private bool isInitialized;

    private void Awake()
    {
        currentPool = new List<FishSO>();
    }


    private void Start()
    {
        FillPool();

        timerHook = GetRandomHookTime();

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        if(timerHook <= 0)
        {
            HandleHook();

            timerHook = GetRandomHookTime();
        }
        else
        {
            timerHook -= Time.deltaTime;
        }
    }

    private void HandleHook()
    {
        // Check knowledge stat success
        bool successKnowledge = GetRandomSuccessFromValue(player.PlayerData.CurrentKnowledge);

        // Get fish from pool that has been hooked
        FishSO hookedFish = GetRandomFishFromPool(successKnowledge);

        // Check success hook
        bool successReflex = GetRandomSuccessFromValue(player.PlayerData.CurrentReflex);

        if (successReflex)
        {
            HandleCaughtSuccess(hookedFish);
        }
        else
        {
            HandleCaughtUnsuccess();
        }

        // Save fisher data
        PlayerManager.Instance.UpdateFisherData(player.PlayerData);
        PlayerManager.Instance.SaveFisherData();
    }

    private void HandleCaughtSuccess(FishSO hookedFish)
    {
        // animate here

        int rewardedExp;

        // Fish caught
        bool hasAlreadyFish = PlayerManager.Instance.Inventory.HasItem(hookedFish.Id);

        if (!hasAlreadyFish)
        {
            // Add fish to caught
            PlayerManager.Instance.Inventory.AddItem(hookedFish.Id, 1);

            // todo: check for fishgroups
        }
        else
        {
            // Dismantle fish into bits? for now
            int bitsToAdd = UtilsItem.DismantleFish(hookedFish.FishRarity);
            PlayerManager.Instance.Inventory.AddBits(bitsToAdd);
        }

        // Save ivnentory
        PlayerManager.Instance.SaveInventoryData();

        // Remove from pool if caught
        currentPool.Remove(hookedFish);

        // refill pool
        FillPool();

        // Give player full exp
        rewardedExp = UtilsItem.GetFishExp(hookedFish.FishRarity);
        player.PlayerData.AddExp(rewardedExp);
    }

    private void HandleCaughtUnsuccess()
    {
        // animate here
        // fish go back into pool, nothing happens

        // Give player some exp
        int rewardedExp = 5;
        player.PlayerData.AddExp(rewardedExp);
    }

    private void FillPool()
    {
        while(currentPool.Count < MAX_FISHES_IN_POOL)
        {
            // Get day moment
            UtilsGeneral.DayMoment currentMoment = UtilsGeneral.GetDayMoment();

            // Get rand rarity 
            UtilsItem.FishRarity randRarity = UtilsGeneral.GetRandomValueFromGeneralChanches(rarityProbabilities);

            // Get luck and cycle until it fails boost rarity
            float baseLuckPlayer = player.PlayerData.CurrentLuck;
            while (GetRandomSuccessFromValue(baseLuckPlayer))
            {
                randRarity = UpgradeRarity(randRarity);
                baseLuckPlayer *= 0.5f;
            }

            // Get random fish
            FishSO randFish = UtilsItem.GetRandomFishByDayMomentAndRarity(currentMoment, randRarity);

            // Add to pool
            currentPool.Add(randFish);
        }
    }

    private FishSO GetRandomFishFromPool(bool successKnowledge)
    {
        bool found;
        FishSO result;

        int tries = 0;
        int maxTries = 1000;

        do
        {
            found = false;

            int randIndex = Random.Range(0, currentPool.Count);
            result = currentPool[randIndex];

            if (!successKnowledge)
            {
                // always found if not knowledge success
                found = true;
            }
            else
            {
                // if success on knowledge, found only if fish hasn't already got caught
                found = !PlayerManager.Instance.Inventory.HasItem(result.Id);
            }

            tries++;
        }
        while (!found && tries < maxTries);

        return result;
    }

    private UtilsItem.FishRarity UpgradeRarity(UtilsItem.FishRarity current)
    {
        int nextValue = Mathf.Min(
            (int)current + 1,
            System.Enum.GetValues(typeof(UtilsItem.FishRarity)).Length - 1
        );

        return (UtilsItem.FishRarity)nextValue;
    }

    private bool GetRandomSuccessFromValue(float value)
    {
        if (Random.value <= value) return true;
        return false;
    }

    private float GetRandomHookTime()
    {
        return Random.Range(minHookTime, CurrentMaxHookTime);
    }
}
