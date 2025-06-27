using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class ProcessoCpfCnpjConfiguration : IEntityTypeConfiguration<ProcessoCpfCnpj>
    {
        public void Configure(EntityTypeBuilder<ProcessoCpfCnpj> builder)
        {
            builder.ToTable("PROCESSO_CPF_CNPJ", "JUR");

            builder.HasKey(x => new { x.NumeroProcesso, x.ComarcaId, x.VaraId, x.TipoVaraId });

            builder.Property(x => x.NumeroProcesso).HasColumnName("NRO_PROCESSO_CARTORIO").IsRequired();
            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA").IsRequired();
            builder.Property(x => x.VaraId).HasColumnName("COD_VARA").IsRequired();
            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA").IsRequired();

        }
    }
}
