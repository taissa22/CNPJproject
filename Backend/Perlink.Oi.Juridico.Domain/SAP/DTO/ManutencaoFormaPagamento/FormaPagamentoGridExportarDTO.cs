using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento
{
    public class FormaPagamentoGridExportarDTO
    {
        public long Codigo { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public bool RequerBordero { get; set; }
        public bool Restrita { get; set; }
    }
}
