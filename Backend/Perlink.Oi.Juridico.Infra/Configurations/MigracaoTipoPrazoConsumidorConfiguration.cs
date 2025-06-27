using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    class MigracaoTipoPrazoConsumidorConfiguration : IEntityTypeConfiguration<MigracaoTipoPrazoConsumidor>
    {
        public void Configure(EntityTypeBuilder<MigracaoTipoPrazoConsumidor> builder)
        {
            builder.ToTable("MIG_TIPO_PRAZO", "JUR");

            builder.HasKey(x => new { x.CodTipoPrazoCivel, x.CodTipoPrazoCivelEstrat });

            builder.Property(x => x.CodTipoPrazoCivel).HasColumnName("COD_TIPO_PRAZO_CIVEL");
            builder.Property(x => x.CodTipoPrazoCivelEstrat).HasColumnName("COD_TIPO_PRAZO_CIVEL_ESTRAT");

        }
    }
}
