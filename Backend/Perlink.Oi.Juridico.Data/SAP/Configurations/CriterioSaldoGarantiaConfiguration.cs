using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Sap.Configurations
{
    public class CriterioSaldoGarantiaConfiguration : IEntityTypeConfiguration<CriterioSaldoGarantia>
    {
        public void Configure(EntityTypeBuilder<CriterioSaldoGarantia> builder)
        {
            builder.ToTable("CRITERIO_SALDO_GARANTIA", "JUR");

            builder.HasKey(pk => pk.Id).HasName("COD_CRITERIO");

            builder.Property(bi => bi.Id)
              .HasColumnName("COD_CRITERIO")
              .ValueGeneratedOnAdd()
              .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "CRIT_SALDO_GARANTIA_SEQ_01"));

            builder.Property(c => c.Id).HasColumnName("COD_CRITERIO").IsRequired(true);
            builder.Property(c => c.CodigoAgendamento).HasColumnName("COD_AGENDAMENTO");
            builder.Property(c => c.Criterio).HasColumnName("CRITERIO");
            builder.Property(c => c.Parametros).HasColumnName("PARAMETROS");
            builder.Property(c => c.ValoresParametros).HasColumnName("VALOR_PARAMETROS");
            builder.Property(c => c.NomeCriterio).HasColumnName("NOM_CRITERIO");
            builder.Property(c => c.NomeCriterioFormatado).HasColumnName("NOM_CRITERIO_FORMATADO");


            builder.HasOne(c => c.Agendamento)
                .WithMany(bb => bb.CriteriosSaldoGarantias)
                .HasForeignKey(c => c.CodigoAgendamento)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
