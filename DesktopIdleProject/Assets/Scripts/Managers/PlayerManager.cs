using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;

    // --- QUESTS
    private Dictionary<string, UtilsQuest.QuestDataProgress> dictQuestsStoryProgress;


    // --- INVENTORY
    private Inventory inventory;


    // --- WARRIOR
    private PlayerFightData playerFightData;

    // --- GATHERER
    private PlayerMinerData playerMinerData;



    public Inventory Inventory => inventory;


    public PlayerFightData PlayerFightData => playerFightData;

    public PlayerMinerData PlayerMinerData => playerMinerData;




    // ---- PLAYER GLOBAL VARIABLES ----

    public float WeaponMinerMultiplier => UtilsPlayer.GetMinerWeaponMultiplier(playerMinerData.WeaponLevel);




    public static PlayerManager Instance { get; private set; }

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

        DontDestroyOnLoad(gameObject);
    }

    // Called after Settings Manager setup
    public void Setup(IDataService service)
    {
        saveService = service;

        LoadQuestsData();
        LoadInventoryData();
        LoadFightData();
        LoadMinerData();
    }

    #region QUESTS

    private void LoadQuestsData()
    {
        LoadStoryQuests();
    }

    private void LoadStoryQuests()
    {
        dictQuestsStoryProgress = new Dictionary<string, UtilsQuest.QuestDataProgress>();

        var storyQuestSOs = UtilsQuest.GetAllStoryQuests();

        // used for debug infos
        int excpetionIndex = 0;

        try
        {
            for (int i = 0; i < storyQuestSOs.Length; i++)
            {
                excpetionIndex = i;

                // get file for single quest
                QuestStorySaveData saveData = saveService.LoadData<QuestStorySaveData>(UtilsSave.GetQuestFile(storyQuestSOs[i].UniqueId), false);

                // save in dictionary
                dictQuestsStoryProgress.Add(saveData.questId, new UtilsQuest.QuestDataProgress(saveData));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Can't load quest data id: " + storyQuestSOs[excpetionIndex].UniqueId);
        }

        Debug.Log("Dictionary quests counter: " + dictQuestsStoryProgress.Count);
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveQuestsData()
    {
        InventorySaveData data = new InventorySaveData(inventory);
        saveService.SaveData(UtilsSave.GetPlayerInventoryFile(), data, false);
    }

    #endregion

    #region INVENTORY DATA

    private void LoadInventoryData()
    {
        try
        {
            InventorySaveData inventorySaveData = saveService.LoadData<InventorySaveData>(UtilsSave.GetPlayerInventoryFile(), false);
            inventory = new Inventory(inventorySaveData);
        }
        catch (Exception e)
        {
            inventory = new Inventory();
            SaveInventoryData();
        }
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveInventoryData()
    {
        InventorySaveData data = new InventorySaveData(inventory);
        saveService.SaveData(UtilsSave.GetPlayerInventoryFile(), data, false);
    }

    #endregion

    #region FIGHT DATA

    private void LoadFightData()
    {
        try
        {
            PlayerFightSaveData fightSaveData = saveService.LoadData<PlayerFightSaveData>(UtilsSave.GetPlayerFightFile(), false);
            playerFightData = new PlayerFightData(fightSaveData);
        }
        catch (Exception e)
        {
            playerFightData = new PlayerFightData();
            SaveFightData();
        }

    }

    public void UpdateFightData(PlayerFightData data)
    {
        playerFightData = data;
    }

    public void SaveFightData()
    {
        PlayerFightSaveData data = new PlayerFightSaveData(playerFightData);
        saveService.SaveData(UtilsSave.GetPlayerFightFile(), data, false);
    }

    #endregion

    #region MINER DATA

    private void LoadMinerData()
    {
        try
        {
            PlayerMinerSaveData minerSaveData = saveService.LoadData<PlayerMinerSaveData>(UtilsSave.GetPlayerMinerFile(), false);
            playerMinerData = new PlayerMinerData(minerSaveData);
        }
        catch (Exception e)
        {
            playerMinerData = new PlayerMinerData();
            SaveMinerData();
        }

    }

    public void UpdateMinerData(PlayerMinerData data)
    {
        playerMinerData = data;
    }

    public void SaveMinerData()
    {
        PlayerMinerSaveData data = new PlayerMinerSaveData(playerMinerData);
        saveService.SaveData(UtilsSave.GetPlayerMinerFile(), data, false);
    }

    #endregion

    public void SaveAll()
    {
        SaveInventoryData();
        SaveFightData();
        SaveMinerData();
    }
}
