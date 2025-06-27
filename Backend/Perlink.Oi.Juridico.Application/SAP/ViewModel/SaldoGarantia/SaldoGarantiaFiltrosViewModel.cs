using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia
{
    public class SaldoGarantiaFiltrosViewModel
    {
        public IEnumerable<BancoListaDTO> ListaBancos { get; set; }
        public IEnumerable<EmpresaDoGrupoDTO> ListaEmpresaDoGrupo { get; set; }
        public IEnumerable<EstadoDTO> ListaEstados { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaFiltrosDTO, SaldoGarantiaFiltrosViewModel>();
            mapper.CreateMap<SaldoGarantiaFiltrosViewModel, SaldoGarantiaFiltrosDTO>();
        }
    }
}
