using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Shared.Domain.Interface.Repository;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ILancamentoProcessoRepository : IBaseCrudRepository<LancamentoProcesso, long>
    {
        Task<ICollection<LancamentoProcesso>> RecuperarPorParteNumeroGuia(long NumeroGuia);
        Task<long> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso);
        Task<long> ObterTotalLancamentosDoLote(List<long> lista);
        Task<ICollection<LoteLancamentoDTO>> ObterLancamentoDoLote(long codigoLote);
        Task<string> RecuperarDataEnvioEscritorio (long codigoLote);
        Task<ICollection<LoteLancamentoExportacaoDTO>> ExportarLancamentoDoLote(long codigoLote);
        Task<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraDTO>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros);
        Task<ICollection<LoteCriacaoResultadoEmpresaGrupoDTO>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<ICollection<LoteCriacaoResultadoLancamentoDTO>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger);
        Task<LancamentoProcesso> ObterLancamentoProcesso(long CodigoProcesso, long CodigoLancamento);

        Task<CargaCompromissoParcela> AtualizarParcelaCompromisso(long codProcesso, long codLancamento);

        Task<bool> ExisteLancamentoProcessoComFornecedor(long codigoFornecedor);
        Task AlterarDataEnvioEscritorio(LancamentoProcesso lancamentoProcesso);
        Task<ICollection<LancamentoProcesso>> ConsultaLancamentoEstorno(long codigoProcesso, long codigoTipoProcesso);
        Task<long?> RecuperarNumeroContaJudicial(long numeroContaJudicial);
        Task<bool> ExisteCategoriaPagAssociadoLancamento(long codigoCategoriaPagamento);
        Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto);
        Task<bool> ExisteLancamentoProcessoComFormaPagamento(long codigoFormaPagamento);
        Task<bool> ExisteLamentoProcessoAssociadoComStatusParcela(long CodigoBBStatusParcela);
        Task<bool> ExisteBBModalidadeAssociadoLancamento(long id);
    }
}
