using System;

namespace Shared.Tools
{
    public static class IntExtensions
    {
        public static bool IsNumeric(this string valor)
        {
            return double.TryParse(valor, out _);
        }
        /// <summary>
        /// Retorna string com valor "S" caso o valor inteiro seja igual a 1, 
        /// caso contrário retorna "N"
        /// </summary>
        /// <returns>string</returns>
        public static string Retorna_S_N(this int valor)
        {
            return valor == 1 ? "S" : "N";
        }
        /// <summary>
        /// Retorna string com valor "S" caso o valor inteiro seja igual a 1 ou 2, 
        /// caso contrário retorna "N"
        /// </summary>
        /// <returns>string</returns>
        public static string HasValor_S_N(this int valor)
        {
            return valor == 1 || valor == 2 ? "S" : "N";
        }

        public static string FormatarValorComAspas(this long? valor)
        {
            var retorno = "";
            if (valor != null)
            {
                retorno = string.Concat("'", valor);
            }
            return retorno;
        }

        public static string FormatarValorComAspas(this long valor)
        {
            var retorno = "";
            if (valor >= 0)
            {
                retorno = string.Concat("'", valor);
            }
            return retorno;
        }

        public static string FormatarValorComAspas(this string valor)
        {
            var retorno = "";
            if (!String.IsNullOrEmpty(valor))
            {
                retorno = string.Concat("'", valor);
            }
            return retorno;
        }
    }
}
