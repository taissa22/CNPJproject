using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel {
    public class ComarcaViewModel : BaseViewModel<long>
    {
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public long CodEstado { get; set; }
        public long SeqComarca { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ComarcaViewModel, Comarca>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome));
            mapper.CreateMap<Comarca, ComarcaViewModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome));
        }
    }
}
