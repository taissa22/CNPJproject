using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia
{
    public class ProcessoSaldoGarantiaDTO
    {
        public string NumeroProcesso { get; set; }
        public string Comarca { get; set; }
        public string Vara { get; set; }
        public string TipoVara { get; set; }
        public string EmpresaGrupo { get; set; }
    }
}
