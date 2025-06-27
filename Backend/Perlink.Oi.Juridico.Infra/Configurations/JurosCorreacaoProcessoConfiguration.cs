using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class JurosCorreacaoProcessoConfiguration : IEntityTypeConfiguration<JurosCorrecaoProcesso>
    {
        public void Configure(EntityTypeBuilder<JurosCorrecaoProcesso> builder)
        {
            builder.ToTable("JUROS_CORRECAO_PROCESSOS", "JUR");

            builder.HasKey(x => new { x.CodTipoProcesso, x.DataVigencia });

            builder.Property(x => x.CodTipoProcesso).HasColumnName("TP_COD_TIPO_PROCESSO").IsRequired();

            builder.Property(x => x.DataVigencia).HasColumnName("DATA_VIGENCIA").IsRequired();

            builder.Property(x => x.ValorJuros).HasColumnName("VALOR_JUROS").IsRequired();
        }
    }
}