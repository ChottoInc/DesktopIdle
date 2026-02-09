
public class QuestDailySaveData
{
    public string questId;

    public bool isActive;

    public int progressCounter;

    public bool isCleared;

    public QuestDailySaveData() { }

    public QuestDailySaveData(string questId, UtilsQuest.QuestDataProgress progress)
    {
        this.questId = questId;

        isActive = progress.isActive;

        progressCounter = progress.progressCounter;

        isCleared = progress.isCleared;
    }
}
