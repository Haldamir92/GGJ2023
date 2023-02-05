using System.Collections.Generic;
using UnityEngine;

public static class IListExtensionMethods
{
    /// <summary>
    /// It shuffles the elements inside the IList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        int j;
        for (int i = n - 1; i > 0; i--)
        {
            j = Random.Range(0, i + 1);
            T k = list[j];
            list[j] = list[i];
            list[i] = k;
        }
    }

    /// <summary>
    /// It returns a random element of the IList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomElement<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
