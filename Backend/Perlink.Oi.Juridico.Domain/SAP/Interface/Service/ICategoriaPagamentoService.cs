using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface ICategoriaPagamentoService : IBaseCrudService<CategoriaPagamento, long>
    {
        Task<bool> ExisteTipoMovimentoGarantia(long codigoCategoriaPagamento);

        Task ExcluirCategoria(long codigoCategoriaPagamento);

        Task<IEnumerable<ComboboxDTO>> RecuperarComboBoxTipoProcesso();

        Task<IEnumerable<ComboboxDTO>> RecuperarComboboxTipoLancamento(long tipoProcesso);

        Task<CategoriaPagamento> CadastrarCategoria(CategoriaPagamento categoriaPagamento);

        Task<IEnumerable<ComboboxDTO>> RecuperarComboboxGrupoCorrecao(long tipoProcesso);

        IEnumerable<ComboboxDTO> RecuperarComboboxFornecedoresPermitidos();

        IEnumerable<ComboboxDTO> RecuperarComboboxPagamentoA();

        Task<IEnumerable<ComboboxDTO>> RecuperarComboboxClasseGarantia(long tipoLancamento);

        Task<ICollection<CategoriaDePagamentoResultadoDTO>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<ICollection<CategoriaDePagamentoEstrategicoDTO>> BuscarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<ICollection<CategoriaDePagamentoConsumidorDTO>> BuscarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<ICollection<CategoriaPagamentoExportacaoDTO>> ExportarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<ICollection<CategoriaPagamentoExtrategicoExportacaoDTO>> ExportarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<ICollection<CategoriaPagamentoConsumidorExportacaoDTO>> ExportarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);

        Task<CategoriaPagamento> AlterarCategoriaPagamento(CategoriaPagamento objCategoriaPagamento);

        Task<string> EnvioSapIsValido(CategoriaPagamento categoria);

        Task<string> ValidaHistorica(CategoriaPagamento categoria);

        Task<string> PagamentoAIsValido(CategoriaPagamento categoria);

        Task<bool> ExibeNotificacaoAoEditar(CategoriaPagamento categoria);

        Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxEstrategico();

        Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxConsumidor();


    }
}

