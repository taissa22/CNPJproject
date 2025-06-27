using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros
{
    public class CategoriaPagamentoFiltroDTO
    {
        public long? TipoProcesso { get; set; }

        public long? TipoLancamento { get; set; }

        public int? Codigo { get; set; }

        public string Ordenacao { get; set; }

        public bool Ascendente { get; set; }
    }
}
