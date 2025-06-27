using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Shared.Tools
{
    public static class StringExtensions
    {
        public static string FormatarCNPJ(this string cnpj)
        {
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormataCPF(this string cpf)
        {
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
        public static string FormataCPF_CNPJ(this string valor)
        {
            if (valor.Length == 11)
                return Convert.ToUInt64(valor).ToString(@"000\.000\.000\-00");
            else
                return Convert.ToUInt64(valor).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormataNumeroProcesso(this string valor)
        {
            return Convert.ToUInt64(valor).ToString(@"0000000\-00\.0000\.0\.00\.0000");
        }
        public static string FormataRG(this string rg)
        {
            return Convert.ToUInt64(rg).ToString(@"00\.000\.000\-0");
        }

        public static string FormataCEP(this string cep)
        {
            return Convert.ToUInt64(cep).ToString(@"00000-000");
        }

        public static string FormataValor(this decimal valor)
        {
            return valor.ToString("N2");
        }

        public static string FormataValor(this string valor)
        {
            if (!valor.Contains("/") && decimal.TryParse(valor.Replace(".", ","), out decimal v))
                return v.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"));
            return valor;
        }

        public static string TrocaVirgulaParaPonto(this string valor)
        {
            string v = valor.Replace(",", ".");
            return v;
        }

        public static string FormataSimOuNao(this bool? valor)
        {
            if (!valor.HasValue) return String.Empty;

            return valor.Value ? "Sim" : "Não";
        }

        public static string RemoverCaracteres(this string conteudo)
        {
            Regex soNumeros = new Regex(@"[^\d]");

            return soNumeros.Replace(conteudo, "");
        }

        public static bool IsString(this string texto)
        {
            return object.ReferenceEquals(texto.GetType(), "".GetType());
        }

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static string FormataValorComMoeda(this string valor)
        {
            if (!valor.Contains("/") && decimal.TryParse(valor.Replace(".", ","), out decimal v))
                return v.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            return valor;
        }
    }
}
