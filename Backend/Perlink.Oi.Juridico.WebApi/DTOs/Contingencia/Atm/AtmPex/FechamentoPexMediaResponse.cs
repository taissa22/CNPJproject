using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class FechamentoPexMediaResponse
    {
        public int? CodSolicFechamentoCont { get; set; }
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
