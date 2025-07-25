﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ESocialContext.Data.Configurations
{
    public partial class VaraConfiguration : IEntityTypeConfiguration<Vara>
    {
        public void Configure(EntityTypeBuilder<Vara> entity)
        {
            entity.HasKey(e => new { e.CodComarca, e.CodVara, e.CodTipoVara });

            entity.ToTable("VARA");

            entity.Property(e => e.CodComarca)
                .HasPrecision(4)
                .HasColumnName("COD_COMARCA");

            entity.Property(e => e.CodVara)
                .HasPrecision(3)
                .HasColumnName("COD_VARA");

            entity.Property(e => e.CodTipoVara)
                .HasPrecision(4)
                .HasColumnName("COD_TIPO_VARA");

            entity.Property(e => e.BborgIdBbOrgao)
                .HasColumnType("NUMBER")
                .HasColumnName("BBORG_ID_BB_ORGAO");

            entity.Property(e => e.CodEscritorioJuizado)
                .HasPrecision(6)
                .HasColumnName("COD_ESCRITORIO_JUIZADO");

            entity.Property(e => e.CodVaraBb)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("COD_VARA_BB");

            entity.Property(e => e.EndVara)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("END_VARA");

            entity.HasOne(d => d.CodComarcaNavigation)
                .WithMany(p => p.Vara)
                .HasForeignKey(d => d.CodComarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMARCA_VARA");

            entity.HasOne(d => d.CodEscritorioJuizadoNavigation)
               .WithMany(p => p.Vara)
               .HasForeignKey(d => d.CodEscritorioJuizado)
               .HasConstraintName("FK_PROFISSIONAL_VARA");

            entity.HasOne(d => d.CodTipoVaraNavigation)
                .WithMany(p => p.Vara)
                .HasForeignKey(d => d.CodTipoVara)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_VARA");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Vara> entity);
    }
}
