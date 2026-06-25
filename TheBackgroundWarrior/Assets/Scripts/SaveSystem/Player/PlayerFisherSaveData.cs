public class PlayerFisherSaveData
{
    public int levelStatCalmness;
    public int levelReflex;
    public int levelKnowledge;
    public int levelStatLuck;
    
    public int availableStatPoints;
    
    public int currentLevel;
    public long currentExp;

    public bool isBaitActive;
    public int activeBaitId;
    public float remainingTimeBait;

    public PlayerFisherSaveData() { }

    public PlayerFisherSaveData(PlayerFisherData data)
    {
        levelStatCalmness = data.LevelStatCalmness;
        levelReflex = data.LevelStatReflex;
        levelKnowledge = data.LevelStatKnowledge;
        levelStatLuck = data.LevelStatLuck;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        isBaitActive = data.IsBaitActive;
        activeBaitId = data.ActiveBait.Id;
        remainingTimeBait = data.RemainingTimeBait;
    }
}
