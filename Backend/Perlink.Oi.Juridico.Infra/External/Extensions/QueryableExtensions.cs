using System;
using System.Linq;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.External
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool conditional)
        {
            if (conditional)
            {
                return source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, object? value)
        {
            if (value is null)
            {
                return source;
            }

            return source.Where(predicate);
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a key and sort type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="Tkey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to sort.</param>
        /// <param name="keySelector">The type of the key returned by the function that is represented by keySelector.</param>
        /// <param name="ascending">The sort type indicator</param>
        /// <returns>
        /// A <see cref="ISorteredQueryable{TSource}"/> whose elements are sorted according to a key and sort type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or keySelector is null
        /// </exception>
        public static ISorteredQueryable<TSource> SortBy<TSource, Tkey>(this IQueryable<TSource> source, Expression<Func<TSource, Tkey>> keySelector, bool ascending)
        {
            if (ascending)
            {
                return new SorteredQueryable<TSource>(source.OrderBy(keySelector));
            }

            return new SorteredQueryable<TSource>(source.OrderByDescending(keySelector));
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence according to a key and sort type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="Tkey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to sort.</param>
        /// <param name="keySelector">The type of the key returned by the function that is represented by keySelector.</param>
        /// <param name="ascending">The sort type indicator</param>
        /// <returns>
        /// A <see cref="ISorteredQueryable{TSource}"/> whose elements are sorted according to a key and sort type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// source or keySelector is null
        /// </exception>
        public static IQueryable<TSource> ThenSortBy<TSource, Tkey>(this ISorteredQueryable<TSource> source, Expression<Func<TSource, Tkey>> keySelector, bool ascending)
        {
            if (ascending)
            {
                return source.ThenBy(keySelector);
            }

            return source.ThenByDescending(keySelector);
        }
    }
}
