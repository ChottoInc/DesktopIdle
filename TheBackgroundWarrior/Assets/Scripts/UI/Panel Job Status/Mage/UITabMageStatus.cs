using UnityEngine;

public class UITabMageStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerMage player;

    private int distributedPointsOnInsight;
    private int distributedPointsOnCastSpeed;
    private int distributedPointsOnScholar;
    private int distributedPointsOnProficiency;

    private void OnDestroy()
    {
        player.PlayerData.OnLevelUp -= OnPlayerLevelUp;
    }

    private void Awake()
    {
        player.PlayerData.OnLevelUp += OnPlayerLevelUp;
    }

    protected override void Resets()
    {
        base.Resets();

        distributedPointsOnInsight = 0;
        distributedPointsOnCastSpeed = 0;
        distributedPointsOnScholar = 0;
        distributedPointsOnProficiency = 0;
    }

    protected override void AssignAvailablePoints()
    {
        availablePoints = player.PlayerData.AvailableStatPoints;
    }

    protected override void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_current_level], player.PlayerData.CurrentLevel);
    }

    public void OnButtonSaveChanges()
    {
        // set changes

        if (distributedPointsOnInsight > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MAGE_INSIGHT, distributedPointsOnInsight);
        }

        if (distributedPointsOnCastSpeed > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MAGE_CASTSPEED, distributedPointsOnCastSpeed);
        }

        if (distributedPointsOnScholar > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MAGE_SCHOLAR, distributedPointsOnScholar);
        }

        if (distributedPointsOnProficiency > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MAGE_PROFICIENCY, distributedPointsOnProficiency);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveMageData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAGE_INSIGHT: distributedPointsOnInsight++; break;
            case UtilsPlayer.ID_MAGE_CASTSPEED: distributedPointsOnCastSpeed++; break;
            case UtilsPlayer.ID_MAGE_SCHOLAR: distributedPointsOnScholar++; break;
            case UtilsPlayer.ID_MAGE_PROFICIENCY: distributedPointsOnProficiency++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAGE_INSIGHT: distributedPointsOnInsight--; break;
            case UtilsPlayer.ID_MAGE_CASTSPEED: distributedPointsOnCastSpeed--; break;
            case UtilsPlayer.ID_MAGE_SCHOLAR: distributedPointsOnScholar--; break;
            case UtilsPlayer.ID_MAGE_PROFICIENCY: distributedPointsOnProficiency--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_MAGE_INSIGHT: return player.PlayerData.LevelStatInsight;
            case UtilsPlayer.ID_MAGE_CASTSPEED: return player.PlayerData.LevelStatCastSpeed;
            case UtilsPlayer.ID_MAGE_SCHOLAR: return player.PlayerData.LevelStatScholar;
            case UtilsPlayer.ID_MAGE_PROFICIENCY: return player.PlayerData.LevelStatProficiency;
        }
    }
}
