using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class JanelaConfiguration : IEntityTypeConfiguration<Janela>
    {
        public void Configure(EntityTypeBuilder<Janela> builder)
        {
            builder.ToTable("ACA_R_JANELAS_MENU", "JUR");

            builder.HasKey(x => new { x.CodAplicacao, x.CodJanela, x.CodMenu });

            builder.Property(x => x.CodAplicacao).HasColumnName("COD_APLICACAO").IsRequired();

            builder.Property(x => x.CodJanela).HasColumnName("COD_JANELA").IsRequired();

            builder.Property(x => x.CodMenu).HasColumnName("COD_MENU").IsRequired();
        }
    }
}
