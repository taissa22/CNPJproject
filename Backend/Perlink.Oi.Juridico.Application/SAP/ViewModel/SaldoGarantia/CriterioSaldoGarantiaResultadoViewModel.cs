using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia
{
    public class CriterioSaldoGarantiaResultadoViewModel
    {

        public string NomeCriterio { get; set; }
        public string Valor { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CriterioSaldoGarantiaResultadoViewModel, CriteriosSaldoGarantiaResultadoDTO>();

            mapper.CreateMap<CriteriosSaldoGarantiaResultadoDTO, CriterioSaldoGarantiaResultadoViewModel>();

        }
    }
}
