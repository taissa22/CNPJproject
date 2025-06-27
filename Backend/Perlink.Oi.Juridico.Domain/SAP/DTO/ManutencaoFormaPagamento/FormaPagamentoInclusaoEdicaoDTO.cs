using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento
{
    public class FormaPagamentoInclusaoEdicaoDTO
    {
        public long Codigo { get; set; }
        public string DescricaoFormaPagamento { get; set; }

        public bool Restrita { get; set; }
        public bool RequerBordero { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FormaPagamentoInclusaoEdicaoDTO, FormaPagamento>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Codigo))
            .ForMember(dest => dest.DescricaoFormaPagamento, opt => opt.MapFrom(orig => orig.DescricaoFormaPagamento))
            .ForMember(dest => dest.IndicaRestrita, opt => opt.MapFrom(orig => orig.Restrita))
            .ForMember(dest => dest.IndicaBordero, opt => opt.MapFrom(orig => orig.RequerBordero));
            mapper.CreateMap<FormaPagamento, FormaPagamentoInclusaoEdicaoDTO>();
        }
    }
}