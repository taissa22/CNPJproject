using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
  public class MigracaoAcaoConfiguration : IEntityTypeConfiguration<MigracaoAcao>
    {  
        public void Configure(EntityTypeBuilder<MigracaoAcao> builder)
        {
            builder.ToTable("MIG_ACAO_CIVEL_ESTRATEGICO", "JUR");

            builder.HasKey(x => new { x.CodAcaoConsumidor, x.CodAcaoEstrategico });

            builder.Property(x => x.CodAcaoConsumidor).HasColumnName("COD_ACAO_CIVEL_CONSUMIDOR");
            builder.Property(x => x.CodAcaoEstrategico).HasColumnName("COD_ACAO_CIVEL_ESTRATEGICO");
            
        }
    }
}
