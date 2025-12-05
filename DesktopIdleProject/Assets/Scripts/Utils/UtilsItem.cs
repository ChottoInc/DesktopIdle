using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsItem
{
    /*
     * Ores ids start from 0
     * Cards ids start from 50
     * */

    public enum ItemType { Ore, Card }


    private static List<ItemSO> ores;
    private static List<ItemSO> cards;

    private static List<ItemSO> otherItems;

    public static void Initialize()
    {
        LoadAllItems();
    }

    private static void LoadAllItems()
    {
        ItemSO[] items = Resources.LoadAll<ItemSO>("Data/Items");

        ores = new List<ItemSO>();
        cards = new List<ItemSO>();
        otherItems = new List<ItemSO>();

        foreach (ItemSO item in items) 
        {
            switch(item.ItemType)
            {
                default: otherItems.Add(item); break;
                case ItemType.Ore: ores.Add(item); break;
                case ItemType.Card: cards.Add(item); break;
            }
        }
    }

    public static List<ItemSO> GetAllItems()
    {
        List<ItemSO> result = new List<ItemSO>();
        result.AddRange(ores);
        result.AddRange(cards);
        result.AddRange(otherItems);
        return result;
    }

    public static ItemSO GetItemById(int id)
    {
        List<ItemSO> items = GetAllItems();

        foreach (ItemSO item in items)
        {
            if(item.Id == id)
                return item;
        }
        return null;
    }


    #region CARDS

    public static ItemSO GetCardById(int id)
    {
        foreach (ItemSO card in cards)
        {
            if (card.Id == id)
                return card;
        }
        return null;
    }

    public static ItemSO[] GetAllCards()
    {
        return cards.ToArray();
    }

    #endregion
}
