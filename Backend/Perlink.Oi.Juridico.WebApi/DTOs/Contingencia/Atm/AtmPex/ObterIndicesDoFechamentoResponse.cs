using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class ObterIndicesDoFechamentoResponse
    {
        public string CodEstado { get; set; }
        public short? CodIndice { get; set; }
    }
}
