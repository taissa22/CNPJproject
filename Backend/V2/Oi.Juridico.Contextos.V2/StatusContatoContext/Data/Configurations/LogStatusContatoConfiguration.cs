﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Data;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.StatusContatoContext.Data.Configurations
{
    public partial class LogStatusContatoConfiguration : IEntityTypeConfiguration<LogStatusContato>
    {
        public void Configure(EntityTypeBuilder<LogStatusContato> entity)
        {
            entity.HasNoKey();

            entity.ToTable("LOG_STATUS_CONTATO");

            entity.Property(e => e.CodStatusContato)
                .HasPrecision(10)
                .HasColumnName("COD_STATUS_CONTATO");

            entity.Property(e => e.CodUsuario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("COD_USUARIO");

            entity.Property(e => e.DatLog)
                .HasPrecision(6)
                .HasColumnName("DAT_LOG");

            entity.Property(e => e.DscStatusContatoA)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DSC_STATUS_CONTATO_A");

            entity.Property(e => e.DscStatusContatoD)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DSC_STATUS_CONTATO_D");

            entity.Property(e => e.IndAcordoRealizadoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ACORDO_REALIZADO_A")
                .IsFixedLength();

            entity.Property(e => e.IndAcordoRealizadoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ACORDO_REALIZADO_D")
                .IsFixedLength();

            entity.Property(e => e.IndAtivoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ATIVO_A")
                .IsFixedLength();

            entity.Property(e => e.IndAtivoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ATIVO_D")
                .IsFixedLength();

            entity.Property(e => e.IndContatoRealizadoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTATO_REALIZADO_A")
                .IsFixedLength();

            entity.Property(e => e.IndContatoRealizadoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTATO_REALIZADO_D")
                .IsFixedLength();

            entity.Property(e => e.Operacao)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("OPERACAO")
                .IsFixedLength();

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<LogStatusContato> entity);
    }
}
