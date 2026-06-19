using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Mage Data", fileName = "MageData")]
public class PlayerJobMageSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainInsight { get; private set; }
    [field: SerializeField] public float PerLevelGainCastSpeed { get; private set; }
    [field: SerializeField] public float PerLevelGainScholar { get; private set; }
    [field: SerializeField] public float PerLevelGainProficiency { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int MaxLevelInsight { get; private set; }
    [field: SerializeField] public int MaxLevelCastSpeed { get; private set; }
    [field: SerializeField] public int MaxLevelScholar { get; private set; }
    [field: SerializeField] public int MaxLevelProficiency { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainInsight(float value)
    {
        PerLevelGainInsight = value;
    }

    public void SetPerLevelGainCastSpeed(float value)
    {
        PerLevelGainCastSpeed = value;
    }

    public void SetPerLevelGainScholar(float value)
    {
        PerLevelGainScholar = value;
    }

    public void SetPerLevelGainProficiency(float value)
    {
        PerLevelGainProficiency = value;
    }



    public void SetMaxLevelInsight(int value)
    {
        MaxLevelInsight = value;
    }

    public void SetMaxLevelCastSpeed(int value)
    {
        MaxLevelCastSpeed = value;
    }

    public void SetMaxLevelScholar(int value)
    {
        MaxLevelScholar = value;
    }

    public void SetMaxLevelProficiency(int value)
    {
        MaxLevelProficiency = value;
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
