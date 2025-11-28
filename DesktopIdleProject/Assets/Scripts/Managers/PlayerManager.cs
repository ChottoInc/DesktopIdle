using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;


    private PlayerFightData playerFightData;



    public PlayerFightData PlayerFightData => playerFightData;


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

    public void Setup(IDataService service)
    {
        saveService = service;

        LoadFightData();
    }

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
        }

        SaveFightData();
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

    public void SaveAll()
    {
        SaveFightData();
    }
}
