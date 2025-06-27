
using System;
using AutoMapper;
using global::Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class Log_LoteViewModel : BaseViewModel<long>
    {
        public string Operacao { get; set; }
        public DateTime DataLog { get; set; }
        public long CodigoStatusPagamentoAntes { get; set; }
        public long CodigoStatusPagamentoDepois { get; set; }
        public string NomeUsuario { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<Log_LoteViewModel, Log_Lote>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.Operacao, opt => opt.MapFrom(orig => orig.Operacao))
              .ForMember(dest => dest.DataLog, opt => opt.MapFrom(orig => orig.DataLog))
              .ForMember(dest => dest.CodigoStatusPagamentoAntes, opt => opt.MapFrom(orig => orig.CodigoStatusPagamentoAntes))
              .ForMember(dest => dest.CodigoStatusPagamentoDepois, opt => opt.MapFrom(orig => orig.CodigoStatusPagamentoDepois));


            mapper.CreateMap<Log_Lote, Log_LoteViewModel>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                  .ForMember(dest => dest.Operacao, opt => opt.MapFrom(orig => orig.Operacao))
                  .ForMember(dest => dest.DataLog, opt => opt.MapFrom(orig => orig.DataLog))
                  .ForMember(dest => dest.CodigoStatusPagamentoAntes, opt => opt.MapFrom(orig => orig.CodigoStatusPagamentoAntes))
                  .ForMember(dest => dest.CodigoStatusPagamentoDepois, opt => opt.MapFrom(orig => orig.CodigoStatusPagamentoDepois));



        }
    }
}
