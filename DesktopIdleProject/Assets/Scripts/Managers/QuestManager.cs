using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static UtilsQuest;

public class QuestManager : MonoBehaviour
{
    private IDataService saveService;

    // --- STORY QUESTS

    // Store every quest progress ever made
    private Dictionary<string, QuestDataProgress> dictQuestsStoryProgress;

    // List of all active story quests
    private List<string> activeStoryQuests;



    public Dictionary<string, QuestDataProgress> DictQuestsStoryProgress => dictQuestsStoryProgress;

    public List<string> ActiveStoryQuests => activeStoryQuests;


    // --- BOUNTY QUESTS

    // Store bounties quest progress
    private Dictionary<string, QuestDataProgress> dictQuestsBountyProgress;

    // Store slot and its active bounty quests
    private Dictionary<int, string> activeBountyQuests;



    public Dictionary<string, QuestDataProgress> DictQuestsBountyProgress => dictQuestsBountyProgress;

    public Dictionary<int, string> ActiveBountyQuests => activeBountyQuests;



    // --- DAILY QUESTS

    // Store every quest progress for daily
    private Dictionary<string, QuestDataProgress> dictQuestsDailyProgress;

    // List of all active daily quests
    private List<string> activeDailyQuests;


    private long lastDailyCreationDate;


    public Dictionary<string, QuestDataProgress> DictQuestsDailyProgress => dictQuestsDailyProgress;

    public List<string> ActiveDailyQuests => activeDailyQuests;


    public long LastDailyCreationDate => lastDailyCreationDate;










    private bool isPlayerObserverInit;

    private PlayerFight playerFight;
    private PlayerMiner playerMiner;



    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        //Called here because is destroyd on scene switch

        if (CombatManager.Instance != null)
            CombatManager.Instance.OnEnemyKill -= OnEnemyKilled;

        if (playerFight != null)
            playerFight.OnStatChange -= OnStatUp;

        if (playerMiner != null)
            playerMiner.OnStatChange -= OnStatUp;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;

        // Called here because player manager is non destroyable
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnItemAdd -= OnItemObtain;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if home only
        if (scene.name == "HomeScene") return;


        LastSceneSettings settings = SettingsManager.Instance.LastSceneSettings;

