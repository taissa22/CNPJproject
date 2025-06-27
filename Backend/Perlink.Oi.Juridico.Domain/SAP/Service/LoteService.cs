using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Perlink.Oi.Juridico.Infra.Providers;
using Shared.Domain.Impl.Service;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class LoteService : BaseCrudService<Lote, long>, ILoteService
    {
        private readonly ILoteRepository repository;
        private readonly ICargaCompromissoParcelaRepository cargaCompromissoParcelaRepository;
        private readonly ILoteLancamentoRepository loteLancamentoRepository;
        private readonly ILancamentoProcessoRepository lancamentoProcessoRepository;
        private readonly ICompromissoProcessoRepository compromissoProcessoRepository;
        private readonly ICompromissoProcessoParcelaRepository compromissoProcessoParcelaRepository;
        private readonly IParametroService parametroService;
        private readonly ICredorCompromissoRepository credorCompromissoRepository;
        private readonly IProcessoRepository processoRepository;
        private readonly IFeriadosRepository feriadosRepository;
        public IUsuarioAtualProvider UsuarioAtual { get; }

        public LoteService(ILoteRepository repository, IUsuarioAtualProvider user, ILancamentoProcessoRepository lancamentoProcessoRepository, ILoteLancamentoRepository loteLancamentoRepository, ICompromissoProcessoParcelaRepository compromissoProcessoParcelaRepository, IParametroService parametroService, ICredorCompromissoRepository credorCompromissoRepository, IProcessoRepository processoRepository, ICompromissoProcessoRepository compromissoProcessoRepository, IFeriadosRepository feriadosRepository, ICargaCompromissoParcelaRepository cargaCompromissoParcelaRepository) : base(repository)
        {
            this.repository = repository;
            this.lancamentoProcessoRepository = lancamentoProcessoRepository;
            this.loteLancamentoRepository = loteLancamentoRepository;
            this.compromissoProcessoParcelaRepository = compromissoProcessoParcelaRepository;
            this.parametroService = parametroService;
            this.UsuarioAtual = user;
            this.credorCompromissoRepository = credorCompromissoRepository;
            this.processoRepository = processoRepository;
            this.compromissoProcessoRepository = compromissoProcessoRepository;
            this.feriadosRepository = feriadosRepository;
            this.cargaCompromissoParcelaRepository = cargaCompromissoParcelaRepository;
        }

        public async Task<ICollection<Lote>> RecuperarPorParteCodigoSAP(long CodigoSapParte)
        {
            return await repository.RecuperarPorParteCodigoSAP(CodigoSapParte);
        }

        public async Task<string> RecuperarPorCodigoSAP(long Cod)
        {
            return await repository.RecuperarPorCodigoSAP(Cod);
        }

        public async Task<long?> AtualizarNumeroLoteBBAsync(long Cod)
        {
            return await repository.AtualizarNumeroLoteBBAsync(Cod);
        }

        public async Task<IEnumerable<Lote>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax)
        {
            return await repository.RecuperarPorDataCriacaoLote(DataCriacaoMin, DataCriacaoMax);
        }

        public async Task<IEnumerable<Lote>> RecuperarPorDataCriacaoPedidoLote(DateTime dataCriacaoPedidoMin, DateTime dataCriacaoPedidoMax)
        {
            return await repository.RecuperarPorDataCriacaoPedidoLote(dataCriacaoPedidoMin, dataCriacaoPedidoMax);
        }

        public async Task<string> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso)
        {
            return await repository.RecuperarPorNumeroSAPCodigoTipoProcesso(NumeroPedidoSAP, CodigoTipoProcesso);
        }
        public async Task<CompromissoLoteDTO> VerificarCompromisso(long codigoLote)
        {
            LoteDetalhesDTO detalhe = await repository.RecuperarDetalhes(codigoLote);

            if (detalhe.QuantLancamento == 1)
            {
                var lancamento = await loteLancamentoRepository.RetornaLancamentoDoLote(codigoLote);
                var compromissoProcessoDados = await compromissoProcessoParcelaRepository.ObterDadosCompromissoParaEstorno(lancamento.Id, lancamento.CodigoLancamento);
                if (compromissoProcessoDados != null)
                {
                    return new CompromissoLoteDTO
                    {
                        QuantidadeCredoresAssociados = compromissoProcessoDados != null ? compromissoProcessoDados.QuantidadeCredores : 0,
                        ValorCompromisso = compromissoProcessoDados != null ? compromissoProcessoDados.ValorCompromisso : 0,
                        CodigoCompromisso = compromissoProcessoDados != null ? compromissoProcessoDados.CodigoCompromisso : 0,
                        CodigoParcela = compromissoProcessoDados != null ? compromissoProcessoDados.CodigoParcela : 0,
                        ValorParcela = compromissoProcessoDados.ValorParcela
                    };
                }
            }

            return null;
        }

        public async Task<LoteDetalhesDTO> RecuperaDetalhes(long codigoLote)
        {
            return await repository.RecuperarDetalhes(codigoLote);
        }

        public async Task<LoteCanceladoDTO> CancelamentoLote(LoteCancelamentoDTO loteCancelamentoDTO, CargaCompromissoParcelaCancelamentoDTO infos=null)
        {            
            Lote lote = await repository.RecuperarPorId(loteCancelamentoDTO.CodigoLote);

            switch (lote.CodigoStatusPagamento)
            {
                case (int)StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP:
                    lote = await CancelarLoteTramitadoNoSAP(lote, lote.CodigoStatusPagamento, (long)StatusPagamentoEnum.LoteAutomaticoCancelado, loteCancelamentoDTO);
                    break;

                case (int)StatusPagamentoEnum.PedidoSAPRecebidoFiscal:
                    lote = await CancelarLoteTramitadoNoSAP(lote, lote.CodigoStatusPagamento, (long)StatusPagamentoEnum.LoteAutomaticoCancelado, loteCancelamentoDTO);
                    await cargaCompromissoParcelaRepository.GravarCancelamento(lote.Id,
                        $"Cancelamento solicitado por {infos.UsrCodSolicCancelamento} em {infos.DataCodSolicCancelamento}. Cancelamento no SAP confirmado em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}",
                        "Lote cancelado pelo usuário", "6" );
                    break;

                case (int)StatusPagamentoEnum.PedidoSAPCriadoAguardando:
                    lote.CodigoStatusPagamento = (long)StatusPagamentoEnum.AguardandoEnvioCancelamento;
                    await repository.Atualizar(lote);
                    await repository.CommitAsync();

                    break;
            }

            // Adicionando informações
            if (infos != null)
            {
                await cargaCompromissoParcelaRepository.GravarLogSolicCancelamento(lote.Id, infos.UsrCodSolicCancelamento, infos.DataCodSolicCancelamento);
            }

            return new LoteCanceladoDTO
            {
                CodigoLote = lote.Id,
                CodigoStatusPagamento = lote.CodigoStatusPagamento
            };
        }

        private async Task<Lote> CancelarLoteTramitadoNoSAP(Lote lote, long codigoStatusPagamentoAnterior, long codigoStatusPagamento, LoteCancelamentoDTO loteCancelamentoDTO)
        {
            lote.CodigoStatusPagamento = codigoStatusPagamento;

            int novoStatusPagamentoLancamento = 0;

            switch (codigoStatusPagamentoAnterior)
            {
                case (long)StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP:
                case (long)StatusPagamentoEnum.PedidoSAPRecebidoFiscal:
                    novoStatusPagamentoLancamento = (int)StatusPagamentoEnum.LancamentoAutomaticoCancelado;
                    break;
                default:
                    return lote;
            }

            List<LancamentoProcesso> lancamentos = await loteLancamentoRepository.RetornaLancamentosDoLote(lote.Id);

            foreach (LancamentoProcesso lancamento in lancamentos)
            {
                Processo processo = await processoRepository.RecuperarPorId(lancamento.Id);

                if (processo.CodigoTipoProcesso != loteCancelamentoDTO.CodigoTipoProcesso)
                    throw new Exception("Tipo do processo do lançamento diferente do informado");

                lancamento.CodigoStatusPagamento = novoStatusPagamentoLancamento;
                await lancamentoProcessoRepository.Atualizar(lancamento);

                if (loteCancelamentoDTO.CodigoTipoProcesso == 2)
                {
                    var dadoscompromissoParcela = await compromissoProcessoParcelaRepository.ObterDadosCompromissoParaEstorno(lancamento.Id, lancamento.CodigoLancamento);

                    if (dadoscompromissoParcela != null)
                    {
                        var cp = await compromissoProcessoParcelaRepository.ObterCompromissoParcela(lancamento.Id, dadoscompromissoParcela.CodigoCompromisso, dadoscompromissoParcela.CodigoParcela);

                        cp.CodigoStatusCompromissoParcela = 6;
                        cp.CodMotivoSuspCancelParcela = Convert.ToInt64(parametroService.RecuperarPorNome("MOT_PAD_CANC_LOTE_CANCEL").Conteudo);
                        cp.Comentario = $"Cancelamento do lote solicitado pelo usuário {UsuarioAtual.ObterUsuario().Nome} em {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.";
                        cp.DataCancelamento = DateTime.Now;

                        await compromissoProcessoParcelaRepository.Atualizar(cp);

                        // Usuário escolheu reduzir o valor a ser pago ao credor
                        if (dadoscompromissoParcela.QuantidadeCredores == 1 && loteCancelamentoDTO.OpcaoCancelamento != 2)
                        {
                            CredorCompromisso credor = await credorCompromissoRepository.ObterCredorCompromisso(lancamento.Id);
                            credor.ValorPagamentoCredor -= dadoscompromissoParcela.ValorParcela;
                            await credorCompromissoRepository.Atualizar(credor);
                            await credorCompromissoRepository.CommitAsync();
                        }
                        //Crio uma nova parcela se o usuário optou ou se tiver mais de um credor
                        else if (loteCancelamentoDTO.OpcaoCancelamento == 2 || dadoscompromissoParcela.QuantidadeCredores > 1)
                        {
                            //Pego a nova data da parcela:
                            DateTime novaData = DateTime.Now.AddDays(Convert.ToInt64(parametroService.RecuperarPorNome("QTD_DIA_NOVA_PARC_ESTCANC").Conteudo));

                            bool isDataUtil = false;

                            do
                            {
                                if(novaData.DayOfWeek== DayOfWeek.Sunday)
                                    novaData.AddDays(1);
                                else if(novaData.DayOfWeek == DayOfWeek.Saturday)
                                    novaData.AddDays(2);

                                isDataUtil = ! await feriadosRepository.Existe(novaData);

                                if (!isDataUtil)
                                    novaData.AddDays(1);

                            } while (isDataUtil=false);

                            //crio nova parcela e reordeno as parcelas existentes:
                            List<CompromissoProcessoParcela> parcelas = compromissoProcessoParcelaRepository.ObterListaParcelasCompromissoProcessoReordenacao(lancamento.Id, dadoscompromissoParcela.CodigoCompromisso);

                            CompromissoProcessoParcela novaparcela = new CompromissoProcessoParcela
                            {
                                CodigoProcesso = lancamento.Id,
                                Id = cp.Id,
                                CodigoParcela = compromissoProcessoParcelaRepository.ObterProximoCodigoParcela(lancamento.Id, dadoscompromissoParcela.CodigoCompromisso),
                                DataVencimento = novaData,
                                CodigoStatusCompromissoParcela = 1,
                                ValorParcela = dadoscompromissoParcela.ValorParcela
                            };

                            long numeroParcela = 1;

                            foreach (CompromissoProcessoParcela parcela in parcelas.OrderBy(p => p.DataVencimento))
                            {
                                if (parcela.CodigoStatusCompromissoParcela == 6)
                                    continue;

                                if (parcela.DataVencimento > novaData && novaparcela.NumeroParcela == null)
                                {
                                    novaparcela.NumeroParcela = numeroParcela;
                                    numeroParcela++;
                                }

                                parcela.NumeroParcela = numeroParcela;
                                await compromissoProcessoParcelaRepository.Atualizar(parcela);
                                numeroParcela++;
                            }

                            if (novaparcela.NumeroParcela == null)
                                novaparcela.NumeroParcela = numeroParcela + 1;

                            await compromissoProcessoParcelaRepository.Inserir(novaparcela);
                        }

                        await compromissoProcessoParcelaRepository.CommitAsync();

                        //chamo as procedures que atualizam o compromisso:
                        await compromissoProcessoRepository.AtualizarCompromisso(lancamento.Id, cp.Id);
                    }

                    await lancamentoProcessoRepository.CommitAsync();
                }
            }

            await repository.Atualizar(lote);
            await repository.CommitAsync();
            return lote;
        }

        public async Task<Lote> CriacaoLote(Lote lote)
        {
            return await repository.CriacaoLote(lote);
        }

        public async Task<bool> ExisteLoteComFormaPagamento(long codigoFormaPagamento)
        {
            return await repository.ExisteLoteComFormaPagamento(codigoFormaPagamento);
        }

        public async Task<bool> ExisteLoteComFornecedor(long codigoFornecedor)
        {
            return await repository.ExisteLoteComFornecedor(codigoFornecedor);
        }

        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            return await repository.ExisteCentroCustoAssociado(codigoCentroCusto);
        }

        public async Task<long> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso)
        {
            return await repository.ValidarNumeroLote(numeroLote, codigoTipoProcesso);
        }
    }
}