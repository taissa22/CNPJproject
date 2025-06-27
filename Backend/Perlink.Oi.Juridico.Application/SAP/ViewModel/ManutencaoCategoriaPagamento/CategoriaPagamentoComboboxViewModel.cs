using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoComboboxViewModel
    {
        public IEnumerable<ComboboxDTO> TiposProcessos { get; set; }
        public IEnumerable<ComboboxDTO> TiposLancamentos { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoComboboxDTO, CategoriaPagamentoComboboxViewModel>();
        }
    }
}
