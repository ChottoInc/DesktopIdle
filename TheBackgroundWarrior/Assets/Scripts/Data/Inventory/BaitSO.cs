using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Bait Data", fileName = "BaitData_")]
public class BaitSO : ItemSO
{
    [Space(10)]
    [SerializeField] UtilsGeneral.DayMoment attractsMoment;
    [SerializeField] float duration;
    [SerializeField] UtilsFisher.BaitEffectivness effectivness;

    public UtilsGeneral.DayMoment AttractsMoment => attractsMoment;
    public float Duration => duration;
    public UtilsFisher.BaitEffectivness Effectivness => effectivness;
}
