using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class ObterAgendamentosResponse
    {
        public decimal AgendamentoId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public int CodSolicFechamento { get; set; }
        public decimal? ValDesvioPadrao { get; set; }
        public short? NumeroDeMeses { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataInicioExecucao { get; set; }
        public DateTime? DataFimExecucao { get; set; }
        public string CodigoUsuario { get; set; }
        public byte Status { get; set; }
        public string MsgErro { get; set; }
        public string NomeUsuario { get; set; }
        public FechamentoPexMediaResponse fechamentoPexMediaResponse { get; set; }
    }
}
