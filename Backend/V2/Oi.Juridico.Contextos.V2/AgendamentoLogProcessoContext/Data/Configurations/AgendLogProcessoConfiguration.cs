﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Data.Configurations
{
    public partial class AgendLogProcessoConfiguration : IEntityTypeConfiguration<AgendLogProcesso>
    {
        public void Configure(EntityTypeBuilder<AgendLogProcesso> entity)
        {
            entity.ToTable("AGEND_LOG_PROCESSO");

            entity.HasIndex(e => e.DatAgendamento, "IDX_F_ID_LPRCA");

            entity.Property(e => e.Id)
                .HasColumnType("NUMBER")
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.CodOperacao)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("COD_OPERACAO");

            entity.Property(e => e.CodTipoLog)
                .HasPrecision(3)
                .HasColumnName("COD_TIPO_LOG");

            entity.Property(e => e.DatAgendamento)
                .HasColumnType("DATE")
                .HasColumnName("DAT_AGENDAMENTO");

            entity.Property(e => e.DatCadastro)
                .HasColumnType("DATE")
                .HasColumnName("DAT_CADASTRO");

            entity.Property(e => e.DatExecucaoFim)
                .HasColumnType("DATE")
                .HasColumnName("DAT_EXECUCAO_FIM");

            entity.Property(e => e.DatExecucaoIni)
                .HasColumnType("DATE")
                .HasColumnName("DAT_EXECUCAO_INI");

            entity.Property(e => e.DatLogFim)
                .HasColumnType("DATE")
                .HasColumnName("DAT_LOG_FIM");

            entity.Property(e => e.DatLogIni)
                .HasColumnType("DATE")
                .HasColumnName("DAT_LOG_INI");

            entity.Property(e => e.Mensagem)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("MENSAGEM");

            entity.Property(e => e.MsgErro)
                .HasColumnType("CLOB")
                .HasColumnName("MSG_ERRO");

            entity.Property(e => e.NomArquivo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("NOM_ARQUIVO");

            entity.Property(e => e.NomArquivoResultado)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("NOM_ARQUIVO_RESULTADO");

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

        partial void OnConfigurePartial(EntityTypeBuilder<AgendLogProcesso> entity);
    }
}
