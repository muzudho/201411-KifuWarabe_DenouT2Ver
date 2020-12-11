using System.Collections.Generic;

namespace Grayscale.Kifuwarane.Entities.Misc
{
    public abstract class ShuffleLib<T>
    {
        public static void Shuffle_FisherYates(ref List<T> items)
        {
            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = RandomLib.Random.Next(n + 1);
                T tmp = items[k];
                items[k] = items[n];
                items[n] = tmp;
            }
        }
    }
}
