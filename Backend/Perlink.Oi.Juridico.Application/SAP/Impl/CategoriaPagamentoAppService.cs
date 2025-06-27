using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.Compartilhado.ValidacoesEspecificas;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class CategoriaPagamentoAppService : BaseCrudAppService<CategoriaPagamentoViewModel, CategoriaPagamento, long>, ICategoriaPagamentoAppService
    {
        private readonly ICategoriaPagamentoService service;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;
        private readonly ILancamentoProcessoService lancamentoProcessoService;
        private readonly ICategoriaFinalizacaoService categoriaFinalizacaoService;
        private readonly IMigracaoCategoriaPagamentoService migCategoriaService;
        private readonly IMigCategoriaPagamentoEstrategicoService migCategoriaServiceEstra;

        public CategoriaPagamentoAppService(ICategoriaPagamentoService service,
                                            IMapper mapper,
                                            IPermissaoService permissaoService,
                                            ILancamentoProcessoService lancamentoProcessoService,
                                            ICategoriaFinalizacaoService categoriaFinalizacaoService,
                                            IMigracaoCategoriaPagamentoService migCategoriaService,
                                            IMigCategoriaPagamentoEstrategicoService migCategoriaServiceEstra
                                            ) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
            this.categoriaFinalizacaoService = categoriaFinalizacaoService;
            this.lancamentoProcessoService = lancamentoProcessoService;
            this.migCategoriaService = migCategoriaService;
            this.migCategoriaServiceEstra = migCategoriaServiceEstra;
        }

        public async Task<IResultadoApplication> ExcluirCategoria(long codigoCategoriaPagamento)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await categoriaFinalizacaoService.ExisteCategoriaFinalizacao(codigoCategoriaPagamento))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Categoria de Pagamento, pois se encontra relacionada com Categoria de Finalização.");
                    return resultado;
                }
                if (await service.ExisteTipoMovimentoGarantia(codigoCategoriaPagamento))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Categoria de Pagamento, pois se encontra relacionada com Tipo de Movimento de Garantia.");
                    return resultado;
                }
                if (await lancamentoProcessoService.ExisteCategoriaPagAssociadoLancamento(codigoCategoriaPagamento))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Categoria de Pagamento, pois se encontra relacionada com Lançamento de Processo.");
                    return resultado;
                }

                await service.ExcluirCategoria(codigoCategoriaPagamento);

                var migCategoria = await migCategoriaService.BuscarPorIDMigracaoCategoriaPagamento(codigoCategoriaPagamento);

                if (migCategoria != null && migCategoria.CodCategoriaPagamentoCivel > 0)
                {
                    migCategoriaService.RemoverCodMigCivel(codigoCategoriaPagamento);
                }

                var migCategoriaEstrategicoID = await migCategoriaServiceEstra.BuscarPorIDMigCategoriaPagamentoEstrategico(codigoCategoriaPagamento);

                if (migCategoriaEstrategicoID != null && migCategoriaEstrategicoID.CodCategoriaPagamentoEstra > 0)
                {
                    migCategoriaServiceEstra.RemoverCodMigEstrategico(codigoCategoriaPagamento);
                }

                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExibirMensagem(ex.Message);
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }

        

        public async Task<IResultadoApplication<CategoriaPagamentoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopup(long tipoProcesso, long tipoLancamento)
        {
            var result = new ResultadoApplication<CategoriaPagamentoPopupComboboxViewModel>();

            try
            {
                var fornecedoresPermitidos = service.RecuperarComboboxFornecedoresPermitidos();
                var classesGarantias = await service.RecuperarComboboxClasseGarantia(tipoLancamento);
                var grupoCorrecao = await service.RecuperarComboboxGrupoCorrecao(tipoProcesso);
                var pagamentoA = service.RecuperarComboboxPagamentoA();
                CategoriaPagamentoPopupComboboxViewModel model = new CategoriaPagamentoPopupComboboxViewModel()
                {
                    FornecedoresPermitidos = fornecedoresPermitidos,
                    ClassesGarantias = classesGarantias,
                    GrupoCorrecao = grupoCorrecao,
                    PagamentoA = pagamentoA
                };
                result.DefinirData(mapper.Map<CategoriaPagamentoPopupComboboxViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> CadastrarCategoriaPagamento(CategoriaPagamentoInclusaoEdicaoDTO data)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<CategoriaPagamento>(data);
            
           var migCategoriaPagamento = new MigracaoCategoriaPagamento();

            var migCategoriaEstrategico = new MigracaoCategoriaPagamentoEstrategico();


            try
            {
                Validar(entidade);
                //Implementa baixa de garantia US222667
                entidade.IndicadorBaixaGarantia = (entidade.ClgarCodigoClasseGarantia == 7) || (entidade.ClgarCodigoClasseGarantia == 6);
                entidade.IndicadorBloqueioDeposito = entidade.ClgarCodigoClasseGarantia == 7 ? "B" : entidade.ClgarCodigoClasseGarantia == 6 ? "D" : string.Empty;
                await service.CadastrarCategoria(entidade);             

                if (data.IdMigracaoEstrategico.HasValue)
                {
                    migCategoriaPagamento.CodCategoriaPagamentoCivel = entidade.Id;
                    migCategoriaPagamento.CodCategoriaPagamentoEstra = data.IdMigracaoEstrategico;
                    await migCategoriaService.CadastrarMigracaoCategoriaPagamento(migCategoriaPagamento);
                }

                if (data.IdMigracaoConsumidor.HasValue)
                {
                    migCategoriaEstrategico.CodCategoriaPagamentoEstra = entidade.Id;
                    migCategoriaEstrategico.CodCategoriaPagamentoCivel = data.IdMigracaoConsumidor;
                    await migCategoriaServiceEstra.CadastrarMigCategoriaPagamentoEstrategico(migCategoriaEstrategico);
                }

                service.Commit();


                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        private void Validar(CategoriaPagamento objCategoriaPagamento)
        {
            if (!objCategoriaPagamento.Validar().IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in objCategoriaPagamento.Validar().Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            } 

            var obj = new CategoriaPagamentoGarantias_CC_Trab_Pex_Handler(service);

            obj.SetNext(new CategoriaPagamento_Garantias_CE_Juizado_Handler(service));
            obj.SetNext(new CategoriaPagamento_Garantias_Tributarios_Handler(service));
            obj.SetNext(new CategoriaPagamento_Pagamentos_Juizado_Handler(service));
            obj.SetNext(new CategoriaPagamento_Pagamentos_CC_CE_Pex_Handler(service));
            obj.SetNext(new CategoriaPagamento_Pagamentos_Trabalhista_Handler(service));
            obj.SetNext(new CategoriaPagamento_Despesas_CE_Trab_PEX_Handler(service));
            obj.SetNext(new CategoriaPagamento_Despesas_CC_Juizado_Handler(service));
            obj.SetNext(new CategoriaPagamento_Pagamento_A_Handler(service));
            var resultadoValidacoes = obj.Handle(objCategoriaPagamento);
            
            if (resultadoValidacoes != null && resultadoValidacoes.ToString() != "")
            {
                throw new Exception(resultadoValidacoes.ToString());
            }
        }

        public async Task<IResultadoApplication<ICollection<CategoriaDePagamentoResultadoViewModel>>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            var result = new PagingResultadoLoteApplication<ICollection<CategoriaDePagamentoResultadoViewModel>>();

            try
            {
                if (categoriaPagamentoFiltroDTO.TipoProcesso.Value == 1)
                {
                    var migEstrategico = await service.BuscarCategoriasPagamentoEstrategico(categoriaPagamentoFiltroDTO);
                    result.DefinirData(mapper.Map<ICollection<CategoriaDePagamentoResultadoViewModel>>(migEstrategico));
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();
                    return result;
                }
                else if (categoriaPagamentoFiltroDTO.TipoProcesso.Value == 9)
                {
                    var migConsumidor = await service.BuscarCategoriasPagamentoConsumidor(categoriaPagamentoFiltroDTO);
                    result.DefinirData(mapper.Map<ICollection<CategoriaDePagamentoResultadoViewModel>>(migConsumidor));
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();
                    return result;
                }

                var model = await service.BuscarCategoriasPagamento(categoriaPagamentoFiltroDTO); 
                result.DefinirData(mapper.Map<ICollection<CategoriaDePagamentoResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarCategoriasPagamento(CategoriaPagamentoFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                if (filtroDTO.TipoProcesso.Value == 1)
                {
                      var listaMig = await service.ExportarCategoriasPagamentoEstrategico(filtroDTO);
                byte[] dadosMig;
                var csvConfigurationMig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfigurationMig))
                {
                    csv.WriteRecords(SelecionarViewModelEstrategico(filtroDTO, listaMig));

                    streamWriter.Flush();
                        dadosMig = memoryStream.ToArray();
                }
                result.DefinirData(dadosMig);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

                  return result;
                }
                else if (filtroDTO.TipoProcesso.Value == 9)
                {

                    var listaMigConsu = await service.ExportarCategoriasPagamentoConsumidor(filtroDTO);
                    byte[] dadosMigConsu;
                    var csvConfigurationMig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                    {
                        Delimiter = ";",
                        TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                        HasHeaderRecord = true,
                        IgnoreBlankLines = true
                    };

                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var csv = new CsvWriter(streamWriter, csvConfigurationMig))
                    {
                        csv.WriteRecords(SelecionarViewModelConsumidor(filtroDTO, listaMigConsu));

                        streamWriter.Flush();
                        dadosMigConsu = memoryStream.ToArray();
                    }
                    result.DefinirData(dadosMigConsu);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();

                    return result;
                }

                var lista = await service.ExportarCategoriasPagamento(filtroDTO);
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
                    csv.WriteRecords(SelecionarViewModel(filtroDTO, lista));

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

        private IEnumerable SelecionarViewModel(CategoriaPagamentoFiltroDTO filtroDTO, ICollection<CategoriaPagamentoExportacaoDTO> lista)
        {
            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                         filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoPadraoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias)
                return mapper.Map<ICollection<CategoriaPagamentoCCGarantiasViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios)
                return mapper.Map<ICollection<CategoriaPagamentoBasicoFinalzContabilViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos)
                return mapper.Map<ICollection<CategoriaPagamentoCCPagViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoCEDespesasViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios)
                return mapper.Map<ICollection<CategoriaPagamentoBasicoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos || filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias))
                return mapper.Map<ICollection<CategoriaPagamentoCE_Pag_Gar_ViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoJuizadoDespViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias)
                return mapper.Map<ICollection<CategoriaPagamentoJuizadoGarantiaViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos)
                return mapper.Map<ICollection<CategoriaPagamentoJuizadoPagViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.TributarioAdministrativo &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais || filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.TributarioAdministrativo &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias || filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoClassGarantiaViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.TributarioJudicial &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais || filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.TributarioJudicial &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias || filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoClassGarantiaViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Procon &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoFinalzContabilViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Procon &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoProconDespesaViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Trabalhista &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios))
                return mapper.Map<ICollection<CategoriaPagamentoBasicoFinalzContabilViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Trabalhista &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias))
                return mapper.Map<ICollection<CategoriaPagamentoTrabGarantiasViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Trabalhista &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos))
                return mapper.Map<ICollection<CategoriaPagamentoTrabPagViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Trabalhista &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais))
                return mapper.Map<ICollection<CategoriaPagamentoTrabDespesasJudViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Administrativo)
                return mapper.Map<ICollection<CategoriaPagamentoBasicoAdmViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Pex &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais))
                return mapper.Map<ICollection<CategoriaPagamentoPexDespesasJudViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Pex &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias))
                return mapper.Map<ICollection<CategoriaPagamentoPexGarantiasViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Pex &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios))
                return mapper.Map<ICollection<CategoriaPagamentoPexHonoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.Pex &&
                (filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos))
                return mapper.Map<ICollection<CategoriaPagamentoPexPagamentosViewModel>>(lista);

            return mapper.Map<ICollection<CategoriaPagamentoPadraoViewModel>>(lista);
        }

        private IEnumerable SelecionarViewModelEstrategico(CategoriaPagamentoFiltroDTO filtroDTO, ICollection<CategoriaPagamentoExtrategicoExportacaoDTO> lista)
        {
            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                         filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoPadraoViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias)
                return mapper.Map<ICollection<CategoriaPagamentoCCGarantiasViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios)
                return mapper.Map<ICollection<CategoriaPagamentoBasicoFinalzContabilViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos)
                return mapper.Map<ICollection<CategoriaPagamentoCCPagViewModel>>(lista);
           

            return mapper.Map<ICollection<CategoriaPagamentoPadraoViewModel>>(lista);
        }


        private IEnumerable SelecionarViewModelConsumidor(CategoriaPagamentoFiltroDTO filtroDTO, ICollection<CategoriaPagamentoConsumidorExportacaoDTO> lista)
        {
            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                         filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.DespesasJudiciais)
                return mapper.Map<ICollection<CategoriaPagamentoPadraoConsumidorViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Garantias)
                return mapper.Map<ICollection<CategoriaPagamentoCEGarantiasConsumidorViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Honorarios)
                return mapper.Map<ICollection<CategoriaPagamentoBasicoFinalzContabilConsumidorViewModel>>(lista);

            if (filtroDTO.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico &&
                filtroDTO.TipoLancamento == (int)TipoLancamentoEnum.Pagamentos)
                return mapper.Map<ICollection<CategoriaPagamentoCCPagConsumidorViewModel>>(lista);


            return mapper.Map<ICollection<CategoriaPagamentoPadraoViewModel>>(lista);
        }


        public async Task<IResultadoApplication> AlterarCategoriaPagamento(CategoriaPagamentoInclusaoEdicaoDTO data)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<CategoriaPagamento>(data);

            try
            {
                Validar(entidade);
                //Implementa baixa de garantia US222667
                entidade.IndicadorBaixaGarantia = (entidade.ClgarCodigoClasseGarantia == 7)  ||  (entidade.ClgarCodigoClasseGarantia == 6);
                entidade.IndicadorBloqueioDeposito = entidade.ClgarCodigoClasseGarantia == 7 ? "B" : entidade.ClgarCodigoClasseGarantia == 6 ? "D" : string.Empty;

                await service.AlterarCategoriaPagamento(entidade);
                result.ExibeNotificacao = await service.ExibeNotificacaoAoEditar(entidade);

                var migCategoria = await migCategoriaService.BuscarPorIDMigracaoCategoriaPagamento(data.Codigo);
                var buscaIdcategoria = await migCategoriaServiceEstra.BuscarPorIDMigCategoriaPagamentoEstrategico(data.Codigo);
 
                if (data.IdMigracaoEstrategico != null && data.IdMigracaoEstrategico > 0)
                {
                    if (migCategoria != null)
                    {
                        migCategoriaService.RemoverCodMigCivel(data.Codigo);
                    }

                    MigracaoCategoriaPagamento migracaocategoriaPagamento = new MigracaoCategoriaPagamento();

                    migracaocategoriaPagamento.CodCategoriaPagamentoCivel = data.Codigo;
                    migracaocategoriaPagamento.CodCategoriaPagamentoEstra = data.IdMigracaoEstrategico;

                    await migCategoriaService.CadastrarMigracaoCategoriaPagamento(migracaocategoriaPagamento);
                }
                else if (data.IdMigracaoConsumidor != null && data.IdMigracaoConsumidor > 0)
                {
                    if (buscaIdcategoria != null)
                    {
                        migCategoriaServiceEstra.RemoverCodMigEstrategico(data.Codigo);
                    }

                    var migCategoriaEstrategico = new MigracaoCategoriaPagamentoEstrategico();
                    migCategoriaEstrategico.CodCategoriaPagamentoEstra = data.Codigo;
                    migCategoriaEstrategico.CodCategoriaPagamentoCivel = data.IdMigracaoConsumidor;

                    await migCategoriaServiceEstra.CadastrarMigCategoriaPagamentoEstrategico(migCategoriaEstrategico);

                }
                else if (migCategoria != null)
                {
                    migCategoriaService.RemoverCodMigCivel(data.Codigo);
                }
                else if (buscaIdcategoria != null)
                {
                    migCategoriaServiceEstra.RemoverCodMigEstrategico(data.Codigo);
                }

                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        public async Task<IResultadoApplication<CategoriaMIgracaoEstrategicoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupEstrategico()
        {
            var result = new ResultadoApplication<CategoriaMIgracaoEstrategicoPopupComboboxViewModel>();

            try
            {
                var model = await service.RecuperarComboboxEstrategico();
                var categoriaPagamentos = new CategoriaMIgracaoEstrategicoPopupComboboxViewModel();
                categoriaPagamentos.CategoriaPagamento = model;
                
                result.DefinirData(categoriaPagamentos);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<CategoriaMIgracaoConsumidorPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupConsumidor()
        {
            var result = new ResultadoApplication<CategoriaMIgracaoConsumidorPopupComboboxViewModel>();

            try
            {
                var model = await service.RecuperarComboboxConsumidor();
                var categoriaPagamentos = new CategoriaMIgracaoConsumidorPopupComboboxViewModel();
                categoriaPagamentos.CategoriaPagamento = model;

                result.DefinirData(categoriaPagamentos);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {

                result.ExecutadoComErro(ex);
            }
            return result;
        }
    }
}
