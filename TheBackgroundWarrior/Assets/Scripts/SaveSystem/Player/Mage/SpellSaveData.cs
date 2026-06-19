

public class SpellSaveData
{
    public int id;
    public bool isUnlocked;
    public int currentRank;
    public int currentLearnPoints;

    public SpellSaveData() { }   

    public SpellSaveData(SpellData data)
    {
        id = data.SpellSO.Id;
        isUnlocked = data.IsUnlocked;
        currentRank = data.CurrentRank;
        currentLearnPoints = data.CurrentLearnPoints;
    }
}
