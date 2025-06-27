using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ComarcaBBConfiguration : IEntityTypeConfiguration<ComarcaBB>
    {
        public void Configure(EntityTypeBuilder<ComarcaBB> builder)
        {
            builder.ToTable("BB_COMARCAS", "JUR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO_BB").IsRequired();
            builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
        }
    }
}
