using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ParteProcessoConfiguration : IEntityTypeConfiguration<ParteProcesso>
    {
        public void Configure(EntityTypeBuilder<ParteProcesso> builder)
        {
            builder.ToTable("PARTE_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.ParteId });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").HasDefaultValue(null);
            //builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.ParteId).HasColumnName("COD_PARTE").HasDefaultValue(null);
            builder.HasOne(x => x.Parte).WithMany().HasForeignKey(x => x.ParteId);

            builder.Property(x => x.TipoParticipacaoId).HasColumnName("COD_TIPO_PARTICIPACAO").HasDefaultValue(null);

            builder.HasOne(x => x.Processo).WithMany(x => x.PartesDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}