
public class PlayerNecromancerSaveData
{
    public int levelStatAptitude;
    public int levelStatSummon;
    public int levelStatMight;
    public int levelStatLifespan;
    public int levelStatHorde;
    public int levelStatLuck;

    public int availableStatPoints;

    public int currentLevel;
    public long currentExp;

    public PlayerNecromancerSaveData() { }

    public PlayerNecromancerSaveData(PlayerNecromancerData data)
    {
        levelStatAptitude = data.LevelStatAptitude;
        levelStatSummon = data.LevelStatSummon;
        levelStatMight = data.LevelStatMight;
        levelStatLifespan = data.LevelStatLifespan;
        levelStatHorde = data.LevelStatHorde;
        levelStatLuck = data.LevelStatLuck;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;
    }
}
