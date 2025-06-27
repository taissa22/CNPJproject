using System.Globalization;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Formatters
{
    public static class StringFormatterExtensions
    {
        public static string WithoutAccents(this string @string)
        {
            return new string(@string
                .Normalize(NormalizationForm.FormD)
                .Where(x => CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static string Padronizar(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.ToUpper().Trim();
        }
    }
}