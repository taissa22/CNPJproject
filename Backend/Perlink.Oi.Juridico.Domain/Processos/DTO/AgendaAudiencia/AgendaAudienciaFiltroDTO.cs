using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.AgendaAudiencia
{
    public class AgendaAudienciaFiltroDTO
    {     
        public string ProcessosEstrategicos { get; set; }
        public string PeriodoPendenciaInicial { get; set; }
        public string PeriodoPendenciaFinal { get; set; } 
        public string PeriodoAudienciaInicial { get; set; }
        public string PeriodoAudienciaFinal { get; set; } 
        public string ClassificacaoHierarquica { get; set; }
        public string ClassificacaoClosing { get; set; }
        public string ListaAdvogado { get; set; } 
        public string ListaAdvogadoAcompanhante { get; set; }
        public string ListaComarca { get; set; }
        public string ListaEmpresa { get; set; }
        public string ListaEstado { get; set; } 
        public string ListaPreposto { get; set; }
        public string ListaPrepostoAcompanhante { get; set; }
        public string ListaEscritorio { get; set; }
        public string ListaEscritorioAcompanhante { get; set; }
        public string ListaVara { get; set; }
        public string TabelaEstado { get; set; }
        public string TabelaComarca { get; set; }
        public string TabelaVara { get; set; }
        public string TabelaTipoVara { get; set; }
    }
}
