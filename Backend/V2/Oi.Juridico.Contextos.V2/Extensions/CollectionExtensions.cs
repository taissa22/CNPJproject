using System;
using System.Collections.Generic;
using System.Linq;

namespace Oi.Juridico.Contextos.V2.Extensions
{
    public static class CollectionExtensions
    {
        public static void RemoveAll<T>(this ICollection<T> source,
                                    Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (predicate == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            source.Where(predicate).ToList().ForEach(e => source.Remove(e));
        }

        public static void AddAll<T>(this ICollection<T> source,
                                    List<T> list)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (list == null)
                throw new ArgumentNullException("list", "list is null.");

            list.ForEach(e => source.Add(e));
        }

        public static void AddAll<T>(this ICollection<T> source,
                                    T[] list)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (list == null)
                throw new ArgumentNullException("list", "list is null.");

            list.ToList().ForEach(e => source.Add(e));
        }
    }
}
