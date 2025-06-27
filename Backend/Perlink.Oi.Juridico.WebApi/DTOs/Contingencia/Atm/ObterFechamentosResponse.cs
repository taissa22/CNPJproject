using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm
{
    public class ObterFechamentosResponse
    {
        public int Id { get; set; }
        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public int NumeroDeMeses { get; set; }
    }
}
