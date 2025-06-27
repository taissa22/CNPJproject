using System;
using System.Diagnostics;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Id,nq} - {Descricao,nq}")]
    public readonly struct TipoDeOutliers : IEquatable<TipoDeOutliers>
    {
        private TipoDeOutliers(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(TipoDeOutliers other) => Id.Equals(other.Id) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is TipoDeOutliers value) && Equals(value);

        public override int GetHashCode() => HashCode.Combine(Id, Descricao);

        public static bool operator ==(TipoDeOutliers left, TipoDeOutliers right) => left.Equals(right);

        public static bool operator !=(TipoDeOutliers left, TipoDeOutliers right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoDeOutliers NAO_DEFINIDO = new TipoDeOutliers(-1, "Sem Exclusão de Outliers");
        public static TipoDeOutliers SEM_EXCLUSAO = new TipoDeOutliers(0, "Sem Exclusão de Outliers");
        public static TipoDeOutliers DESVIO_PADRAO = new TipoDeOutliers(1, "Com exclusão de Outliers por Desvio Padrão");
        public static TipoDeOutliers PERCENTUAL = new TipoDeOutliers(2, "Com exclusão de Outliers por Percentual");

        #endregion ENUM

        public static TipoDeOutliers PorId(int id)
        {
            return id switch
            {
                0 => SEM_EXCLUSAO,
                1 => DESVIO_PADRAO,
                2 => PERCENTUAL,
                _ => NAO_DEFINIDO
            };
        }
    }
}