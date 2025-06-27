using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoTipoAudienciaConfiguration : IEntityTypeConfiguration<MigracaoTipoAudienciaEstrategico>
    {
        public void Configure(EntityTypeBuilder<MigracaoTipoAudienciaEstrategico> builder)
        {
            builder.ToTable("MIG_TIPO_AUDIENCIA_CIVEL_ESTR", "JUR");

            builder.HasKey(x => new { x.CodTipoAudienciaCivelEstrat, x.CodTipoAudienciaCivelCons });

            builder.Property(x => x.CodTipoAudienciaCivelEstrat).HasColumnName("COD_TIPO_AUD_CIVEL_ESTRAT");
            builder.Property(x => x.CodTipoAudienciaCivelCons).HasColumnName("COD_TIPO_AUD_CIVEL_CONS");

        }
    }
}
