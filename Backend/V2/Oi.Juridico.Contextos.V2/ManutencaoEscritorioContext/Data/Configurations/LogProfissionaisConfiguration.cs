﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Data.Configurations
{
    public partial class LogProfissionaisConfiguration : IEntityTypeConfiguration<LogProfissionais>
    {
        public void Configure(EntityTypeBuilder<LogProfissionais> entity)
        {
            entity.HasKey(e => new { e.CodProfissional, e.TipoOperacao, e.DataOperacao })
                .HasName("LPROF_PK");

            entity.ToTable("LOG_PROFISSIONAIS");

            entity.HasIndex(e => new { e.CodProfissional, e.TipoOperacao }, "IDX_LPROF_01");

            entity.HasIndex(e => e.TipoOperacao, "IDX_LPROF_02");

            entity.Property(e => e.CodProfissional)
                .HasPrecision(6)
                .HasColumnName("COD_PROFISSIONAL");

            entity.Property(e => e.TipoOperacao)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TIPO_OPERACAO")
                .IsFixedLength();

            entity.Property(e => e.DataOperacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_OPERACAO");

            entity.Property(e => e.AlertaEmA)
                .HasPrecision(3)
                .HasColumnName("ALERTA_EM_A");

            entity.Property(e => e.AlertaEmD)
                .HasPrecision(3)
                .HasColumnName("ALERTA_EM_D");

            entity.Property(e => e.CodEstadoOabA)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("COD_ESTADO_OAB_A");

            entity.Property(e => e.CodEstadoOabD)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("COD_ESTADO_OAB_D");

            entity.Property(e => e.CodGrupoLoteJuizadoA)
                .HasPrecision(4)
                .HasColumnName("COD_GRUPO_LOTE_JUIZADO_A");

            entity.Property(e => e.CodGrupoLoteJuizadoD)
                .HasPrecision(4)
                .HasColumnName("COD_GRUPO_LOTE_JUIZADO_D");

            entity.Property(e => e.CodTipoPessoaA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("COD_TIPO_PESSOA_A")
                .IsFixedLength();

            entity.Property(e => e.CodTipoPessoaD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("COD_TIPO_PESSOA_D")
                .IsFixedLength();

            entity.Property(e => e.CodUsrOfensorA)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("COD_USR_OFENSOR_A");

            entity.Property(e => e.CodUsrOfensorD)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("COD_USR_OFENSOR_D");

            entity.Property(e => e.DataLogRetro)
                .HasColumnType("DATE")
                .HasColumnName("DATA_LOG_RETRO");

            entity.Property(e => e.DescEmailA)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DESC_EMAIL_A");

            entity.Property(e => e.DescEmailD)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DESC_EMAIL_D");

            entity.Property(e => e.DescricaoEndAdicionaisA)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO_END_ADICIONAIS_A");

            entity.Property(e => e.DescricaoEndAdicionaisD)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO_END_ADICIONAIS_D");

            entity.Property(e => e.DescricaoTelAdicionaisA)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO_TEL_ADICIONAIS_A");

            entity.Property(e => e.DescricaoTelAdicionaisD)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO_TEL_ADICIONAIS_D");

            entity.Property(e => e.EndProfissionalA)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("END_PROFISSIONAL_A");

            entity.Property(e => e.EndProfissionalD)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("END_PROFISSIONAL_D");

            entity.Property(e => e.IndAdvogadoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ADVOGADO_A")
                .IsFixedLength();

            entity.Property(e => e.IndAdvogadoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ADVOGADO_D")
                .IsFixedLength();

            entity.Property(e => e.IndAlterarValorInternetA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ALTERAR_VALOR_INTERNET_A")
                .IsFixedLength();

            entity.Property(e => e.IndAlterarValorInternetD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ALTERAR_VALOR_INTERNET_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaCivelA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_CIVEL_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaCivelD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_CIVEL_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaCivelEstrategicoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_CIVEL_ESTRATEGICO_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaCivelEstrategicoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_CIVEL_ESTRATEGICO_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaJuizadoA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_JUIZADO_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaJuizadoD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_JUIZADO_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaRegulatoriaA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_REGULATORIA_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaRegulatoriaD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_REGULATORIA_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaTrabalhistaA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_TRABALHISTA_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaTrabalhistaD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_TRABALHISTA_D")
                .IsFixedLength();

            entity.Property(e => e.IndAreaTributariaA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_TRIBUTARIA_A")
                .IsFixedLength();

            entity.Property(e => e.IndAreaTributariaD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_AREA_TRIBUTARIA_D")
                .IsFixedLength();

            entity.Property(e => e.IndCivelAdmA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CIVEL_ADM_A")
                .IsFixedLength();

            entity.Property(e => e.IndCivelAdmD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CIVEL_ADM_D")
                .IsFixedLength();

            entity.Property(e => e.IndContadorA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTADOR_A")
                .IsFixedLength();

            entity.Property(e => e.IndContadorD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTADOR_D")
                .IsFixedLength();

            entity.Property(e => e.IndContadorPexA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTADOR_PEX_A")
                .IsFixedLength();

            entity.Property(e => e.IndContadorPexD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CONTADOR_PEX_D")
                .IsFixedLength();

            entity.Property(e => e.IndCriminalAdmA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CRIMINAL_ADM_A")
                .IsFixedLength();

            entity.Property(e => e.IndCriminalAdmD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CRIMINAL_ADM_D")
                .IsFixedLength();

            entity.Property(e => e.IndCriminalJudicialA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CRIMINAL_JUDICIAL_A")
                .IsFixedLength();

            entity.Property(e => e.IndCriminalJudicialD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_CRIMINAL_JUDICIAL_D")
                .IsFixedLength();

            entity.Property(e => e.IndEscritorio)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_ESCRITORIO")
                .IsFixedLength();

            entity.Property(e => e.IndOfensorA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_OFENSOR_A")
                .IsFixedLength();

            entity.Property(e => e.IndOfensorD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_OFENSOR_D")
                .IsFixedLength();

            entity.Property(e => e.IndOperacaoWeb)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_OPERACAO_WEB")
                .IsFixedLength();

            entity.Property(e => e.IndOperacaoWebRetro)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_OPERACAO_WEB_RETRO")
                .IsFixedLength();

            entity.Property(e => e.IndPexA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PEX_A")
                .IsFixedLength();

            entity.Property(e => e.IndPexD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PEX_D")
                .IsFixedLength();

            entity.Property(e => e.IndProconA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCON_A")
                .IsFixedLength();

            entity.Property(e => e.IndProconD)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_PROCON_D")
                .IsFixedLength();

            entity.Property(e => e.MesAnoCont)
                .HasColumnType("DATE")
                .HasColumnName("MES_ANO_CONT");

            entity.Property(e => e.MesAnoContRetro)
                .HasColumnType("DATE")
                .HasColumnName("MES_ANO_CONT_RETRO");

            entity.Property(e => e.NomeProfissionalA)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("NOME_PROFISSIONAL_A");

            entity.Property(e => e.NomeProfissionalD)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("NOME_PROFISSIONAL_D");

            entity.Property(e => e.NumCnpjProfissionalA)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("NUM_CNPJ_PROFISSIONAL_A");

            entity.Property(e => e.NumCnpjProfissionalD)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("NUM_CNPJ_PROFISSIONAL_D");

            entity.Property(e => e.NumCpfProfissionalA)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("NUM_CPF_PROFISSIONAL_A");

            entity.Property(e => e.NumCpfProfissionalD)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("NUM_CPF_PROFISSIONAL_D");

            entity.Property(e => e.NumOabAdvogadoA)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("NUM_OAB_ADVOGADO_A");

            entity.Property(e => e.NumOabAdvogadoD)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("NUM_OAB_ADVOGADO_D");

            entity.Property(e => e.UsrCodUsuarioOperRetro)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USR_COD_USUARIO_OPER_RETRO");

            entity.Property(e => e.UsrCodUsuarioOperacao)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USR_COD_USUARIO_OPERACAO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<LogProfissionais> entity);
    }
}
