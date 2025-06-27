using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class EstadoViewModel : BaseViewModel<string>
    {
        public string Descricao { get; set; }
        public bool Persistido { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EstadoDTO, EstadoViewModel>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao))
                  .ForMember(dest => dest.Persistido, opt => opt.MapFrom(orig => orig.Persistido));
        }
    }
}