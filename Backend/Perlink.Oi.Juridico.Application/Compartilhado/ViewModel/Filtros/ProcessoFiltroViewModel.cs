using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros
{
    public class ProcessoFiltroViewModel
    {
        public long Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string Estado { get; set; }
        public string Comarca { get; set; }
        public string Vara { get; set; }
        public string TipoVara { get; set; }
        public string EmpresaGrupo { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ProcessoDTO, ProcessoFiltroViewModel>();
            mapper.CreateMap<ProcessoEstadoFiltroDTO, ProcessoFiltroViewModel>()
                  .ForMember(pfvm => pfvm.EmpresaGrupo, opt => opt.Ignore());
            mapper.CreateMap<ProcessoEmpresaFiltroDTO, ProcessoFiltroViewModel>()
                 .ForMember(pfvm => pfvm.Estado, opt => opt.Ignore());
            mapper.CreateMap<ProcessoFiltroViewModel, ProcessoDTO>();
            mapper.CreateMap<Processo, ProcessoFiltroViewModel>();
        }
    }
}