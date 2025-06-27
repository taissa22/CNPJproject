using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoAssuntoEstrategicoConfiguration : IEntityTypeConfiguration<MigracaoAssuntoEstrategico>
    {
        public void Configure(EntityTypeBuilder<MigracaoAssuntoEstrategico> builder)
        {
            builder.ToTable("MIG_ASSUNTO_CIVEL_ESTRATEGICO", "JUR");

            builder.HasKey(x => new { x.CodAssuntoCivelEstrat, x.CodAssuntoCivelCons });

            builder.Property(x => x.CodAssuntoCivelEstrat).HasColumnName("COD_ASSUNTO_CIVEL_ESTRAT");
            builder.Property(x => x.CodAssuntoCivelCons).HasColumnName("COD_ASSUNTO_CIVEL_CONS");

        }
    }
}

