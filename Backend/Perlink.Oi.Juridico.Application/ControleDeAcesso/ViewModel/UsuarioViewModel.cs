using AutoMapper;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel {
    public class UsuarioViewModel : BaseViewModel<string> {

        public string Nome { get; set; }
        public string CPF { get; set; }


        public static void Mapping(Profile mapper) {
            mapper.CreateMap<UsuarioViewModel, Usuario>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(orig => orig.CPF));

            mapper.CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(orig => orig.CPF));
        }
    }
}