using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class CotacaoConfiguration : IEntityTypeConfiguration<Cotacao>
    {
        public void Configure(EntityTypeBuilder<Cotacao> builder)
        {
            builder.ToTable("COTACAO_INDICE", "JUR");

            builder.HasKey(x => new { x.Id, x.DataCotacao });
            builder.Property(x => x.Id).HasColumnName("COD_INDICE").IsRequired();
            builder.HasOne(x => x.Indice).WithMany().HasForeignKey(x => x.Id);
            builder.Property(x => x.DataCotacao).HasColumnName("DAT_COTACAO").IsRequired();            
            builder.Property(x => x.Valor).HasColumnName("VAL_COTACAO").IsRequired();
            builder.Property(x => x.ValorAcumulado).HasColumnName("VAL_COTACAO_ACUMULADO");
        }
    }
}