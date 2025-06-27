using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TempCotacaoIndiceTrabConfiguration : IEntityTypeConfiguration<TempCotacaoIndiceTrab>
    {
        public void Configure(EntityTypeBuilder<TempCotacaoIndiceTrab> builder)
        {
            builder.ToTable("TEMP_COTACAO_INDICE_TRAB", "JUR");

            builder.HasKey(x => new { x.DataBase, x.DataCorrecao });

            builder.Property(x => x.DataBase).HasColumnName("DAT_BASE").IsRequired();
            builder.Property(x => x.DataCorrecao).HasColumnName("DAT_CORRECAO").IsRequired();
            builder.Property(x => x.ValorCotacao).HasColumnName("VAL_COTACAO").IsRequired();
        }
    }
}
