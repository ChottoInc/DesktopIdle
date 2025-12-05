public class PlayerMinerSaveData
{
    // ---- LEVEL STAT POINTS

    public int levelStatPower;
    public int levelStatSmashSpeed;
    public int levelStatPrecision;
    public int levelStatLuck;

    // ---- POINTS

    public int availableStatPoints;


    // ---- STAT VALUES

    public int currentLevel;
    public int currentExp;

    // ---- WEAPON

    public int levelWeaponMiner;



    public PlayerMinerSaveData() { }

    public PlayerMinerSaveData(PlayerMinerData data)
    {
        levelStatPower = data.LevelStatPower;
        levelStatSmashSpeed = data.LevelStatSmashSpeed;
        levelStatPrecision = data.LevelStatPrecision;
        levelStatLuck = data.LevelStatLuck;


        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        levelWeaponMiner = data.WeaponLevel;
    }
}
