using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Rocks/Rock Data", fileName = "RockData_")]
public class RockSO : ListableGameDataSO
{
    [SerializeField] Sprite sprite;
    [SerializeField] UtilsGather.RockType rockType;
    [SerializeField] float baseLootChance;

    [Space(10)]
    [SerializeField] UtilsGeneral.GeneralChances<ItemSO>[] possibleItems;

    public Sprite Sprite => sprite;
    public UtilsGather.RockType RockType => rockType;
    public float BaseLootChance => baseLootChance;

    public UtilsGeneral.GeneralChances<ItemSO>[] PossibleItems => possibleItems;
}
