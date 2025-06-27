using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FechamentoProvisaoTrabalhista : Notifiable, IEntity, INotifiable
    {
        // EF Requires
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

        private FechamentoProvisaoTrabalhista()
        {
        }

#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

        public int Id { get; private set; }
        public DateTime DataFechamento { get; private set; }
        public int NumeroDeMeses { get; private set; }

        //IND_MENSAL                    CHAR(1 BYTE)        No  'N'
        //public bool Mensal { get; private set; }

        //DATA_IND_MENSAL               DATE                Yes
        //public DateTime? DataMensal { get; private set; }

        //IND_BASE_GERADA               CHAR(1 BYTE)        No
        //public bool GeraBase { get; private set; }

        //DATA_GERACAO                  DATE                No
        //public DateTime DataGeracao { get; private set; }

        //USR_COD_USUARIO               VARCHAR2(30 BYTE)   No
        //public string Usuario { get; private set; }

        //EMPCE_COD_EMP_CENTRALIZADORA  NUMBER(4,0)         Yes
        public int? EmpresaCentralizadoraId { get; private set; }

        public EmpresaCentralizadora? EmpresaCentralizadora { get; private set; }

        //COD_TIPO_OUTLIER              NUMBER(2,0)         No
        public int TipoDeOutlierId { get; private set; }

        public TipoDeOutliers TipoDeOutliers => TipoDeOutliers.PorId(TipoDeOutlierId);

        //VAL_AJUSTE_DESVIO_PADRAO      NUMBER(5,2)         Yes
        public decimal? AjusteDesvioPadrao { get; private set; }

        //VAL_PERCENT_PROC_OUTLIERS     NUMBER(5,2)         Yes
        public decimal? PercentualOutliers { get; private set; }
    }
}