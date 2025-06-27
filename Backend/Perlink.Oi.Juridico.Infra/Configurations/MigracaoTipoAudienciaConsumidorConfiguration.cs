using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    class MigracaoTipoAudienciaConsumidorConfiguration : IEntityTypeConfiguration<MigracaoTipoAudienciaConsumidor>
    {
        public void Configure(EntityTypeBuilder<MigracaoTipoAudienciaConsumidor> builder)
        {
            builder.ToTable("MIG_TIPO_AUDIENCIA", "JUR");

            builder.HasKey(x => new { x.CodTipoAudCivel, x.CodTipoAudCivelEstrat });

            builder.Property(x => x.CodTipoAudCivel).HasColumnName("COD_TIPO_AUD_CIVEL");
            builder.Property(x => x.CodTipoAudCivelEstrat).HasColumnName("COD_TIPO_AUD_CIVEL_ESTRAT");

        }
    }
}
