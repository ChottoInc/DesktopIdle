using UnityEngine;
using static UtilsItem;

[CreateAssetMenu(menuName = "Data/Inventory/Item Data", fileName = "ItemData_")]
public class ItemSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] ItemType itemType;

    [Space(10)]
    [SerializeField] Sprite sprite;
    [SerializeField] string itemNameTextId;
    [SerializeField] string itemName;

    [Space(10)]
    [SerializeField] string itemDescTextId;

    [TextArea]
    [SerializeField] string itemDesc;

    public int Id => id;
    public ItemType ItemType => itemType;

    public Sprite Sprite => sprite;
    public string ItemName
    {
        get
        {
            string res = UtilsText.ItemNamesTextDictionary[itemNameTextId];
            if (res != null) return res; else return itemName;
        }
    }

    public string ItemDesc
    {
        get
        {
            string res = UtilsText.ItemDescsTextDictionary[itemDescTextId];
            if (res != null) return res; else return itemDesc;
        }
    }



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
