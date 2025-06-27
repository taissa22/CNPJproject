using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;

namespace Perlink.Oi.Juridico.Data.Processos.Configurations
{
    public class AudienciaProcessoConfiguration : IEntityTypeConfiguration<AudienciaProcesso>
    {
        public void Configure(EntityTypeBuilder<AudienciaProcesso> builder)
        {
            builder.ToTable("AUDIENCIA_PROCESSO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.SequenciaAudiencia });
            builder.Property(ap => ap.Id)
                   .HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(ap => ap.SequenciaAudiencia)
                   .HasColumnName("SEQ_AUDIENCIA").IsRequired();
            builder.Property(ap => ap.CodigoPreposto)
                   .HasColumnName("COD_PREPOSTO");
            builder.Property(ap => ap.CodigoPrepostoAcompanhante)
                   .HasColumnName("COD_PREPOSTO_ACOMP");
            builder.Property(ap => ap.TipoAudienciaId)
                   .HasColumnName("COD_TIPO_AUD");

            builder.Property(ap => ap.CodigoProfissional).HasColumnName("ADVES_COD_PROFISSIONAL");
            builder.Property(ap => ap.CodigoAdvogado).HasColumnName("ADVES_COD_ADVOGADO");
            builder.Property(ap => ap.CodigoProfissionalAcompanhante).HasColumnName("ADVES_COD_PROFISSIONAL_ACOMP");
            builder.Property(ap => ap.CodigoAdvogadoAcompanhante).HasColumnName("ADVES_COD_ADVOGADO_ACOMP");

            builder.HasOne(ap => ap.Processo)
                   .WithMany(ap => ap.AudienciaProcessos)
                   .HasForeignKey(ap => ap.Id);

            builder.HasOne(ap => ap.Preposto)
                   .WithMany(preposto => preposto.AudienciaProcessos)
                   .HasForeignKey(ap => ap.CodigoPreposto);

            builder.HasOne(ap => ap.TipoAudiencia)
                   .WithMany(ta => ta.AudienciasProcesso)
                   .HasForeignKey(ap => ap.TipoAudienciaId);
        }    
    }
}
