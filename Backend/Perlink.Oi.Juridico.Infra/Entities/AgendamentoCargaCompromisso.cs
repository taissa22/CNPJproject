﻿using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities {

    public sealed class AgendamentoCargaCompromisso : Notifiable, IEntity, INotifiable {

        private AgendamentoCargaCompromisso() {
        }

        public int Id { get; private set; }

        public DateTime? DataAgendamento { get; private set; }

        public DateTime? DataExecucao { get; private set; }

        public DateTime? DataFinalizacao { get; private set; }

        public string UsuarioSolicitanteId { get; private set; }

        public int StatusAgendamentoId { get; private set; }
        public StatusAgendamento StatusAgendamento => StatusAgendamento.PorId(StatusAgendamentoId);

        public string NomeArquivoGerado { get; private set; }

        public string NomeArquivoBaseFGV { get; private set; }

        public string MensagemErro { get; private set; }

        public string MensagemErroTrace { get; private set; }

        public string DescricaoResultadoCarga { get; private set; }

        private void Validate()
        {
        }

        public void Atualizar()
        {
            Validate();
        }

        public static AgendamentoCargaCompromisso Criar(string usuarioSolicitanteId)
        {
            var agendamentoCargaCompromissos = new AgendamentoCargaCompromisso()
            {
                DataAgendamento = DateTime.Now,
                UsuarioSolicitanteId = usuarioSolicitanteId,
                StatusAgendamentoId = StatusAgendamento.AGENDADO.Id,
                DescricaoResultadoCarga = "Esta carga de compromissos será processada em breve. Por favor aguarde."
            };
            agendamentoCargaCompromissos.Validate();
            return agendamentoCargaCompromissos;
        }

        public void AtualizarNomesCompromisso(string nomeArquivoBaseFGV)
        {
            NomeArquivoBaseFGV = nomeArquivoBaseFGV;
            Validate();
        }

        public void AtualizarStatusAgendamento(int statusAgendamentoId)
        {
            StatusAgendamentoId = statusAgendamentoId;
            DataExecucao = DateTime.Now;
        }

        public void AtualizarResultadoAgendamento(int statusAgendamentoId, string nomeArquivoGerado, string descricaoResultadoCarga)
        {
            StatusAgendamentoId = statusAgendamentoId;
            NomeArquivoGerado = nomeArquivoGerado;
            DescricaoResultadoCarga = descricaoResultadoCarga;
            DataFinalizacao = DateTime.Now;
        }

        public void AtualizarErroAgendamento(int statusAgendamentoId, string mensagemErro, string mensagemErroTrace, string descricaoResultadoCarga)
        {
            StatusAgendamentoId = statusAgendamentoId;
            MensagemErro = mensagemErro;
            MensagemErroTrace = mensagemErroTrace;
            DescricaoResultadoCarga = descricaoResultadoCarga;
            DataExecucao = DataExecucao.HasValue ? DataExecucao : DateTime.Now;
            DataFinalizacao = DateTime.Now;
        }
    }
}