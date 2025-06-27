using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class ObterFechamentosResponse
    {
        public int? CodSolicFechamento { get; set; }

        public decimal? ValDesvioPadrao { get; set; }

        public DateTime DataFechamento { get; set; }

        public int NumeroDeMeses { get; set; }

    }
}
