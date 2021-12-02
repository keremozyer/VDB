using System;
using System.Collections.Generic;
using System.Linq;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !(enumerable?.Any()).GetValueOrDefault();
        }

        public static bool HasElements<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable?.Any()).GetValueOrDefault();
        }

        public static IEnumerable<T> ConcatSafe<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            if (firstEnumerable.IsNullOrEmpty() && secondEnumerable.IsNullOrEmpty()) return null;
            if (firstEnumerable.IsNullOrEmpty()) return secondEnumerable;
            if (secondEnumerable.IsNullOrEmpty()) return firstEnumerable;

            return firstEnumerable.Concat(secondEnumerable);
        }

        public static IEnumerable<T> ExceptSafe<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            return secondEnumerable.IsNullOrEmpty() ? firstEnumerable : firstEnumerable.Except(secondEnumerable);
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, Func<T, object> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.GroupBy(selector).Select(g => g.First());
        }

        public static IEnumerable<T> IntersectSafe<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            if (firstEnumerable.IsNullOrEmpty() || secondEnumerable.IsNullOrEmpty()) return null;

            return firstEnumerable.Intersect(secondEnumerable);
        }
    }
}
