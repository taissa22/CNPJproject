using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia
{
    public class CabecalhoProvisaoTrabalhistaPoResponse
    {
        public string NomeEmpresaCentralizadora { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public string ProprioTerceiro { get; set; }
        public string RiscoPerda { get; set; }
        public DateTime DataFechamento { get; set; }
        public int NumeroMeses { get; set; }
        public string TipoDeOutliers { get; set; }
        public string DataFormatada { get; set; }
    }
}
