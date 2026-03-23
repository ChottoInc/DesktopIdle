using Newtonsoft.Json;
using System.ComponentModel;

public class PlayerBlacksmithSaveData
{
    // ---- LEVEL STAT POINTS

    public int levelStatCraftSpeed;
    public int levelStatEfficiency;
    public int levelStatLuck;

    // ---- POINTS

    public int availableStatPoints;


    // ---- STAT VALUES

    public int currentLevel;

    public int currentExp;

    // ---- WEAPON

    public int levelHelmetBlacksmith;
    public int levelArmorBlacksmith;
    public int levelGlovesBlacksmith;
    public int levelBootsBlacksmith;

    // ---- FORGING

    public int currentForgingOre;



    public PlayerBlacksmithSaveData() { }

    public PlayerBlacksmithSaveData(PlayerBlacksmithData data)
    {
        levelStatCraftSpeed = data.LevelStatCraftSpeed;
        levelStatEfficiency = data.LevelEfficiency;
        levelStatLuck = data.LevelStatLuck;


        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        levelHelmetBlacksmith = data.HelmetLevel;
        levelArmorBlacksmith = data.ArmorLevel;
        levelGlovesBlacksmith = data.GlovesLevel;
        levelBootsBlacksmith = data.BootsLevel;

        currentForgingOre = data.CurrentForgingOre;
    }
}
