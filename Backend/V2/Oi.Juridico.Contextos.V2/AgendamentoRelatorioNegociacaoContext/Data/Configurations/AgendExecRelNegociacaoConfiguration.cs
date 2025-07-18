﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Data.Configurations
{
    public partial class AgendExecRelNegociacaoConfiguration : IEntityTypeConfiguration<AgendExecRelNegociacao>
    {
        public void Configure(EntityTypeBuilder<AgendExecRelNegociacao> entity)
        {
            entity.HasKey(e => e.CodAgendExecRelNegociacao)
                .HasName("PK_COD_AGEND_EXEC_REL_NEGO");

            entity.ToTable("AGEND_EXEC_REL_NEGOCIACAO");

            entity.Property(e => e.CodAgendExecRelNegociacao)
                .HasPrecision(10)
                .HasColumnName("COD_AGEND_EXEC_REL_NEGOCIACAO");

            entity.Property(e => e.DatAgendamento)
                .HasColumnType("DATE")
                .HasColumnName("DAT_AGENDAMENTO");

            entity.Property(e => e.DatFimExec)
                .HasColumnType("DATE")
                .HasColumnName("DAT_FIM_EXEC");

            entity.Property(e => e.DatFimNegociacao)
                .HasColumnType("DATE")
                .HasColumnName("DAT_FIM_NEGOCIACAO");

            entity.Property(e => e.DatIniExec)
                .HasColumnType("DATE")
                .HasColumnName("DAT_INI_EXEC");

            entity.Property(e => e.DatInicioNegociacao)
                .HasColumnType("DATE")
                .HasColumnName("DAT_INICIO_NEGOCIACAO");

            entity.Property(e => e.DatProxExec)
                .HasColumnType("DATE")
                .HasColumnName("DAT_PROX_EXEC");

            entity.Property(e => e.DiaDaSemana)
                .HasPrecision(2)
                .HasColumnName("DIA_DA_SEMANA");

            entity.Property(e => e.DiaDoMes)
                .HasPrecision(2)
                .HasColumnName("DIA_DO_MES");

            entity.Property(e => e.IndNegociacoesAtivas)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_NEGOCIACOES_ATIVAS")
                .IsFixedLength();

            entity.Property(e => e.IndProcessoCc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCESSO_CC")
                .IsFixedLength();

            entity.Property(e => e.IndProcessoJec)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCESSO_JEC")
                .IsFixedLength();

            entity.Property(e => e.IndProcessoProcon)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCESSO_PROCON")
                .IsFixedLength();

            entity.Property(e => e.IndUltimoDiaMes)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ULTIMO_DIA_MES")
                .IsFixedLength();

            entity.Property(e => e.Mensagem)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("MENSAGEM");

            entity.Property(e => e.MensagemErroTrace)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("MENSAGEM_ERRO_TRACE");

            entity.Property(e => e.NomArquivoGerado)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("NOM_ARQUIVO_GERADO");

            entity.Property(e => e.PeriodicidadeExecucao)
                .HasPrecision(2)
                .HasColumnName("PERIODICIDADE_EXECUCAO");

            entity.Property(e => e.PeriodoMensal)
                .HasPrecision(2)
                .HasColumnName("PERIODO_MENSAL");

            entity.Property(e => e.PeriodoSemanal)
                .HasPrecision(2)
                .HasColumnName("PERIODO_SEMANAL");

            entity.Property(e => e.Status)
                .HasPrecision(2)
                .HasColumnName("STATUS");

            entity.Property(e => e.UsrCodUsuario)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USR_COD_USUARIO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AgendExecRelNegociacao> entity);
    }
}
