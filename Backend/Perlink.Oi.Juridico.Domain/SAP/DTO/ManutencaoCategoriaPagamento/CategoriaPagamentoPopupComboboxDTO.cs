using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoPopupComboboxDTO
    {
        public IEnumerable<ComboboxDTO> FornecedoresPermitidos { get; set; }
        public IEnumerable<ComboboxDTO> ClassesGarantias { get; set; }
        public IEnumerable<ComboboxDTO> GrupoCorrecao { get; set; }
    }
}
