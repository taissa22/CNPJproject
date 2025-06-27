using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class AgendaAudienciaFiltrosViewModel
    {
        public IEnumerable<AdvogadoEscritorioDTO> ListaAdvogado { get; set; }
        public IEnumerable<ComarcaDTO> ListaComarca { get; set; }
        public IEnumerable<EmpresaDoGrupoDTO> ListaEmpresa { get; set; }
        public IEnumerable<EscritorioDTO> ListaEscritorio { get; set; }
        public IEnumerable<EstadoDTO> ListaEstado { get; set; }
        public IEnumerable<PrepostoDTO> ListaPreposto { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendaAudienciaPedidosDTO, AgendaAudienciaFiltrosViewModel>();
           
        }
    }
}
