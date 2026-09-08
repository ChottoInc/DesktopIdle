using UnityEngine;

public class UITabNecromancerStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerNecromancer player;

    private int distributedPointsOnAptitude;
    private int distributedPointsOnSummon;
    private int distributedPointsOnMight;
    private int distributedPointsOnLifespan;
    private int distributedPointsOnHorde;
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

        distributedPointsOnAptitude = 0;
        distributedPointsOnSummon = 0;
        distributedPointsOnMight = 0;
        distributedPointsOnLifespan = 0;
        distributedPointsOnHorde = 0;
        distributedPointsOnLuck = 0;
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

        if (distributedPointsOnAptitude > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_APTITUDE, distributedPointsOnAptitude);
        }

        if (distributedPointsOnSummon > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_SUMMON, distributedPointsOnSummon);
        }

        if (distributedPointsOnMight > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_MIGHT, distributedPointsOnMight);
        }

        if (distributedPointsOnLifespan > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_LIFESPAN, distributedPointsOnLifespan);
        }

        if (distributedPointsOnHorde > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_HORDE, distributedPointsOnHorde);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_NECROMANCER_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveNecromancerData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_NECROMANCER_APTITUDE: distributedPointsOnAptitude++; break;
            case UtilsPlayer.ID_NECROMANCER_SUMMON: distributedPointsOnSummon++; break;
            case UtilsPlayer.ID_NECROMANCER_MIGHT: distributedPointsOnMight++; break;
            case UtilsPlayer.ID_NECROMANCER_LIFESPAN: distributedPointsOnLifespan++; break;
            case UtilsPlayer.ID_NECROMANCER_HORDE: distributedPointsOnHorde++; break;
            case UtilsPlayer.ID_NECROMANCER_LUCK: distributedPointsOnLuck++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_NECROMANCER_APTITUDE: distributedPointsOnAptitude--; break;
            case UtilsPlayer.ID_NECROMANCER_SUMMON: distributedPointsOnSummon--; break;
            case UtilsPlayer.ID_NECROMANCER_MIGHT: distributedPointsOnMight--; break;
            case UtilsPlayer.ID_NECROMANCER_LIFESPAN: distributedPointsOnLifespan--; break;
            case UtilsPlayer.ID_NECROMANCER_HORDE: distributedPointsOnHorde--; break;
            case UtilsPlayer.ID_NECROMANCER_LUCK: distributedPointsOnLuck--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_NECROMANCER_APTITUDE: return player.PlayerData.LevelStatAptitude;
            case UtilsPlayer.ID_NECROMANCER_SUMMON: return player.PlayerData.LevelStatSummon;
            case UtilsPlayer.ID_NECROMANCER_MIGHT: return player.PlayerData.LevelStatMight;
            case UtilsPlayer.ID_NECROMANCER_LIFESPAN: return player.PlayerData.LevelStatLifespan;
            case UtilsPlayer.ID_NECROMANCER_HORDE: return player.PlayerData.LevelStatHorde;
            case UtilsPlayer.ID_NECROMANCER_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
