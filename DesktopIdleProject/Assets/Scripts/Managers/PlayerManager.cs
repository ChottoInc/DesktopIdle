using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;


    // --- INVENTORY
    private Inventory inventory;

    // Trigger used for quests
    public event Action<int> OnItemAdd;


    // ------- JOBS -------------
    private List<UtilsPlayer.PlayerJob> availableJobs;


    // --- WARRIOR
    private PlayerFightData playerFightData;

    // --- GATHERER
    private PlayerMinerData playerMinerData;

    private PlayerBlacksmithData playerBlacksmithData;






    public Inventory Inventory => inventory;



    public List<UtilsPlayer.PlayerJob> AvailableJobs => availableJobs;


    public PlayerFightData PlayerFightData => playerFightData;

    public PlayerMinerData PlayerMinerData => playerMinerData;

    public PlayerBlacksmithData PlayerBlacksmithData => playerBlacksmithData;




    // ---- PLAYER GLOBAL VARIABLES ----

    public float WeaponMinerMultiplier => UtilsPlayer.GetMinerWeaponMultiplier(playerMinerData.WeaponLevel);

    public float HelmetMaxHpBlacksmithMultiplier => UtilsPlayer.GetBlacksmithHelmetMaxHpMultiplier(playerBlacksmithData.HelmetLevel);
    public float ArmorDefBlacksmithMultiplier => UtilsPlayer.GetBlacksmithArmorDefMultiplier(playerBlacksmithData.ArmorLevel);
    public float GlovesAtkSpdBlacksmithMultiplier => UtilsPlayer.GetBlacksmithGlovesAtkSpdMultiplier(playerBlacksmithData.GlovesLevel);
    public float GlovesCritDmgBlacksmithMultiplier => UtilsPlayer.GetBlacksmithGlovesCritDmgMultiplier(playerBlacksmithData.GlovesLevel);
    public float BootsDefBlacksmithMultiplier => UtilsPlayer.GetBlacksmithBootsDefMultiplier(playerBlacksmithData.BootsLevel);
    public float BootsCritRateBlacksmithMultiplier => UtilsPlayer.GetBlacksmithBootsCritRateMultiplier(playerBlacksmithData.BootsLevel);




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

    private void OnDestroy()
    {
        if(inventory != null)
        {
            inventory.OnItemAdd -= ItemAdd;
        }
    }

    // Called after Settings Manager setup
    public void Setup(IDataService service)
    {
        saveService = service;

        LoadJobsData();
        LoadInventoryData();

        LoadMinerData();
        LoadBlacksmithData();

        LoadFightData();
    }

    #region JOBS DATA

    private void LoadJobsData()
    {
        try
        {
            PlayerJobsSaveData jobsSaveData = saveService.LoadData<PlayerJobsSaveData>(UtilsSave.GetPlayerJobsFile(), false);

            availableJobs = new List<UtilsPlayer.PlayerJob>();

            foreach (var job in jobsSaveData.availableJobs)
            {
                availableJobs.Add((UtilsPlayer.PlayerJob)job);
            }
        }
        catch
        {
            // default available jobs
            availableJobs = new List<UtilsPlayer.PlayerJob>
            {
                UtilsPlayer.PlayerJob.None,
                UtilsPlayer.PlayerJob.Warrior,
                UtilsPlayer.PlayerJob.Miner,
                //UtilsPlayer.PlayerJob.Blacksmith
            };

            SaveJobsData();
        }
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveJobsData()
    {
        PlayerJobsSaveData data = new PlayerJobsSaveData(this);
        saveService.SaveData(UtilsSave.GetPlayerJobsFile(), data, false);
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
        catch
        {
            inventory = new Inventory();
            SaveInventoryData();
        }

        inventory.OnItemAdd += ItemAdd;
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

    private void ItemAdd(int id)
    {
        OnItemAdd?.Invoke(id);
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
        catch
        {
            playerFightData = new PlayerFightData();
            SaveFightData();
        }

    }

    public void UpdateFightData(PlayerFightData data)
    {
        playerFightData = data;
        SaveFightData();
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
        catch
        {
            playerMinerData = new PlayerMinerData();
            SaveMinerData();
        }

    }

    public void UpdateMinerData(PlayerMinerData data)
    {
        playerMinerData = data;
        SaveMinerData();
    }

    public void SaveMinerData()
    {
        PlayerMinerSaveData data = new PlayerMinerSaveData(playerMinerData);
        saveService.SaveData(UtilsSave.GetPlayerMinerFile(), data, false);
    }

    #endregion

    #region BLACKSMITH

    private void LoadBlacksmithData()
    {
        try
        {
            PlayerBlacksmithSaveData blacksmithSaveData = saveService.LoadData<PlayerBlacksmithSaveData>(UtilsSave.GetPlayerBlacksmithFile(), false);
            playerBlacksmithData = new PlayerBlacksmithData(blacksmithSaveData);
        }
        catch
        {
            playerBlacksmithData = new PlayerBlacksmithData();
            SaveBlacksmithData();
        }

    }

    public void UpdateBlacksmithData(PlayerBlacksmithData data)
    {
        playerBlacksmithData = data;
        SaveBlacksmithData();
    }

    public void SaveBlacksmithData()
    {
        PlayerBlacksmithSaveData data = new PlayerBlacksmithSaveData(playerBlacksmithData);
        saveService.SaveData(UtilsSave.GetPlayerBlacksmithFile(), data, false);
    }

    #endregion

    public void SaveAll()
    {
        SaveInventoryData();
        SaveFightData();
        SaveMinerData();
    }
}
