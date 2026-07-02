using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Alchemist Data", fileName = "AlchemistData")]
public class PlayerJobAlchemistSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainRoutine { get; private set; }
    [field: SerializeField] public float PerLevelGainYield { get; private set; }
    [field: SerializeField] public float PerLevelGainResearch { get; private set; }
    [field: SerializeField] public float PerLevelGainStability { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int MaxLevelRoutine { get; private set; }
    [field: SerializeField] public int MaxLevelYield { get; private set; }
    [field: SerializeField] public int MaxLevelResearch { get; private set; }
    [field: SerializeField] public int MaxLevelStability { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainRoutine(float value)
    {
        PerLevelGainRoutine = value;
    }

    public void SetPerLevelGainYield(float value)
    {
        PerLevelGainYield = value;
    }

    public void SetPerLevelGainResearch(float value)
    {
        PerLevelGainResearch = value;
    }

    public void SetPerLevelGainStability(float value)
    {
        PerLevelGainStability = value;
    }



    public void SetMaxLevelRoutine(int value)
    {
        MaxLevelRoutine = value;
    }

    public void SetMaxLevelYield(int value)
    {
        MaxLevelYield = value;
    }

    public void SetMaxLevelResearch(int value)
    {
        MaxLevelResearch = value;
    }

    public void SetMaxLevelStability(int value)
    {
        MaxLevelStability = value;
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
