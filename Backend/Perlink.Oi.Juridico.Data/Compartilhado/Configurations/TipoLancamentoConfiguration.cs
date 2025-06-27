using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class TipoLancamentoConfiguration : IEntityTypeConfiguration<TipoLancamento> {
        public void Configure(EntityTypeBuilder<TipoLancamento> builder) {
            builder.ToTable("TIPO_LANCAMENTO", "JUR");

            builder.HasKey(tl => tl.Id).HasName("COD_TIPO_LANCAMENTO");
            builder.Property(tl => tl.Id).HasColumnName("COD_TIPO_LANCAMENTO");
            builder.Property(tl => tl.Descricao).HasColumnName("DSC_TIPO_LANCAMENTO");
        }
    }
}
