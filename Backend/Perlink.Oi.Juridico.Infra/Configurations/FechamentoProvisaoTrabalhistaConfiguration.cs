using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoProvisaoTrabalhistaConfiguration : IEntityTypeConfiguration<FechamentoProvisaoTrabalhista>
    {
        public void Configure(EntityTypeBuilder<FechamentoProvisaoTrabalhista> builder)
        {
            builder.ToTable("FECHAMENTOS_PROVISOES_TRAB", "JUR");

            // CREATE UNIQUE INDEX "JUR"."FPTR_UK_01" ON "JUR"."FECHAMENTOS_PROVISOES_TRAB" ("DATA_FECHAMENTO", "EMPCE_COD_EMP_CENTRALIZADORA", "NUM_MESES_MEDIA_HISTORICA", "COD_TIPO_OUTLIER")

            //ID                            NUMBER              No
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");

            //DATA_FECHAMENTO               DATE                No
            builder.Property(x => x.DataFechamento).HasColumnName("DATA_FECHAMENTO");

            //NUM_MESES_MEDIA_HISTORICA     NUMBER(3,0)         No
            builder.Property(x => x.NumeroDeMeses).HasColumnName("NUM_MESES_MEDIA_HISTORICA");

            //IND_MENSAL                    CHAR(1 BYTE)        No  'N'
            //builder.Property(x => x.Mensal).HasColumnName("IND_MENSAL")
            //        .HasConversion(ValueConverters.BoolToString);

            //DATA_IND_MENSAL               DATE                Yes
            //public DateTime? DataMensal { get; private set; }

            //IND_BASE_GERADA               CHAR(1 BYTE)        No
            //public bool GeraBase { get; private set; }

            //DATA_GERACAO                  DATE                No
            //public DateTime DataGeracao { get; private set; }

            //USR_COD_USUARIO               VARCHAR2(30 BYTE)   No
            //builder.Property(x => x.Usuario).HasColumnName("USR_COD_USUARIO");

            //EMPCE_COD_EMP_CENTRALIZADORA  NUMBER(4,0)         Yes
            builder.Property(x => x.EmpresaCentralizadoraId).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA");
            builder.HasOne(x => x.EmpresaCentralizadora).WithMany().HasForeignKey(x => x.EmpresaCentralizadoraId);
            //public EmpresaCentralizadora EmpresaCentralizadora { get; private set; }

            //COD_TIPO_OUTLIER              NUMBER(2,0)         No
            builder.Property(x => x.TipoDeOutlierId).HasColumnName("COD_TIPO_OUTLIER");

            //VAL_AJUSTE_DESVIO_PADRAO      NUMBER(5,2)         Yes
            builder.Property(x => x.AjusteDesvioPadrao).HasColumnName("VAL_AJUSTE_DESVIO_PADRAO");

            //VAL_PERCENT_PROC_OUTLIERS     NUMBER(5,2)         Yes
            builder.Property(x => x.PercentualOutliers).HasColumnName("VAL_PERCENT_PROC_OUTLIERS");

            builder
                .HasIndex(x => new { x.DataFechamento, x.EmpresaCentralizadoraId, x.NumeroDeMeses, x.TipoDeOutlierId })
                .IsUnique(true);
        }
    }
}