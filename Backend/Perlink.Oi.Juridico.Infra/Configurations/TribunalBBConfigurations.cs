using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TribunalBBConfigurations : IEntityTypeConfiguration<TribunalBB>
    {
        public void Configure(EntityTypeBuilder<TribunalBB> builder)
        {
            builder.ToTable("BB_TRIBUNAIS","JUR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
            builder.Property(x => x.IndInstanciaDesignada).HasColumnName("IND_INSTANCIA_DESIGNADA").IsRequired();
        }
    }
}
