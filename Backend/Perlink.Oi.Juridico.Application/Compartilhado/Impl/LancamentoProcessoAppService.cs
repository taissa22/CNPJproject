using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl {
    public class LancamentoProcessoAppService : BaseCrudAppService<LancamentoProcessoViewModel, LancamentoProcesso, long>, ILancamentoProcessoAppService {
        private readonly ILancamentoProcessoService service;
        private readonly IPedidoService pedidoService;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;
        private readonly IProcessoService processoService;
        private readonly IParteService parteService;
        private readonly ICompromissoProcessoParcelaService compromissoProcessoParcelaService;
        private readonly ICompromissoProcessoService compromissoProcessoService;
        private readonly IStatusCompromissoParcelaService statusCompromissoParcelaService;
        private readonly IMotivoSuspensaoCancelamentoParcelaService MotivoSuspensaoCancelamentoParcelaService;
        private readonly IParametroService parametroService;
        private readonly ICredorCompromissoService credorCompromissoService;
        private readonly ICompromissoProcessoCredorService compromissoProcessoCredorService;
        private readonly IFeriadosService feriadosService;
        private readonly ICompromissoProcessoParcelaRepository compromissoParcelaRepository;
        private readonly ICompromissoProcessoRepository compromissoProcessoRepository;

        public LancamentoProcessoAppService(ILancamentoProcessoService service,
            IMapper mapper, IPermissaoService permissaoService, IParteService parteService, IProcessoService processoService, IPedidoService pedidoService,
            ICompromissoProcessoParcelaService compromissoProcessoParcelaService, ICompromissoProcessoService compromissoProcessoService,
            IStatusCompromissoParcelaService statusCompromissoParcelaService, IMotivoSuspensaoCancelamentoParcelaService MotivoSuspensaoCancelamentoParcelaService,
            IParametroService parametroService, ICredorCompromissoService credorCompromissoService, ICompromissoProcessoCredorService compromissoProcessoCredorService,
            IFeriadosService feriadosService,
            ICompromissoProcessoParcelaRepository compromissoParcelaRepository,
            ICompromissoProcessoRepository compromissoProcessoRepository) : base(service, mapper) {
            this.service = service;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
            this.processoService = processoService;
            this.pedidoService = pedidoService;
            this.parteService = parteService;
            this.compromissoProcessoParcelaService = compromissoProcessoParcelaService;
            this.compromissoProcessoService = compromissoProcessoService;
            this.statusCompromissoParcelaService = statusCompromissoParcelaService;
            this.MotivoSuspensaoCancelamentoParcelaService = MotivoSuspensaoCancelamentoParcelaService;
            this.parametroService = parametroService;
            this.credorCompromissoService = credorCompromissoService;
            this.compromissoProcessoCredorService = compromissoProcessoCredorService;
            this.feriadosService = feriadosService;
            this.compromissoParcelaRepository = compromissoParcelaRepository;
            this.compromissoProcessoRepository = compromissoProcessoRepository;

        }

        public async Task<IResultadoApplication<ICollection<LancamentoProcessoViewModel>>> RecuperarPorParteNumeroGuia(long NumeroGuia) {
            var result = new ResultadoApplication<ICollection<LancamentoProcessoViewModel>>();

            try {
                var model = await service.RecuperarPorParteNumeroGuia(NumeroGuia);
                result.DefinirData(mapper.Map<ICollection<LancamentoProcessoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<long>> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso) {
            var result = new ResultadoApplication<long>();

            try {
                var model = await service.RecuperarLancamentoProcesso(NumeroGuia, CodigoTipoProcesso);
                result.DefinirData(mapper.Map<long>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteLancamentoViewModel>>> ObterLancamentoDoLote(long codigoLote) {
            var result = new ResultadoApplication<ICollection<LoteLancamentoViewModel>>();
            try {
                var model = await service.RecuperarLancamentoLote(codigoLote);
                result.DefinirData(mapper.Map<ICollection<LoteLancamentoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoDTO>>> ObterLancamentoDoCriacao(long codigoEmpresaCentralizadora) {
            var result = new ResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoDTO>>();

            try {
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarLancamentoDoLote(long codigoLote, long codigoTipoProcesso) {
            var result = new ResultadoApplication<byte[]>();
            try {
                if ((!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesCivelCons) &&
                    codigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesEstrat) &&
                    codigoTipoProcesso == TipoProcessoEnum.CivelEstrategico.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesJuizado) &&
                    codigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesPex) &&
                    codigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesTrabalhista) &&
                    codigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode()))
                    throw new Exception("O usuário não possui permissão para Exportar Lançamentos do lote.");

                var lista = await service.ExportarLancamentoDoLote(codigoLote);
                byte[] dados;
                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR")) {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfiguration)) {
                    switch (codigoTipoProcesso) {
                        case 9:
                            csv.WriteRecords(mapper.Map<ICollection<LoteLancamentoCeViewModel>>(lista));
                            break;

                        case 2:
                            csv.WriteRecords(mapper.Map<ICollection<LoteLancamentoTrabViewModel>>(lista));
                            break;

                        case 1:
                            csv.WriteRecords(mapper.Map<ICollection<LoteLancamentoCcViewModel>>(lista));
                            break;

                        case 7:
                            csv.WriteRecords(mapper.Map<ICollection<LoteLancamentoJecViewModel>>(lista));
                            break;

                        case 18:
                            csv.WriteRecords(mapper.Map<ICollection<LoteLancamentoPexViewModel>>(lista));
                            break;
                    }
                    streamWriter.Flush();
                    dados = memoryStream.ToArray();
                }
                result.DefinirData(dados);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraViewModel>>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros) {
            var result = new ResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraViewModel>>();

            try {
                var model = await service.ObterLotesAgrupadoPorEmpresaCentralizadora(filtros);
                result.DefinirData(mapper.Map<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IPagingResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaGrupoViewModel>>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros) {
            var result = new PagingResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaGrupoViewModel>>();

            try {
                var model = await service.ObterLotesAgrupadoPorEmpresadoGrupo(filtros);
                result.DefinirData(mapper.Map<ICollection<LoteCriacaoResultadoEmpresaGrupoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros) {
          return await service.ObterTotalLotesAgrupadoPorEmpresadoGrupo(filtros);
        }

        public async Task<IResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoViewModel>>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger) {
            var result = new ResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoViewModel>>();
            try {
                var model = await service.ObterLancamentosLoteCriacao(filtros, logger);
                Stopwatch s = new Stopwatch();
                s.Start();
                logger.LogInformation("Inicio do mapper");
                result.DefinirData(mapper.Map<ICollection<LoteCriacaoResultadoLancamentoViewModel>>(model));
                s.Stop();                
                logger.LogInformation("tempo gasto: " + s.Elapsed.TotalSeconds + " em segundos");
                logger.LogInformation("Fim do mapper");
                var teste = s.Elapsed.TotalSeconds;
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication> AlterarDataEnvioEscritorio(List<LoteLancamentoViewModel> loteLancamentos) {
            var result = new ResultadoApplication();
            try {
                bool valido = loteLancamentos.Any(l => l.DataEnvioEscritorio.HasValue && l.DataEnvioEscritorio.Value < DateTime.Today.AddDays(1));

                if (valido) {
                    foreach (var lancamento in loteLancamentos) {
                        LancamentoProcesso lancamentoProcesso = await service.ObterLancamentoProcesso(lancamento.CodigoProcesso, lancamento.CodigoLancamento);
                        lancamentoProcesso.DataEnvioEscritorio = lancamento.DataEnvioEscritorio;
                        await service.AlterarDataEnvioEscritorio(lancamentoProcesso);
                    }
                } else
                    throw new Exception(Textos.Shared_Mensagem_Erro_Data_Maior_Que_Atual);
                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<ICollection<DadosProcessoEstornoViewModel>>> ConsultaLancamentoEstorno(LancamentoEstornoFiltroDTO filtros) {
            var result = new ResultadoApplication<ICollection<DadosProcessoEstornoViewModel>>();

            try {
                List<DadosProcessoEstornoDTO> model = new List<DadosProcessoEstornoDTO>();
                var processos = await processoService.RecuperarPorIdentificador(filtros.NumeroProcesso, filtros.CodigoInterno, filtros.TipoProcesso);
                foreach (var item in processos) {
                    var processo = new DadosProcessoEstornoDTO() {
                        NumeroProcesso = item.NumeroProcessoCartorio,
                        CodigoProcesso = item.Id,
                        NomeComarca = item.Comarca.Nome,
                        ClassificacaoHierarquica = item.CodigoClassificacaoProcesso,
                        CodigoVara = item.CodigoVara.ToString(),
                        NomeTipoVara = item.TipoVara.NomeTipoVara,
                        UF = item.Comarca.CodigoEstado,
                        DescricaoEmpresaDoGrupo = item.Parte.Nome,
                        DescricaoEscritorio = item.Profissional.NomeProfissional,
                        ResponsavelInterno = item.ResponsavelInterno,
                        Pedidos = await pedidoService.ConsultarPedido(filtros.TipoProcesso, item.Id),
                        DadosLancamentosEstorno = await service.ConsultaLancamentoEstorno(item.Id, filtros.TipoProcesso),
                        Reclamadas = await parteService.RecuperarReclamanteReclamadas(filtros, true, item.Id),
                        Reclamantes = await parteService.RecuperarReclamanteReclamadas(filtros, false, item.Id)
                    };

                    model.Add(processo);
                }

                result.DefinirData(mapper.Map<ICollection<DadosProcessoEstornoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<ICollection<PedidoEstornoDTO>> ConsultaPedidoEstorno(LancamentoEstornoFiltroDTO filtros) {
            ICollection<PedidoEstornoDTO> pedidos;
            try {
                pedidos = await pedidoService.ConsultarPedido(filtros.TipoProcesso, filtros.TipoProcesso);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            return pedidos;
        }

        public async Task<IResultadoApplication> EstornaLancamento(DadosLancamentoEstornoDTO dadosLancamentoEstornoDTO) {
            var result = new ResultadoApplication();
            try {
                //Atualizar lançamento
                var lancamentoprocesso = await service.ObterLancamentoProcesso(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoLancamento);
                lancamentoprocesso.IndicadorExluido = true;


                var parcelaCompromissoNovo = service.RecuperarCompromissoParcela(dadosLancamentoEstornoDTO);

                await service.AtualizarLancamentoProcesso(lancamentoprocesso);


                if (dadosLancamentoEstornoDTO.CodigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode() && dadosLancamentoEstornoDTO.ValorCompromisso > 0) {

                    //Atualizar parcela do compromisso
                    var parcelaCompromisso = await compromissoProcessoParcelaService.ObterCompromissoParcela(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso, dadosLancamentoEstornoDTO.CodigoParcela);
                    parcelaCompromisso.DataCancelamento = DateTime.Now;
                    parcelaCompromisso.CodigoStatusCompromissoParcela = StatusCompromissoParcelaEnum.Cancelada.GetHashCode();
                    parcelaCompromisso.CodMotivoSuspCancelParcela = MotivoSuspensaoCancelamentoParcelaEnum.LancamentoEstornado.GetHashCode();
                    await compromissoProcessoParcelaService.AtualizarCompromissoProcessoParcela(parcelaCompromisso);


                    if (dadosLancamentoEstornoDTO.ReduzirPagamentoCredor) {

                        if (dadosLancamentoEstornoDTO.QuantidadeCredoresAssociados == 1) {
                            //ToDo - Criar um método no service do compromissoProcessoParcelaService que retorne o credorCompromisso associado a parcela
                            CompromissoProcessoCredor compromissoProcessoCredor = await compromissoProcessoCredorService.ObterCompromissoProcessoCredor(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso);                            
                            CredorCompromisso credorCompromisso = await credorCompromissoService.ObterCredorCompromisso(dadosLancamentoEstornoDTO.CodigoProcesso, compromissoProcessoCredor.CodigoCredorCompromisso);
                            credorCompromisso.ValorPagamentoCredor = dadosLancamentoEstornoDTO.ValorCompromisso - dadosLancamentoEstornoDTO.Valor;
                            await credorCompromissoService.AtualizarCredorCompromisso(credorCompromisso);
                        }

                        //comitt alterações na parcela, necessário para utilizar as procidures dentro do método AtualizarValorCompromisso
                        compromissoProcessoParcelaService.Commit();

                        //Chamada da procedure que atualiza o valor do compromisso
                        await compromissoProcessoService.AtualizarValorCompromisso(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso);

                    } else if (dadosLancamentoEstornoDTO.CriarNovaParcelaFutura) {

                        //Incluir nova Parcela
                        long nextCodigoParcelaCompromissoProcesso = await compromissoProcessoParcelaService.ObterNextCodigoParcelaCompromissoProcesso(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso);
                        long nextNumeroParcelaCompromissoProcesso = await compromissoProcessoParcelaService.ObterNextNumeroParcelaCompromissoProcesso(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso);

                        CompromissoProcessoParcela compromissoProcessoParcela = new CompromissoProcessoParcela() {
                            Id = dadosLancamentoEstornoDTO.CodigoCompromisso,
                            CodigoProcesso = dadosLancamentoEstornoDTO.CodigoProcesso,
                            CodigoParcela = nextCodigoParcelaCompromissoProcesso,
                            NumeroParcela = nextNumeroParcelaCompromissoProcesso,
                            DataVencimento = await feriadosService.ValidarDataNovaParcela(Convert.ToInt32(parametroService.RecuperarPorId("QTD_DIA_NOVA_PARC_ESTCANC").Result.Conteudo)),
                            CodigoStatusCompromissoParcela = (int)StatusCompromissoParcelaEnum.Agendada,
                            ValorParcela = dadosLancamentoEstornoDTO.Valor
                        };


                        await compromissoProcessoParcelaService.Inserir(compromissoProcessoParcela);
                    }


                }

                service.Commit();
                compromissoProcessoParcelaService.Commit();
                credorCompromissoService.Commit();

                await compromissoProcessoRepository.AtualizarCompromisso(dadosLancamentoEstornoDTO.CodigoProcesso, dadosLancamentoEstornoDTO.CodigoCompromisso);

                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<long?>> RecuperarNumeroContaJudicial(long NumeroContaJudicial) {
            var result = new ResultadoApplication<long?>();
            try {
                var model = await service.RecuperarNumeroContaJudicial(NumeroContaJudicial);
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {

                result.ExecutadoComErro(ex);
            }
            return result;
        }
    }
}