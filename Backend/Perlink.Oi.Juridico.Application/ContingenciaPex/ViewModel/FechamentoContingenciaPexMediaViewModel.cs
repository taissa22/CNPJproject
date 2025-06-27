using AutoMapper;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using Shared.Application.ViewModel;
using Shared.Tools;
using System;

namespace Perlink.Oi.Juridico.Application.ContingenciaPex.ViewModel
{
    public class FechamentoContingenciaPexMediaViewModel : BaseViewModel<long>
    {
        public DateTime DataFechamento { get; set; }
        public string NomeUsuario { get; set; }
        public string NumeroMeses { get; set; }
        public string PercentualHaircut { get; set; }
        public string MultDesvioPadrao { get; set; }
        public string IndAplicarHaircut { get; set; }
        public DateTime DataExecucao { get; set; }
        public string Empresas { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FechamentoContingenciaPexMediaDTO, FechamentoContingenciaPexMediaViewModel>();
            mapper.CreateMap<FechamentoContingenciaPexMediaViewModel, FechamentoContingenciaPexMediaDTO>();
        }
    }
}
