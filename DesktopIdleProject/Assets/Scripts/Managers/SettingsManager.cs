using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private IDataService saveService;

    // ---- TUTORIAL ----

    private bool hasSeenIntroTutorial;


    public bool HasSeenIntroTutorial => hasSeenIntroTutorial;


    // --- LAST OPEN VARS ---

    private LastSceneSettings lastSceneSettings;

    

    public LastSceneSettings LastSceneSettings => lastSceneSettings;

    // --- SETTINGS ---
    private bool isAutoBattleOn;



    public bool IsAutoBattleOn => isAutoBattleOn;









    public static SettingsManager Instance { get; private set; }

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

        try
        {
            SettingsSaveData saveData = saveService.LoadData<SettingsSaveData>(UtilsSave.GetSettingsFile(), false);
            SetupFromFile(saveData);
        }
        catch(Exception e)
        {
            SetupFromDefault();
        }
    }

    private void SetupFromFile(SettingsSaveData saveData)
    {
        // tutorial

        hasSeenIntroTutorial = saveData.hasSeenIntroTutorial;


        // last scene

        lastSceneSettings = new LastSceneSettings();
        lastSceneSettings.lastSceneName = saveData.lastSceneName;
        lastSceneSettings.lastSceneType = (SceneLoaderManager.SceneType)saveData.lastSceneType;
        lastSceneSettings.lastCombatMapId = saveData.lastCombatMapId;

        // settings
        SetIsAutoBattle(saveData.isAutoBattleOn, false);
    }

    private void SetupFromDefault()
    {
        // tutorial

        hasSeenIntroTutorial = false;


        // last scene

        lastSceneSettings = new LastSceneSettings();
        lastSceneSettings.lastSceneName = "ForestScene";
        lastSceneSettings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
        lastSceneSettings.lastCombatMapId = 0;

        // first save for each map
        CombatMapSO[] maps = UtilsCombatMap.GetAllMaps();
        for (int i = 0; i < maps.Length; i++)
        {
            CombatMapSaveData mapData = new CombatMapSaveData(i, 1, 1, 0);
            saveService.SaveData(UtilsSave.GetCombatMapFile(maps[i].MapName + i.ToString()), mapData, false);
        }
       

        // settings
        SetIsAutoBattle(true, false);

        Save();
    }

    #region TUTORIAL

    public void SetSeenTutorial(int idTutorial, bool save = true)
    {
        switch (idTutorial)
        {
            default:
            case UtilsGeneral.ID_INTRO_TUTORIAL: hasSeenIntroTutorial = true; break;
        }

        if (save)
            Save();
    }

    #endregion

    #region SCENE

    public void SetSceneSettings(LastSceneSettings settings, bool save = true)
    {
        lastSceneSettings = settings;

        if (save)
            Save();
    }

    public CombatMapSaveData GetCombatMapSaveData(CombatMapSO mapSO)
    {
        try
        {
            CombatMapSaveData saveData = saveService.LoadData<CombatMapSaveData>(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), false);
            return saveData;
        }
        catch (Exception e)
        {
            Debug.LogError("Can't load combat map data id: " + mapSO.IdMap);
            return null;
        }
    }

    public void SaveCombatMapData(CombatMapSO mapSO, int currentStage, int reachedStage, int reachedPrestige)
    {
        CombatMapSaveData mapData = new CombatMapSaveData(mapSO.IdMap, currentStage, reachedStage, reachedPrestige);
        saveService.SaveData(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), mapData, false);
    }

    public void SaveCombatMapData(CombatMapSO mapSO, CombatMapSaveData combatMapSaveData)
    {
        saveService.SaveData(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), combatMapSaveData, false);
    }

    #endregion


    #region COMBAT

    public void SetIsAutoBattle(bool isOn, bool save = true)
    {
        isAutoBattleOn = isOn;

        if(save)
            Save();
    }

    #endregion

    public void Save()
    {
        SettingsSaveData data = new SettingsSaveData(this);
        saveService.SaveData(UtilsSave.GetSettingsFile(), data, false);
    }
}
