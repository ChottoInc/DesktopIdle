using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroup
{
    private int idItem;
    private int quantity;

    public int IdItem => idItem;
    public int Quantity => quantity;

    public ItemGroup(int idItem, int quantity)
    {
        this.idItem = idItem;
        this.quantity = quantity;
    }

    public ItemGroup(ItemGroupSaveData save)
    {
        idItem = save.idItem;
        quantity = save.quantity;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (amount > quantity) return false;

        quantity -= amount;
        return true;
    }




    public override bool Equals(object obj)
    {
        ItemGroup otherGroup = obj as ItemGroup;
        return otherGroup.idItem == idItem;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
