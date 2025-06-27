using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class CotacaoIndiceTrabalhistaConfiguration : IEntityTypeConfiguration<CotacaoIndiceTrabalhista>
    {
        public void Configure(EntityTypeBuilder<CotacaoIndiceTrabalhista> builder)
        {
            builder.ToTable("COTACAO_INDICE_TRABALHISTA", "JUR");

            builder.HasKey(x => new { x.DataBase, x.DataCorrecao });

            builder.Property(x => x.DataBase).HasColumnName("DAT_BASE").IsRequired();
            builder.Property(x => x.DataCorrecao).HasColumnName("DAT_CORRECAO").IsRequired();
            builder.Property(x => x.ValorCotacao).HasColumnName("VAL_COTACAO").IsRequired();
        }
    }
}