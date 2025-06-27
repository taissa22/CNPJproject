using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Id,nq} - {Descricao,nq}")]
    public readonly struct TipoProcessoManutencao : IEquatable<TipoProcessoManutencao>
    {
        private TipoProcessoManutencao(double id, string descricao)
        {
            Id = id;
            Descricao = descricao;           
        }

        public double Id { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(TipoProcessoManutencao other) => Id.Equals(other.Id) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is TipoProcessoManutencao tipoProcesso) && Equals(tipoProcesso);

        public override int GetHashCode() => HashCode.Combine(Id, Descricao);

        public static bool operator == (TipoProcessoManutencao left, TipoProcessoManutencao right) => left.Equals(right);

        public static bool operator != (TipoProcessoManutencao left, TipoProcessoManutencao right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoProcessoManutencao NAO_DEFINIDO = new TipoProcessoManutencao(-1, "Não Definido");

        public static TipoProcessoManutencao CIVEL_CONSUMIDOR = new TipoProcessoManutencao(1, "Cível Consumidor");
        public static TipoProcessoManutencao TRABALHISTA = new TipoProcessoManutencao(2, "Trabalhista");
        public static TipoProcessoManutencao ADMINISTRATIVO = new TipoProcessoManutencao(3, "Administrativo");
        public static TipoProcessoManutencao TRIBUTARIO_ADMINISTRATIVO = new TipoProcessoManutencao(4, "Tributário Administrativo");
        public static TipoProcessoManutencao TRIBUTARIO_JUDICIAL = new TipoProcessoManutencao(5, "Tributário Judicial");
        public static TipoProcessoManutencao TRABALHISTA_ADMINISTRATIVO = new TipoProcessoManutencao(6, "Trabalhista Administrativo");
        public static TipoProcessoManutencao JEC = new TipoProcessoManutencao(7, "Juizado Especial Cível");
        public static TipoProcessoManutencao CIVEL_ESTRATEGICO = new TipoProcessoManutencao(9, "Cível Estratégico");
        public static TipoProcessoManutencao CIVEL_ADMINISTRATIVO = new TipoProcessoManutencao(12, "Cível Administrativo");
        public static TipoProcessoManutencao CRIMINAL_ADMINISTRATIVO = new TipoProcessoManutencao(14, "Criminal Administrativo");
        public static TipoProcessoManutencao CRIMINAL_JUDICIAL = new TipoProcessoManutencao(15, "Criminal Judicial");
        public static TipoProcessoManutencao PROCON = new TipoProcessoManutencao(17, "Procon");
        public static TipoProcessoManutencao PEX_CONSUMIDOR = new TipoProcessoManutencao(18.1, "Pex Cível Consumidor");
        public static TipoProcessoManutencao PEX_JUIZADO = new TipoProcessoManutencao(18.2, "Pex Juizado");

        #endregion ENUM

        private static IReadOnlyCollection<TipoProcessoManutencao> Todos { get; } = new[] { NAO_DEFINIDO, CIVEL_CONSUMIDOR, TRABALHISTA, ADMINISTRATIVO, TRIBUTARIO_ADMINISTRATIVO, TRIBUTARIO_JUDICIAL, TRABALHISTA_ADMINISTRATIVO, JEC, CIVEL_ESTRATEGICO,  CIVEL_ADMINISTRATIVO, CRIMINAL_ADMINISTRATIVO, CRIMINAL_JUDICIAL, PROCON, PEX_CONSUMIDOR, PEX_JUIZADO };

        public static TipoProcessoManutencao PorId(double id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new TipoProcessoManutencao(id, "Não Encontrado")).Single();
    }
}
