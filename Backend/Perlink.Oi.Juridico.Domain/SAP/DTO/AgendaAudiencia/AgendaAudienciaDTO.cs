using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class AgendaAudienciaDTO
    {
        public IEnumerable<AdvogadoEscritorioDTO> ListaAdvogado { get; set; }
        public IEnumerable<ComarcaDTO> ListaComarca { get; set; }
        public IEnumerable<EmpresaDoGrupoDTO> ListaEmpresa { get; set; }
        public IEnumerable<EscritorioDTO> ListaEscritorio { get; set; }
        public IEnumerable<EstadoDTO> ListaEstado { get; set; }
        public IEnumerable<PrepostoDTO> ListaPreposto { get; set; }
    }
}
