﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data.Configurations
{
    public partial class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> entity)
        {
            entity.HasKey(e => e.CodProfissional);

            entity.ToTable("PROFISSIONAL");

            entity.Property(e => e.CodProfissional)
                //.HasPrecision(6)
                .HasColumnName("COD_PROFISSIONAL");

            entity.Property(e => e.DscEmail)
                .HasMaxLength(60)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("DSC_EMAIL");

            entity.Property(e => e.NomProfissional)
                .HasMaxLength(400)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("NOM_PROFISSIONAL");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Profissional> entity);
    }
}
