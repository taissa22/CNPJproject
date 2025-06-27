using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations {
    public class AgendamentoSaldoGarantiaConfiguration : IEntityTypeConfiguration<AgendamentoSaldoGarantia> {
        public void Configure(EntityTypeBuilder<AgendamentoSaldoGarantia> builder) {

            builder.ToTable("AGENDAMENTO_SALDO_GARANTIA", "JUR");

            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Id)
              .HasColumnName("COD_AGENDAMENTO")
              .ValueGeneratedOnAdd()
              .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "AGEND_SALDO_GARANTIA_SEQ_01"));

            builder.Property(bi => bi.CodigoTipoProcesso).HasColumnName("COD_TIPO_PROCESSO").IsRequired();
            builder.Property(bi => bi.DataAgendamento).HasColumnName("DAT_AGENDAMENTO").IsRequired();
            builder.Property(bi => bi.NomeAgendamento).HasColumnName("NOM_AGENDAMENTO").IsRequired();
            builder.Property(bi => bi.DataExecucao).HasColumnName("DAT_EXECUCAO");
            builder.Property(bi => bi.DataFinalizacao).HasColumnName("DAT_FINALIZACAO");
            builder.Property(bi => bi.CodigoUsuario).HasColumnName("COD_USUARIO_SOLICITANTE").IsRequired();
            builder.Property(bi => bi.CodigoStatusAgendamento).HasColumnName("COD_STATUS_AGENDAMENTO_CARGA").IsRequired();
            builder.Property(bi => bi.NomeArquivoGerado).HasColumnName("NOM_ARQUIVO_GERADO");
            builder.Property(bi => bi.MensagemErro).HasColumnName("MENSAGEM_ERRO");
            builder.Property(bi => bi.MensagemErroTrace).HasColumnName("MENSAGEM_ERRO_TRACE");
            
        }
    }
}
