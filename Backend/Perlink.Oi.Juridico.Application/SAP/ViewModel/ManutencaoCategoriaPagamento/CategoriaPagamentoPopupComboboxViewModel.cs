using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoPopupComboboxViewModel
    {
        public IEnumerable<ComboboxDTO> FornecedoresPermitidos { get; set; }
        public IEnumerable<ComboboxDTO> ClassesGarantias { get; set; }
        public IEnumerable<ComboboxDTO> GrupoCorrecao { get; set; }
        public IEnumerable<ComboboxDTO> PagamentoA { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoPopupComboboxDTO, CategoriaPagamentoComboboxViewModel>();
            mapper.CreateMap<CategoriaPagamentoComboboxViewModel, CategoriaPagamentoPopupComboboxDTO > ();

        }
    }
}
