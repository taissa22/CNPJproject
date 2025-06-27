using AutoMapper;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel {
    public class ParametroViewModel : BaseViewModel<string> {
        public string TipoParametro { get; set; }
        public string Descricao { get; set; }
        public string Conteudo { get; set; }


        public static void Mapping(Profile mapper) {
            mapper.CreateMap<ParametroViewModel, Parametro>()
                .ForMember(dest => dest.TipoParametro, opt => opt.MapFrom(orig => orig.TipoParametro))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao))
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(orig => orig.Conteudo));

            mapper.CreateMap<Parametro, ParametroViewModel>()
                .ForMember(dest => dest.TipoParametro, opt => opt.MapFrom(orig => orig.TipoParametro))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao))
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(orig => orig.Conteudo));
        }
    }
}