using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores
{
    public class FornecedorContigenciaResultadoDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public double ValorCartaFianca { get; set; }
        public DateTime? DataVencimentoCartaFianca { get; set; }
        public long StatusFornecedor { get; set; }
        public string CNPJ { get; set; }

    }
    public class FornecedorContigenciaExportarDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string ValorCartaFianca { get; set; }
        public DateTime? DataVencimentoCartaFianca { get; set; }
        public string StatusFornecedor { get; set; }
        public string CNPJ { get; set; }
    }
}
