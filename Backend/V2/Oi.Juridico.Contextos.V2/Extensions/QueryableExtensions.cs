using System;
using System.Linq;
using System.Linq.Expressions;

namespace Oi.Juridico.Contextos.V2.PermissaoContext.Extensions
{
    public static class QueryableExtensions
    {
        public static T ConverterOuPadrao<T>(string value, T defaultValue) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out T result))
            {
                return result;
            }
            return defaultValue;
        }

        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, object? value)
        {
            if (value is null)
            {
                return source;
            }

            return source.Where(predicate);
        }

        public static IQueryable<T> WhereIfNotEmpty<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return source;
            }

            return source.Where(predicate);
        }
 
        public static IQueryable<TSource> Pagina<TSource>(this IQueryable<TSource> source, int pagina, int tamanhoPagina)
        {
            return source.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina);
        }
    }
}
