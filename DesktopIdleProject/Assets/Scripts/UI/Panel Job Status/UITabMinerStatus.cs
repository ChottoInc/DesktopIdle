using UnityEngine;

public class UITabMinerStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerMiner player;

    private int distributedPointsOnPower;
    private int distributedPointsOnSmashSpeed;
    private int distributedPointsOnPrecision;
    private int distributedPointsOnLuck;

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

        distributedPointsOnPower = 0;
        distributedPointsOnSmashSpeed = 0;
        distributedPointsOnPrecision = 0;
        distributedPointsOnLuck = 0;
    }

    protected override void AssignAvailablePoints()
    {
        availablePoints = player.PlayerData.AvailableStatPoints;
    }

    protected override void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = $"Lv. : {player.PlayerData.CurrentLevel}";
    }

    public void OnButtonSaveChanges()
    {
        // set changes

        if (distributedPointsOnPower > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MINER_POWER, distributedPointsOnPower);
        }

        if (distributedPointsOnSmashSpeed > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MINER_SMASHSPEED, distributedPointsOnSmashSpeed);
        }

        if (distributedPointsOnPrecision > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MINER_PRECISION, distributedPointsOnPrecision);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_MINER_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveFightData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MINER_POWER: distributedPointsOnPower++; break;
            case UtilsPlayer.ID_MINER_SMASHSPEED: distributedPointsOnSmashSpeed++; break;
            case UtilsPlayer.ID_MINER_PRECISION: distributedPointsOnPrecision++; break;
            case UtilsPlayer.ID_MINER_LUCK: distributedPointsOnLuck++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MINER_POWER: distributedPointsOnPower--; break;
            case UtilsPlayer.ID_MINER_SMASHSPEED: distributedPointsOnSmashSpeed--; break;
            case UtilsPlayer.ID_MINER_PRECISION: distributedPointsOnPrecision--; break;
            case UtilsPlayer.ID_MINER_LUCK: distributedPointsOnLuck--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_MINER_POWER: return player.PlayerData.LevelStatPower;
            case UtilsPlayer.ID_MINER_SMASHSPEED: return player.PlayerData.LevelStatSmashSpeed;
            case UtilsPlayer.ID_MINER_PRECISION: return player.PlayerData.LevelStatPrecision;
            case UtilsPlayer.ID_MINER_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
