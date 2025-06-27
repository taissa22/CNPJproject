using Perlink.Oi.Juridico.Infra.Formatters;
using System;
using System.Diagnostics;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.ValueObjects
{
    /// <summary>
    /// Texto formatado, sem acentos, em <c>CAPs</c> e sem espaços no início e fim.
    /// </summary>
    [DebuggerDisplay("{value}")]
    public readonly struct DataString : IEquatable<DataString>
    {
        private readonly string value;

        private DataString(string value)
        {
            this.value = value.WithoutAccents().ToUpper().Trim();
        }

        public override string ToString() => value;

        public bool Equals(DataString other) => value.Equals(other.value);

        public override bool Equals(object obj) => (obj is DataString value) && Equals(value);

        public override int GetHashCode() => value.GetHashCode();

        public static bool operator ==(DataString left, DataString right) => left.Equals(right);

        public static bool operator !=(DataString left, DataString right) => !(left == right);

        public static implicit operator string(DataString value) => value.ToString();

        /// <summary>
        /// Gera uma <see cref="DataString"/> a partir de uma <see cref="string"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="DataString"/>.</param>
        /// <returns>
        /// A <see cref="DataString"/> que este método gera.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public static DataString FromString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "O 'value' não pode ser nulo");
            }

            return new DataString(value);
        }

        /// <summary>
        /// Gera uma <see cref="DataString"/> a partir de uma <see cref="string"/> se não <see langword="null"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="DataString"/>.</param>
        /// <returns>
        /// A <see cref="DataString"/> que este método gera ou <see langword="null"/>.
        /// </returns>
        public static DataString? FromNullableString(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return new DataString(value);
        }
    }
}