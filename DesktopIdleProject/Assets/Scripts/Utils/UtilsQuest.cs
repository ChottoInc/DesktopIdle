using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsQuest
{
    public enum QuestType { Story, Daily, Bounties }

    public enum QuestObjectiveType { Kill, Obtain, LevelUp }


    private static QuestStorySO[] storySOs;
    private static QuestBountySO[] bountySOs;
    private static QuestDailySO[] dailySOs;


    public static void Initialize()
    {
        storySOs = LoadStoryQuests();
        bountySOs = LoadBountyQuests();
        dailySOs = LoadDailyQuests();
    }

    #region STORY

    private static QuestStorySO[] LoadStoryQuests()
    {
        return Resources.LoadAll<QuestStorySO>("Data/Quests/Story");
    }


    public static QuestStorySO[] GetAllStoryQuests()
    {
        return storySOs;
    }

    public static QuestStorySO GetStoryQuestById(string id)
    {
        foreach (var quest in storySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    #endregion

    #region BOUNTY

    private static QuestBountySO[] LoadBountyQuests()
    {
        return Resources.LoadAll<QuestBountySO>("Data/Quests/Bounty");
    }


    public static QuestBountySO[] GetAllBountyQuests()
    {
        return bountySOs;
    }

    public static QuestBountySO GetBountyQuestById(string id)
    {
        foreach (var quest in bountySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    #endregion

    #region DAILY

    private static QuestDailySO[] LoadDailyQuests()
    {
        return Resources.LoadAll<QuestDailySO>("Data/Quests/Daily");
    }


    public static QuestDailySO[] GetAllDailyQuests()
    {
        return dailySOs;
    }

    public static QuestDailySO GetDailyQuestById(string id)
    {
        foreach (var quest in dailySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    public static QuestDailySO GetRandomDailyQuest()
    {
        int rand = Random.Range(0, dailySOs.Length);
        return dailySOs[rand];
    }


    #endregion



    public static string GetQuestProgress(QuestData data, QuestDataProgress progress)
    {
        string result = GetQuestDescription(data);

        switch (data.questObjectiveType)
        {
            case QuestObjectiveType.Kill:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountKill);
                break;

            case QuestObjectiveType.Obtain:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountObtain);
                break;

            case QuestObjectiveType.LevelUp:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountStat);
                break;
        }

        return result;
    }

    public static string GetQuestDescription(QuestData data)
    {
        string result = "";

        switch (data.questObjectiveType)
        {
            case QuestObjectiveType.Kill:
                if (data.questKillSpecific)
                {
                    EnemySO enemySO = UtilsEnemy.GetEnemySOById(data.monsterId);
                    result = string.Format("Kill {0} {1}", data.amountKill, enemySO.EnemyPoolName);
                }
                else
                {
                    result = string.Format("Kill {0} monsters", data.amountKill);
                }
                break;

            case QuestObjectiveType.Obtain:
                if (data.questObtainSpecific)
                {
                    ItemSO itemSO = UtilsItem.GetItemById(data.itemId);
                    result = string.Format("Obtain {0} {1}", data.amountObtain, itemSO.ItemName);
                }
                else
                {
                    string itemType = string.Empty;

                    switch (data.itemType)
                    {
                        case UtilsItem.ItemType.Ore: itemType = "Ores"; break;
                        case UtilsItem.ItemType.Card: itemType = "Cards"; break;
                    }

                    result = string.Format("Obtain {0} {1}", data.amountObtain, itemType);
                }
                break;

            case QuestObjectiveType.LevelUp:
                string timesString = "times";

                if (data.questLevelUpSpecific)
                {
                    string statName = UtilsPlayer.GetStatNameById(data.statId);

                    if (data.amountStat < 2)
                        timesString = "time";

                    result = string.Format("Level up {0} {1} {2}", statName, data.amountStat, timesString);
                }
                else
                {
                    if (data.amountStat < 2)
                        timesString = "time";

                    result = string.Format("Level up any stat {0} {1}", data.amountStat, timesString);
                }
                break;
        }


        return result;
    }



    #region DATA

    [System.Serializable]
    public struct QuestData
    {
        public QuestObjectiveType questObjectiveType;

        // --------- Quest Kill ---------
        public bool questKillSpecific;

        // --- Specific
        public int monsterId;

        public int amountKill;

        // --------- Quest Obtain ---------
        public UtilsItem.ItemType itemType;
        public bool questObtainSpecific;

        // --- Specific
        public int itemId;

        public int amountObtain;

        // --------- Quest Level Up ---------
        public bool questLevelUpSpecific;

        // --- Specific
        public int statId;

        public int amountStat;


        // --------- Reward ---------
        public int rewardAmount;
    }

    [System.Serializable]
    public struct QuestDataProgress
    {
        public QuestDataProgress(QuestStorySaveData saveData)
        {
            isActive = saveData.isActive;

            progressCounter = saveData.progressCounter;

            isCleared = saveData.isCleared;
        }

        public QuestDataProgress(QuestBountySaveData saveData)
        {
            isActive = true;

            progressCounter = saveData.progressCounter;

            isCleared = saveData.isCleared;
        }

        public QuestDataProgress(QuestDailySaveData saveData)
        {
            isActive = saveData.isActive;

            progressCounter = saveData.progressCounter;

            isCleared = saveData.isCleared;
        }

        public bool isActive;

        public int progressCounter;

        public bool isCleared;
    }

    #endregion
}
