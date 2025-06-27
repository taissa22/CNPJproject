using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FaseProcessoConfiguration : IEntityTypeConfiguration<FaseProcesso>
    {
        public void Configure(EntityTypeBuilder<FaseProcesso> builder)
        {
            builder.ToTable("FASE_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.SeqFase});
            builder.Property(x => x.SeqFase).HasColumnName("SEQ_FASE").IsRequired();
            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(x => x.EventoId).HasColumnName("COD_DECISAO");
            builder.Property(x => x.DecisaoId).HasColumnName("COD_DECISAO");

        }
    }
}