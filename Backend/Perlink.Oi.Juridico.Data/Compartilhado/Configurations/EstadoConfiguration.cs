using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado> {
        public void Configure(EntityTypeBuilder<Estado> builder) {
            builder.ToTable("ESTADO", "JUR");

            builder.HasKey(pk => pk.Id);

            builder.Property(P => P.Id).IsRequired().HasMaxLength(3).HasColumnName("COD_ESTADO");
            builder.Property(P => P.NomeEstado).IsRequired().HasMaxLength(30).HasColumnName("NOM_ESTADO");
            builder.Property(P => P.CodigoIndice).HasColumnName("COD_INDICE");
            builder.Property(P => P.ValorJuros).HasColumnName("VAL_JUROS");
            builder.Property(P => P.SequencialMunicipio).HasColumnName("SEQ_MUNICIPIO");

        }
    }
}
