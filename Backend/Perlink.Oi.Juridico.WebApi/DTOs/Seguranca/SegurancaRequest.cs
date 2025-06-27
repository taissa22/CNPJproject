using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Seguranca
{
    public class SegurancaRequest
    {
       public string  dataFechamento { get; set; }
       public DateTime dataSolicitacao { get; set; }
       public string  status { get; set; }
    }
}
