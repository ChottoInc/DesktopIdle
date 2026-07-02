using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Fish Data", fileName = "FishData_")]
public class FishSO : ItemSO
{
    [SerializeField] UtilsItem.FishRarity fishRarity;
    [SerializeField] UtilsGeneral.DayMoment spawnDayMoment;

    public UtilsItem.FishRarity FishRarity => fishRarity;
    public UtilsGeneral.DayMoment SpawnDayMoment => spawnDayMoment;


    public override string ToString()
    {
        return string.Format("Fish {0} with id {1}, rarity: {2}, spawn moment: {3}", ItemName, Id, FishRarity, SpawnDayMoment);
    }
}
