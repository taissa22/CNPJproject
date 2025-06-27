using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
   public class LinhaProcessoConfiguration : IEntityTypeConfiguration<LinhaProcesso>
    {
        
            
       public void Configure(EntityTypeBuilder<LinhaProcesso> builder)
        {
            builder.ToTable("LINHA_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.SegLinha });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();

            builder.Property(x => x.SegLinha).HasColumnName("SEQ_LINHA").IsRequired();

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO");

        }
    }
}
