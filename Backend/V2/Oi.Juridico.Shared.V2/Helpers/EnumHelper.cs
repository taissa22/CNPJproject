using System;
using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns the text description for a enum value, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="value">Enum value.</param>
        /// <returns>Text description.</returns>
        public static string GetEnumText(Enum value)
        {
            return GetEnumText(value.GetType(), value.ToString());
        }

        /// <summary>
        /// Returns the text description for a enum value, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="type">Enum type to be used.</param>
        /// <param name="value">Enum value.</param>
        /// <returns>Text description.</returns>
        public static string GetEnumText(Type type, int value)
        {
            var name = Enum.GetName(type, value);
            return GetEnumText(type, name);
        }

        /// <summary>
        /// Returns the text description for a enum name, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="type">Enum type to be used.</param>
        /// <param name="name">Enum name.</param>
        /// <returns>Text description.</returns>
        public static string GetEnumText(Type type, string name)
        {
            var fi = type.GetField(name);
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                                             (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : name;
        }

    }
}
