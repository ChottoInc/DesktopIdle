
public class QuestStorySaveData
{
    public string questId;

    public bool isActive;

    public int progressCounter;

    public bool isCleared;

    public QuestStorySaveData() { }

    public QuestStorySaveData(string questId, UtilsQuest.QuestDataProgress progress)
    {
        this.questId = questId;

        isActive = progress.isActive;

        progressCounter = progress.progressCounter;

        isCleared = progress.isCleared;
    }
}
