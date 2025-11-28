using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightData
{
    /*
     * make leveling stats possible
     * for each stat max cap is current level * 3
     * use gain per level const to calculate the new stat
     * */

    
    private const float PER_LEVEL_GAIN_MAXHP = 20;
    private const float PER_LEVEL_GAIN_ATK = 2;
    private const float PER_LEVEL_GAIN_DEF = 1;
    private const float PER_LEVEL_GAIN_ATK_SPEED = 0.01f;
    private const float PER_LEVEL_GAIN_CRIT_RATE = 0.001f;
    private const float PER_LEVEL_GAIN_CRIT_DMG = 0.02f;
    private const float PER_LEVEL_GAIN_LUCK = 0.1f;


    // ---- BASE STAT VALUES

    private float baseMaxHp;

    private float baseAtk;
    private float baseDef;

    private float baseAtkSpd;

    private float baseCritRate;
    private float baseCritDmg;

    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatMaxHp = 1;
            
    private int levelStatAtk = 1;
    private int levelStatDef = 1;
             
    private int levelStatAtkSpd = 1;
             
    private int levelStatCritRate = 1;
    private int levelStatCritDmg = 1;
             
    private int levelStatLuck = 1;



    public int LevelStatMaxHp => levelStatMaxHp;

    public int LevelStatAtk => levelStatAtk;
    public int LevelStatDef => levelStatDef;

    public int LevelStatAtkSpd => levelStatAtkSpd;

    public int LevelStatCritRate => levelStatCritRate;
    public int LevelStatCritDmg => levelStatCritDmg;

    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private int currentExp;

    private float currentHp;





    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => UtilsPlayer.RequiredExpForLevel(currentLevel + 1) - UtilsPlayer.RequiredExpForLevel(currentLevel);
    public int TotalExpToNextLevel => UtilsPlayer.RequiredExpForLevel(currentLevel + 1);
    public int TotalExp => UtilsPlayer.RequiredExpForLevel(currentLevel) + currentExp;


    public float MaxHp => baseMaxHp + PER_LEVEL_GAIN_MAXHP * (levelStatMaxHp - 1);
    public float CurrentHp => currentHp;

    public float CurrentAtk => baseAtk + PER_LEVEL_GAIN_ATK * (levelStatAtk - 1);
    public float CurrentDef => baseDef + PER_LEVEL_GAIN_DEF * (levelStatDef - 1);

    // todo: if more mehods will be available to increase atk spd and crit rate, then check if you want those stats to be past the max threshold
    public float CurrentAtkSpd => baseAtkSpd + PER_LEVEL_GAIN_ATK_SPEED * (levelStatAtkSpd - 1);
    public float CurrentCritRate => baseCritRate + PER_LEVEL_GAIN_CRIT_RATE * (levelStatCritRate - 1);
    public float CurrentCritDmg => baseCritDmg + PER_LEVEL_GAIN_CRIT_DMG * (levelStatCritDmg - 1);

    // affects drops, crit rolls, rarity
    public float CurrentLuck => baseLuck + PER_LEVEL_GAIN_LUCK * (levelStatLuck - 1);



    public event Action OnAddedExp;
    public event Action OnLevelUp;

    public event Action OnHpChange;




    public PlayerFightData()
    {
        GenerateBaseStats();
    }

    public PlayerFightData(PlayerFightSaveData saveData)
    {
        GenerateBaseStats();

        levelStatMaxHp = saveData.levelStatMaxHp;

        levelStatAtk = saveData.levelStatAtk;
        levelStatDef = saveData.levelStatDef;

        levelStatAtkSpd = saveData.levelStatAtkSpd;

        levelStatCritRate = saveData.levelStatCritRate;
        levelStatCritDmg = saveData.levelStatCritDmg;

        levelStatLuck = saveData.levelStatLuck;


        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        currentHp = MaxHp;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        baseMaxHp = 120;
        currentHp = MaxHp;

        baseAtk = 10;
        baseDef = 2.5f;

        baseAtkSpd = 1.2f;   // 1 attack per second

        baseCritRate = 0.05f;  // 5%
        baseCritDmg = 1.5f;  // +50%

        baseLuck = 1.0f;
    }

    #region STATS

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
        while(TotalExp >= TotalExpToNextLevel)
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
            case UtilsPlayer.ID_MAXHP: levelStatMaxHp += amount; break;
            case UtilsPlayer.ID_ATK: levelStatAtk += amount; break;
            case UtilsPlayer.ID_DEF: levelStatDef += amount; break;
            case UtilsPlayer.ID_ATKSPD: levelStatAtkSpd += amount; break;
            case UtilsPlayer.ID_CRITRATE: levelStatCritRate += amount; break;
            case UtilsPlayer.ID_CRITDMG: levelStatCritDmg += amount; break;
            case UtilsPlayer.ID_LUCK: levelStatLuck += amount; break;
        }
    }

    #endregion

    #region FIGHT

    public void TakeDamage(EnemyData data)
    {
        // can't take less than 0 or it will cure
        float baseDamage = data.CurrentAtk;
        float total;

        if (UnityEngine.Random.value <= data.CurrentCritRate)
        {
            baseDamage *= data.CurrentCritDmg;
        }

        total = Mathf.Max(0f, baseDamage - CurrentDef);

        // subtract total to hp
        currentHp -= total;

        if (currentHp <= 0f)
        {
            currentHp = 0;
        }

        OnHpChange?.Invoke();
    }

    public void ResetAfterStage()
    {
        currentHp = MaxHp;

        OnHpChange?.Invoke();
    }

    #endregion
}
