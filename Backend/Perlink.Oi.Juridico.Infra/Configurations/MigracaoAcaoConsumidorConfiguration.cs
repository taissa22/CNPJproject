using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoAcaoConsumidorConfiguration : IEntityTypeConfiguration<MigracaoAcaoConsumidor>
    {
        public void Configure(EntityTypeBuilder<MigracaoAcaoConsumidor> builder)
        {
            builder.ToTable("MIG_ACAO", "JUR");

            builder.HasKey(x => new { x.CodAcaoCivel, x.CodAcaoCivelEstrategico });

            builder.Property(x => x.CodAcaoCivel).HasColumnName("COD_ACAO_CIVEL");
            builder.Property(x => x.CodAcaoCivelEstrategico).HasColumnName("COD_ACAO_CIVEL_ESTRAT");

        }
    }
}
