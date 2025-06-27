using AutoMapper;
using Shared.Application.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros
{
    public class BancoViewModel : BaseViewModel<long>
    {
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BancoViewModel, Banco>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.NomeBanco, opt => opt.MapFrom(orig => orig.Descricao));

            mapper.CreateMap<Banco, BancoViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.NomeBanco));
        }
    }
}
