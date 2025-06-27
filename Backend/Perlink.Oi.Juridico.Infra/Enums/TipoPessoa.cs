using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Valor,nq} - {Descricao,nq}")]
    public readonly struct TipoPessoa : IEquatable<TipoPessoa>
    {
        private TipoPessoa(string valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public string Valor { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(TipoPessoa other) => Valor != null && Valor.Equals(other.Valor) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is TipoPessoa TipoParte) && Equals(TipoParte);

        public override int GetHashCode() => HashCode.Combine(Valor, Descricao);

        public static bool operator ==(TipoPessoa left, TipoPessoa right) => left.Equals(right);

        public static bool operator !=(TipoPessoa left, TipoPessoa right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoPessoa NAO_DEFINIDO = new TipoPessoa(null, "Não Definido");
        public static TipoPessoa PESSOA_FISICA = new TipoPessoa("F", "Pessoa Física");
        public static TipoPessoa PESSOA_JURIDICA = new TipoPessoa("J", "Pessoa Jurídica");

        #endregion ENUM

        private static IReadOnlyCollection<TipoPessoa> Todos { get; } = new[] { NAO_DEFINIDO, PESSOA_FISICA, PESSOA_JURIDICA };

        public static TipoPessoa PorValor(string valor) => Todos.Where(x => x.Valor == valor).DefaultIfEmpty(new TipoPessoa(valor, valor)).Single();
    }
}