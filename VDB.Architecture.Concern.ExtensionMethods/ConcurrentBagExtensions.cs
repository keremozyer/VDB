using System.Collections.Concurrent;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class ConcurrentBagExtensions
    {
        public static void AddRangeSafe<T>(this ConcurrentBag<T> source, ConcurrentBag<T> elementsToAdd)
        {
            if (elementsToAdd.IsNullOrEmpty()) return;
            source.AddRange(elementsToAdd);
        }

        public static void AddRange<T>(this ConcurrentBag<T> source, ConcurrentBag<T> elementsToAdd)
        {
            foreach (T item in elementsToAdd)
            {
                source.Add(item);
            }
        }
    }
}
