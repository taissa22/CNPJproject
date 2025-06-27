using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class BaseDeCalculoConfiguration: IEntityTypeConfiguration<BaseDeCalculo>
    {
        public void Configure(EntityTypeBuilder<BaseDeCalculo> builder)
        {
            builder.ToTable("BASE_CALCULO", "JUR");

            builder.HasKey(c => c.Codigo);
            builder.Property(bi => bi.Codigo).HasColumnName("COD_BASE_CALCULO").IsRequired()
                .HasSequentialIdGenerator<BaseDeCalculo>("BASE CALCULO");

            builder.Property(bi => bi.Descricao).HasColumnName("DSC_BASE_CALCULO");

            builder.Property(bi => bi.IndBaseInicial).HasColumnName("IND_BASE_INICIAL")
                .HasConversion(ValueConverters.BoolToString);
        }
    }
}
