using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento
{
    public class FormaPagamentoGridViewModel
    {
        [Name("Código")]
        public long Codigo { get; set; }
        [Name("Descrição da Forma de Pagamento")]
        public string DescricaoFormaPagamento { get; set; }
        [Name("Requer Borderô")]
        public bool RequerBordero { get; set; }
        [Name("Restrita")]
        public bool Restrita { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FormaPagamentoGridDTO, FormaPagamentoGridViewModel>();

            mapper.CreateMap<FormaPagamentoGridViewModel, FormaPagamentoGridDTO>();
        }
    }
}
