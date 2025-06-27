using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
   public class FornecedorFiltroDTO
    {
        public long? CodigoEscritorio { get; set; }
        public long? CodigoTipoFornecedor { get; set; }
        public long? CodigoProfissional { get; set; }
        public long? CodigoBanco { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSAP { get; set; }

        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public int Total { get; set; }
        public string Ordenacao { get; set; }
        public bool Ascendente { get; set; }
    }
}
