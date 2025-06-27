using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class TipoDocumentoMigracaoConfiguration : IEntityTypeConfiguration<TipoDocumentoMigracao>
    {
        public void Configure(EntityTypeBuilder<TipoDocumentoMigracao> builder)
        {
            builder.ToTable("MIG_TIPO_DOC_CIVEL_CONS_ESTR", "JUR");

            builder.HasKey(x => new { x.CodTipoDocCivelConsumidor, x.CodTipoDocCivelEstrategico });


            builder.Property(x => x.CodTipoDocCivelConsumidor).HasColumnName("COD_TIPO_DOC_CIVEL_CONS");
            builder.Property(x => x.CodTipoDocCivelEstrategico).HasColumnName("COD_TIPO_DOC_CIVEL_ESTR");

        }
    }
}
