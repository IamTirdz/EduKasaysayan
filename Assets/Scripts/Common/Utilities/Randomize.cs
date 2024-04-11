using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize
{
    public static int Index(int maxIndex, List<int> lastIndex)
    {
        int currentIndex = UnityEngine.Random.Range(0, maxIndex);
        while (lastIndex.Contains(currentIndex))
        {
            currentIndex = UnityEngine.Random.Range(0, maxIndex);
        }

        lastIndex.Add(currentIndex);
        return currentIndex;
    }
}
