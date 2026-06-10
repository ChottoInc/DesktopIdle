
public class CompanionSaveData
{
    public int companionId;

    public int currentExp;
    public int currentLevel;

    public int currentSlot;


    public CompanionSaveData() { }

    public CompanionSaveData(CompanionData data)
    {
        companionId = data.CompanionSO.Id;

        currentExp = data.CurrentExp;
        currentLevel = data.CurrentLevel;

        currentSlot = data.CurrentSlot;
    }
}
