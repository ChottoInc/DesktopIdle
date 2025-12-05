using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerMinerData
{
    // ---- BASE STAT VALUES

    private float basePower;
    private float baseSmashSpeed;
    private float basePrecision;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatPower = 1;
    private int levelSmashSpeed = 1;
    private int levelPrecision = 1;
    private int levelStatLuck = 1;

    // ---- WEAPON MINER

    private int levelWeaponMiner;



    public int LevelStatPower => levelStatPower;
    public int LevelStatSmashSpeed => levelSmashSpeed;
    public int LevelStatPrecision => levelPrecision;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private int currentExp;





    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => RequiredExpForMinerLevel(currentLevel + 1) - RequiredExpForMinerLevel(currentLevel);
    public int TotalExpToNextLevel => RequiredExpForMinerLevel(currentLevel + 1);
    public int TotalExp => RequiredExpForMinerLevel(currentLevel) + currentExp;


    public float CurrentPower => basePower + PER_LEVEL_MINER_GAIN_POWER * (levelStatPower - 1);
    public float CurrentSmashSpeed => baseSmashSpeed + PER_LEVEL_MINER_GAIN_SMASHSPEED * (levelSmashSpeed - 1);
    public float CurrentPrecision => basePrecision + PER_LEVEL_MINER_GAIN_PRECISION * (levelPrecision - 1);
    public float CurrentLuck => baseLuck + PER_LEVEL_MINER_GAIN_LUCK * (levelStatLuck - 1);

    public int WeaponLevel => levelWeaponMiner;



    public event Action OnAddedExp;
    public event Action OnLevelUp;




    public PlayerMinerData()
    {
        GenerateBaseStats();
    }
    
    public PlayerMinerData(PlayerMinerSaveData saveData)
    {
        GenerateBaseStats();

        levelStatPower = saveData.levelStatPower;
        levelSmashSpeed = saveData.levelStatSmashSpeed;
        levelPrecision = saveData.levelStatPrecision;
        levelStatLuck = saveData.levelStatLuck;


        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        // ---- WEAPON

        levelWeaponMiner = saveData.levelWeaponMiner;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        basePower = 10;

        baseSmashSpeed = 1f;
        basePrecision = 0.7f;

        baseLuck = 0f;

        // ---- WEAPON

        levelWeaponMiner = 1;
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
            case ID_MINER_POWER: levelStatPower += amount; break;
            case ID_MINER_SMASHSPEED: levelSmashSpeed += amount; break;
            case ID_MINER_PRECISION: levelPrecision += amount; break;
            case ID_MINER_LUCK: levelStatLuck += amount; break;
        }
    }
}
