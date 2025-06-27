using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class FornecedorExportarDTO
    {
        public long Id { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSap { get; set; }
        public string TipoFornecedor { get; set; }
        public string Escritorio { get; set; }
        public string profissional { get; set; }
        public string Banco { get; set; }
    }
}
