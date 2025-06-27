using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB
{
    public class BBModalidadeDTO
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Modalidade BB")]
        public long CodigoBB { get; set; }
        [Name("Descrição")]
        public string Descricao { get; set; }
    }
}
