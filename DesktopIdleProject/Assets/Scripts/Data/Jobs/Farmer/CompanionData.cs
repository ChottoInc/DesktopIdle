using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionData
{
    private CompanionSO companionSO;

    private int currentLevel;

    private int currentSlot;


    public CompanionSO CompanionSO => companionSO;

    public int CurrentLevel => currentLevel;

    public int CurrentSlot => currentSlot;



    public CompanionData(CompanionSO companionSO)
    {
        this.companionSO = companionSO;

        currentSlot = - 1;
    }

    public CompanionData(CompanionSaveData saveData)
    {
        companionSO = UtilsGather.GetCompanionById(saveData.companionId);

        currentLevel = saveData.currentLevel;

        currentSlot = saveData.currentSlot;
    }

    public void SetSlot(int id)
    {
        currentSlot = id;
    }
}
