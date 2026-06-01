using UnityEngine;

public class ShopItemSO : ListableGameDataSO
{
    [SerializeField] protected string uniqueId;

    [Space(10)]
    [SerializeField] protected bool isUnique;
    [SerializeField] protected bool isDaily;

    [Space(10)]
    [SerializeField] protected UtilsShop.ShopItemType shopItemType;

    [Space(10)]
    [SerializeField] protected string itemNameTextId;
    [SerializeField] protected string itemName;

    [Space(10)]
    [SerializeField] protected string itemDescTextId;

    [TextArea]
    [SerializeField] protected string itemDesc;

    [Space(10)]
    [SerializeField] protected int price;

    [Space(10)]
    [SerializeField] protected Sprite sprite;


    public string UniqueId => uniqueId;

    public bool IsUnique => isUnique;
    public bool IsDaily => isDaily;

    public UtilsShop.ShopItemType ShopItemType => shopItemType;

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

    public int Price => price;

    public Sprite Sprite => sprite;
}
