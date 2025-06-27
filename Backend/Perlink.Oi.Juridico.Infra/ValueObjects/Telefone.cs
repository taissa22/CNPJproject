using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Diagnostics;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.ValueObjects
{
    /// <summary>
    /// Telefone Completo, com Área + Número.
    /// </summary>
    [DebuggerDisplay("{ToString, nq}")]
    public readonly struct Telefone : IEquatable<Telefone>
    {
        private Telefone(string value)
        {
            if (!Telefone.IsValid(value))
            {
                throw new InvalidOperationException("O 'value' não representa um Telefone válido");
            }
            this.Area = value.Substring(0, 2);
            this.Numero = value.Substring(2);
        }

        public readonly string Area { get; }
        public readonly string Numero { get; }

        public override string ToString() => $"({Area}) {Numero}";

        public bool Equals(Telefone other) => Area.Equals(other.Area) && Numero.Equals(other.Numero);

        public override bool Equals(object obj) => (obj is Telefone value) && Equals(value);

        public override int GetHashCode() => HashCode.Combine(Area, Numero);

        public static bool operator ==(Telefone left, Telefone right) => left.Equals(right);

        public static bool operator !=(Telefone left, Telefone right) => !(left == right);

        /// <summary>
        /// Gera um <see cref="Telefone"/> a partir de uma <see cref="string"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="Telefone"/>.</param>
        /// <returns>
        /// A <see cref="Telefone"/> que este método gera.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public static Telefone FromString(string value)
        {
            if (value is null || string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "O 'value' não pode ser nulo");
            }

            return new Telefone(value);
        }

        /// <summary>
        /// Gera um <see cref="Telefone"/> a partir de uma <see cref="string"/> se não <see langword="null"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="Telefone"/>.</param>
        /// <returns>
        /// A <see cref="Telefone"/> que este método gera ou <see langword="null"/>.
        /// </returns>
        public static Telefone? FromNullableString(string? value)
        {
            if (value is null || string.IsNullOrEmpty(value))
            {
                return null;
            }

            return new Telefone(value);
        }

        public static bool IsValid(string? telefone)
        {
            if (string.IsNullOrEmpty(telefone))
            {
                return false;
            }

            if (!telefone.IsNumeric())
            {
                return false;
            }

            if (!telefone.HasLengthBetween(10, 11))
            {
                return false;
            }

            return true;
        }
    }
}