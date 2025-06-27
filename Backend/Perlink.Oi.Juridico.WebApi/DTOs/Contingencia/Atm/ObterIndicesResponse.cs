using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm
{
    public class ObterIndicesResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string CodigoTipoIndice { get; set; }
        public string CodigoValorIndice { get; set; }
        public bool Acumulado { get; set; }
    }
}
