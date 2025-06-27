using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.DTOs
{
    public class AgendamentoLogProcessoResponse
    {
        public decimal Id { get; set; }
        public string DataIni { get; set; }
        public string DataFim { get; set; }
        public string DataDeAgendamento { get; set; }
        public string DataDeCadastro { get; set; }
        public string UsuarioId { get; set; } = "";
        public string Operacao { get; set; } = "";
        public int TipoLog { get; set; }
        public int Status { get; set; }
        public string NomeDoArquivo { get; set; } = "";
        public string MensagemErro { get; set; } = "";
        public string DataIniExecucao { get; set; }
        public string DataFimExecucao { get; set; }

        public AgendamentoLogProcessoResponse(AgendLogProcesso entity)
        {
            Id = entity.Id;
            DataIni = entity.DatLogIni.ToString("dd/MM/yyyy");
            DataFim = entity.DatLogFim.ToString("dd/MM/yyyy");
            DataDeAgendamento = entity.DatAgendamento.ToString("dd/MM/yyyy");
            DataDeCadastro = entity.DatCadastro.ToString("dd/MM/yyyy HH:mm");
            UsuarioId = entity.UsrCodUsuario;
            Operacao = entity.CodOperacao;
            TipoLog = entity.CodTipoLog;
            Status = entity.Status;
            NomeDoArquivo = entity.NomArquivo;
            MensagemErro = entity.Mensagem;
            DataIniExecucao = entity.DatExecucaoIni != null ? entity.DatExecucaoIni.Value.ToString("dd/MM/yyyy"): "";
            DataFimExecucao = entity.DatExecucaoFim != null ? entity.DatExecucaoFim.Value.ToString("dd/MM/yyyy"): "";
        }

    }
}
