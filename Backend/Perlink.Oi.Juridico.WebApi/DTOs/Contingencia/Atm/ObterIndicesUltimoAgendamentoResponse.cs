using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm
{
    public class ObterIndicesUltimoAgendamentoResponse
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public int IndiceId { get; set; }
        public bool Acumulado { get; set; }
    }
}
