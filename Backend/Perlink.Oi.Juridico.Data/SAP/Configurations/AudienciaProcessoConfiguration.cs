using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class AudienciaProcessoConfiguration : IEntityTypeConfiguration<AudienciaProcesso>
    {
        public void Configure(EntityTypeBuilder<AudienciaProcesso> builder)
        {
            builder.ToTable("AUDIENCIA_PROCESSO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.SequenciaAudiencia });
            builder.Property(bi => bi.Id)
             .HasColumnName("COD_PROCESSO").HasMaxLength(8);
            builder.Property(bi => bi.SequenciaAudiencia)
            .HasColumnName("SEQ_AUDIENCIA").HasMaxLength(4);
            builder.Property(bi => bi.CodigoPreposto)
             .HasColumnName("COD_PREPOSTO");

            builder.Property(bi => bi.CodigoProfissional).HasColumnName("ADVES_COD_PROFISSIONAL");
            builder.Property(bi => bi.CodigoProfissionalAcompanhante).HasColumnName("ADVES_COD_PROFISSIONAL_ACOMP");
            builder.HasOne(fk => fk.Processo)
                .WithMany(fk => fk.AudienciaProcessos)
                .HasForeignKey(fk => fk.Id);
            builder.HasOne(ap => ap.Preposto)
                    .WithMany(preposto => preposto.AudienciaProcessos)
                    .HasForeignKey(ap => ap.CodigoPreposto);
        }
    
    }
}
