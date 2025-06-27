using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Valor,nq} - {Descricao,nq}")]
    public readonly struct TipoOrgao : IEquatable<TipoOrgao>
    {
        private TipoOrgao(string valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public string Valor { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(TipoOrgao other) => Valor.Equals(other.Valor) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is TipoOrgao TipoParte) && Equals(TipoParte);

        public override int GetHashCode() => HashCode.Combine(Valor, Descricao);

        public static bool operator ==(TipoOrgao left, TipoOrgao right) => left.Equals(right);

        public static bool operator !=(TipoOrgao left, TipoOrgao right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoOrgao NAO_DEFINIDO = new TipoOrgao("", "Não Definido");

        public static TipoOrgao CIVEL_ADMINISTRATIVO = new TipoOrgao("1", "Orgão Cível Administrativo");
        public static TipoOrgao CRIMINAL_ADMINISTRATIVO = new TipoOrgao("2", "Orgão Criminal Administrativo");
        public static TipoOrgao DEMAIS_TIPOS = new TipoOrgao("O", "Demais Orgãos");

        #endregion ENUM

        private static IReadOnlyCollection<TipoOrgao> Todos { get; } = new[] { NAO_DEFINIDO, CIVEL_ADMINISTRATIVO, CRIMINAL_ADMINISTRATIVO, DEMAIS_TIPOS };

        public static TipoOrgao PorValor(string valor) => Todos.Where(x => x.Valor == valor).DefaultIfEmpty(new TipoOrgao(valor, "Não Identificado")).Single();

        public static bool IsValid(string value)
        {
            return Todos.Any(x => x.Valor == value);
        }
    }
}