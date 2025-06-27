using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto
{
    public  class CentroCustoViewModel
    {
        public long codigo { get; set; }
        public string descricaoCentroCusto { get; set; }
        public string CentroCustoSAP { get; set; }
        public bool indicaAtivo { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CentroCustoResultadoDTO, CentroCustoViewModel>();

            mapper.CreateMap<CentroCustoViewModel, CentroCustoResultadoDTO>();

            mapper.CreateMap<CentroCustoViewModel, CentroCusto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.codigo))
            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.descricaoCentroCusto))
            .ForMember(dest => dest.CodigoCentroCustoSAP, opt => opt.MapFrom(orig => orig.CentroCustoSAP))
            .ForMember(dest => dest.IndicaAtivo, opt => opt.MapFrom(orig => orig.indicaAtivo));
          ;
        }
    }
}
