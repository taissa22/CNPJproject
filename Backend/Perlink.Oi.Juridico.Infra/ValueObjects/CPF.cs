using System;
using System.Diagnostics;
using Perlink.Oi.Juridico.Infra.Validators;

namespace Perlink.Oi.Juridico.Infra.ValueObjects
{
    /// <summary>
    /// Número de CPF válido, segundo as regras do SISJUR.
    /// </summary>
    [DebuggerDisplay("{value}")]
    public readonly struct CPF : IEquatable<CPF>
    {
        private readonly string value;

        private CPF(string value)
        {
            // TODO: Denilson - Validar CPF
            //this.value = value.WithoutAccents().ToUpper().Trim();
            //throw new InvalidOperationException("O 'CPF' não pôde ser gerado a partir de 'value'");
            this.value = value;
        }

        public override string ToString() => value;

        public bool Equals(CPF other) => value.Equals(other.value);

        public override bool Equals(object obj) => (obj is CPF value) && Equals(value);

        public override int GetHashCode() => value.GetHashCode();

        public static bool operator ==(CPF left, CPF right) => left.Equals(right);

        public static bool operator !=(CPF left, CPF right) => !(left == right);

        public static implicit operator string(CPF value) => value.ToString();

        /// <summary>
        /// Gera um <see cref="CPF"/> a partir de uma <see cref="string"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="CPF"/>.</param>
        /// <returns>
        /// O <see cref="CPF"/> que este método gera.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public static CPF FromString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), $"O {value} não pode ser nulo");
            }

            if (!IsValidForSisjur( value))
            {
                throw new InvalidOperationException($"O {value} não representa um CPF válido");
            }

            return new CPF(value);
        }

        /// <summary>
        /// Verifica se a <see langword="string"/> representa um <c>CPF</c> válido
        /// </summary>
        public static bool IsValid(string value)
        {        
            if (value.Length != 11)
                return false;

            if (value.Equals("00000000000") ||
                value.Equals("11111111111") ||
                value.Equals("22222222222") ||
                value.Equals("33333333333") ||
                value.Equals("44444444444") ||
                value.Equals("55555555555") ||
                value.Equals("66666666666") ||
                value.Equals("77777777777") ||
                value.Equals("88888888888") ||
                value.Equals("99999999999"))
            {
                return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;

            if (value.Length != 11)
                return false;

            tempCpf = value.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();
            if (!value.EndsWith(digito))
                return false;

            return true;
        }

        /// <summary>
        /// Verifica se a <see langword="string"/> representa um <c>CPF</c> válido nos moldes do SISJUR
        /// </summary>
        public static bool IsValidForSisjur(string value)
        {
            if (value.Equals("11111111111") || value.Equals("99999999999"))
            {
                return true;
            }

            return IsValid(value);
        }
    }
}