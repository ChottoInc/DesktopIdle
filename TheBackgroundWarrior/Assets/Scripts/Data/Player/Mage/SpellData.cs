using System;
using UnityEngine;

public class SpellData
{
    public SpellSO SpellSO { get; private set; }

    public bool IsUnlocked { get; private set; }

    public int CurrentRank { get; private set; }
    public int CurrentLearnPoints { get; private set; }

    public int RequiredPointsToNextRank 
    { 
        get 
        {
            // get base required points and adds the stat
            int baseRequirePoints = UtilsMage.RequiredPointsForNextRank(this);
            return baseRequirePoints - Mathf.FloorToInt((float)baseRequirePoints * PlayerManager.Instance.PlayerMageData.CurrentInsight); 
        } 
    }

    public float CooldownCast => SpellSO.BaseCooldownCastWarrior - Mathf.FloorToInt(SpellSO.BaseCooldownCastWarrior * PlayerManager.Instance.PlayerMageData.CurrentCastSpeed);



    public float PercDamage => SpellSO.CombatData.percDamage + (SpellSO.CombatData.percAddDamagePerLevel * CurrentRank);
    public float Radius => SpellSO.CombatData.radius + (SpellSO.CombatData.percAddRadiusPerLevel * CurrentRank);
    public float PercentageMoreDamageFromSpells => SpellSO.CombatData.percMoreDamageFromSpells + (SpellSO.CombatData.percAddMoreDamageFromSpellsPerLevel * CurrentRank);
    public float PercentageLifesteal => SpellSO.CombatData.percLifesteal + (SpellSO.CombatData.percAddLifestealPerLevel * CurrentRank);
    public int Bounces => SpellSO.CombatData.maxBounces + (SpellSO.CombatData.addBouncesPerLevel * CurrentRank);



    public event Action OnPointsAdded;


    public SpellData(SpellSO spellSO)
    {
        SpellSO = spellSO;

        IsUnlocked = false;

        CurrentRank = 0;
        CurrentLearnPoints = 0;
    }

    public SpellData(SpellSaveData saveData)
    {
        SpellSO = UtilsMage.GetSpellById(saveData.id);
        IsUnlocked = saveData.isUnlocked;
        CurrentRank = saveData.currentRank;
        CurrentLearnPoints = saveData.currentLearnPoints;
    }

    public void SetUnlocked()
    {
        IsUnlocked = true;
    }

    public void AddPoints(int value = 1)
    {
        // check max rank
        if (CurrentRank >= SpellSO.MaxRank)
        {
            // set current points to 0
            CurrentLearnPoints = 0;
            return;
        }

        CurrentLearnPoints += value;

        // looping for every rank gained
        while (CurrentLearnPoints >= RequiredPointsToNextRank)
        {
            // recalculate current points
            CurrentLearnPoints -= RequiredPointsToNextRank;

            // give rank
            CurrentRank++;

            //OnLevelUp?.Invoke();
        }

        OnPointsAdded?.Invoke();
    }
}
