using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TipoOrientacaoJuridicaConfiguration : IEntityTypeConfiguration<TipoOrientacaoJuridica>
    {

        public void Configure(EntityTypeBuilder<TipoOrientacaoJuridica> builder)
        {
            builder.ToTable("TIPO_ORIENTACAO_JURIDICA", "JUR");

            builder.HasKey(x => new { x.Id });
            builder.Property(x => x.Id).HasColumnName("COD_TIPO_ORIENTACAO_JURIDICA").IsRequired()
            .HasSequentialIdGenerator<TipoOrientacaoJuridica>("TIPO_ORIENTACAO_JURIDICA");

            builder.Property(x => x.Descricao).HasColumnName("DSC_TIPO_ORIENTACAO_JURIDICA").IsRequired();

        }

    }
}
