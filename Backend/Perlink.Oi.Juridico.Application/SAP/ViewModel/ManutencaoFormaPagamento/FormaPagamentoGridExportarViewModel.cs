using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento
{
    class FormaPagamentoGridExportarViewModel
    {
        [Name("Código")]
        public long Codigo { get; set; }
        [Name("Descrição da Forma de Pagamento")]
        public string DescricaoFormaPagamento { get; set; }
        [Name("Requer Borderô")]
        public string RequerBordero { get; set; }
        [Name("Restrita")]
        public string Restrita { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FormaPagamentoGridExportarDTO, FormaPagamentoGridExportarViewModel>()
                  .ForMember(dest => dest.Codigo, opt => opt.MapFrom(orig => orig.Codigo))
                  .ForMember(dest => dest.DescricaoFormaPagamento, opt => opt.MapFrom(orig => orig.DescricaoFormaPagamento))
                  .ForMember(dest => dest.RequerBordero, opt => opt.MapFrom(orig => orig.RequerBordero ? "Sim" : "Não"))
                  .ForMember(dest => dest.Restrita, opt => opt.MapFrom(orig => orig.Restrita ? "Sim" : "Não"));
        }
    }
}
