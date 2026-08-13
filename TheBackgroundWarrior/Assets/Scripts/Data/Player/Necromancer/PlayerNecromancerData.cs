using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNecromancerData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseAptitude;
    private float baseSummon;
    private float baseMight;
    private float baseLifespan;
    private float baseHorde;
    private float baseLuck;

    // ---- LEVEL STAT POINTS


    private int startLevelAptitude = 0;
    private int startLevelSummon = 1;
    private int startLevelMight = 1;
    private int startLevelLifespan = 1;
    private int startLevelHorde = 0;
    private int startLevelLuck = 1;


    public int LevelStatAptitude { get; private set; }
    public int LevelStatSummon { get; private set; }
    public int LevelStatMight { get; private set; }
    public int LevelStatLifespan { get; private set; }
    public int LevelStatHorde { get; private set; }
    public int LevelStatLuck { get; private set; }



    // ---- FINAL STAT VALUES
    public long ExpToNextLevel => UtilsNecromancer.RequiredExpForNecromancerLevel(CurrentLevel + 1);


    public float CurrentAptitude => baseAptitude + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_APTITUDE * LevelStatAptitude;
    public float CurrentSummon => baseSummon + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_SUMMON * (LevelStatSummon - 1);
    public float CurrentMight => baseMight + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_MIGHT * (LevelStatMight - 1);
    public float CurrentLifespan => baseLifespan + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_LIFESPAN * (LevelStatLifespan - 1);
    public float CurrentHorde => baseLifespan + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_HORDE * LevelStatHorde;
    public float CurrentLuck => baseLifespan + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_LUCK * (LevelStatLuck - 1);


    // ---- SPELLS

    public List<SpellData> Spells { get; private set; }



    public PlayerNecromancerData()
    {
        GenerateBaseStats();
    }

    public PlayerNecromancerData(PlayerMageSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatAptitude = saveData.levelStatInsight;
        LevelStatSummon = saveData.levelStatCastSpeed;
        LevelStatMight = saveData.levelStatScholar;
        LevelStatLifespan = saveData.levelStatProficiency;

        LevelStatAptitude = Math.Min(LevelStatAptitude, UtilsMage.PER_LEVEL_MAGE_MAX_INSIGHT);
        LevelStatSummon = Math.Min(LevelStatSummon, UtilsMage.PER_LEVEL_MAGE_MAX_CASTSPEED);
        LevelStatMight = Math.Min(LevelStatMight, UtilsMage.PER_LEVEL_MAGE_MAX_SCHOLAR);
        LevelStatLifespan = Math.Min(LevelStatLifespan, UtilsMage.PER_LEVEL_MAGE_MAX_PROFICIENCY);

        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatAptitude + LevelStatSummon + LevelStatMight + LevelStatLifespan +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsMage.MAX_LEVEL_MAGE)
        {
            AvailableStatPoints = UtilsMage.MAX_LEVEL_MAGE - 1 -
               LevelStatAptitude - LevelStatSummon - LevelStatMight - LevelStatLifespan;
            CurrentExp = 0;
        }

        // load spells
        Spells = saveData.spells.Select(spell => new SpellData(spell)).ToList();
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;


        LevelStatAptitude = startLevelAptitude;
        LevelStatSummon = startLevelSummon;
        LevelStatMight = startLevelMight;
        LevelStatLifespan = startLevelLifespan;


        // multiplier
        baseAptitude = 0f; // reduced learn time spells, up to 25%

        baseSummon = 0f; // reduce cast speed, up to 20%
        baseMight = 0f; // unlocks new spell, check on whole values

        baseLifespan = 0f; // unlocks new slots, check on whole values

        // creat default spells
        Spells = UtilsMage.GetAllSpells().Select(spell => new SpellData(spell)).ToList();
    }

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= UtilsMage.MAX_LEVEL_MAGE,
            () => ExpToNextLevel
        );
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAGE_INSIGHT: LevelStatAptitude += amount; break;
            case UtilsPlayer.ID_MAGE_CASTSPEED: LevelStatSummon += amount; break;
            case UtilsPlayer.ID_MAGE_SCHOLAR:
                LevelStatMight += amount;
                int maxIndex = (int)CurrentHorde + 1;
                for (int i = 0; i < maxIndex; i++)
                {
                    if (!Spells[i].IsUnlocked)
                        Spells[i].SetUnlocked();
                }
                break;
            case UtilsPlayer.ID_MAGE_PROFICIENCY: LevelStatLifespan += amount; break;
        }

        InvokeStatChange(id, amount);
    }


    public SpellData GetSpellByType(UtilsMage.MageSpellType spellType)
    {
        return Spells.Where(spell => spell.SpellSO.SpellType == spellType).FirstOrDefault();
    }

    public void UpdateSpellData(SpellData data)
    {
        int index = Spells.FindIndex(spell => spell.SpellSO.SpellType == data.SpellSO.SpellType);
        if (index >= 0)
            Spells[index] = data;
    }
}
