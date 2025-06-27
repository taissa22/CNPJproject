using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Processos.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations.Processos
{
    public class TipoContratoEscritorioConfiguration : IEntityTypeConfiguration<TipoContratoEscritorio>
    {
        public void Configure(EntityTypeBuilder<TipoContratoEscritorio> builder)
        {
            builder.ToTable("TIPO_CONTRATO_ESCRITORIO", "JUR");

            builder.HasKey(pk => pk.Id).HasName("COD_TIPO_CONTRATO_ESCRITORIO");
            builder.Property(bi => bi.Id).HasColumnName("COD_TIPO_CONTRATO_ESCRITORIO");
            builder.Property(c => c.DscTipoContrato).HasColumnName("DSC_TIPO_CONTRATO");
            builder.Property(c => c.IndAtivo).HasColumnName("IND_ATIVO");            
        }
    }
}
