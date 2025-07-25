﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.AtmPexContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Contextos.AtmPexContext.Data.Configurations
{
    public partial class SolicFechamentoContConfiguration : IEntityTypeConfiguration<SolicFechamentoCont>
    {
        public void Configure(EntityTypeBuilder<SolicFechamentoCont> entity)
        {
            entity.HasKey(e => e.CodSolicFechamentoCont);

            entity.ToTable("SOLIC_FECHAMENTO_CONT");

            entity.Property(e => e.CodSolicFechamentoCont)
                //.HasPrecision(8)
                .ValueGeneratedNever()
                .HasColumnName("COD_SOLIC_FECHAMENTO_CONT");

            entity.Property(e => e.AnoContabil)
                //.HasPrecision(4)
                .HasColumnName("ANO_CONTABIL");

            entity.Property(e => e.CodTipoFechamento)
                //.HasPrecision(4)
                .HasColumnName("COD_TIPO_FECHAMENTO");

            entity.Property(e => e.CodTipoFechamentoTrab)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("COD_TIPO_FECHAMENTO_TRAB");

            entity.Property(e => e.CodUsuarioSolicitacao)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COD_USUARIO_SOLICITACAO");

            entity.Property(e => e.DatProximoAgend)
                .HasColumnType("DATE")
                .HasColumnName("DAT_PROXIMO_AGEND");

            entity.Property(e => e.DatProximoFechamento)
                .HasColumnType("DATE")
                .HasColumnName("DAT_PROXIMO_FECHAMENTO");

            entity.Property(e => e.DatUltimoAgend)
                .HasColumnType("DATE")
                .HasColumnName("DAT_ULTIMO_AGEND");

            entity.Property(e => e.DataCadastro)
                .HasColumnType("DATE")
                .HasColumnName("DATA_CADASTRO");

            entity.Property(e => e.DataDiariaFim)
                .HasColumnType("DATE")
                .HasColumnName("DATA_DIARIA_FIM");

            entity.Property(e => e.DataDiariaIni)
                .HasColumnType("DATE")
                .HasColumnName("DATA_DIARIA_INI");

            entity.Property(e => e.DataEspecifica)
                .HasColumnType("DATE")
                .HasColumnName("DATA_ESPECIFICA");

            entity.Property(e => e.DataPrevia)
                .HasColumnType("DATE")
                .HasColumnName("DATA_PREVIA");

            entity.Property(e => e.DiaDaSemana)
                //.HasPrecision(1)
                .HasColumnName("DIA_DA_SEMANA");

            entity.Property(e => e.DiaDoMes)
                //.HasPrecision(2)
                .HasColumnName("DIA_DO_MES");

            entity.Property(e => e.IndAplicarHaircutProcGar)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_APLICAR_HAIRCUT_PROC_GAR")
                .IsFixedLength(true);

            entity.Property(e => e.IndAtivo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ATIVO")
                .IsFixedLength(true);

            entity.Property(e => e.IndExecucaoImediata)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_EXECUCAO_IMEDIATA")
                .IsFixedLength(true);

            entity.Property(e => e.IndFechamentoMensal)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_FECHAMENTO_MENSAL")
                .IsFixedLength(true);

            entity.Property(e => e.IndGerarBaseDadosFec)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_GERAR_BASE_DADOS_FEC")
                .HasDefaultValueSql("'N'\n")
                .IsFixedLength(true);

            entity.Property(e => e.IndSomenteDiaUtil)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_SOMENTE_DIA_UTIL")
                .IsFixedLength(true);

            entity.Property(e => e.IndUltimoDiaDoMes)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ULTIMO_DIA_DO_MES")
                .IsFixedLength(true);

            entity.Property(e => e.MesContabil)
                //.HasPrecision(2)
                .HasColumnName("MES_CONTABIL");

            entity.Property(e => e.MultDesvioPadrao)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("MULT_DESVIO_PADRAO");

            entity.Property(e => e.NumeroDeMeses)
                //.HasPrecision(2)
                .HasColumnName("NUMERO_DE_MESES");

            entity.Property(e => e.ObsValCorteOutliers)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("OBS_VAL_CORTE_OUTLIERS");

            entity.Property(e => e.PercentualHaircut)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("PERCENTUAL_HAIRCUT");

            entity.Property(e => e.PeriodicidadeExecucao)
                //.HasPrecision(2)
                .HasColumnName("PERIODICIDADE_EXECUCAO");

            entity.Property(e => e.ValAjusteDesvioPadrao)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("VAL_AJUSTE_DESVIO_PADRAO");

            entity.Property(e => e.ValCorteOutliers)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("VAL_CORTE_OUTLIERS");

            entity.Property(e => e.ValPercentProcOutliers)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("VAL_PERCENT_PROC_OUTLIERS");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SolicFechamentoCont> entity);
    }
}
