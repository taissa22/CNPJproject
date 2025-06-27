using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations {
    public class CentroCustoConfiguration : IEntityTypeConfiguration<CentroCusto> {
        public void Configure(EntityTypeBuilder<CentroCusto> builder) {

            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CENTRO_CUSTO", "JUR");

            builder.HasKey(c => c.Id)
                .HasName("COD_CENTRO_CUSTO");

            builder.Property(c => c.Id)
                   .IsRequired()
                   .HasColumnName("COD_CENTRO_CUSTO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("CENTRO_CUSTO"));

            builder.Property(c => c.Descricao)
                .HasColumnName("DSC_CENTRO_CUSTO")
                .HasMaxLength(100);

           builder.Property(c => c.CodigoCentroCustoSAP)
                .HasColumnName("COD_CENTRO_CUSTO_SAP")
                .HasMaxLength(10);

            builder.Property(c => c.IndicaAtivo)
                .HasColumnName("IND_ATIVO")
                .HasConversion(boolConverter);
        }
    }
}
