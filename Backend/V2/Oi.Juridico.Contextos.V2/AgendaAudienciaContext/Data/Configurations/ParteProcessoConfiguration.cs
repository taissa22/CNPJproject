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
    public partial class ParteProcessoConfiguration : IEntityTypeConfiguration<ParteProcesso>
    {
        public void Configure(EntityTypeBuilder<ParteProcesso> entity)
        {
            entity.HasKey(e => new { e.CodProcesso, e.CodParte });

            entity.ToTable("PARTE_PROCESSO");

            entity.HasIndex(e => new { e.CodTipoParticipacao, e.CodProcesso, e.CodParte }, "IDX$$_0C340002");

            entity.HasIndex(e => e.CodParte, "IDX_PARTE_PROCESSO_01");

            entity.HasIndex(e => new { e.CodProcesso, e.CodParte, e.CodTipoParticipacao }, "IDX_PARTE_PROCESSO_02")
                .IsUnique();

            entity.HasIndex(e => e.CodTipoParticipacao, "PPR_IX_01");

            entity.HasIndex(e => e.CodProcesso, "PRTPR_PROC_01_IX");

            entity.Property(e => e.CodProcesso)
                .HasPrecision(8)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_PROCESSO");

            entity.Property(e => e.CodParte)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_PARTE");

            entity.Property(e => e.CodTipoIdentificacaoParte)
                .HasMaxLength(1)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_TIPO_IDENTIFICACAO_PARTE")
                .IsFixedLength();

            entity.Property(e => e.CodTipoParticipacao)
                .HasPrecision(2)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_TIPO_PARTICIPACAO");

            entity.Property(e => e.CpfReu)
                .HasMaxLength(11)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("CPF_REU");

            entity.Property(e => e.DatDesligamento)
                .HasColumnType("DATE")
                .ValueGeneratedOnAdd()
                .HasColumnName("DAT_DESLIGAMENTO");

            entity.Property(e => e.DataUltAtuStatusAndNeg)
                .HasColumnType("DATE")
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_ULT_ATU_STATUS_AND_NEG");

            entity.Property(e => e.DescNegociacaoFornecedor)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("DESC_NEGOCIACAO_FORNECEDOR");

            entity.Property(e => e.IdClassificacaoAutor)
                .HasColumnType("NUMBER")
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_CLASSIFICACAO_AUTOR");

            entity.Property(e => e.IndAcessoRestrito)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("IND_ACESSO_RESTRITO")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.StnegCodStatusAndamentoNeg)
                .HasPrecision(4)
                .ValueGeneratedOnAdd()
                .HasColumnName("STNEG_COD_STATUS_ANDAMENTO_NEG");

            entity.Property(e => e.TpreCodTipoRelacEmpresa)
                .HasPrecision(4)
                .ValueGeneratedOnAdd()
                .HasColumnName("TPRE_COD_TIPO_RELAC_EMPRESA");

            entity.Property(e => e.ValorDepositoRecursal)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("VALOR_DEPOSITO_RECURSAL")
                .HasDefaultValueSql("0");

            entity.Property(e => e.ValorRecuperado)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("VALOR_RECUPERADO")
                .HasDefaultValueSql("0");

            entity.Property(e => e.ValorRecuperadoCustas)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("VALOR_RECUPERADO_CUSTAS")
                .HasDefaultValueSql("0\n");

            entity.HasOne(d => d.CodParteNavigation)
                .WithMany(p => p.ParteProcesso)
                .HasForeignKey(d => d.CodParte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PARTE_PROCESSO");

            entity.HasOne(d => d.CodProcessoNavigation)
                .WithMany(p => p.ParteProcesso)
                .HasForeignKey(d => d.CodProcesso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PROCESSO_PARTE");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ParteProcesso> entity);
    }
}
