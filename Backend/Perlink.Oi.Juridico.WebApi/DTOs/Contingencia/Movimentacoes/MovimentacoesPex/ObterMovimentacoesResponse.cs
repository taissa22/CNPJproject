using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.DTO;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Enums;
using System;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Movimentacoes
{
    public class ObterMovimentacoesResponse
    {
        public decimal Id { get; set; }
        public DateTime DatAgendamento { get;  set; }
        public int FechamentoPexMediaIniCodSolic { get;  set; }
        public FechamentoPexMediaResponse FechamentoIni { get; set; }
        public int FechamentoPexMediaFimCodSolic { get;  set; }
        public FechamentoPexMediaResponse FechamentoFim { get; set; }
        public DateTime DatFechamentoIni { get; set; }
        public DateTime DatFechamentoFim { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public StatusMovimentacaoPex Status { get; set; }
        public string MsgErro { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
    }
}
