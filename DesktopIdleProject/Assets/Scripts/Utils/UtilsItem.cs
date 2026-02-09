using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsItem
{
    /*
     * Ores ids start from 0
     * Cards ids start from 50
     * Metals ids start from 150
     * */

    public enum ItemType { Ore, Card, Metal }

    public enum CardRarity { Common, Uncommon, Rare }


    private static List<ItemSO> ores;
    private static List<ItemSO> cards;
    private static List<ItemSO> metals;

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
        metals = new List<ItemSO>();
        otherItems = new List<ItemSO>();

        foreach (ItemSO item in items) 
        {
            switch(item.ItemType)
            {
                default: otherItems.Add(item); break;
                case ItemType.Ore: ores.Add(item); break;
                case ItemType.Card: cards.Add(item); break;
                case ItemType.Metal: metals.Add(item); break;
            }
        }
    }

    public static List<ItemSO> GetAllItems()
    {
        List<ItemSO> result = new List<ItemSO>();
        result.AddRange(ores);
        result.AddRange(cards);
        result.AddRange(metals);
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

    #region ORES

    public static ItemSO[] GetAllOres()
    {
        return ores.ToArray();
    }

    #endregion

    #region EMTALS

    public static ItemSO[] GetAllMetals()
    {
        return metals.ToArray();
    }

    #endregion


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

    public static CardSO GetRandomCardByRarity(CardRarity rarity)
    {
        bool found = false;
        CardSO card = null;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            int rand = UnityEngine.Random.Range(0, cards.Count);

            card = cards[rand] as CardSO;

            if (card.CardRarity == rarity)
                found = true;

            tries++;
        }

        if (found)
            return card;
        return null;
    }

    public static bool DoesCardListContainRarity(List<CardSO> cards, CardRarity rarity)
    {
        foreach (var card in cards)
        {
            if (card.CardRarity == rarity)
                return true;
        }
        return false;
    }

    public static int GetRandomIndexLowestRarityCard(List<CardSO> cards)
    {
        int cardRaritiesCount = Enum.GetNames(typeof(CardRarity)).Length;
        List<int> indexes = new List<int>();

        for (int i = 0; i < cardRaritiesCount; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if ((int)cards[j].CardRarity == i)
                    indexes.Add(j);
            }

            // check only the lowest rarity in list
            if (indexes.Count > 0)
                break;
        }

        return indexes[UnityEngine.Random.Range(0, indexes.Count)];
    }

    #endregion
}
