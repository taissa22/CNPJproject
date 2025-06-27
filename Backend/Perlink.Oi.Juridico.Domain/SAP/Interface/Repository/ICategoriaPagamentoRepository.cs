using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface ICategoriaPagamentoRepository : IBaseCrudRepository<CategoriaPagamento, long>
    {
        bool CodigoMaterialSAPValido(long CodigoCategoriaPagamento);
        Task<bool> ExisteTipoMovimentoGarantia(long codigoCategoriaPagamento);
        Task<ICollection<CategoriaDePagamentoResultadoDTO>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<ICollection<CategoriaDePagamentoEstrategicoDTO>> BuscarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<ICollection<CategoriaDePagamentoConsumidorDTO>> BuscarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<ICollection<CategoriaPagamentoExportacaoDTO>> ExportarCategoriaPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<ICollection<CategoriaPagamentoExtrategicoExportacaoDTO>> ExportarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO filtros);

        Task<ICollection<CategoriaPagamentoConsumidorExportacaoDTO>> ExportarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO filtros);

        Task<CategoriaPagamento> CadastrarCategoria(CategoriaPagamento categoriaPagamento);
        Task<CategoriaPagamento> AlterarCategoriaPagamento(CategoriaPagamento categoriaPagamento);
        Task<ICollection<CategoriaDePagamentofiltroDTO>> ListaCategoriaPagamento(long codigoTipoProcesso);
        Task<string> EnvioSapIsValido(CategoriaPagamento categoria);
        Task<string> ValidaHistorica(CategoriaPagamento categoria);
        Task<bool> ExibeNotificacaoAoEditar(CategoriaPagamento categoria);
        Task<string> PagamentoAIsValido(CategoriaPagamento categoria);
        Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxEstrategico();
        Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxConsumidor();

    }
}
