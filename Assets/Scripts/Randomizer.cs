using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public static void Randomize<T>(List<T> items)
    {
        System.Random rand = new System.Random();

        // For each spot in the array, pick
        // a random item to swap into that spot.
        for (int i = 0; i < items.Count - 1; i++)
        {
            int j = rand.Next(i, items.Count);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
