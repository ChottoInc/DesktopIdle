using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightData
{
    // ---- VISITABLE MAPS

    private List<int> availableMaps;


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


    public List<int> AvailableMaps => availableMaps;



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
    public int ExpToNextLevel => UtilsWarrior.RequiredExpForWarriorLevel(currentLevel + 1);


    public float MaxHp => 
        (baseMaxHp + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_MAXHP * (levelStatMaxHp - 1)) *
        PlayerManager.Instance.HelmetMaxHpBlacksmithMultiplier *
        PlayerManager.Instance.FisherLifeSeriesMultiplier;

    public float CurrentHp => currentHp;

    /*
     * using weapon miner level 
     */
    public float CurrentAtk => 
        (baseAtk + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_ATK * (levelStatAtk - 1)) *
        PlayerManager.Instance.WeaponMinerMultiplier *
        PlayerManager.Instance.FisherPredatorSeriesMultiplier;

    public float CurrentDef => 
        (baseDef + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_DEF * (levelStatDef - 1)) *
        PlayerManager.Instance.ArmorDefBlacksmithMultiplier *
        PlayerManager.Instance.BootsDefBlacksmithMultiplier *
        PlayerManager.Instance.FisherGuardianSeriesMultiplier;

    // todo: if more mehods will be available to increase atk spd and crit rate, then check if you want those stats to be past the max threshold
    public float CurrentAtkSpd => 
        (baseAtkSpd + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_ATK_SPEED * (levelStatAtkSpd - 1)) *
        PlayerManager.Instance.GlovesAtkSpdBlacksmithMultiplier *
        PlayerManager.Instance.FisherDartSeriesMultiplier;

    public float CurrentCritRate => 
        (baseCritRate + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_CRIT_RATE * (levelStatCritRate - 1)) *
        PlayerManager.Instance.BootsCritRateBlacksmithMultiplier *
        PlayerManager.Instance.FisherSharpSeriesMultiplier;

    public float CurrentCritDmg => 
        (baseCritDmg + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_CRIT_DMG * (levelStatCritDmg - 1)) *
        PlayerManager.Instance.GlovesCritDmgBlacksmithMultiplier *
        PlayerManager.Instance.FisherPiercingSeriesMultiplier;

    // affects card drop rates, and gives a one more chance to crit rate check
    public float CurrentLuck => 
        (baseLuck + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_LUCK * (levelStatLuck - 1)) *
        PlayerManager.Instance.FisherGoldenSeriesMultiplier;



    public event Action OnAddedExp;
    public event Action OnLevelUp;

    public event Action OnHpChange;


    public event Action<int, int> OnStatChange;
    public event Action<int> OnAddMap;




    public PlayerFightData()
    {
        GenerateBaseStats();
    }

    public PlayerFightData(PlayerFightSaveData saveData)
    {
        GenerateBaseStats();

        availableMaps = new List<int>();
        availableMaps.AddRange(saveData.availableMaps);


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
        availableMaps = new List<int>
        {
            0 // Woods map by default
        };


        currentLevel = 1;
        currentExp = 0;

        baseMaxHp = 120;
        currentHp = MaxHp;

        baseAtk = 10;
        baseDef = 2.5f;

        baseAtkSpd = 1.2f;   // 1 attack per second
        //baseAtkSpd = 5f;   // 1 attack per second

        baseCritRate = 0.05f;  // 5%
        baseCritDmg = 1.5f;  // +50%

        baseLuck = 0f;
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
        // check max level
        if (currentLevel > UtilsWarrior.MAX_LEVEL_WARRIOR)
        {
            // set current exp to 0
            currentExp = 0;
            return;
        }

        currentExp += amount;

        // looping for every level gained
        while(currentExp >= ExpToNextLevel)
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
            case UtilsPlayer.ID_WARRIOR_MAXHP: levelStatMaxHp += amount; break;
            case UtilsPlayer.ID_WARRIOR_ATK: levelStatAtk += amount; break;
            case UtilsPlayer.ID_WARRIOR_DEF: levelStatDef += amount; break;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: levelStatAtkSpd += amount; break;
            case UtilsPlayer.ID_WARRIOR_CRITRATE: levelStatCritRate += amount; break;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: levelStatCritDmg += amount; break;
            case UtilsPlayer.ID_WARRIOR_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
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

    /// <summary>
    /// Used only for testing
    /// </summary>
    public void TakeDamageCheat(float damage)
    {
        float total = Mathf.Max(0f, damage);

        // subtract total to hp
        currentHp -= total;

        OnHpChange?.Invoke();

        if (currentHp <= 0f)
        {
            currentHp = 0;
        }
    }

    public void ResetAfterStage()
    {
        currentHp = MaxHp;

        //Debug.Log("Max hp after death: " + currentHp);

        OnHpChange?.Invoke();
    }

    #endregion


    public void AddAvailableMap(int id)
    {
        availableMaps.Add(id);
        OnAddMap?.Invoke(id);
    }
}
