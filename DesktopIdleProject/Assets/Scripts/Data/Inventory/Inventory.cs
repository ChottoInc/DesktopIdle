using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemGroup> itemGroups;


    public List<ItemGroup> ItemGroups => itemGroups;


    public Inventory()
    {
        itemGroups = new List<ItemGroup>();
    }

    public Inventory(InventorySaveData saveData)
    {
        itemGroups = new List<ItemGroup>();

        foreach (var group in saveData.groupSaves)
        {
            itemGroups.Add(new ItemGroup(group));
        }
    }

    public void AddItem(int id, int quantity)
    {
        if (!HasItem(id))
        {
            ItemGroup group = new ItemGroup(id, quantity);
            itemGroups.Add(group);
        }
        else
        {
            int index = GetGroupIndex(id);
            itemGroups[index].AddQuantity(quantity);
        }
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
}
