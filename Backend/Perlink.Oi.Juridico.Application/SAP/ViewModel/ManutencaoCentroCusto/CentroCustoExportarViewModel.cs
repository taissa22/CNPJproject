using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto
{
    public class CentroCustoExportarViewModel
    {
        [Name("Código")]
        public long codigo { get; set; }
        [Name("Descrição do Centro Custo")]
        public string descricaoCentroCusto { get; set; }
        [Name("Centro Custo SAP")]
        public string centroCustoSAP { get; set; }
        [Name("Ativo")]
        public string indicaAtivo { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CentroCustoResultadoDTO, CentroCustoExportarViewModel>()
              .ForMember(dest => dest.indicaAtivo, opt => opt.MapFrom(orig => orig.IndicaAtivo.RetornaSimNao()))
              .ForMember(dest => dest.centroCustoSAP, opt => opt.MapFrom(orig => $"'{orig.CentroCustoSAP}"));
            mapper.CreateMap<CentroCustoExportarViewModel, CentroCustoResultadoDTO>();
            ;
        }

    }
}
