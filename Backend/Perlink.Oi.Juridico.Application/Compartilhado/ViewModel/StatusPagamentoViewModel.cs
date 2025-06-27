using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel {
    public class StatusPagamentoViewModel : BaseViewModel<long>
    {
       
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<StatusPagamentoViewModel, StatusPagamento>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
            mapper.CreateMap<StatusPagamento, StatusPagamentoViewModel>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
        }
    }
}
