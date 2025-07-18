﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;
using System.Collections.Generic;
using System;

namespace Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data.Configurations
{
    public partial class PrepostoConfiguration : IEntityTypeConfiguration<Preposto>
    {
        public void Configure(EntityTypeBuilder<Preposto> entity)
        {
            entity.HasKey(e => e.CodPreposto);

            entity.ToTable("PREPOSTO");

            entity.Property(e => e.CodPreposto)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("COD_PREPOSTO");

            //entity.Property(e => e.CodEstado)
            //    .IsRequired()
            //    .HasMaxLength(3)
            //    .IsUnicode(false)
            //    .HasColumnName("COD_ESTADO");

            entity.Property(e => e.CodUsuario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("COD_USUARIO");

            entity.Property(e => e.IndCivelEstrategico)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CIVEL_ESTRATEGICO")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndEscritorio)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ESCRITORIO")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndPex)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PEX")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndPrepostoAtivo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PREPOSTO_ATIVO")
                .HasDefaultValueSql("'S' ")
                .IsFixedLength();

            entity.Property(e => e.IndPrepostoCivel)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PREPOSTO_CIVEL")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndPrepostoJuizado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PREPOSTO_JUIZADO")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndPrepostoTrabalhista)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PREPOSTO_TRABALHISTA")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.IndProcon)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCON")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.Matricula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATRICULA");

            entity.Property(e => e.NomPreposto)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("NOM_PREPOSTO");

            entity.HasOne(d => d.CodUsuarioNavigation)
                .WithMany(p => p.Preposto)
                .HasForeignKey(d => d.CodUsuario)
                .HasConstraintName("FK_USUARIO_PREPOSTO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Preposto> entity);
    }
}
