using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Dtos
{
    public class ListarCcResponse
    {
        public decimal Id { get; set; }
        public DateTime DatAgendamento { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public StatusMovimentacaoCc Status { get; set; }
        public string MsgErro { get; set; } = "";
        public string UsuarioId { get; set; } = "";
        public string UsuarioNome { get; set; } = "";
        public DateTime IniDataFechamento { get; set; }
        public short IniNumMesesMediaHistorica { get; set; }
        public string IniIndicadorFechamentoMensal { get; set; } = "";
        public decimal IniPercentualHaircut { get; set; }
        public bool IniIndFechamentoParcial { get; set; }
        public string IniEmpresas { get; set; } = "";
        public DateTime FimDataFechamento { get; set; }
        public short FimNumMesesMediaHistorica { get; set; }
        public string FimIndicadorFechamentoMensal { get; set; } = "";
        public decimal FimPercentualHaircut { get; set; }
        public bool FimIndFechamentoParcial { get; set; }
        public string FimEmpresas { get; set; } = "";

    }
}
