using UnityEngine;

[System.Serializable]
public class CompanionStatModifier
{
    [Tooltip("Id of stat to change")]
    [SerializeField] int statModifier;

    [SerializeField] float baseModifierValue = 0.01f;

    [SerializeField] float increasePerLevelValue = 0.01f;

    public int StatModifier => statModifier;
    public float BaseModifierValue => baseModifierValue;
    public float IncreasePerLevelValue => increasePerLevelValue;
}
