using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    private static readonly System.Random _random = new System.Random();

    /// <summary>
    /// Select an item from the given list according to their respective weights.
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="weightSelector"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Index of randomly picked item.</returns>
    public static int PickWeightedElementIndex(this List<float> list)
    {
        if (list.Any() == false)
        {
            return -1;
        }
        if (list.Count == 1)
        {
            return 0;
        }
        float totalWeight = list.Sum();
        if (totalWeight == 0)
        {
            return _random.Next(list.Count);
        }

        float itemWeightIndex = (float)_random.NextDouble() * totalWeight;
        float currentWeightIndex = 0;

        int index = 0;
        foreach (float weightedItem in list)
        {
            float weight = weightedItem;
            currentWeightIndex += weight;
            if (currentWeightIndex >= itemWeightIndex)
            {
                return index;
            }

            index++;
        }

        return default;
    }
}
