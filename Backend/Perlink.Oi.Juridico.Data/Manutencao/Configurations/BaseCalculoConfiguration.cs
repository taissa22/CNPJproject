using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Manutencao.Configurations
{
    public class BaseCalculoConfiguration : IEntityTypeConfiguration<BaseCalculo>
    {
        public void Configure(EntityTypeBuilder<BaseCalculo> builder)
        {
            builder.ToTable("BASE_CALCULO", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");

            // PRIMARY KEY
            builder.HasKey(ta => ta.Id).HasName("COD_BASE_CALCULO");

            builder.Property(ta => ta.Id)
                   .IsRequired()
                   .HasColumnName("COD_BASE_CALCULO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("BASE CALCULO"));

            // PROPERTIES
            builder.Property(ta => ta.Descricao)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("DSC_BASE_CALCULO");

            builder.Property(ta => ta.EhBaseInicial)
                   .IsRequired()
                   .HasColumnName("IND_BASE_INICIAL").HasConversion(boolConverter);

            // IGNORE
            builder.Ignore(ta => ta.Notifications);
            builder.Ignore(ta => ta.Invalid);
            builder.Ignore(ta => ta.Valid);

            // RELATIONSHIP
        }
    }
}
