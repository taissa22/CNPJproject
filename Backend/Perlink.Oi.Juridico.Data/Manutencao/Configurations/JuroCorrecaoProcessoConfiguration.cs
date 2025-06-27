using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;

namespace Perlink.Oi.Juridico.Data.Manutencao.Configurations
{
    public class JuroCorrecaoProcessoConfiguration : IEntityTypeConfiguration<JuroCorrecaoProcesso>
    {
        public void Configure(EntityTypeBuilder<JuroCorrecaoProcesso> builder)
        {
            builder.ToTable("JUROS_CORRECAO_PROCESSOS", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.DataVigencia });

            builder.Property(bi => bi.Id).HasColumnName("TP_COD_TIPO_PROCESSO").IsRequired();
            builder.Property(bi => bi.DataVigencia).HasColumnName("DATA_VIGENCIA").IsRequired();
            builder.Property(bi => bi.ValorJuros).HasColumnName("VALOR_JUROS");

            builder.HasOne(fk => fk.TipoProcesso)
                   .WithMany(fk => fk.ListaDeJuroCorrecaoProcesso)
                   .HasForeignKey(fk => fk.Id);
        }
    }
}
