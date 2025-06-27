using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia
{
    public class AgendamentoSaldoGarantiaViewModel : BaseViewModel<long>
    {
       
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendamentoSaldoGarantia, AgendamentoSaldoGarantiaViewModel>();
            mapper.CreateMap<AgendamentoSaldoGarantiaViewModel, AgendamentoSaldoGarantia>();
        }
    }
    public class AgendamentoResultadoViewModel
    {
        public long Id { get; set; }
        public string NomeAgendamento { get; set; }
        public string StatusAgendamento { get; set; }
        public string DataAgendamento { get; set; }
        public string DataFinalizacao { get; set; }
        public string MensagemErro { get; set; }
        public string NomeArquivo { get; set; }
        public long TipoProcesso { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendamentoResultadoViewModel, AgendamentoResultadoDTO>();

            mapper.CreateMap<AgendamentoResultadoDTO, AgendamentoResultadoViewModel>()
                .ForMember(dest => dest.DataAgendamento, opt => opt.MapFrom(orig => orig.DataAgendamento.ToString("dd/MM/yyyy HH:mm")));

        }
    }
}
