using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Buff/Buff Data", fileName = "BuffData_")]
public class BuffSO : ListableGameDataSO
{
    [SerializeField] UtilsBuffs.BuffType _buffType;
    [SerializeField] Sprite _sprite;

    [SerializeField] string _itemNameTextId;
    [SerializeField] string _itemName;

    [Space(10)]
    [SerializeField] string _itemDescTextId;

    [TextArea]
    [SerializeField] string _itemDesc;

    public UtilsBuffs.BuffType BuffType => _buffType;
    public Sprite Sprite => _sprite;

    public string ItemName
    {
        get
        {
            string res = UtilsText.AllText[_itemNameTextId];
            if (res != null) return res; else return _itemName;
        }
    }

    public string ItemDesc
    {
        get
        {
            string res = UtilsText.AllText[_itemDescTextId];
            if (res != null) return res; else return _itemDesc;
        }
    }
}
