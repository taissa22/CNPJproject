using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LogLoteDTO
    {
        public long Id { get; set; }
        public string DataLog { get; set; }
        public string StatusPagamento {get; set;}
        public string NomeUsuario { get; set; }
        public string DescricaoStatusPagamento { get; set; }
    }
}
