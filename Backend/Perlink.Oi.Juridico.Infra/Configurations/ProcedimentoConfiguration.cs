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
    internal class ProcedimentoConfiguration : IEntityTypeConfiguration<Procedimento>
    {

        public void Configure(EntityTypeBuilder<Procedimento> builder)
        {
            builder.ToTable("PROCEDIMENTO", "JUR");

            builder.HasKey(c => c.Codigo);
            builder.Property(x => x.Codigo).HasColumnName("COD_PROCEDIMENTO").IsRequired()
                .HasSequentialIdGenerator<Procedimento>("PROCEDIMENTO");            

            builder.Property(x => x.Descricao).HasColumnName("DSC_PROCEDIMENTO").IsRequired();

            builder.Property(x => x.CodTipoParticipacao1).HasColumnName("COD_TIPO_PARTICIPACAO_1");
            builder.HasOne(x => x.TipoDeParticipacao1).WithMany().HasForeignKey(x => x.CodTipoParticipacao1);

            builder.Property(x => x.CodTipoParticipacao2).HasColumnName("COD_TIPO_PARTICIPACAO_2");
            builder.HasOne(x => x.TipoDeParticipacao2).WithMany().HasForeignKey(x => x.CodTipoParticipacao2);

            builder.Property(x => x.IndOrgao1).HasColumnName("IND_ORGAO_1").HasDefaultValue(false)
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndOrgao2).HasColumnName("IND_ORGAO_2").HasDefaultValue(false)
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAdministrativo).HasColumnName("IND_ADMINISTRATIVO").HasDefaultValue(false)
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndTributario).HasColumnName("IND_TRIBUTARIO").HasDefaultValue(false)
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndTrabalhistaAdm).HasColumnName("IND_TRABALHISTA_ADM").HasDefaultValue(false)
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndProvisionado).HasColumnName("IND_PROVISIONADO").IsRequired()
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndPoloPassivoUnico).HasColumnName("IND_POLO_PASSIVO_UNICO").IsRequired()
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAtivo).HasColumnName("IND_ATIVO").IsRequired()
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndCriminalAdm).HasColumnName("IND_CRIMINAL_ADM").IsRequired()
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndCivelAdm).HasColumnName("IND_CIVEL_ADM").IsRequired()
                .HasConversion(ValueConverters.BoolToString);

        }
    }
}
