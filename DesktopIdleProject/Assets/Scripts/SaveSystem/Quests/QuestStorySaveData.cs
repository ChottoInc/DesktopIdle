using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorySaveData
{
    public string questId;

    public int progressCounter;
    public bool isCleared;

    public QuestStorySaveData() { }

    public QuestStorySaveData(string questId, UtilsQuest.QuestDataProgress progress)
    {
        this.questId = questId;

        progressCounter = progress.progressCounter;
        isCleared = progress.isCleared;
    }
}
