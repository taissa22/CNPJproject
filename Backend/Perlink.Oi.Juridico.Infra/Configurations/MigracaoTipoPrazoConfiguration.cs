using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoTipoPrazoConfiguration : IEntityTypeConfiguration<MigracaoTipoPrazoEstrategico>
    {
        public void Configure(EntityTypeBuilder<MigracaoTipoPrazoEstrategico> builder)
        {
            builder.ToTable("MIG_TIPO_PRAZO_CIVEL_ESTR", "JUR");

            builder.HasKey(x => new { x.CodTipoPrazoCivelEstrat, x.CodTipoPrazoCivelCons });

            builder.Property(x => x.CodTipoPrazoCivelEstrat).HasColumnName("COD_TIPO_PRAZO_CIVEL_ESTRAT");
            builder.Property(x => x.CodTipoPrazoCivelCons).HasColumnName("COD_TIPO_PRAZO_CIVEL_CONS");

        }
    }
}
