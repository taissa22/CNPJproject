using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class ObterIndicesResponse
    {
        //public decimal Id { get; set; }
        //public int? CodigoTipoProcesso { get; set; }
        //public string CodEstado { get; set; }
        //public short? CodIndice { get; set; }
        public string Descricao { get; set; }
        public bool Acumulado { get; set; }
        public short? CodIndice { get; set; }
    }
}
