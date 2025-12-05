
public class PlayerFightSaveData
{
    // ---- LEVEL STAT POINTS

    public int levelStatMaxHp;
    
    public int levelStatAtk;
    public int levelStatDef;
    
    public int levelStatAtkSpd;
    
    public int levelStatCritRate;
    public int levelStatCritDmg;
    
    public int levelStatLuck;

    // ---- POINTS

    public int availableStatPoints;


    // ---- STAT VALUES

    public int currentLevel;
    public int currentExp;

    public PlayerFightSaveData() { }

    public PlayerFightSaveData(PlayerFightData data)
    {
        levelStatMaxHp = data.LevelStatMaxHp;

        levelStatAtk = data.LevelStatAtk;
        levelStatDef = data.LevelStatDef;

        levelStatAtkSpd = data.LevelStatAtkSpd;

        levelStatCritRate = data.LevelStatCritRate;
        levelStatCritDmg = data.LevelStatCritDmg;

        levelStatLuck = data.LevelStatLuck;


        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;
    }   
}
