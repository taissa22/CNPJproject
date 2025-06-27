using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
   public class LancamentoEstornoFiltroDTO
    {
        public long TipoProcesso { get; set; }
        public long CodigoInterno { get; set; }
        public string NumeroProcesso { get; set; }

    }
}