        switch (settings.lastSceneType)
        {
            case SceneLoaderManager.SceneType.CombatMap:
                playerFight = FindFirstObjectByType<PlayerFight>();
                playerFight.OnStatChange += OnStatUp;

                CombatManager.Instance.OnEnemyKill += OnEnemyKilled;
                break;

            case SceneLoaderManager.SceneType.Miner:
                playerMiner = FindFirstObjectByType<PlayerMiner>();
                playerMiner.OnStatChange += OnStatUp;
                break;
        }
    }

    private void Update()
    {
        if (isPlayerObserverInit) return;

        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnItemAdd += OnItemObtain;
            isPlayerObserverInit = true;
        }
    }

    //Called after player manager
    public void Setup(IDataService service)
    {
        saveService = service;

        try
        {
            QuestsSaveData saveData = saveService.LoadData<QuestsSaveData>(UtilsSave.GetQuestFile(), false);
            SetupFromFile(saveData);
        }
        catch
        {
            SetupFromDefault();
            SaveQuestsData();

            //Debug.Log("Datas quest: " + dictQuestsStoryProgress.Count);
            //Debug.Log("Datas quest active: " + activeStoryQuests.Count);
        }
    }

    #region DEFAULT

    private void SetupFromDefault()
    {
        lastDailyCreationDate = DateTime.UtcNow.Ticks;

        InitializeStoryQuests();
        InitializeBountyQuests();
        InitializeDailyQuests();
    }

    private void InitializeStoryQuests()
    {
        // initialize dict and first actives quests
        activeStoryQuests = new List<string>();
        dictQuestsStoryProgress = new Dictionary<string, QuestDataProgress>();

        // create default for every story
        QuestStorySO[] storyQuests = GetAllStoryQuests();
        for (int i = 0; i < storyQuests.Length; i++)
        {
            QuestStorySO so = storyQuests[i];
            QuestDataProgress questProgress = new QuestDataProgress();

            questProgress.isActive = so.IsActiveFromStart;

            // add to active if stom SO
            if (so.IsActiveFromStart)
            {
                activeStoryQuests.Add(so.UniqueId);
            }

            // initialize quest data progress
            switch (so.QuestData.questObjectiveType)
            {
                case QuestObjectiveType.Kill:
                case QuestObjectiveType.Obtain:
                case QuestObjectiveType.LevelUp:
                    questProgress.progressCounter = 0;
                    break;
            }

            questProgress.isCleared = false;

            // save in dictionary
            dictQuestsStoryProgress.Add(so.UniqueId, questProgress);
        }
    }

    private void InitializeBountyQuests()
    {
        // initialize dict and first actives quests
        activeBountyQuests = new Dictionary<int, string>();
        dictQuestsBountyProgress = new Dictionary<string, QuestDataProgress>();
    }

    /// <summary>
    /// Called when file is empty, or when comparing dates and day has changed
    /// </summary>
    private void InitializeDailyQuests()
    {
        // initialize dict and first actives quests
        activeDailyQuests = new List<string>();
        dictQuestsDailyProgress = new Dictionary<string, QuestDataProgress>();

        //TODO:  change 3 with const value or random one between values
        for (int i = 0; i < 3; i++)
        {
            int tries = 0;
            int MAX_TRIES = 1000;
            bool valid;

            QuestDailySO daily;

            do
            {
                valid = true;

                daily = GetRandomDailyQuest();

                // check if daily quest has already been pulled and added to dailies
                if (activeDailyQuests.Contains(daily.UniqueId))
                    valid = false;

                if (valid)
                {
                    // check if player has the job for the quest
                    if (!daily.AvailableFor.SharesAnyValueWith(PlayerManager.Instance.PlayerJobsData.AvailableJobs))
                        valid = false;

                    // check if daily need to increase a stat level
                    // if stat is already at max, discard
                    if(daily.QuestData.questObjectiveType == QuestObjectiveType.LevelUp)
                    {
                        if (daily.QuestData.questLevelUpSpecific)
                        {
                            int statId = daily.QuestData.statId;

                            int currentLevel = UtilsPlayer.GetStatCurrentLevelById(statId);
                            int maxLevel = UtilsPlayer.GetStatMaxLevelById(statId);

                            if (currentLevel >= maxLevel)
                                valid = false;
                        }
                    }
                }

                tries++;
            } while (!valid && tries < MAX_TRIES);

            if (valid)
            {
                // add to active and create progress
                activeDailyQuests.Add(daily.UniqueId);

                QuestDataProgress questProgress = new QuestDataProgress();
                questProgress.isActive = true;

                // initialize quest data progress
                switch (daily.QuestData.questObjectiveType)
                {
                    case QuestObjectiveType.Kill:
                    case QuestObjectiveType.Obtain:
                    case QuestObjectiveType.LevelUp:
                        questProgress.progressCounter = 0;
                        break;
                }

                questProgress.isCleared = false;

                // save in dictionary
                dictQuestsDailyProgress.Add(daily.UniqueId, questProgress);
            }
        }
    }

    #endregion

    #region FROM FILE

    private void SetupFromFile(QuestsSaveData saveData)
    {
        lastDailyCreationDate = saveData.lastDailyCreationDate;

        LoadStoryQuests(saveData.storySaveDatas);
        LoadBountyQuests(saveData.bountySaveDatas);

        // check for daily using date
        DateTime lastDailyDate = new DateTime(lastDailyCreationDate, DateTimeKind.Utc);
        if(DateTime.UtcNow.Date != lastDailyDate)
        {
            InitializeDailyQuests();
        }
        else
        {
            LoadDailyQuests(saveData.dailySaveDatas);
        }
    }

    private void LoadStoryQuests(List<QuestStorySaveData> datas)
    {
        activeStoryQuests = new List<string>();
        dictQuestsStoryProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);
                dictQuestsStoryProgress.Add(datas[i].questId, dataProgress);

                // set active from reading
                if (dataProgress.isActive)
                {
                    activeStoryQuests.Add(datas[i].questId);
                }
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + dictQuestsStoryProgress.Count);
    }

    private void LoadBountyQuests(List<QuestBountySaveData> datas)
    {
        activeBountyQuests = new Dictionary<int, string>();
        dictQuestsBountyProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);
                dictQuestsBountyProgress.Add(datas[i].questId, dataProgress);

                activeBountyQuests.Add(datas[i].slotTab, datas[i].questId);
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + dictQuestsBountyProgress.Count);
    }

    public bool IsBountyActiveById(string id)
    {
        foreach (var activeId in activeBountyQuests.Values)
        {
            if (activeId == id)
                return true;
        }
        return false;
    }

    private void LoadDailyQuests(List<QuestDailySaveData> datas)
    {
        activeDailyQuests = new List<string>();
        dictQuestsDailyProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);
                dictQuestsDailyProgress.Add(datas[i].questId, dataProgress);

                // set active from reading
                if (dataProgress.isActive)
                {
                    activeDailyQuests.Add(datas[i].questId);
                }
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + dictQuestsStoryProgress.Count);
    }

    #endregion

    public void AddActiveBountyQuest(int slot, string id)
    {
        // add to active
        activeBountyQuests.Add(slot, id);

        // add to dictionary
        QuestDataProgress progress = new QuestDataProgress();
        progress.isActive = true;
        dictQuestsBountyProgress.Add(id, progress);
    }

    #region STORY

    public void SetStoryQuestCleared(string id)
    {
        QuestDataProgress progressData = dictQuestsStoryProgress[id];
        progressData.isCleared = true;
        dictQuestsStoryProgress[id] = progressData;
    }

    public void UpdateStoryQuests()
    {
        // copy of dictionary with new progress info
        Dictionary<string, QuestDataProgress> copyDict = new Dictionary<string, QuestDataProgress>();

        foreach (var pair in dictQuestsStoryProgress)
        {
            if (pair.Value.isCleared)
            {
                QuestStorySO so = GetStoryQuestById(pair.Key);

                // get next quests
                var nexts = so.Nexts;

                // set quest progress as cleared
                QuestDataProgress copyProgress = pair.Value;
                copyProgress.isActive = false;
                
                // new progress into copy dictionary
                copyDict.Add(pair.Key, copyProgress);

                // update list of actives
                if(nexts != null)
                {
                    foreach(var next in nexts)
                    {
                        activeStoryQuests.Add(next.UniqueId);
                    }
                }

                activeStoryQuests.Remove(pair.Key);
            }
            else
            {
                copyDict.Add(pair.Key, pair.Value);
            }
        }

        dictQuestsStoryProgress = copyDict;

        // update progress after list update
        foreach (var active in activeStoryQuests)
        {
            QuestDataProgress copyProgress = dictQuestsStoryProgress[active];
            copyProgress.isActive = true;
            dictQuestsStoryProgress[active] = copyProgress;
        }

        SaveQuestsData();
    }

    #endregion

    #region BOUNTY

    public void SetBountyQuestCleared(string id)
    {
        QuestDataProgress progressData = dictQuestsBountyProgress[id];
        progressData.isCleared = true;
        dictQuestsBountyProgress[id] = progressData;
    }

    public void UpdateBountyQuests()
    {
        // copy of dictionary active bounties
        Dictionary<int, string> copyDict = new Dictionary<int, string>();

        // cycle active bounties
        foreach (var pair in activeBountyQuests)
        {
            // check progress
            QuestDataProgress progress = dictQuestsBountyProgress[pair.Value];
            if (progress.isCleared)
            {
                // remove progerss from dictionary
                dictQuestsBountyProgress.Remove(pair.Value);
            }
            else
            {
                copyDict.Add(pair.Key, pair.Value);
            }
        }

        activeBountyQuests = copyDict;

        SaveQuestsData();
    }

    #endregion

    #region DAILY

    public void ClearDailyQuests()
    {
        activeDailyQuests.Clear();
        dictQuestsDailyProgress.Clear();
    }

    #endregion


    #region EVENT ACTIONS

    private void OnEnemyKilled(EnemySO enemySO)
    {
        bool needSave = false;

        // Story quest checks
        foreach (var quest in activeStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);

            if(NeedUpdateKillProgress(so.QuestData, enemySO))
            {
                UpdateKillProgress(QuestType.Story, quest);
                needSave = true;
            }
        }

        // Bounties quest checks
        foreach (var quest in activeBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);

            if (NeedUpdateKillProgress(so.QuestData, enemySO))
            {
                UpdateKillProgress(QuestType.Bounties, quest);
                needSave = true;
            }
        }

        // Daily quest checks
        foreach (var quest in activeDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);

            if (NeedUpdateKillProgress(so.QuestData, enemySO))
            {
                UpdateKillProgress(QuestType.Daily, quest);
                needSave = true;
            }
        }

        if (needSave)
        {
            SaveQuestsData();
        }
    }

    private bool NeedUpdateKillProgress(QuestData data, EnemySO enemySO)
    {
        if (data.questObjectiveType == QuestObjectiveType.Kill)
        {
            // check specific
            if (data.questKillSpecific)
            {
                // Check actual pooling name, since the prefabs are identical
                // If the check is on the id, different monsters data would be compared instead of monster type
                string enemyName = UtilsEnemy.GetEnemySOById(data.monsterId).EnemyPoolName;
                if (enemyName == enemySO.EnemyPoolName)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateKillProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = dictQuestsStoryProgress[questId];

                progress.progressCounter++;
                dictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = dictQuestsBountyProgress[questId];

                progress.progressCounter++;
                dictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = dictQuestsDailyProgress[questId];

                progress.progressCounter++;
                dictQuestsDailyProgress[questId] = progress;
                break;
        }
    }



    private void OnItemObtain(int id)
    {
        bool needSave = false;

        // Story quest checks
        foreach (var quest in activeStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);

            if (NeedUpdateObtainProgress(so.QuestData, id))
            {
                UpdateObtainProgress(QuestType.Story, quest);
                needSave = true;
            }
        }

        // Bounties quest checks
        foreach (var quest in activeBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);

            if (NeedUpdateObtainProgress(so.QuestData, id))
            {
                UpdateObtainProgress(QuestType.Bounties, quest);
                needSave = true;
            }
        }

        // Daily quest checks
        foreach (var quest in activeDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);

            if (NeedUpdateObtainProgress(so.QuestData, id))
            {
                UpdateObtainProgress(QuestType.Daily, quest);
                needSave = true;
            }
        }

        if (needSave)
        {
            SaveQuestsData();
        }
    }

    private bool NeedUpdateObtainProgress(QuestData data, int itemId)
    {
        if (data.questObjectiveType == QuestObjectiveType.Obtain)
        {
            // check specific
            if (data.questObtainSpecific)
            {
                if (data.itemId == itemId)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateObtainProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = dictQuestsStoryProgress[questId];

                progress.progressCounter++;
                dictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = dictQuestsBountyProgress[questId];

                progress.progressCounter++;
                dictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = dictQuestsDailyProgress[questId];

                progress.progressCounter++;
                dictQuestsDailyProgress[questId] = progress;
                break;
        }
    }


    private void OnStatUp(int id, int amount)
    {
        bool needSave = false;

        // Story quest checks
        foreach (var quest in activeStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);

            if (NeedUpdateStatUpProgress(so.QuestData, id))
            {
                UpdateStatLevelUpProgress(QuestType.Story, quest, amount);
                needSave = true;
            }
        }

        // Bounties quest checks
        foreach (var quest in activeBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);

            if (NeedUpdateStatUpProgress(so.QuestData, id))
            {
                UpdateStatLevelUpProgress(QuestType.Bounties, quest, amount);
                needSave = true;
            }
        }

        // Daily quest checks
        foreach (var quest in activeDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);

            if (NeedUpdateStatUpProgress(so.QuestData, id))
            {
                UpdateStatLevelUpProgress(QuestType.Daily, quest, amount);
                needSave = true;
            }
        }

        if (needSave)
        {
            SaveQuestsData();
        }
    }

    private bool NeedUpdateStatUpProgress(QuestData data, int statId)
    {
        if (data.questObjectiveType == QuestObjectiveType.LevelUp)
        {
            // check specific
            if (data.questLevelUpSpecific)
            {
                if (data.statId == statId)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateStatLevelUpProgress(QuestType questType, string questId, int amount)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = dictQuestsStoryProgress[questId];

                progress.progressCounter += amount;
                dictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = dictQuestsBountyProgress[questId];

                progress.progressCounter += amount;
                dictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = dictQuestsDailyProgress[questId];

                progress.progressCounter += amount;
                dictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion



    public void SaveQuestsData()
    {
        QuestsSaveData data = new QuestsSaveData(this);
        saveService.SaveData(UtilsSave.GetQuestFile(), data, false);
    }
}
