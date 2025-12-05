using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerFightData
{
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
    public int ExpToNextLevel => RequiredExpForWarriorLevel(currentLevel + 1) - RequiredExpForWarriorLevel(currentLevel);
    public int TotalExpToNextLevel => RequiredExpForWarriorLevel(currentLevel + 1);
    public int TotalExp => RequiredExpForWarriorLevel(currentLevel) + currentExp;


    public float MaxHp => baseMaxHp + PER_LEVEL_WARRIOR_GAIN_MAXHP * (levelStatMaxHp - 1);
    public float CurrentHp => currentHp;

    /*
     * using weapon miner level 
     */
    public float CurrentAtk => 
        (baseAtk + PER_LEVEL_WARRIOR_GAIN_ATK * (levelStatAtk - 1)) *
        PlayerManager.Instance.WeaponMinerMultiplier;

    public float CurrentDef => baseDef + PER_LEVEL_WARRIOR_GAIN_DEF * (levelStatDef - 1);

    // todo: if more mehods will be available to increase atk spd and crit rate, then check if you want those stats to be past the max threshold
    public float CurrentAtkSpd => baseAtkSpd + PER_LEVEL_WARRIOR_GAIN_ATK_SPEED * (levelStatAtkSpd - 1);
    public float CurrentCritRate => baseCritRate + PER_LEVEL_WARRIOR_GAIN_CRIT_RATE * (levelStatCritRate - 1);
    public float CurrentCritDmg => baseCritDmg + PER_LEVEL_WARRIOR_GAIN_CRIT_DMG * (levelStatCritDmg - 1);

    // affects drops, crit rolls, rarity
    public float CurrentLuck => baseLuck + PER_LEVEL_WARRIOR_GAIN_LUCK * (levelStatLuck - 1);



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
            case ID_WARRIOR_MAXHP: levelStatMaxHp += amount; break;
            case ID_WARRIOR_ATK: levelStatAtk += amount; break;
            case ID_WARRIOR_DEF: levelStatDef += amount; break;
            case ID_WARRIOR_ATKSPD: levelStatAtkSpd += amount; break;
            case ID_WARRIOR_CRITRATE: levelStatCritRate += amount; break;
            case ID_WARRIOR_CRITDMG: levelStatCritDmg += amount; break;
            case ID_WARRIOR_LUCK: levelStatLuck += amount; break;
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
