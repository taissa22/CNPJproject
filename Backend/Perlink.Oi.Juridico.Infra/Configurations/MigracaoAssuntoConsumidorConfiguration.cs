using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
  public class MigracaoAssuntoConsumidorConfiguration : IEntityTypeConfiguration<MigracaoAssunto>
    {
        public void Configure(EntityTypeBuilder<MigracaoAssunto> builder)
        {
            builder.ToTable("MIG_ASSUNTO", "JUR");

            builder.HasKey(x => new { x.CodAssuntoCivel, x.CodAssuntoCivelEstrategico });            
            
            builder.Property(x => x.CodAssuntoCivel).HasColumnName("COD_ASSUNTO_CIVEL");
            builder.Property(x => x.CodAssuntoCivelEstrategico).HasColumnName("COD_ASSUNTO_CIVEL_ESTRAT");  

        }
    }
}
