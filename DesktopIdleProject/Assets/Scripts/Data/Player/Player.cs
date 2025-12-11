using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<UtilsItem.ItemType> OnItemAdd;

    public virtual void AddItem(int id, int quantity)
    {
        PlayerManager.Instance.Inventory.AddItem(id, quantity);
        PlayerManager.Instance.SaveInventoryData();

        ItemSO itemSO = UtilsItem.GetItemById(id);
        AddItemEvent(itemSO.ItemType);
    }

    public virtual void AddItemEvent(UtilsItem.ItemType itemType)
    {
        OnItemAdd?.Invoke(itemType);
    }
}
