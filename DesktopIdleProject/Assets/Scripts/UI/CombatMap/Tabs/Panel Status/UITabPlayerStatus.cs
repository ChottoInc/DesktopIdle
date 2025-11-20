using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITabPlayerStatus : UITabWindow
{
    [Header("Player")]
    [SerializeField] PlayerFight player;

    [Header("Level")]
    [SerializeField] TMP_Text textCurrentLevel;
    [SerializeField] UIPlayerExpBar playerExpBar;

    [Header("Points")]
    [SerializeField] TMP_Text textAvailablePoints;

    private int availablePoints;

    private int tempAvailablePoints;
    private int totalDistributedPoints;


    private int distributedPointsOnMaxHp;
    private int distributedPointsOnAtk;
    private int distributedPointsOnDef;
    private int distributedPointsOnAtkSpd;
    private int distributedPointsOnCritRate;
    private int distributedPointsOnCritDmg;
    private int distributedPointsOnLuck;


    public event Action OnStatusSave;


    private void OnDestroy()
    {
        player.PlayerData.OnLevelUp -= OnPlayerLevelUp;
    }

    private void Awake()
    {
        player.PlayerData.OnLevelUp += OnPlayerLevelUp;
    }

    public override void Open()
    {
        base.Open();

        Setup();
    }

    public override void Close()
    {
        base.Close();

        Resets();
    }

    private void Setup()
    {
        availablePoints = player.PlayerData.AvailableStatPoints;

        tempAvailablePoints = availablePoints;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();
    }

    private void OnPlayerLevelUp()
    {
        if (!isOpen) return;

        availablePoints++;

        tempAvailablePoints++;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();
    }

    private void Resets()
    {
        totalDistributedPoints = 0;

        distributedPointsOnMaxHp = 0;
        distributedPointsOnAtk = 0;
        distributedPointsOnDef = 0;
        distributedPointsOnAtkSpd = 0;
        distributedPointsOnCritRate = 0;
        distributedPointsOnCritDmg = 0;
        distributedPointsOnLuck = 0;
    }


    private void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = $"Lv. : {player.PlayerData.CurrentLevel}";
    }

    private void UpdateAvailablePointsUI()
    {
        textAvailablePoints.text = $"Available points: {tempAvailablePoints}";
    }



    public void OnButtonClose()
    {
        Close();
    }

    public void OnButtonSaveChanges()
    {
        // set changes

        if(distributedPointsOnMaxHp > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MAXHP, distributedPointsOnMaxHp);
        }

        if (distributedPointsOnAtk > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ATK, distributedPointsOnAtk);
        }

        if (distributedPointsOnDef > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_DEF, distributedPointsOnDef);
        }

        if (distributedPointsOnAtkSpd > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ATKSPD, distributedPointsOnAtkSpd);
        }

        if (distributedPointsOnCritRate > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_CRITRATE, distributedPointsOnCritRate);
        }

        if (distributedPointsOnCritDmg > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_CRITDMG, distributedPointsOnCritDmg);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        OnStatusSave?.Invoke();
    }

    public bool IncreaseStatLevel(int id)
    {
        if (tempAvailablePoints <= 0) return false;

        totalDistributedPoints++;
        tempAvailablePoints--;

        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAXHP: distributedPointsOnMaxHp++; break;
            case UtilsPlayer.ID_ATK: distributedPointsOnAtk++; break;
            case UtilsPlayer.ID_DEF: distributedPointsOnDef++; break;
            case UtilsPlayer.ID_ATKSPD: distributedPointsOnAtkSpd++; break;
            case UtilsPlayer.ID_CRITRATE: distributedPointsOnCritRate++; break;
            case UtilsPlayer.ID_CRITDMG: distributedPointsOnCritDmg++; break;
            case UtilsPlayer.ID_LUCK: distributedPointsOnLuck++; break;
        }

        UpdateAvailablePointsUI();

        return true;
    }

    public bool DecreaseStatLevel(int id)
    {
        if (tempAvailablePoints >= availablePoints) return false;

        totalDistributedPoints--;
        tempAvailablePoints++;

        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAXHP: distributedPointsOnMaxHp--; break;
            case UtilsPlayer.ID_ATK: distributedPointsOnAtk--; break;
            case UtilsPlayer.ID_DEF: distributedPointsOnDef--; break;
            case UtilsPlayer.ID_ATKSPD: distributedPointsOnAtkSpd--; break;
            case UtilsPlayer.ID_CRITRATE: distributedPointsOnCritRate--; break;
            case UtilsPlayer.ID_CRITDMG: distributedPointsOnCritDmg--; break;
            case UtilsPlayer.ID_LUCK: distributedPointsOnLuck--; break;
        }

        UpdateAvailablePointsUI();

        return true;
    }

    public int GetStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_MAXHP: return player.PlayerData.LevelStatMaxHp;
            case UtilsPlayer.ID_ATK: return player.PlayerData.LevelStatAtk;
            case UtilsPlayer.ID_DEF: return player.PlayerData.LevelStatDef;
            case UtilsPlayer.ID_ATKSPD: return player.PlayerData.LevelStatAtkSpd;
            case UtilsPlayer.ID_CRITRATE: return player.PlayerData.LevelStatCritRate;
            case UtilsPlayer.ID_CRITDMG: return player.PlayerData.LevelStatCritDmg;
            case UtilsPlayer.ID_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
