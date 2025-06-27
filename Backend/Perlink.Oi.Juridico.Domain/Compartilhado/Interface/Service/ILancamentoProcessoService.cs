using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ILancamentoProcessoService : IBaseCrudService<LancamentoProcesso, long>
    {
        Task<ICollection<LancamentoProcesso>> RecuperarPorParteNumeroGuia(long NumeroGuia);
        Task<long> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso);
        Task<ICollection<LoteLancamentoDTO>> RecuperarLancamentoLote(long codigoLote);
        Task<ICollection<LoteLancamentoExportacaoDTO>> ExportarLancamentoDoLote(long codigoLote);
        Task<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraDTO>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros);
        Task<ICollection<LoteCriacaoResultadoEmpresaGrupoDTO>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<ICollection<LoteCriacaoResultadoLancamentoDTO>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger);
        Task<bool> ExisteLancamentoProcessoComFornecedor(long codigoFornecedor);
        Task<bool> ExisteLancamentoProcessoComFormaPagamento(long codigoFormaPagamento);
        Task AlterarDataEnvioEscritorio(LancamentoProcesso lancamentoProcesso);
        Task<LancamentoProcesso> ObterLancamentoProcesso(long codigoProcesso, long codigoLancamento);
        Task<ICollection<DadosLancamentoEstornoDTO>> ConsultaLancamentoEstorno(long codigoProcesso, long codigoTipoProcesso);
        Task AtualizarLancamentoProcesso(LancamentoProcesso lancamentoprocesso);
        Task<long?> RecuperarNumeroContaJudicial(long numeroContaJudicial);
        Task<bool> ExisteCategoriaPagAssociadoLancamento(long codigoCategoriaPagamento);
        Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto);
        Task<bool> ExisteLamentoProcessoAssociadoComStatusParcela(long CodigoBBStatusParcela);
        Task<bool> ExisteBBModalidadeAssociadoLancamento(long id);

        Task<CargaCompromissoParcela> RecuperarCompromissoParcela(DadosLancamentoEstornoDTO lancamento);
    }
}
