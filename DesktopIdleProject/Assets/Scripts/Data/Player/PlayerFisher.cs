using System;
using UnityEngine;

public class PlayerFisher : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;


    private PlayerFisherData playerData;


    // ------ FISHING VARS

    public event Action<int, int> OnStatChange;

    public event Action<FishSO> OnFishCaught;




    public PlayerFisherData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= SaveFisherData;

            playerData.OnStatChange -= OnStatChangeFisher;
        }
    }

    public void Setup(PlayerFisherData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += SaveFisherData;

            playerData.OnStatChange += OnStatChangeFisher;
        }
    }

    public void HandleHook()
    {
        // Check knowledge stat success
        bool successKnowledge = UtilsGeneral.GetRandomSuccessFromValue(playerData.CurrentKnowledge);

        // Get fish from pool that has been hooked
        FishSO hookedFish = FishSpawnManager.Instance.GetRandomFishFromPool(successKnowledge);

        bool successReflex;

        if (FishSpawnManager.Instance.AlwaysCatchFishCheat)
        {
            successReflex = true;
        }
        else
        {
            // Check success hook
            successReflex = UtilsGeneral.GetRandomSuccessFromValue(playerData.CurrentReflex);
        }

        //Debug.Log("Success: " + successReflex);

        if (successReflex)
        {
            HandleCaughtSuccess(hookedFish);
        }
        else
        {
            HandleCaughtUnsuccess();
        }

        // Save fisher data
        PlayerManager.Instance.UpdateFisherData(playerData);
        PlayerManager.Instance.SaveFisherData();
    }

    private void HandleCaughtSuccess(FishSO hookedFish)
    {
        // animation
        animator.SetTrigger("Caught");

        long rewardedExp;

        // Fish caught
        bool hasAlreadyFish = PlayerManager.Instance.Inventory.HasItem(hookedFish.Id);

        if (!hasAlreadyFish)
        {
            // Add fish to caught
            PlayerManager.Instance.Inventory.AddItem(hookedFish.Id, 1);

            // check for fishgroups
            playerData.FillFishGroupsSeriesCompletion();
        }
        else
        {
            // Dismantle fish into bits? for now
            // TODO: remake this option?
            int bitsToAdd = UtilsItem.DismantleFish(hookedFish.FishRarity);
            PlayerManager.Instance.Inventory.AddBits(bitsToAdd);
        }

        // Save ivnentory
        PlayerManager.Instance.SaveInventoryData();

        // Remove from pool if caught
        FishSpawnManager.Instance.RemoveFishFromPool(hookedFish);

        // refill pool
        FishSpawnManager.Instance.FillPool();

        // Give player full exp
        rewardedExp = UtilsItem.GetFishExp(hookedFish.FishRarity);
        playerData.AddExp(rewardedExp);

        OnFishCaught?.Invoke(hookedFish);
    }

    private void HandleCaughtUnsuccess()
    {
        // animation
        animator.SetTrigger("Fled");

        // fish go back into pool, nothing happens

        // Give player some exp
        int rewardedExp = 5;
        playerData.AddExp(rewardedExp);
    }

    public void HandleSwitchScene()
    {

    }


    #region SAVE

    public void SaveFisherData()
    {
        PlayerManager.Instance.UpdateFisherData(playerData);
        PlayerManager.Instance.SaveFisherData();
    }

    #endregion

    #region HANDLE EVENTS FROM FISHER DATA

    private void OnStatChangeFisher(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
