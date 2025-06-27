using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AgendamentoMigracaoPedidosSapConfiguration : IEntityTypeConfiguration<AgendamentoMigracaoPedidosSap>
    {

        public void Configure(EntityTypeBuilder<AgendamentoMigracaoPedidosSap> builder)
        {
            builder.ToTable("AGEND_EXEC_MIGR_PED_SAP", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("COD_AGENDAMENTO").IsRequired()
                .HasNextSequenceValueGenerator("jur", "AGEND_EXEC_MIGR_PED_SAP_SEQ");

            builder.Property(x => x.DataAgendamento).HasColumnName("DAT_AGENDAMENTO").IsRequired();
            builder.Property(x => x.DataExecucao).HasColumnName("DAT_EXECUCAO");
            builder.Property(x => x.DataFinalizacao).HasColumnName("DAT_FINALIZACAO");
            builder.Property(x => x.UsuarioSolicitanteId).HasColumnName("COD_USUARIO_SOLICITANTE").IsRequired();
            builder.Property(x => x.StatusAgendamentoId).HasColumnName("COD_STATUS_AGENDAMENTO_CARGA").IsRequired();
            builder.Property(x => x.NomeArquivoGerado).HasColumnName("NOME_ARQUIVO_GERADO");
            builder.Property(x => x.NomeArquivoDeParaMigracao).HasColumnName("NOME_ARQUIVO_DE_PARA_MIGRACAO");
            builder.Property(x => x.MensagemErro).HasColumnName("MENSAGEM_ERRO");
            builder.Property(x => x.MensagemErroTrace).HasColumnName("MENSAGEM_ERRO_TRACE");
            builder.Property(x => x.DescricaoResultadoCarga).HasColumnName("DSC_RESULTADO_CARGA");
        }
    }
}