using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsPlayer;

public class PlayerFisherData
{
    // ---- BASE STAT VALUES

    private float baseCalmness;
    private float baseReflex;
    private float baseKnowledge;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatCalmness = 1;
    private int levelReflex = 1;
    private int levelKnowledge = 1;
    private int levelStatLuck = 1;


    public int LevelStatCalmness => levelStatCalmness;
    public int LevelReflex => levelReflex;
    public int LevelKnowledge => levelKnowledge;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private int currentExp;


    // ---- FISH GROUPS CHECKS COMPLETION

    private bool isLifeSeriesCompleted;
    private bool isPredatorSeriesCompleted;
    private bool isGuardianSeriesCompleted;
    private bool isDartSeriesCompleted;
    private bool isSharpSeriesCompleted;
    private bool isPiercingSeriesCompleted;
    private bool isGoldenSeriesCompleted;
    private bool isElderSeriesCompleted;
    private bool isQuickSeriesCompleted;





    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => RequiredExpForFisherLevel(currentLevel + 1) - RequiredExpForFisherLevel(currentLevel);
    public int TotalExpToNextLevel => RequiredExpForFisherLevel(currentLevel + 1);
    public int TotalExp => RequiredExpForFisherLevel(currentLevel) + currentExp;


    public float CurrentCalmness => baseCalmness + PER_LEVEL_FISHER_GAIN_CALMNESS * (levelStatCalmness - 1);
    public float CurrentReflex => baseReflex + PER_LEVEL_FISHER_GAIN_REFLEX * (levelReflex - 1);
    public float CurrentKnowledge => baseKnowledge + PER_LEVEL_FISHER_GAIN_KNOWLEDGE * (levelKnowledge - 1);
    public float CurrentLuck => baseLuck + PER_LEVEL_FISHER_GAIN_LUCK * (levelStatLuck - 1);



    public bool IsLifeSeriesCompleted => isLifeSeriesCompleted;
    public bool IsPredatorSeriesCompleted => isPredatorSeriesCompleted;
    public bool IsGuardianSeriesCompleted => isGuardianSeriesCompleted;
    public bool IsDartSeriesCompleted => isDartSeriesCompleted;
    public bool IsSharpSeriesCompleted => isSharpSeriesCompleted;
    public bool IsPiercingSeriesCompleted => isPiercingSeriesCompleted;
    public bool IsGoldenSeriesCompleted => isGoldenSeriesCompleted;
    public bool IsElderSeriesCompleted => isElderSeriesCompleted;
    public bool IsQuickSeriesCompleted => isQuickSeriesCompleted;



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

        levelStatCalmness = saveData.levelStatCalmness;
        levelReflex = saveData.levelReflex;
        levelKnowledge = saveData.levelKnowledge;
        levelStatLuck = saveData.levelStatLuck;
        
        
        availableStatPoints = saveData.availableStatPoints;
        
        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        FillFishGroupsSeriesCompletion();
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        // multiplier
        baseCalmness = 1f; // reduced max time for spawn fish, up to 0.5f - 50%

        baseReflex = 0.5f; // stat contrlling if the fish is caught, up to 0.75 - 25%
        baseKnowledge = 0f; // reduce chances of same species, up to 0.3 - 30%

        baseLuck = 0f; // controls rarity of fish, up to 0.4 - 40%
    }

    private void FillFishGroupsSeriesCompletion()
    {
        FishGroupSO currentGroup = null;

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Life);
        isLifeSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Predator);
        isPredatorSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Guardian);
        isGuardianSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Dart);
        isDartSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Sharp);
        isSharpSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Piercing);
        isPiercingSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Golden);
        isGoldenSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Elder);
        isElderSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Quick);
        isQuickSeriesCompleted = IsGroupCaught(currentGroup);
    }

    private bool IsGroupCaught(FishGroupSO group)
    {
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

    public void AddExp(int amount)
    {
        currentExp += amount;

        // looping for every level gained
        while (TotalExp >= TotalExpToNextLevel)
        {
            // recalculate current exp
            currentExp = TotalExp - TotalExpToNextLevel;

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
            case ID_FISHER_CALMNESS: levelStatCalmness += amount; break;
            case ID_FISHER_REFLEX: levelReflex += amount; break;
            case ID_FISHER_KNOWLEDGE: levelKnowledge += amount; break;
            case ID_FISHER_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }
}
