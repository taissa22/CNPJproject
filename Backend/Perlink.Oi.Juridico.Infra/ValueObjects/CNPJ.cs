using Perlink.Oi.Juridico.Infra.Validators;
using System;
using System.Diagnostics;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.ValueObjects
{
    /// <summary>
    /// Número de CNPJ válido, segundo as regras do SISJUR.
    /// </summary>
    [DebuggerDisplay("{value}")]
    public readonly struct CNPJ : IEquatable<CNPJ>
    {
        private readonly string value;

        private CNPJ(string value)
        {
            // TODO: Denilson - Validar CNPJ
            //this.value = value.WithoutAccents().ToUpper().Trim();
            //throw new InvalidOperationException("O 'CNPJ' não pôde ser gerado a partir de 'value'");

            if (!IsValidForSisjur(value))
            {
                throw new InvalidOperationException($"O valor '{value}' não representa um CNPJ válido");
            }

            this.value = value;
        }

        public override string ToString() => value;

        public bool Equals(CNPJ other) => value.Equals(other.value);

        public override bool Equals(object obj) => (obj is CNPJ value) && Equals(value);

        public override int GetHashCode() => value.GetHashCode();

        public static bool operator ==(CNPJ left, CNPJ right) => left.Equals(right);

        public static bool operator !=(CNPJ left, CNPJ right) => !(left == right);

        public static implicit operator string(CNPJ value) => value.ToString();

        /// <summary>
        /// Gera um <see cref="CNPJ"/> a partir de uma <see cref="string"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="CNPJ"/>.</param>
        /// <returns>
        /// O <see cref="CNPJ"/> que este método gera.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public static CNPJ FromString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), $"O CNPJ informado não pode ser nulo");
            }

            if (!IsValidForSisjur(value))
            {
                throw new InvalidOperationException($"O valor '{value}' não representa um CNPJ válido");
            }

            return new CNPJ(value);
        }

        /// <summary>
        /// Gera um <see cref="CNPJ"/> a partir de uma <see cref="string"/> se não <see langword="null"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> a ser representada como <see cref="CNPJ"/>.</param>
        /// <returns>
        /// A <see cref="CNPJ"/> que este método gera ou <see langword="null"/>.
        /// </returns>
        public static CNPJ? FromNullableString(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return new CNPJ(value);
        }

        /// <summary>
        /// Verifica se a <see langword="string"/> representa um <c>CNPJ</c> válido
        /// </summary>
        public static bool IsValid(string value)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            if (value.Length != 14)
                return false;

            if (value.Equals("00000000000000") ||
                value.Equals("11111111111111") ||
                value.Equals("22222222222222") ||
                value.Equals("33333333333333") ||
                value.Equals("44444444444444") ||
                value.Equals("55555555555555") ||
                value.Equals("66666666666666") ||
                value.Equals("77777777777777") ||
                value.Equals("88888888888888") ||
                value.Equals("99999999999999"))
            {
                return false;
            }

            tempCnpj = value.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return value.EndsWith(digito);
        }

        /// <summary>
        /// Verifica se a <see langword="string"/> representa um <c>CNPJ</c> válido nos moldes do SISJUR
        /// </summary>
        public static bool IsValidForSisjur(string value)
        {
            if (value.Equals("11111111111111") || value.Equals("99999999999999"))
            {
                return true;
            }
            return IsValid(value);
        }
        public static string FormatCNPJ(string CNPJ)
        {
            if (String.IsNullOrEmpty(CNPJ)  || CNPJ.Length < 14 ) return "";  
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }
        public static string FormatCPF(string CPF)
        {
            if (String.IsNullOrEmpty(CPF)   || CPF.Length < 11) return "";
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }


    }
}
