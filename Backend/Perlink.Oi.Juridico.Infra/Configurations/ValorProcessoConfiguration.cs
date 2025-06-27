using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ValorProcessoConfiguration : IEntityTypeConfiguration<ValorProcesso>
    {
        public void Configure(EntityTypeBuilder<ValorProcesso> builder)
        {
            builder.ToTable("VALOR_PROCESSO", "JUR");

            builder.HasKey(x => new
            {
                x.ProcessoId,
                x.Sequencial
            });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(x => x.Sequencial).HasColumnName("COD_SEQ_VALOR").IsRequired();

            builder.Property(x => x.COD_PARTE_EFETUOU).HasColumnName("COD_PARTE_EFETUOU");
            builder.Property(x => x.COD_PARTE_LEVANTOU).HasColumnName("COD_PARTE_LEVANTOU");
        }
    }
}