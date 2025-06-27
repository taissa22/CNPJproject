using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros
{
    public class LoteCriteriosFiltrosViewModel
    {
        public IEnumerable<EmpresaDoGrupoDTO> ListaEmpresaDoGrupo { get; set; }
        public IEnumerable<EscritorioDTO> ListaEscritorio { get; set; }
        public IEnumerable<FornecedorDTOFiltro> ListaFornecedor { get; set; }
        public IEnumerable<CentroCusto> ListaCentroCusto { get; set; }
        public IEnumerable<TipoLancamento> ListaTipodeLancamento { get; set; }
        public IEnumerable<StatusPagamento> ListaStatusPagamento { get; set; }
        public IEnumerable<CategoriaDePagamentofiltroDTO> ListaCategoriaPagamento { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteCriteriosFiltrosDTO, LoteCriteriosFiltrosViewModel>();

            mapper.CreateMap<LoteCriteriosFiltrosViewModel, LoteCriteriosFiltrosDTO>();
        }
    }
}