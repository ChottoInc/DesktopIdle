using UnityEngine;

public class RockData
{
    private RockSO rockSO;

    private float currentDurability;




    public RockSO RockSO => rockSO;

    public float CurrentDurability => currentDurability;


    public RockData(RockSO rockSO)
    {
        this.rockSO = rockSO;

        currentDurability = UtilsGather.GetRockDurabilityByType(rockSO.RockType);
    }

    public void TakeDamage(PlayerMinerData data)
    {
        // can't take less than 0 or it will cure

        float baseDamage = data.CurrentPower;
        float total;

        total = Mathf.Max(0f, baseDamage * data.CurrentPrecision);

        // subtract total to hp
        currentDurability -= total;

        if (currentDurability <= 0f)
        {
            currentDurability = 0;
        }
    }

    public void SetSmashed()
    {
        currentDurability = 0;
    }
}
