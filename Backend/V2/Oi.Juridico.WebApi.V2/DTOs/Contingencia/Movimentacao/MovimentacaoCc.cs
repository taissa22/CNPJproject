using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.DTOs.Contingencia.Fechamento;

namespace Oi.Juridico.WebApi.V2.DTOs.Contingencia.Movimentacao
{
    public class MovimentacaoCc
    {
        public decimal Id { get; set; }
        public DateTime DatAgendamento { get; set; }
        public int FechamentoCcMediaIniCodSolic { get; set; }
        public FechamentoCc FechamentoIni { get; set; } = new();
        public int FechamentoCcMediaFimCodSolic { get; set; }
        public FechamentoCc FechamentoFim { get; set; } = new();
        public DateTime DatFechamentoIni { get; set; }
        public DateTime DatFechamentoFim { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public string IndFechamentoMensal { get; set; } = "";
        public DateTime? DatFimExecucao { get; set; }
        public StatusMovimentacaoCc Status { get; set; }
        public string MsgErro { get; set; } = "";
        public string UsuarioId { get; set; } = "";
        public string UsuarioNome { get; set; } = "";
    }
}
