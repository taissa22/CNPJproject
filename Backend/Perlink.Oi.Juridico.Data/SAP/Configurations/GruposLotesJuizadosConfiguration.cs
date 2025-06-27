using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class GruposLotesJuizadosConfiguration : IEntityTypeConfiguration<GruposLotesJuizados>
    {
        public void Configure(EntityTypeBuilder<GruposLotesJuizados> builder)
        {
            builder.ToTable("GRUPOS_LOTES_JUIZADOS", "JUR");

            builder.HasKey(pk => pk.Id).HasName("CODIGO");

            builder.Property(c => c.Id)
                   .HasColumnName("CODIGO")
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("GRUPOS_LOTES_JUIZADOS"));

            builder.Property(c => c.Descricao).HasColumnName("DESCRICAO");
        }
    }
}
