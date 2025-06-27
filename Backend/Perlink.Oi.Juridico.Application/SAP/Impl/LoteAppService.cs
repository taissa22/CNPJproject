using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class LoteAppService : BaseCrudAppService<LoteViewModel, Lote, long>, ILoteAppService
    {
        private readonly ILoteService service;
        private readonly IMapper mapper;
        private readonly ILoteRepository repository;
        private readonly ILoteLancamentoRepository loteLancamentoRepository;
        private readonly ILancamentoProcessoRepository lancamentoProcessoRepository;
        private readonly ICategoriaPagamentoRepository categoriaPagamentoRepository;
        private readonly ILoteLancamentoService Lanservice;
        private readonly IUsuarioService usuarioService;
        private readonly IBorderoService borderoService;
        private readonly IPermissaoService permissaoService;
        private readonly IEmpresaDoGrupoService empresaDoGrupoService;
        private readonly ICentroCustoService centroCustoService;
        private readonly IEscritorioService escritorioService;
        private readonly IFornecedorService fornecedorService;
        private readonly IStatusPagamentoService statusPagamentoService;
        private readonly ITipoLancamentoService tipoLancamentoService;
        private readonly ILoteBBService loteBBService;

        public LoteAppService(ILoteService service, IMapper mapper, ITipoLancamentoService tipoLancamentoService, IStatusPagamentoService statusPagamentoService, ILoteRepository repository, IFornecedorService fornecedorService, IEscritorioService escritorioService, ICentroCustoService centroCustoService, IEmpresaDoGrupoService empresaDoGrupoService, ILoteLancamentoService lanservice, ILoteLancamentoRepository loteLancamentoRepository, ILancamentoProcessoRepository lancamentoProcessoRepository, ICategoriaPagamentoRepository categoriaPagamentoRepository, IUsuarioService usuarioService, IBorderoService borderoService, IPermissaoService permissaoService, ILoteBBService loteBBService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.repository = repository;
            this.loteLancamentoRepository = loteLancamentoRepository;
            this.lancamentoProcessoRepository = lancamentoProcessoRepository;
            this.categoriaPagamentoRepository = categoriaPagamentoRepository;
            this.Lanservice = lanservice;
            this.usuarioService = usuarioService;
            this.borderoService = borderoService;
            this.permissaoService = permissaoService;
            this.empresaDoGrupoService = empresaDoGrupoService;
            this.centroCustoService = centroCustoService;
            this.escritorioService = escritorioService;
            this.fornecedorService = fornecedorService;
            this.statusPagamentoService = statusPagamentoService;
            this.tipoLancamentoService = tipoLancamentoService;
            this.loteBBService = loteBBService;
        }

        #region Criação Lote
        public async Task<IResultadoApplication<LoteCanceladoViewModel>> CancelamentoLote(LoteCancelamentoDTO loteCancelamentoDTO)
        {
            var result = new ResultadoApplication<LoteCanceladoViewModel>();

            var usrSolicitCancelamento = "";
            var dataSolicitCancelamento = "";
            try
            {
                var usuario = usuarioService.ObterUsuarioLogado().Result;
                usrSolicitCancelamento = usuario.Id;
                usrSolicitCancelamento  += " - " + usuario.Nome;
                dataSolicitCancelamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            catch
            {

            }

            try
            {
                if ((!permissaoService.TemPermissao(PermissaoEnum.f_CancelarLotesCivelCons) &&
                    loteCancelamentoDTO.CodigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_CancelarLotesCivelEstrat) &&
                    loteCancelamentoDTO.CodigoTipoProcesso == TipoProcessoEnum.CivelEstrategico.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_CancelarLotesJuizado) &&
                    loteCancelamentoDTO.CodigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_CancelarLotesPex) &&
                    loteCancelamentoDTO.CodigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_CancelarLotesTrabalhista) &&
                    loteCancelamentoDTO.CodigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode()))
                    throw new Exception("O usuário não possui permissão para cancelar lote.");

                var model = await service.CancelamentoLote(loteCancelamentoDTO, new CargaCompromissoParcelaCancelamentoDTO()
                {
                    DataCodSolicCancelamento = dataSolicitCancelamento,
                    UsrCodSolicCancelamento = usrSolicitCancelamento
                });
                result.DefinirData(mapper.Map<LoteCanceladoViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> Exportar(LoteFiltroDTO loteFiltroDTO, long codigoTipoProcesso)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
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
                    throw new Exception("O usuário não possui permissão para Exportar lote.");

                loteFiltroDTO.Quantidade = 0;
                loteFiltroDTO.Pagina = 0;
                var lista = await loteLancamentoRepository.ExportarConsultaLote(loteFiltroDTO);
                byte[] dados;
                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                {
                    switch (codigoTipoProcesso)
                    {
                        case 2:
                        case 9:
                            csv.WriteRecords(mapper.Map<ICollection<LoteExportarToCEorTrabViewModel>>(lista));
                            break;

                        default:
                            csv.WriteRecords(mapper.Map<ICollection<LoteExportarViewModel>>(lista));
                            break;
                    }
                    streamWriter.Flush();
                    dados = memoryStream.ToArray();
                }
                result.DefinirData(dados);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication> CriarLote(LoteCriacaoViewModel loteCriacaoViewModel)
        {
            var resultado = new ResultadoApplication<LoteCriacaoViewModel>();

            try
            {
                Boolean valido = true;

                List<LancamentoProcesso> LancamentosProcessosValidos = new List<LancamentoProcesso>();
                foreach (var item in loteCriacaoViewModel.DadosLancamentoDTOs)
                {
                    LancamentoProcesso LancamentoProcesso = await this.lancamentoProcessoRepository.ObterLancamentoProcesso(item.CodigoProcesso, item.CodigoLancamento);
                    if (LancamentoProcesso != null)
                    {
                        if (item.ValorLancamento != LancamentoProcesso.ValorLancamento)
                            item.MensagemErro = "O valor do lançamento foi alterado depois do carregamento dos lançamentos deste lote.";

                        if (item.CodigoStatusPagamento != LancamentoProcesso.CodigoStatusPagamento)
                            item.MensagemErro = "O status do lançamento foi alterado depois do carregamento dos lançamentos deste lote.";
                    }
                    else
                    {
                        item.MensagemErro = "O lançamento foi excluído depois do carregamento dos lançamentos deste lote.";
                    }

                    if (!this.categoriaPagamentoRepository.CodigoMaterialSAPValido(LancamentoProcesso.CodigoCatPagamento))
                        item.MensagemErro = "O código do material SAP referente á categoria de pagamento do lançamento não pode ser 0(zero) e nem null. Por favor, contate o administrador do sistema.";

                    if (string.IsNullOrEmpty(item.MensagemErro))
                        LancamentosProcessosValidos.Add(LancamentoProcesso);
                }

                valido = loteCriacaoViewModel.DadosLancamentoDTOs.Where(l => !String.IsNullOrEmpty(l.MensagemErro)).Count() == 0;
                var usuario = usuarioService.ObterUsuarioLogado().Result;
                loteCriacaoViewModel.CodigoUsuario = usuario.Id;
                loteCriacaoViewModel.NomeUsuario = usuario.Nome;
                resultado.DefinirData(loteCriacaoViewModel);

                if (valido)
                {
                    var lote = await service.CriacaoLote(loteCriacaoViewModel.MapearParaEntidadeLote());

                    await this.Lanservice.VinculoLancamento(lote, loteCriacaoViewModel.DadosLancamentoDTOs);
                    await this.borderoService.CriacaoBordero(loteCriacaoViewModel.Borderos, lote);

                    foreach (var lancamentoProcesso in LancamentosProcessosValidos)
                    {
                        lancamentoProcesso.CodigoStatusPagamento = Convert.ToInt64(StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP);
                        await this.lancamentoProcessoRepository.Atualizar(lancamentoProcesso);
                    }

                    this.service.Commit();
                    resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                    resultado.ExecutadoComSuccesso();
                }
                else
                {
                    resultado.ExecutadoComErro("Um ou mais lançamentos entraram em crítica e precisam ser desmarcados para que o lote possa ser gerado. Favor verificar a coluna “Descrição do erro” na grid de lançamentos.");
                    resultado.ExibeNotificacao = true;
                }
            }
            catch (Exception excecao)
            {
                resultado.ExibirMensagem(excecao.Message);
                resultado.ExecutadoComErro(excecao);
            }

            return resultado;
        }
        #endregion
        
        #region Consulta Lote

        public async Task<IResultadoApplication<LoteCriteriosFiltrosViewModel>> CarregarFiltros(long codigoTipoProcesso)
        {
            var result = new ResultadoApplication<LoteCriteriosFiltrosViewModel>();

            try
            {
                var model = new LoteCriteriosFiltrosDTO() {

                    ListaEmpresaDoGrupo = await empresaDoGrupoService.RecuperarEmpresaDoGrupo(),
                    ListaCentroCusto = await centroCustoService.RecuperarCentroCustoParaFiltroLote(),
                    ListaEscritorio = await escritorioService.RecuperarEscritorioFiltro(codigoTipoProcesso),
                    ListaFornecedor = await fornecedorService.RecuperarFornecedorParaFiltroLote(),
                    ListaStatusPagamento = await statusPagamentoService.RecuperarStatusPagamentoParaFiltroLote(),
                    ListaTipodeLancamento = await tipoLancamentoService.RecuperarTipoLancamentoParaFiltroLote(),
                    ListaCategoriaPagamento = await categoriaPagamentoRepository.ListaCategoriaPagamento(codigoTipoProcesso),
                };
               
         
                result.DefinirData(mapper.Map<LoteCriteriosFiltrosViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IPagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>> RecuperarPorFiltro(LoteFiltroDTO loteFiltroDTO)
        {
            var result = new PagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>();

            try
            {
                var model = await loteLancamentoRepository.FiltroConsultaLote(loteFiltroDTO);
                result.DefinirData(mapper.Map<ICollection<LoteResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IPagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>> ObterQuantidadeTotalPorFiltro(LoteFiltroDTO loteFiltroDTO)
        {
            var result = new PagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>();

            try
            {
                //Zerando pra pegar a query sem quebra de página
                loteFiltroDTO.Quantidade = 0;
                loteFiltroDTO.Pagina = 0;
                var model = await loteLancamentoRepository.TotalFiltroConsultaLote(loteFiltroDTO);

                result.Total = model.Total;
                result.TotalLotes = model.TotalLotes;
                result.TotalValorLotes = model.TotalValorLotes;
                result.QuantidadesLancamentos = model.QuantidadesLancamentos;
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<CompromissoLoteViewModel>> VerificarCompromisso(long CodigoLote)
        {
            var result = new ResultadoApplication<CompromissoLoteViewModel>();

            try
            {
                var model = await service.VerificarCompromisso(CodigoLote);
                result.DefinirData(mapper.Map<CompromissoLoteViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<LoteDetalhesViewModel>> RecuperaDetalhes(long CodigoLote)
        {
            var result = new ResultadoApplication<LoteDetalhesViewModel>();

            try
            {
                var model = await service.RecuperaDetalhes(CodigoLote);
                result.DefinirData(mapper.Map<LoteDetalhesViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<long>> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso) {
            var result = new ResultadoApplication<long>();

            try {
                var model = await service.ValidarNumeroLote(numeroLote, codigoTipoProcesso);
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

            } catch (Exception ex) {

                result.ExecutadoComErro(ex);
            }

            return result;
        }

        #region Metodos Não utilizados

        public async Task<IResultadoApplication<string>> RecuperarPorCodigoSAP(long codigo)
        {
            var result = new ResultadoApplication<string>();

            try
            {
                var model = await service.RecuperarPorCodigoSAP(codigo);
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
        public async Task<IResultadoApplication<string>> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso)
        {
            var result = new ResultadoApplication<string>();

            try
            {
                var model = await service.RecuperarPorNumeroSAPCodigoTipoProcesso(NumeroPedidoSAP, CodigoTipoProcesso);
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorParteCodigoSAP(long CodigoSapParte)
        {
            var result = new ResultadoApplication<ICollection<LoteViewModel>>();

            try
            {
                var model = await service.RecuperarPorParteCodigoSAP(CodigoSapParte);
                result.DefinirData(mapper.Map<ICollection<LoteViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax)
        {
            var result = new ResultadoApplication<ICollection<LoteViewModel>>();

            try
            {
                var model = await service.RecuperarPorDataCriacaoLote(DataCriacaoMin, DataCriacaoMax);
                result.DefinirData(mapper.Map<ICollection<LoteViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorDataPedidoCriacaoLote(DateTime DataCriacaoPedidoMin, DateTime DataCriacaoPedidoMax)
        {
            var result = new ResultadoApplication<ICollection<LoteViewModel>>();

            try
            {
                var model = await service.RecuperarPorDataCriacaoPedidoLote(DataCriacaoPedidoMin, DataCriacaoPedidoMax);
                result.DefinirData(mapper.Map<ICollection<LoteViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<long?>> AtualizarNumeroLoteBBAsync(long CodigoParte)
        {
            var result = new ResultadoApplication<long?>();

            try
            {
                var model = await service.AtualizarNumeroLoteBBAsync(CodigoParte);
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public IResultadoApplication<dynamic> RegerarArquivoBB(ArquivoBBDTO regerarArquivoBBDTO)
        {
            var result = new ResultadoApplication<dynamic>();
            try
            {
                var msgErro = "";
                var arquivo = loteBBService.Gerar(regerarArquivoBBDTO, ref msgErro);
                var objRetorno = new 
                {
                    fileName = regerarArquivoBBDTO.NomeArquivo,
                    file = arquivo
                };

                if (string.IsNullOrEmpty(msgErro))
                {
                    result.DefinirData(objRetorno);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();
                    loteBBService.Commit();
                }
                else
                {
                    result.ExecutadoComErro(msgErro);
                }
            }
            catch (Exception e)
            {
                result.ExecutadoComErro($"Ocorreu erro ao tentar regerar arquivo BB: {e.Message}");
            }

            return result;
        }       

        #endregion Metodos Não utilizados

        #endregion Consulta Lote
    }
}