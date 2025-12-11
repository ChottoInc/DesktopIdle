using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsGeneral
{
    public struct MyColors
    {
        public static Color CommonRarity
        {
            get
            {
                return new Color(255f / 255f, 195f / 255f, 95f / 255f, 1f);
            }
        }

        public static Color UncommonRarity
        {
            get
            {
                return new Color(96f / 255f, 180f / 255f, 255f / 255f, 1f);
            }
        }

        public static Color RareRarity
        {
            get
            {
                return new Color(255f / 255f, 125f / 255f, 95f / 255f, 1f);
            }
        }
    }


    public static Color GetColorByRarity(UtilsItem.CardRarity rarity)
    {
        switch(rarity)
        {
            default:
            case UtilsItem.CardRarity.Common: return MyColors.CommonRarity;
            case UtilsItem.CardRarity.Uncommon: return MyColors.UncommonRarity;
            case UtilsItem.CardRarity.Rare: return MyColors.RareRarity;
        }
    }


    [System.Serializable]
    public struct GeneralChances<T>
    {
        public T value;
        public int chanches;
    }

    public static T GetRandomValueFromGeneralChanches<T>(GeneralChances<T>[] array)
    {
        float randValue = Random.value;
        float tempSumChance = 0;

        T result = default;

        for (int i = 0; i < array.Length; i++)
        {
            tempSumChance += (float)array[i].chanches / 100f;
            if (randValue <= tempSumChance)
            {
                result = array[i].value;
                break;
            }
        }

        return result;
    }
}
