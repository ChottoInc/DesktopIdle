using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsItem;

public static class UtilsQuest
{
    public enum QuestType { Kill, Obtain, LevelUp }


    private static QuestStorySO[] storySOs;


    public static void Initialize()
    {
        storySOs = LoadStoryQuests();
    }

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

    #region DATA

    [System.Serializable]
    public struct QuestData
    {
        public QuestType questType;

        // --------- Quest Kill ---------
        public bool questKillSpecific;

        // --- Specific
        public int monsterId;

        public int amountKill;

        // --------- Quest Obtain ---------
        public ItemType itemType;
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
        public int progressCounter;
    }

    #endregion
}
