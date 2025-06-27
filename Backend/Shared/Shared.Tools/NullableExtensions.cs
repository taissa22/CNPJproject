using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shared.Tools
{
    public static class NullableExtensions
    {
        public static string ListToItensInString<TType>(this List<TType> listaIn)
        {
            var lista = (List<TType>)listaIn;
            var join = string.Join(",", lista);
            join = $"'{join.Replace(",","','")}'";
            return join;
        }
        public static bool HasValueAndTrue(this bool? valor)
        {
            return valor.HasValue && valor.Value;
        }
        public static long GetValueOrZero(this long? valor)
        {
            return valor.HasValue && valor.Value > 0 ? valor.Value : 0;
        }
        public static bool HasValidValue(this long? valor)
        {
            return valor.HasValue && valor.Value > 0;
        }
        public static string NullableToString(this DateTime? valor)
        {
            return valor.HasValue ? valor.ToString() : "";
        }
        public static string NullableToString(this DateTime? valor, string formato)
        {
            return valor.HasValue && valor != new DateTime(0001,01,01) ? valor.Value.ToString(formato) : "";
        }
    }
}
