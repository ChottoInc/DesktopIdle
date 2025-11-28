using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsGeneral
{
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
