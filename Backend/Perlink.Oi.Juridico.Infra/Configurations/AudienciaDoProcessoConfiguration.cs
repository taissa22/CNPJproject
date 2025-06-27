using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AudienciaDoProcessoConfiguration : IEntityTypeConfiguration<AudienciaDoProcesso>
    {
        public void Configure(EntityTypeBuilder<AudienciaDoProcesso> builder)
        {
            builder.ToTable("AUDIENCIA_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.Sequencial).HasColumnName("SEQ_AUDIENCIA").IsRequired();

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.PrepostoId).HasColumnName("COD_PREPOSTO").HasDefaultValue(null);
            builder.HasOne(x => x.Preposto).WithMany().HasForeignKey(x => x.PrepostoId);

            builder.Property(x => x.EscritorioId).HasColumnName("ADVES_COD_PROFISSIONAL").HasDefaultValue(null);
            builder.HasOne(x => x.Escritorio).WithMany().HasForeignKey(x => x.EscritorioId);

            builder.Property(x => x.AdvogadoEscritorioId).HasColumnName("ADVES_COD_ADVOGADO").HasDefaultValue(null);
            builder.HasOne(x => x.AdvogadoEscritorio).WithMany().HasForeignKey(x => new { x.AdvogadoEscritorioId, x.EscritorioId });

            builder.Property(x => x.SituacaoId).HasColumnName("COD_STATUS_AUDIENCIA").HasDefaultValue(null);
            builder.HasOne(x => x.Situacao).WithMany().HasForeignKey(x => x.SituacaoId);            

            builder.Property(x => x.TipoAudienciaId).HasColumnName("COD_TIPO_AUD").HasDefaultValue(null);
            builder.HasOne(x => x.TipoAudiencia).WithMany().HasForeignKey(x => x.TipoAudienciaId);

            builder.Property(x => x.DataAudiencia).HasColumnName("DAT_AUDIENCIA").HasDefaultValue(null);

            builder.Property(x => x.HoraAudiencia).HasColumnName("HOR_AUDIENCIA").HasDefaultValue(null);

            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO").HasDefaultValue(null);
        }
    }
}