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
    internal class OrientacaoJuridicaConfiguration : IEntityTypeConfiguration<OrientacaoJuridica>
    {

        public void Configure(EntityTypeBuilder<OrientacaoJuridica> builder)
        {
            builder.ToTable("ORIENTACAO_JURIDICA", "JUR");

            builder.HasKey(x => new { x.CodOrientacaoJuridica });

            builder.Property(x => x.CodOrientacaoJuridica).HasColumnName("COD_ORIENTACAO_JURIDICA").IsRequired()
                .HasSequentialIdGenerator<OrientacaoJuridica>("ORIENTACAO_JURIDICA");

            builder.Property(x => x.CodTipoOrientacaoJuridica).HasColumnName("COD_TIPO_ORIENTACAO_JURIDICA").IsRequired();
            builder.HasOne(x => x.TipoOrientacaoJuridica).WithMany().HasForeignKey(x => x.CodTipoOrientacaoJuridica);

            builder.Property(x => x.Nome).HasColumnName("NOM_ORIENTACAO_JURIDICA").IsRequired();

            builder.Property(x => x.Descricao).HasColumnName("DSC_ORIENTACAO_JURIDICA").IsRequired();
            builder.Property(x => x.PalavrasChave).HasColumnName("DSC_PALAVRAS_CHAVE");
            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_TRABALHISTA").HasConversion(ValueConverters.BoolToString); ;
            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVA").HasConversion(ValueConverters.BoolToString);

        }

    }
}
