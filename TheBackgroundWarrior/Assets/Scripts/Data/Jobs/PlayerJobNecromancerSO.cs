using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Necromancer Data", fileName = "NecromancerData")]
public class PlayerJobNecromancerSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainAptitude { get; private set; }
    [field: SerializeField] public float PerLevelGainSummon { get; private set; }
    [field: SerializeField] public float PerLevelGainMight { get; private set; }
    [field: SerializeField] public float PerLevelGainLifespan { get; private set; }
    [field: SerializeField] public float PerLevelGainHorde { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int MaxLevelAptitude { get; private set; }
    [field: SerializeField] public int MaxLevelSummon { get; private set; }
    [field: SerializeField] public int MaxLevelMight { get; private set; }
    [field: SerializeField] public int MaxLevelLifespan { get; private set; }
    [field: SerializeField] public int MaxLevelHorde { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }



    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainAptitude(float value)
    {
        PerLevelGainAptitude = value;
    }

    public void SetPerLevelGainSummon(float value)
    {
        PerLevelGainSummon = value;
    }

    public void SetPerLevelGainMight(float value)
    {
        PerLevelGainMight = value;
    }

    public void SetPerLevelGainLifespan(float value)
    {
        PerLevelGainLifespan = value;
    }

    public void SetPerLevelGainHorde(float value)
    {
        PerLevelGainHorde = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }



    public void SetMaxLevelAptitude(int value)
    {
        MaxLevelAptitude = value;
    }

    public void SetMaxLevelSummon(int value)
    {
        MaxLevelSummon = value;
    }

    public void SetMaxLevelMight(int value)
    {
        MaxLevelMight = value;
    }

    public void SetMaxLevelLifespan(int value)
    {
        MaxLevelLifespan = value;
    }

    public void SetMaxLevelHorde(int value)
    {
        MaxLevelHorde = value;
    }

    public void SetMaxLevelLuck(int value)
    {
        MaxLevelLuck = value;
    }



    public void SetBaseExpGrowth(float value)
    {
        BaseExpGrowth = value;
    }

    public void SetExpoExpGrowth(float value)
    {
        ExpoExpGrowth = value;
    }

    public void SetFlatExpGrowth(float value)
    {
        FlatExpGrowth = value;
    }
}
