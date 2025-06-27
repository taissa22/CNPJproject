using System;
using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class Log_LoteHistoricoViewModel : BaseViewModel<long>
    {
        public string DataLog { get; set; }
       public string DescricaoStatusPagamento { get; set; }
        public string NomeUsuario { get; set; }



        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LogLoteDTO, Log_LoteHistoricoViewModel>();

            mapper.CreateMap<Log_LoteHistoricoViewModel, LogLoteDTO>();
        }

    }


}
