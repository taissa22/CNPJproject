using AutoMapper;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel {
    public class PermissaoViewModel : BaseViewModel<string> {
        public PermissaoEnum Permissao { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<PermissaoViewModel, Permissao>()
                .ForMember(dest => dest.Menu, opt => opt.MapFrom(orig => orig.Permissao));

            mapper.CreateMap<Permissao, PermissaoViewModel>()
                .ForMember(dest => dest.Permissao, opt => opt.MapFrom(orig => orig.Menu));
        }
    }
}