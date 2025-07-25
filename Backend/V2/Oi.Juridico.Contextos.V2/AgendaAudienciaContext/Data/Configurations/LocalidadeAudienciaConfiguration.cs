﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data.Configurations
{
    public partial class LocalidadeAudienciaConfiguration : IEntityTypeConfiguration<LocalidadeAudiencia>
    {
        public void Configure(EntityTypeBuilder<LocalidadeAudiencia> entity)
        {
            entity.HasKey(e => e.CodLocalidadeAudiencia);

            entity.ToTable("LOCALIDADE_AUDIENCIA");

            entity.Property(e => e.CodLocalidadeAudiencia)
                .HasPrecision(10)
                .HasColumnName("COD_LOCALIDADE_AUDIENCIA");

            entity.Property(e => e.DscLocalidadeAudiencia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DSC_LOCALIDADE_AUDIENCIA");

            entity.Property(e => e.IndAtivo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ATIVO")
                .IsFixedLength();

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<LocalidadeAudiencia> entity);
    }
}
