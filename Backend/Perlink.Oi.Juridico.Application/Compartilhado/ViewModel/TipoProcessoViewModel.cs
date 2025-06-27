using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class TipoProcessoViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<TipoProcessoViewModel, TipoProcesso>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));

            mapper.CreateMap<TipoProcesso, TipoProcessoViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
        }
    }
}