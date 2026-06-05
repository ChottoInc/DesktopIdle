using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Miner/Rock Data", fileName = "RockData_")]
public class RockSO : ListableGameDataSO
{
    [SerializeField] Sprite sprite;
    [SerializeField] UtilsMiner.RockType rockType;
    [SerializeField] float baseLootChance;

    [Space(10)]
    [SerializeField] UtilsGeneral.GeneralChances<ItemSO>[] possibleItems;

    public Sprite Sprite => sprite;
    public UtilsMiner.RockType RockType => rockType;
    public float BaseLootChance => baseLootChance;

    public UtilsGeneral.GeneralChances<ItemSO>[] PossibleItems => possibleItems;
}
