using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBountySaveData
{
    public int slotTab;

    public string questId;

    public int progressCounter;

    public bool isCleared;

    public QuestBountySaveData() { }

    public QuestBountySaveData(int slotTab, string questId, UtilsQuest.QuestDataProgress progress)
    {
        this.slotTab = slotTab;

        this.questId = questId;

        progressCounter = progress.progressCounter;

        isCleared = progress.isCleared;
    }
}
