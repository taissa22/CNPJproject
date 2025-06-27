using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class TipoDocumentoMigracaoEstrategicoConfiguration : IEntityTypeConfiguration<TipoDocumentoMigracaoEstrategico>
    {
        public void Configure(EntityTypeBuilder<TipoDocumentoMigracaoEstrategico> builder)
        {
            builder.ToTable("MIG_TIPO_DOC_CIVEL_ESTR_CONS", "JUR");

            builder.HasKey(x => new { x.CodTipoDocCivelEstrategico,  x.CodTipoDocCivelConsumidor });


            builder.Property(x => x.CodTipoDocCivelEstrategico).HasColumnName("COD_TIPO_DOC_CIVEL_ESTR"); 
            builder.Property(x => x.CodTipoDocCivelConsumidor).HasColumnName("COD_TIPO_DOC_CIVEL_CONS"); 



        }
    }
}
