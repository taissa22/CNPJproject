using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel
{
    public class EmpresaViewModel : BaseViewModel<long>
    {
        public string Nome { get; set; }
        public long ParteId { get; set; }
        public bool Persistido { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EmpresaDoGrupoDTO, EmpresaViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.ParteId, opt => opt.MapFrom(orig => orig.ParteId))
                .ForMember(dest => dest.Persistido, opt => opt.MapFrom(orig => orig.Persistido))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome));
        }
    }
}