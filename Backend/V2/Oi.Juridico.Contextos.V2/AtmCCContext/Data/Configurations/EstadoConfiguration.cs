﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.AtmCCContext.Data;
using Oi.Juridico.Contextos.V2.AtmCCContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.AtmCCContext.Data.Configurations
{
    public partial class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> entity)
        {
            entity.HasKey(e => e.CodEstado);

            entity.ToTable("ESTADO");

            entity.Property(e => e.CodEstado)
            .HasMaxLength(3)
            .IsUnicode(false)
            .ValueGeneratedOnAdd()
            .HasColumnName("COD_ESTADO");
            entity.Property(e => e.CodIndice)
            .HasPrecision(4)
            .ValueGeneratedOnAdd()
            .HasColumnName("COD_INDICE");
            entity.Property(e => e.NomEstado)
            .IsRequired()
            .HasMaxLength(30)
            .IsUnicode(false)
            .ValueGeneratedOnAdd()
            .HasColumnName("NOM_ESTADO");
            entity.Property(e => e.SeqMunicipio)
            .HasPrecision(4)
            .HasDefaultValueSql("0 ")
            .HasColumnName("SEQ_MUNICIPIO");
            entity.Property(e => e.ValJuros)
            .ValueGeneratedOnAdd()
            .HasColumnType("NUMBER(8,5)")
            .HasColumnName("VAL_JUROS");

            entity.HasOne(d => d.CodIndiceNavigation).WithMany(p => p.Estado)
            .HasForeignKey(d => d.CodIndice)
            .HasConstraintName("FK_INDICE_ESTADO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Estado> entity);
    }
}
