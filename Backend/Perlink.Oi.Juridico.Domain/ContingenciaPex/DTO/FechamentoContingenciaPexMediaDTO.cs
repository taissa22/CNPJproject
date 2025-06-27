using System;

namespace Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO
{
    public class FechamentoContingenciaPexMediaDTO
    {
        public long Id { get; set; }
        public DateTime DataFechamento { get; set; }
        public string NomeUsuario { get; set; }
        public long NumeroMeses { get; set; }
        public decimal? PercentualHaircut { get; set; }
        public decimal? MultDesvioPadrao { get; set; }
        public string IndAplicarHaircut { get; set; }
        public string Empresas { get; set; }        
        public DateTime DataExecucao { get; set; }
    }   
}
