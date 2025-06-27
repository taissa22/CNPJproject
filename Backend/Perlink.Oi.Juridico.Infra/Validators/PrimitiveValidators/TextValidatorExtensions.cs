using System.Text.RegularExpressions;

namespace Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators
{
    public static class TextValidatorExtensions
    {
        public static bool HasMinLength(this string value, int length)
        {
            return length <= value.Length;
        }

        public static bool HasMaxLength(this string value, int length)
        {
            return length >= value.Length;
        }

        public static bool HasLengthBetween(this string value, int minLength, int maxLength)
        {
            return value.HasMinLength(minLength) && value.HasMaxLength(maxLength);
        }

        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^\d+$");
        }
    }
}