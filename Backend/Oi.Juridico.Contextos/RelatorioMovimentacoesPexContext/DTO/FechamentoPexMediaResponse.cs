using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.DTO
{
    public class FechamentoPexMediaResponse
    {
        public int? Id { get; set; }
        public DateTime DataFechamento { get; set; }
        public string NomeUsuario { get; set; }
        public long NumeroMeses { get; set; }
        public decimal? PercentualHaircut { get; set; }
        public decimal? MultDesvioPadrao { get; set; }
        public string IndAplicarHaircut { get; set; }
        public string Empresas { get; set; }
        public DateTime DataExecucao { get; set; }
        public DateTime? DataAgendamento { get; set; }
    }
}
