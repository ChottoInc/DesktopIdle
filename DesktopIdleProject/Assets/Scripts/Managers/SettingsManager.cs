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

    // --------- GAMEPLAY

    private bool isAutoBattleOn;
    private bool areTooltipsOn;


    public bool IsAutoBattleOn => isAutoBattleOn;
    public bool AreTooltipsOn => areTooltipsOn;


    // --------- VIDEO

    private bool isAlwaysOnTop;
    private bool isClickThrough;
    private bool is60FPS;



    public event Action<bool> OnAlwaysOnTopChange;
    public event Action<bool> OnClickThroughChange;


    public bool IsAlwaysOnTop => isAlwaysOnTop;
    public bool IsClickThrough => isClickThrough;
    public bool Is60FPS => is60FPS;


    // --------- AUDIO

    private float masterVolume;


    public float MasterVolume => masterVolume;






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
        // --- gameplay
        SetIsAutoBattle(saveData.isAutoBattleOn, false);
        SetAreTooltipsOn(saveData.areTooltipsOn, false);

        // --- video
        SetIsAlwaysOnTop(saveData.isAlwaysOnTop, false);
        SetIsClickThrough(saveData.isClickThrough, false);
        SetIs60FPS(saveData.is60FPS, false);

        // --- audio
        SetMasterVolume(saveData.masterVolume, false);
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


        // Combat map files
        // first save for each map
        CombatMapSO[] maps = UtilsCombatMap.GetAllMaps();
        for (int i = 0; i < maps.Length; i++)
        {
            CombatMapSaveData mapData = new CombatMapSaveData(maps[i].IdMap, 1, 1, 0);
            saveService.SaveData(UtilsSave.GetCombatMapFile(maps[i].MapName + i.ToString()), mapData, false);
        }


        // Quest files
        InitializeQuests();


        // settings
        // --- gameplay
        SetIsAutoBattle(true, false);
        SetAreTooltipsOn(true, false);

        // --- video
        SetIsAlwaysOnTop(false, false);
        SetIsClickThrough(true, false);
        SetIs60FPS(true, false);

        // --- audio
        SetMasterVolume(1f, false);

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

    #region QUESTS

    private void InitializeQuests()
    {
        InitializeStoryQuests();
    }

    private void InitializeStoryQuests()
    {
        // first save for each story quest
        QuestStorySO[] storyQuests = UtilsQuest.GetAllStoryQuests();
        for (int i = 0; i < storyQuests.Length; i++)
        {
            QuestStorySO so = storyQuests[i];
            UtilsQuest.QuestDataProgress questProgress = new UtilsQuest.QuestDataProgress();

            // initialize quest data progress
            switch (so.QuestData.questType)
            {
                case UtilsQuest.QuestType.Kill:
                case UtilsQuest.QuestType.Obtain:
                case UtilsQuest.QuestType.LevelUp:
                    questProgress.progressCounter = 0;
                    break;
            }

            // save first time for each story quest
            QuestStorySaveData questStoryData = new QuestStorySaveData(storyQuests[i].UniqueId, questProgress);
            saveService.SaveData(UtilsSave.GetQuestFile(so.UniqueId), questStoryData, false);
        }
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


    #region GAMEPLAY

    public void SetIsAutoBattle(bool isOn, bool save = true)
    {
        isAutoBattleOn = isOn;

        if(save)
            Save();
    }

    public void SetAreTooltipsOn(bool isOn, bool save = true)
    {
        areTooltipsOn = isOn;

        if (save)
            Save();
    }

    #endregion

    #region VIDEO

    //TODO -  Call events to trigger changes

    public void SetIsAlwaysOnTop(bool isOn, bool save = true)
    {
        isAlwaysOnTop = isOn;
        OnAlwaysOnTopChange?.Invoke(isAlwaysOnTop);

        if (save)
            Save();
    }

    public void SetIsClickThrough(bool isOn, bool save = true)
    {
        isClickThrough = isOn;
        OnClickThroughChange?.Invoke(isClickThrough);

        if (save)
            Save();
    }

    public void SetIs60FPS(bool isOn, bool save = true)
    {
        is60FPS = isOn;

        if (is60FPS)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }
        //OnFPSChange?.Invoke(is60FPS);

        if (save)
            Save();
    }

    #endregion

    #region AUDIO

    public void SetMasterVolume(float value, bool save = true)
    {
        masterVolume = value;

        AudioManager.Instance.SetMasterVolume(masterVolume);

        if (save)
            Save();
    }

    #endregion


    public void Save()
    {
        SettingsSaveData data = new SettingsSaveData(this);
        saveService.SaveData(UtilsSave.GetSettingsFile(), data, false);
    }
}
