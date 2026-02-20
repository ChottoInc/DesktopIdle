using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFisherSaveData
{
    public int levelStatCalmness;
    public int levelReflex;
    public int levelKnowledge;
    public int levelStatLuck;
    
    public int availableStatPoints;
    
    public int currentLevel;
    public int currentExp;

    public PlayerFisherSaveData() { }

    public PlayerFisherSaveData(PlayerFisherData data)
    {
        levelStatCalmness = data.LevelStatCalmness;
        levelReflex = data.LevelReflex;
        levelKnowledge = data.LevelKnowledge;
        levelStatLuck = data.LevelStatLuck;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

    }
}
