using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerFisherData : IBasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseCalmness;
    private float baseReflex;
    private float baseKnowledge;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int startLevelCalmness = 1;
    private int startLevelReflex = 1;
    private int startLevelKnowledge = 1;
    private int startLevelLuck = 1;


    public int LevelStatCalmness { get; private set; }
    public int LevelStatReflex { get; private set; }
    public int LevelStatKnowledge { get; private set; }
    public int LevelStatLuck { get; private set; }


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private long currentExp;



    public int CurrentLevel => currentLevel;
    public long CurrentExp => currentExp;
    public long ExpToNextLevel => UtilsFisher.RequiredExpForFisherLevel(currentLevel + 1);

    public float CurrentCalmness => baseCalmness + UtilsFisher.PER_LEVEL_FISHER_GAIN_CALMNESS * (LevelStatCalmness);
    public float CurrentReflex => baseReflex + UtilsFisher.PER_LEVEL_FISHER_GAIN_REFLEX * (LevelStatReflex - 1);
    public float CurrentKnowledge => baseKnowledge + UtilsFisher.PER_LEVEL_FISHER_GAIN_KNOWLEDGE * (LevelStatKnowledge - 1);
    public float CurrentLuck => baseLuck + UtilsFisher.PER_LEVEL_FISHER_GAIN_LUCK * (LevelStatLuck - 1);


    // ---- FISH GROUPS CHECKS COMPLETION

    public bool IsLifeSeriesCompleted { get; private set; }
    public bool IsPredatorSeriesCompleted { get; private set; }
    public bool IsGuardianSeriesCompleted { get; private set; }
    public bool IsDartSeriesCompleted { get; private set; }
    public bool IsSharpSeriesCompleted { get; private set; }
    public bool IsPiercingSeriesCompleted { get; private set; }
    public bool IsGoldenSeriesCompleted { get; private set; }
    public bool IsElderSeriesCompleted { get; private set; }
    public bool IsQuickSeriesCompleted { get; private set; }


    // ---- BAITS VARS

    public bool IsBaitActive { get; private set; }
    public BaitSO ActiveBait { get; private set; }
    public float RemainingTimeBait { get; private set; }



    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public PlayerFisherData()
    {
        GenerateBaseStats();

        FillFishGroupsSeriesCompletion();
    }

    public PlayerFisherData(PlayerFisherSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatCalmness = saveData.levelStatCalmness;
        LevelStatReflex = saveData.levelReflex;
        LevelStatKnowledge = saveData.levelKnowledge;
        LevelStatLuck = saveData.levelStatLuck;


        LevelStatCalmness = Math.Min(LevelStatCalmness, UtilsFisher.PER_LEVEL_FISHER_MAX_CALMNESS);
        LevelStatReflex = Math.Min(LevelStatReflex, UtilsFisher.PER_LEVEL_FISHER_MAX_REFLEX);
        LevelStatKnowledge = Math.Min(LevelStatKnowledge, UtilsFisher.PER_LEVEL_FISHER_MAX_KNOWLEDGE);
        LevelStatLuck = Math.Min(LevelStatLuck, UtilsFisher.PER_LEVEL_FISHER_MAX_LUCK);


        availableStatPoints = saveData.availableStatPoints;
        
        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        int sumLevels =
            LevelStatCalmness + LevelStatReflex + LevelStatKnowledge + LevelStatLuck +
            //startLevelCalmness + startLevelReflex + startLevelKnowledge + startLevelLuck +
            availableStatPoints +
            1;

        currentLevel = Math.Min(currentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (currentLevel >= UtilsFisher.MAX_LEVEL_FISHER)
        {
            availableStatPoints = UtilsFisher.MAX_LEVEL_FISHER - 1 -
               LevelStatCalmness - LevelStatReflex - LevelStatKnowledge - LevelStatLuck;
            currentExp = 0;
        }

        FillFishGroupsSeriesCompletion();

        IsBaitActive = saveData.isBaitActive;
        ActiveBait = UtilsItem.GetItemById(saveData.activeBaitId) as BaitSO;
        RemainingTimeBait = saveData.remainingTimeBait;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        LevelStatCalmness = startLevelCalmness;
        LevelStatReflex = startLevelReflex;
        LevelStatKnowledge = startLevelKnowledge;
        LevelStatLuck = startLevelLuck;

        // multiplier
        baseCalmness = 0f; // reduced max time for spawn fish, up to 0.5f - 50%

        baseReflex = 0.5f; // stat contrlling if the fish is caught, up to 0.75 - 25%
        baseKnowledge = 0f; // reduce chances of same species, up to 0.3 - 30%

        baseLuck = 0f; // controls rarity of fish, up to 0.4 - 40%

        IsBaitActive = false;
        ActiveBait = null;
        RemainingTimeBait = 0f;
    }

    public void FillFishGroupsSeriesCompletion()
    {
        FishGroupSO currentGroup = null;

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Life);
        IsLifeSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Predator);
        IsPredatorSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Guardian);
        IsGuardianSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Dart);
        IsDartSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Sharp);
        IsSharpSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Piercing);
        IsPiercingSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Golden);
        IsGoldenSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Elder);
        IsElderSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Quick);
        IsQuickSeriesCompleted = IsGroupCaught(currentGroup);
    }

    private bool IsGroupCaught(FishGroupSO group)
    {
        if (group == null) return false;

        bool result = true;

        foreach (var fish in group.Fishes)
        {
            // check for not caught fish
            if (!PlayerManager.Instance.Inventory.HasItem(fish.Id))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    public void AddStatPoints(int amount)
    {
        availableStatPoints += amount;
    }

    public void RemoveStatPoints(int amount)
    {
        availableStatPoints -= amount;
    }

    public void AddLevel(int amount)
    {
        if (currentLevel + amount > UtilsFisher.MAX_LEVEL_FISHER)
        {
            amount = UtilsFisher.MAX_LEVEL_FISHER - currentLevel;
        }
        currentLevel += amount;
        availableStatPoints += amount;
    }

    public void AddExp(long amount)
    {
        // check max level
        if (currentLevel >= UtilsFisher.MAX_LEVEL_FISHER)
        {
            // set current exp to 0
            currentExp = 0;
            return;
        }

        currentExp += amount;

        // looping for every level gained
        while (currentExp >= ExpToNextLevel)
        {
            // recalculate current exp
            currentExp -= ExpToNextLevel;

            // give level and stat point
            currentLevel++;
            AddStatPoints(1);

            OnLevelUp?.Invoke();
        }

        OnAddedExp?.Invoke();
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case ID_FISHER_CALMNESS: LevelStatCalmness += amount; break;
            case ID_FISHER_REFLEX: LevelStatReflex += amount; break;
            case ID_FISHER_KNOWLEDGE: LevelStatKnowledge += amount; break;
            case ID_FISHER_LUCK: LevelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }
}
