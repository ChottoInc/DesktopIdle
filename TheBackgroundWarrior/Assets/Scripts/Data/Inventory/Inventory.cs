using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    // base currency
    private int currentBits;

    private List<ItemGroup> itemGroups;


    public int CurrentBits => currentBits;

    public List<ItemGroup> ItemGroups => itemGroups;



    // Trigger used for quests
    public event Action<int> OnItemAdd;



    public Inventory()
    {
        currentBits = 0;
        itemGroups = new List<ItemGroup>();
    }

    public Inventory(InventorySaveData saveData)
    {
        currentBits = saveData.currentBits;

        itemGroups = new List<ItemGroup>();

        foreach (var group in saveData.groupSaves)
        {
            itemGroups.Add(new ItemGroup(group));
        }
    }

    #region CURRENCIES

    public void AddBits(int amount)
    {
        currentBits += amount;
    }

    public bool RemoveBits(int amount)
    {
        if(currentBits < amount)
        {
            Debug.Log("Insufficient bits");
            return false;
        }

        currentBits -= amount;
        return true;
    }

    #endregion

    #region ITEMS

    public void AddItems(List<int> ids)
    {
        foreach (var id in ids)
        {
            AddItem(id, 1);
        }
    }

    public void AddItems(List<ItemGroup> groups)
    {
        foreach (var group in groups)
        {
            AddItem(group.IdItem, group.Quantity);
        }
    }

    public void AddItem(int id, int quantity)
    {
        //Debug.Log("id: " + id);
        OnItemAdd?.Invoke(id);

        if (!HasItem(id))
        {
            ItemGroup group = new ItemGroup(id, quantity);
            itemGroups.Add(group);
        }
        else
        {
            //ItemSO itemSO = UtilsItem.GetItemById(id);

            int index = GetGroupIndex(id);
            itemGroups[index].AddQuantity(quantity);
            /*
            if (itemSO.ItemType != UtilsItem.ItemType.Fish)
            {
                int index = GetGroupIndex(id);
                itemGroups[index].AddQuantity(quantity);
            }*/
        }

        itemGroups.Sort();
    }

    public bool RemoveItem(int id, int quantity)
    {
        if (!HasItem(id)) return false;

        int index = GetGroupIndex(id);

        bool result = itemGroups[index].RemoveQuantity(quantity);

        if(result)
        {
            if (itemGroups[index].Quantity <= 0)
            {
                itemGroups.RemoveAt(index);
            }
        }

        return result;
    }

    public bool HasItem(int id)
    {
        foreach (var group in itemGroups)
        {
            if (group.IdItem == id)
                return true;
        }
        return false;
    }

    public bool HasEnough(int id, int amount)
    {
        if (!HasItem(id)) return false;

        int index = GetGroupIndex(id);

        if (index > -1)
        {
            if (itemGroups[index].Quantity >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public int GetGroupIndex(int id)
    {
        for (int i = 0; i < itemGroups.Count; i++)
        {
            if (itemGroups[i].IdItem == id)
                return i;
        }
        return -1;
    }

    public int GetItemQuantity(int id)
    {
        int index = GetGroupIndex(id);

        if (index == -1) return -1;
        return itemGroups[index].Quantity;
    }

    public List<ItemGroup> GetGroupsOfType(UtilsItem.ItemType itemType)
    {
        return itemGroups.Where(group => UtilsItem.GetItemById(group.IdItem).ItemType == itemType).ToList();
    }

    public List<ItemGroup> GetAllCards()
    {
        List<ItemGroup> result = new List<ItemGroup>();
        foreach (var group in itemGroups)
        {
            ItemSO item = UtilsItem.GetItemById(group.IdItem);
            if (item.ItemType == UtilsItem.ItemType.Card)
                result.Add(group);
        }
        return result;
    }

    #endregion
}
