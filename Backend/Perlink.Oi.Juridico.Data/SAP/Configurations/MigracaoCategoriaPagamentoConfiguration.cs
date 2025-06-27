using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class MigracaoCategoriaPagamentoConfiguration : IEntityTypeConfiguration<MigracaoCategoriaPagamento>
    {
        public void Configure(EntityTypeBuilder<MigracaoCategoriaPagamento> builder)
        {          
             
            builder.ToTable("MIG_CATEGORIA_PAGTO_CIVEL_CONS", "JUR");
            builder.HasKey(x => new { x.CodCategoriaPagamentoCivel });

            builder.Ignore(x => x.Id);
            builder.Property(x => x.CodCategoriaPagamentoCivel)
                .HasColumnName("COD_CAT_PAGTO_CIVEL_CONS");

            builder.Property(x => x.CodCategoriaPagamentoEstra)
                .HasColumnName("COD_CAT_PAGTO_CIVEL_ESTRAT");

        }
    }     
}
