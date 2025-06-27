using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class AreaEnvolvidaConfiguration : IEntityTypeConfiguration<AreaEnvolvida>
    {
        public void Configure(EntityTypeBuilder<AreaEnvolvida> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("AREA_ENVOLVIDA", "JUR");
            builder.HasKey(c => c.Id);
            builder.Property(bi => bi.Id).HasColumnName("ID");
            builder.Property(bi => bi.Nome).HasColumnName("NOME");
            builder.Property(bi => bi.IndAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndCivelConsumidor).HasColumnName("IND_CIVEL_CONSUMIDOR").HasConversion(boolConverter);
            builder.Property(bi => bi.IndJec).HasColumnName("IND_JEC").HasConversion(boolConverter);
            builder.Property(bi => bi.IndTrabalhista).HasColumnName("IND_TRABALHISTA").HasConversion(boolConverter);
            builder.Property(bi => bi.IndTribJudicial).HasColumnName("IND_TRIB_JUDICIAL").HasConversion(boolConverter);
            builder.Property(bi => bi.IndTribAdm).HasColumnName("IND_TRIB_ADMINISTRATIVO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndCivelAdm).HasColumnName("IND_CIVEL_ADMINISTRATIVO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndTrabAdm).HasColumnName("IND_TRABALHISTA_ADMINISTRATIVO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndAdm).HasColumnName("IND_ADMINISTRATIVO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndProcon).HasColumnName("IND_PROCON").HasConversion(boolConverter);
            builder.Property(bi => bi.IndCrimJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").HasConversion(boolConverter);
            builder.Property(bi => bi.IndCrimAdm).HasColumnName("IND_CRIMINAL_ADMINISTRATIVO").HasConversion(boolConverter);
        }
    }
}
