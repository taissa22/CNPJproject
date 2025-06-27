using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ProcedimentoConfiguration : IEntityTypeConfiguration<Procedimento>
    {
        public void Configure(EntityTypeBuilder<Procedimento> builder)
        {
            builder.ToTable("PROCEDIMENTO", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");

            // PRIMARY KEY
            builder.HasKey(proc => proc.Id).HasName("COD_PROCEDIMENTO");

            builder.Property(proc => proc.Id)
                   .IsRequired()
                   .HasColumnName("COD_PROCEDIMENTO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("PROCEDIMENTO"));

            // PROPERTIES
            builder.Property(proc => proc.Descricao)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("DSC_PROCEDIMENTO");

            builder.Property(proc => proc.EhOrgao1)
                   .IsRequired()
                   .HasColumnName("IND_ORGAO_1").HasConversion(boolConverter);

            builder.Property(proc => proc.EhOrgao2)
                   .IsRequired()
                   .HasColumnName("IND_ORGAO_2").HasConversion(boolConverter);

            builder.Property(proc => proc.EhAdministrativo)
                   .IsRequired()
                   .HasColumnName("IND_ADMINISTRATIVO")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EhTributario)
                   .IsRequired()
                   .HasColumnName("IND_TRIBUTARIO")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EhTrabalhistaAdmin)
                   .IsRequired()
                   .HasColumnName("IND_TRABALHISTA_ADM")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EhProvisionado)
                   .IsRequired()
                   .HasColumnName("IND_PROVISIONADO")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EhPoloPassivoUnico)
                   .IsRequired()
                   .HasColumnName("IND_POLO_PASSIVO_UNICO")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EstaAtivo)
                   .IsRequired()
                   .HasColumnName("IND_ATIVO").HasConversion(boolConverter);

            builder.Property(proc => proc.EhCriminalAdmin)
                   .IsRequired()
                   .HasColumnName("IND_CRIMINAL_ADM")
                   .HasConversion(boolConverter);

            builder.Property(proc => proc.EhCivelAdmin)
                   .IsRequired()
                   .HasColumnName("IND_CIVEL_ADM")
                   .HasConversion(boolConverter);

            // IGNORE
            builder.Ignore(proc => proc.Notifications);
            builder.Ignore(proc => proc.Invalid);
            builder.Ignore(proc => proc.Valid);

            // RELATIONSHIP
            builder.HasOne(proc => proc.TipoParticipacao1)
                   .WithMany(pp => pp.Procedimentos1)
                   .HasForeignKey(proc => proc.TipoParticipacao1Id);

            builder.HasOne(proc => proc.TipoParticipacao2)
                   .WithMany(pp => pp.Procedimentos2)
                   .HasForeignKey(proc => proc.TipoParticipacao2Id);
        }
    }
}
