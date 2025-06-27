using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.DTOs.Contingencia.Fechamento;

namespace Oi.Juridico.WebApi.V2.DTOs.Contingencia.Movimentacao
{
    public class MovimentacaoJec
    {
        public decimal Id { get; set; }
        public DateTime DatAgendamento { get; set; }
        public int FechamentoJecMediaIniCodSolic { get; set; }
        public FechamentoJec FechamentoIni { get; set; } = new();
        public int FechamentoJecMediaFimCodSolic { get; set; }
        public FechamentoJec FechamentoFim { get; set; } = new();
        public DateTime DatFechamentoIni { get; set; }
        public string IndFechamentoMensal { get; set; } = "";
        public DateTime DatFechamentoFim { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public StatusMovimentacaoJec Status { get; set; }
        public string MsgErro { get; set; } = "";
        public string UsuarioId { get; set; } = "";
        public string UsuarioNome { get; set; } = "";
    }
}
