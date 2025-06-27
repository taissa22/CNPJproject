using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm
{
    public class ObterAgendamentosResponse
    {
        public int Id { get; set; }
        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public int NumeroDeMeses { get; set; }
        public string CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime? InicioDaExecucao { get; set; }
        public DateTime? FimDaExecucao { get; set; }
        public int Status { get; set; }
        public string Erro { get; set; }
    }
}
