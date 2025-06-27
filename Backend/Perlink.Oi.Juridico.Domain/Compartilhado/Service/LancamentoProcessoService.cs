using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class LancamentoProcessoService : BaseCrudService<LancamentoProcesso, long>, ILancamentoProcessoService
    {
        private readonly ILancamentoProcessoRepository repository;
        private readonly ITipoParticipacaoRepository _tipoParticipacaoRepository;
        private readonly ICompromissoProcessoParcelaService compromissoProcessoParcelaService;

        public LancamentoProcessoService(ILancamentoProcessoRepository repository, ITipoParticipacaoRepository tipoParticipacaoRepository, ICompromissoProcessoParcelaService compromissoProcessoParcelaService) : base(repository)
        {
            this.repository = repository;
            _tipoParticipacaoRepository = tipoParticipacaoRepository;
            this.compromissoProcessoParcelaService = compromissoProcessoParcelaService;
        }

        public async Task<ICollection<LancamentoProcesso>> RecuperarPorParteNumeroGuia(long NumeroGuia)
        {
            return await repository.RecuperarPorParteNumeroGuia(NumeroGuia);
        }

        public async Task<long> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso)
        {
            return await repository.RecuperarLancamentoProcesso(NumeroGuia, CodigoTipoProcesso);
        }

        public async Task<ICollection<LoteLancamentoDTO>> RecuperarLancamentoLote(long codigoLote)
        {
            return await repository.ObterLancamentoDoLote(codigoLote);
        }

        public async Task<ICollection<LoteLancamentoExportacaoDTO>> ExportarLancamentoDoLote(long codigoLote)
        {
            return await repository.ExportarLancamentoDoLote(codigoLote);
        }

        public async Task<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraDTO>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros)
        {
            return await repository.ObterLotesAgrupadoPorEmpresaCentralizadora(filtros);
        }

        public async Task<ICollection<LoteCriacaoResultadoEmpresaGrupoDTO>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros)
        {
            return await repository.ObterLotesAgrupadoPorEmpresadoGrupo(filtros);
        }

        public async Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros)
        {
            return await repository.ObterTotalLotesAgrupadoPorEmpresadoGrupo(filtros);
        }

        public async Task<ICollection<LoteCriacaoResultadoLancamentoDTO>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger)
        {
            return await repository.ObterLancamentosLoteCriacao(filtros, logger);
        }

        public async Task<bool> ExisteLancamentoProcessoComFornecedor(long codigoFornecedor)
        {
            return await repository.ExisteLancamentoProcessoComFornecedor(codigoFornecedor);
        }

        public async Task AlterarDataEnvioEscritorio(LancamentoProcesso lancamentoProcesso)
        {
            await repository.AlterarDataEnvioEscritorio(lancamentoProcesso);
        }

        public async Task<bool> ExisteLancamentoProcessoComFormaPagamento(long codigoFormaPagamento)
        {
            var repositoryTask = await repository.ExisteLancamentoProcessoComFormaPagamento(codigoFormaPagamento);

            return repositoryTask;
        }

        public async Task<LancamentoProcesso> ObterLancamentoProcesso(long codigoProcesso, long codigoLancamento)
        {
            return await repository.ObterLancamentoProcesso(codigoProcesso, codigoLancamento);
        }

        public async Task<ICollection<DadosLancamentoEstornoDTO>> ConsultaLancamentoEstorno(long codigoProcesso, long codigoTipoProcesso)
        {
            var lancamentosProcesso = await repository.ConsultaLancamentoEstorno(codigoProcesso, codigoTipoProcesso);
            ICollection<DadosLancamentoEstornoDTO> dadosLancamentosEstornoDTO = new Collection<DadosLancamentoEstornoDTO>();
            foreach (var item in lancamentosProcesso)
            {
                var compromissoProcessoDados = await compromissoProcessoParcelaService.ObterDadosCompromissoParaEstorno(item.Id, item.CodigoLancamento);

                var lancamento = new DadosLancamentoEstornoDTO()
                {
                    CodigoProcesso = item.Id,
                    CodigoLancamento = item.CodigoLancamento,
                    CodigoTipoLancamento = item.CodigoTipoLancamento,
                    DescricaoTipoLancamento = item.TipoLancamento.Descricao,
                    DataCriacaoPedido = item.DataCriacaoPedido.HasValue ? item.DataCriacaoPedido.Value.ToString("dd/MM/yyy") : string.Empty,
                    PedidoSAP = item.NumeroPedidoSap.ToString(),
                    DataRecebimentoFiscal = item.DataRecebimentoFiscal.HasValue ? item.DataRecebimentoFiscal.Value.ToString("dd/MM/yyy") : string.Empty,
                    DataPagamento = item.DataPagamentoPedido.HasValue ? item.DataPagamentoPedido.Value.ToString("dd/MM/yyy") : string.Empty,
                    DataLancamento = item.DataLancamento.ToString("dd/MM/yyy"),
                    Valor = item.ValorLancamento,
                    StatusPagamento = item.StatusPagamento.Descricao,
                    CodigoCategoriaPagamento = item.CodigoCatPagamento,
                    CategoriaPagamento = item.CategoriaPagamento.DescricaoCategoriaPagamento,
                    FormaPagamento = item.FormaPagamento.DescricaoFormaPagamento,
                    Fornecedor = item.Fornecedor.NomeFornecedor,
                    Centro = item.CentroCusto.Descricao,
                    CentroCusto = item.CodigoCentroSAP,
                    DataLevantamento = item.DataGarantiaLevantada.HasValue ? item.DataGarantiaLevantada.Value.ToString("dd/MM/yyy") : string.Empty,
                    ParteEfetivouLevantamento = item.CodigoTipoParticipacao.HasValue ? _tipoParticipacaoRepository.Get(item.CodigoTipoParticipacao.Value).Descricao : string.Empty, //verificar
                    Comentario = item.Comentario,
                    CriarNovaParcelaFutura = false,
                    ReduzirPagamentoCredor = false,
                    QuantidadeCredoresAssociados = compromissoProcessoDados != null ? compromissoProcessoDados.QuantidadeCredores : 0,
                    ValorCompromisso = compromissoProcessoDados != null ? compromissoProcessoDados.ValorCompromisso : 0,
                    CodigoCompromisso = compromissoProcessoDados != null ? compromissoProcessoDados.CodigoCompromisso : 0,
                    CodigoParcela = compromissoProcessoDados != null ? compromissoProcessoDados.CodigoParcela : 0
                };
                dadosLancamentosEstornoDTO.Add(lancamento);
            }
            return dadosLancamentosEstornoDTO;
        }

        public async Task AtualizarLancamentoProcesso(LancamentoProcesso lancamentoprocesso)
        {
            await repository.Atualizar(lancamentoprocesso);
        }

        public async Task<CargaCompromissoParcela> RecuperarCompromissoParcela(DadosLancamentoEstornoDTO lancamento)
        {
            return await repository.AtualizarParcelaCompromisso(lancamento.CodigoProcesso, lancamento.CodigoLancamento);
        }

        public async Task<long?> RecuperarNumeroContaJudicial(long numeroContaJudicial)
        {
            return await repository.RecuperarNumeroContaJudicial(numeroContaJudicial);
        }

        public async Task<bool> ExisteCategoriaPagAssociadoLancamento(long codigoCategoriaPagamento)
        {
            return await repository.ExisteCategoriaPagAssociadoLancamento(codigoCategoriaPagamento);
        }

        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            return await repository.ExisteCentroCustoAssociado(codigoCentroCusto);
        }

        public async Task<bool> ExisteLamentoProcessoAssociadoComStatusParcela(long CodigoBBStatusParcela)
        {
            return await repository.ExisteLamentoProcessoAssociadoComStatusParcela(CodigoBBStatusParcela);
        }

        public async Task<bool> ExisteBBModalidadeAssociadoLancamento(long id)
        {
            return await repository.ExisteBBModalidadeAssociadoLancamento(id);
        }
    }
}