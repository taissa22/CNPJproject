using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class CompromissoLoteDTO
    {
        public int QuantidadeCredoresAssociados { get; set; }
        public decimal ValorCompromisso { get; set; }
        public long CodigoCompromisso { get; set; }
        public decimal ValorParcela { get; set; }
        public long CodigoParcela { get; set; }
    }
}
