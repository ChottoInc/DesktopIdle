using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerBlacksmithData
{
    // ---- BASE STAT VALUES

    private float baseCraftSpeed;
    private float baseEfficiency;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatCraftSpeed = 1;
    private int levelEfficiency = 1;
    private int levelStatLuck = 1;

    // ---- WEAPON MINER

    private int levelHelmetBlacksmith;

    private int levelArmorBlacksmith;

    private int levelGlovesBlacksmith;

    private int levelBootsBlacksmith;



    public int LevelStatCraftSpeed => levelStatCraftSpeed;
    public int LevelEfficiency => levelEfficiency;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private int currentExp;


    // ---- FORGING VARIABLES

    private int currentForgingOre;





    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => RequiredExpForBlacksmithLevel(currentLevel + 1) - RequiredExpForBlacksmithLevel(currentLevel);
    public int TotalExpToNextLevel => RequiredExpForBlacksmithLevel(currentLevel + 1);
    public int TotalExp => RequiredExpForBlacksmithLevel(currentLevel) + currentExp;


    public float CurrentCraftSpeed => baseCraftSpeed + PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED * (levelStatCraftSpeed - 1);
    public float CurrentEfficiency => baseEfficiency + PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY * (levelEfficiency - 1);
    public float CurrentLuck => baseLuck + PER_LEVEL_BLACKSMITH_GAIN_LUCK * (levelStatLuck - 1);

    public float CurrentCraftTime => 60f / CurrentCraftSpeed;



    public int HelmetLevel => levelHelmetBlacksmith;
    public int ArmorLevel => levelArmorBlacksmith;
    public int GlovesLevel => levelGlovesBlacksmith;
    public int BootsLevel => levelBootsBlacksmith;



    public int CurrentForgingOre => currentForgingOre;



    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public PlayerBlacksmithData()
    {
        GenerateBaseStats();
    }

    public PlayerBlacksmithData(PlayerBlacksmithSaveData saveData)
    {
        GenerateBaseStats();

        levelStatCraftSpeed = saveData.levelStatCraftSpeed;
        levelEfficiency = saveData.levelStatEfficiency;
        levelStatLuck = saveData.levelStatLuck;


        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        // ---- WEAPON

        levelHelmetBlacksmith = saveData.levelHelmetBlacksmith;
        levelArmorBlacksmith = saveData.levelArmorBlacksmith;
        levelGlovesBlacksmith = saveData.levelGlovesBlacksmith;
        levelBootsBlacksmith = saveData.levelBootsBlacksmith;


        currentForgingOre = saveData.currentForgingOre;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        baseCraftSpeed = 1f;

        baseEfficiency = 0f;

        baseLuck = 0f;

        // ---- EQUIPMENT

        levelHelmetBlacksmith = 1;
        levelArmorBlacksmith = 1;
        levelGlovesBlacksmith = 1;
        levelBootsBlacksmith = 1;

        // ---- FORGING

        currentForgingOre = -1;
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
            case ID_BLACKSMITH_CRAFTSPEED: levelStatCraftSpeed += amount; break;
            case ID_BLACKSMITH_EFFICIENCY: levelEfficiency += amount; break;
            case ID_BLACKSMITH_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }

    public void AddBlacksmithHelmetLevel(int level)
    {
        levelHelmetBlacksmith += level;
    }

    public void AddBlacksmithArmorLevel(int level)
    {
        levelArmorBlacksmith += level;
    }

    public void AddBlacksmithGlovesLevel(int level)
    {
        levelGlovesBlacksmith += level;
    }

    public void AddBlacksmithBootsLevel(int level)
    {
        levelBootsBlacksmith += level;
    }

    public void SetForgingOre(int id)
    {
        currentForgingOre = id;
    }
}
