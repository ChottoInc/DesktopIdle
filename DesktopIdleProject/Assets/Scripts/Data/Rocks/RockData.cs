using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockData
{
    private RockSO rockSO;

    private float currentDurability;




    public RockSO RockSO => rockSO;

    public float CurrentDurability => currentDurability;


    public void Setup(RockSO rockSO)
    {
        this.rockSO = rockSO;

        currentDurability = UtilsGather.GetRockDurabilityByType(rockSO.RockType);
    }

    public void SetSmashed()
    {
        currentDurability = 0;
    }
}
