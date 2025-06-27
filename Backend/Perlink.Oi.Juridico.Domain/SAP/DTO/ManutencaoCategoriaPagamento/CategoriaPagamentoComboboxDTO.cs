using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoComboboxDTO
    {
        public IEnumerable<ComboboxDTO> TiposProcessos { get; set; }
        public IEnumerable<ComboboxDTO> TiposLancamentos { get; set; }
    }
}
