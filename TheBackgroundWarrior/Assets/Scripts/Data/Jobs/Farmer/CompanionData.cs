
public class CompanionData
{
    public CompanionSO CompanionSO { get; private set; }

    public int CurrentLevel { get; private set; }

    public int CurrentExp { get; private set; }
    public int ExpToNextLevel => UtilsFarmer.RequiredExpForCompanionLevel(CurrentLevel + 1);

    public int CurrentSlot { get; private set; }



    public float CurrentAtkPerc => CompanionSO.BaseAtkPerc;
    public float CurrentAtkSpd => CompanionSO.BaseAtkSpd;



    public CompanionData(CompanionSO companionSO)
    {
        CompanionSO = companionSO;

        CurrentExp = 0;
        CurrentLevel = 1;

        CurrentSlot = -1;
    }

    public CompanionData(CompanionSaveData saveData)
    {
        CompanionSO = UtilsFarmer.GetCompanionById(saveData.companionId);

        CurrentExp = saveData.currentExp;
        CurrentLevel = saveData.currentLevel;

        CurrentSlot = saveData.currentSlot;
    }

    public void SetSlot(int id)
    {
        CurrentSlot = id;
    }


    public void AddExp(int amount)
    {
        // check max level
        if (CurrentLevel >= UtilsFarmer.MAX_LEVEL_COMPANIONS)
        {
            // set current exp to 0
            CurrentExp = 0;
            return;
        }

        CurrentExp += amount;
        //UnityEngine.Debug.Log("current exp: " + CurrentExp);

        // looping for every level gained
        while (CurrentExp >= ExpToNextLevel)
        {
            // recalculate current exp
            CurrentExp -= ExpToNextLevel;

            // give level
            CurrentLevel++;
        }
    }
}
