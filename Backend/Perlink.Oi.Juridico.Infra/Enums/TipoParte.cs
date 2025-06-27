using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Enums {
    [DebuggerDisplay("{Valor,nq} - {Descricao,nq}")]
    public readonly struct TipoParte : IEquatable<TipoParte> {
        private TipoParte(string? valor, string descricao) {
            Valor = valor;
            Descricao = descricao;
        }

        public string? Valor { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(TipoParte other) => Valor != null && Valor.Equals(other.Valor) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is TipoParte TipoParte) && Equals(TipoParte);

        public override int GetHashCode() => HashCode.Combine(Valor, Descricao);

        public static bool operator ==(TipoParte left, TipoParte right) => left.Equals(right);

        public static bool operator !=(TipoParte left, TipoParte right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoParte NAO_DEFINIDO = new TipoParte(null, "Não Definido");
        internal static TipoParte EMPRESA_DO_GRUPO = new TipoParte("E", "Empresa do Grupo");
        internal static TipoParte ESTABELECIMENTO = new TipoParte("B", "Estabelecimento");
        public static TipoParte PESSOA_FISICA = new TipoParte("F", "Pessoa Física");
        public static TipoParte PESSOA_JURIDICA = new TipoParte("J", "Pessoa Jurídica");


        #endregion ENUM

        private static IReadOnlyCollection<TipoParte> Todos { get; } = new[] { NAO_DEFINIDO, EMPRESA_DO_GRUPO, ESTABELECIMENTO, PESSOA_FISICA, PESSOA_JURIDICA };

        public static TipoParte PorValor(string valor) => Todos.Where(x => x.Valor == valor).DefaultIfEmpty(new TipoParte(valor, valor)).Single();
    }
}
