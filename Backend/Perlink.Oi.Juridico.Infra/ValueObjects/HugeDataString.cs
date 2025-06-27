using Perlink.Oi.Juridico.Infra.Formatters;
using System;
using System.Diagnostics;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.ValueObjects
{
    /// <summary>
    /// Texto formatado, sem acentos, em <c>CAPs</c> e sem espaços no início e fim, com limite de 4.000 caracteres.
    /// </summary>
    [DebuggerDisplay("{value}")]
    public readonly struct HugeDataString : IEquatable<HugeDataString>
    {
        private readonly string value;

        private HugeDataString(string value)
        {
            string finalValue = value.WithoutAccents().ToUpper().Trim();
            this.value = finalValue.WithoutAccents().ToUpper().Trim().Substring(0, Math.Min(4_000, finalValue.Length));
        }

        public override string ToString() => value;

        public bool Equals(HugeDataString other) => value.Equals(other.value);

        public override bool Equals(object obj) => (obj is HugeDataString value) && Equals(value);

        public override int GetHashCode() => value.GetHashCode();

        public static bool operator ==(HugeDataString left, HugeDataString right) => left.Equals(right);

        public static bool operator !=(HugeDataString left, HugeDataString right) => !(left == right);

        public static implicit operator string(HugeDataString value) => value.ToString();

        /// <summary>
        /// Gera uma <see cref="HugeDataString"/> a partir de uma <see cref="string"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="HugeDataString"/>.</param>
        /// <returns>
        /// A <see cref="HugeDataString"/> que este método gera.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public static HugeDataString FromString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "O 'value' não pode ser nulo");
            }

            return new HugeDataString(value);
        }

        /// <summary>
        /// Gera uma <see cref="HugeDataString"/> a partir de uma <see cref="string"/> se não <see langword="null"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="HugeDataString"/>.</param>
        /// <returns>
        /// A <see cref="HugeDataString"/> que este método gera ou <see langword="null"/>.
        /// </returns>
        public static HugeDataString? FromNullableString(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return new HugeDataString(value);
        }
    }
}
