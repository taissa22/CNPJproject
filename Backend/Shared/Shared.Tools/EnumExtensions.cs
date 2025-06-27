using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Shared.Tools
{
    public static class EnumExtensions
    {
        public static Dictionary<string, string> ToDictionary<TEnum>() where TEnum : struct
        {
            return System.Enum.GetValues(typeof(TEnum))
                .Cast<System.Enum>()
                .Select(item => new
                {
                    key = (int)(object)item,
                    value = item.Descricao()
                })
                .OrderBy(x => x.value)
                .ToDictionary(x => x.key.ToString(), x => x.value)
                ;
        }
        /// <summary>
        /// Busca o valor informado em um Array do tipo do Enum e retorna a sua descrição
        /// </summary>
        /// <param name="valor" </param>
        /// <returns>string com a descrição do valor</returns>
        public static string GetDescricaoFromValue<TEnum>(long valor) where TEnum : struct
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)).Cast<Enum>())
            {
                if (item.GetHashCode() == valor)
                    return item.Descricao();
            }
            return "Descrição não definida";
        }
        /// <summary>
        /// Busca o valor informado em um Array do tipo do Enum e retorna o seu nome
        /// </summary>
        /// <param name="valor" </param>
        /// <returns>string com o nome do valor</returns>
        public static string GetNomeFromValue<TEnum>(long valor) where TEnum : struct
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)).Cast<Enum>())
            {
                if (item.GetHashCode() == valor)
                    return item.ToString();
            }
            return "";
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return System.Enum.TryParse<T>(value, true, out T result) ? result : defaultValue;
        }

        public static string Descricao(this System.Enum @enum)
        {
            if (@enum == null)
                return string.Empty;

            var enumItem = @enum.GetType()
                            .GetMember(@enum.ToString())
                            .FirstOrDefault();
            if (enumItem == null)
                return string.Empty;
            var attribute = enumItem.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attribute == null || attribute.Length == 0)
                return @enum.ToString();

            return (attribute[0] as DescriptionAttribute).Description;
        }

        public static int ValorNumerico(this System.Enum @enum)
        {
            return Convert.ToInt16(@enum);
        }

        public static string Valor(this System.Enum @enum)
        {
            return Convert.ToInt16(@enum).ToString();
        }

        public static TEnum ToEnum<TEnum>(this string strEnumValue)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }
    }
}
