using UnityEngine;

public class UITabAlchemistStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerAlchemist player;

    private int distributedPointsOnRoutine;
    private int distributedPointsOnYield;
    private int distributedPointsOnResearch;
    private int distributedPointsOnStability;

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

        distributedPointsOnRoutine = 0;
        distributedPointsOnYield = 0;
        distributedPointsOnResearch = 0;
        distributedPointsOnStability = 0;
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

        if (distributedPointsOnRoutine > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ALCHEMIST_ROUTINE, distributedPointsOnRoutine);
        }

        if (distributedPointsOnYield > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ALCHEMIST_YIELD, distributedPointsOnYield);
        }

        if (distributedPointsOnResearch > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ALCHEMIST_RESEARCH, distributedPointsOnResearch);
        }

        if (distributedPointsOnStability > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_ALCHEMIST_STABILITY, distributedPointsOnStability);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveAlchemistData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_ALCHEMIST_ROUTINE: distributedPointsOnRoutine++; break;
            case UtilsPlayer.ID_ALCHEMIST_YIELD: distributedPointsOnYield++; break;
            case UtilsPlayer.ID_ALCHEMIST_RESEARCH: distributedPointsOnResearch++; break;
            case UtilsPlayer.ID_ALCHEMIST_STABILITY: distributedPointsOnStability++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_ALCHEMIST_ROUTINE: distributedPointsOnRoutine--; break;
            case UtilsPlayer.ID_ALCHEMIST_YIELD: distributedPointsOnYield--; break;
            case UtilsPlayer.ID_ALCHEMIST_RESEARCH: distributedPointsOnResearch--; break;
            case UtilsPlayer.ID_ALCHEMIST_STABILITY: distributedPointsOnStability--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_ALCHEMIST_ROUTINE: return player.PlayerData.LevelStatRoutine;
            case UtilsPlayer.ID_ALCHEMIST_YIELD: return player.PlayerData.LevelStatYield;
            case UtilsPlayer.ID_ALCHEMIST_RESEARCH: return player.PlayerData.LevelStatResearch;
            case UtilsPlayer.ID_ALCHEMIST_STABILITY: return player.PlayerData.LevelStatStability;
        }
    }
}
