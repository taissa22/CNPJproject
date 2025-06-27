using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations.Processos
{
    public class PrepostoConfiguration : IEntityTypeConfiguration<Preposto>
    {
        public void Configure(EntityTypeBuilder<Preposto> builder)
        {
            builder.ToTable("PREPOSTO", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");
            builder.HasKey(pk => pk.Id).HasName("COD_PREPOSTO");
            builder.Property(bi => bi.Id).HasColumnName("COD_PREPOSTO");
            builder.Property(c => c.IndicaPrepostoAtivo).HasColumnName("IND_PREPOSTO_ATIVO").HasConversion(boolConverter);
            builder.Property(c => c.NomePreposto).HasColumnName("NOM_PREPOSTO");
            builder.Property(c => c.IndicaPrepostoTrabalhista).HasColumnName("IND_PREPOSTO_TRABALHISTA").HasConversion(boolConverter);
            //builder.HasOne(pk => pk.AudienciaProcesso).WithMany(id => id.Prepostos).HasForeignKey(pk => pk.Id);
        }
    }
}
