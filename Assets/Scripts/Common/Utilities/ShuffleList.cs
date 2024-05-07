using System.Collections.Generic;

public static class ShuffleList
{
    public static List<T> OfItems<T>(List<T> items)
    {
        List<T> randomList = new List<T>();

        int randomIndex = 0;
        while (items.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, items.Count);
            randomList.Add(items[randomIndex]);
            items.RemoveAt(randomIndex);
        }

        return randomList;
    }
}
