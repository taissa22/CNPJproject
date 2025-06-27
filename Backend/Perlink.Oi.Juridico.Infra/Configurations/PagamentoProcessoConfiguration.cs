using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PagamentoProcessoConfiguration : IEntityTypeConfiguration<PagamentoProcesso>
    {
        public void Configure(EntityTypeBuilder<PagamentoProcesso> builder)
        {
            builder.ToTable("PAGAMENTO_PROCESSO", "JUR");

            builder.HasKey(x => new {x.ProcessoId, x.Sequencial});

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(x => x.Sequencial).HasColumnName("COD_SEQ_PAGAMENTO").IsRequired();

            builder.Property(x => x.ParteId).HasColumnName("COD_PARTE");
            builder.HasOne(x => x.Parte).WithMany().HasForeignKey(x => x.ParteId);

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");
            builder.HasOne(x => x.Profissional).WithMany().HasForeignKey(x => x.ProfissionalId);
        }
    }
}