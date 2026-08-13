using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabPlayerStatus : UITabWindow
{
    [Header("Level")]
    [SerializeField] protected TMP_Text textCurrentLevel;

    [Header("Points")]
    [SerializeField] protected TMP_Text textAvailablePoints;

    protected int availablePoints;

    protected int tempAvailablePoints;
    protected int totalDistributedPoints;

    [Header("Texts")]
    [SerializeField] Toggle toggleAdvancedStats;
    [SerializeField] protected TMP_Text textButtonSave;


    public event Action OnToggleAdvancedStats;

    public event Action OnStatusSave;


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

    protected virtual void Setup()
    {
        if (toggleAdvancedStats != null)
            toggleAdvancedStats.SetIsOnWithoutNotify(SettingsManager.Instance.IsShowAdvancedStatOn);

        AssignAvailablePoints();

        tempAvailablePoints = availablePoints;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();

        RefreshTexts();
    }

    protected virtual void RefreshTexts()
    {
        textButtonSave.text = UtilsText.AllText[UtilsText.text_button_savechanges];
    }

    protected virtual void AssignAvailablePoints()
    {
        availablePoints = 0;
    }

    protected void OnPlayerLevelUp()
    {
        if (!IsOpen) return;

        availablePoints++;

        tempAvailablePoints++;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();
    }

    protected virtual void Resets()
    {
        totalDistributedPoints = 0;
    }


    protected virtual void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_current_level], "0");
    }

    private void UpdateAvailablePointsUI()
    {
        textAvailablePoints.text = string.Format(UtilsText.AllText[UtilsText.text_job_available_points], tempAvailablePoints);
    }


    #region STAT HANDLES



    public bool IncreaseStatLevel(int id)
    {
        if (tempAvailablePoints <= 0) return false;

        totalDistributedPoints++;
        tempAvailablePoints--;

        HandleIncreaseJobStat(id);

        UpdateAvailablePointsUI();

        return true;
    }

    protected virtual void HandleIncreaseJobStat(int id)
    {
        // handle every stat here
    }

    public bool DecreaseStatLevel(int id)
    {
        if (tempAvailablePoints >= availablePoints) return false;

        totalDistributedPoints--;
        tempAvailablePoints++;

        HandleDecreaseJobStat(id);

        UpdateAvailablePointsUI();

        return true;
    }

    protected virtual void HandleDecreaseJobStat(int id)
    {
        // handle every stat here
    }


    /// <summary>
    /// Used by single stat ui to know its level by id
    /// </summary>
    public int GetStatLevel(int id)
    {
        return HandleGetJobStatLevel(id);
    }

    protected virtual int HandleGetJobStatLevel(int id)
    {
        // handle every stat here
        return -1;
    }

    public virtual void OnToggleShowAdvancedStats(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsShowAdvancedStatsOn(isOn);

        // called here event to update if the tab is open on stats
        OnToggleAdvancedStats?.Invoke();
    }

    #endregion


    protected virtual void SaveChanges()
    {
        OnStatusSave?.Invoke();
    }



    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();
        Close();
    }
}
