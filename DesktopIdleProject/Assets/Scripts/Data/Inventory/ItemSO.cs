using UnityEngine;
using static UtilsItem;

[CreateAssetMenu(menuName = "Data/Inventory/Item Data", fileName = "ItemData_")]
public class ItemSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] ItemType itemType;

    public int Id;
    public ItemType ItemType => itemType;

    public override bool Equals(object other)
    {
        ItemSO otherItem = other as ItemSO;
        return id == otherItem.id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
