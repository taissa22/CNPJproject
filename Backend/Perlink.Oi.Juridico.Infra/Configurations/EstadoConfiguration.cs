using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ToTable("ESTADO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_ESTADO").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOM_ESTADO").IsRequired();
            builder.Property(x => x.IndiceId).HasColumnName("COD_INDICE").IsRequired();
            builder.Property(x => x.ValorJuros).HasColumnName("VAL_JUROS").IsRequired();
            builder.HasMany(x => x.Municipios).WithOne(x => x.Estado).HasForeignKey(x => x.EstadoId).HasConstraintName("COD_ESTADO");

        }
    }
}
