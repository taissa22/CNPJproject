using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class CompromissoLoteViewModel
    {
        public int QuantidadeCredoresAssociados { get; set; }
        public decimal ValorCompromisso { get; set; }
        public long CodigoCompromisso { get; set; }
        public long CodigoParcela { get; set; }
        public decimal ValorParcela { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CompromissoLoteDTO, CompromissoLoteViewModel>();
            mapper.CreateMap<CompromissoLoteViewModel, CompromissoLoteDTO>();
        }
    }
}
