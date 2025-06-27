using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento
{
    public class CategoriaMIgracaoEstrategicoPopupComboboxViewModel
    {       

        public IEnumerable<CategoriaDePagamentoDTO> CategoriaPagamento { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaDePagamentoDTO, CategoriaMIgracaoEstrategicoPopupComboboxViewModel>();
            mapper.CreateMap<CategoriaMIgracaoEstrategicoPopupComboboxViewModel, CategoriaDePagamentoDTO>();

        }

    }
}
