using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Interface;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.EFRepository;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Manutencao.Impl
{
    public class JuroCorrecaoProcessoAppService : BaseCrudAppService<JuroCorrecaoProcessoViewModel, JuroCorrecaoProcesso, long>, IJuroCorrecaoProcessoAppService
    {
        private readonly IJuroCorrecaoProcessoService _juroCorrecaoProcessoService;
        private readonly ITipoProcessoService _tipoProcessoService;
        private readonly IJuroCorrecaoProcessoRepository _juroCorrecaoProcessoRepository;
        private readonly IMapper _mapper;

        public JuroCorrecaoProcessoAppService(IJuroCorrecaoProcessoService service, ITipoProcessoService tipoProcessoService,
                                              IJuroCorrecaoProcessoRepository repository, IMapper mapper) 
            : base(service, mapper)
        {
            _juroCorrecaoProcessoService = service;
            _tipoProcessoService = tipoProcessoService;
            _juroCorrecaoProcessoRepository = repository;
            _mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<JuroCorrecaoProcessoViewModel>>> ObterJuroCorrecaoProcessoPorFiltro(VigenciaCivilFiltrosViewModel viewModel)
        {
            var result = new PagingResultadoApplication<ICollection<JuroCorrecaoProcessoViewModel>>();

            try
            {
                if (viewModel.Filtro == null) throw new Exception(Textos.ViewModel_Invalido);

                if (
                    (viewModel.Filtro.DataInicio.HasValue && !viewModel.Filtro.DataFim.HasValue) ||
                    (!viewModel.Filtro.DataInicio.HasValue && viewModel.Filtro.DataFim.HasValue) ||
                    (viewModel.Filtro.DataInicio.HasValue && viewModel.Filtro.DataFim.HasValue && viewModel.Filtro.DataFim.Value < viewModel.Filtro.DataInicio.Value)
                )
                {
                    throw new Exception(Textos.Datas_Invalidas);
                }

                var listaOrdenada = await _juroCorrecaoProcessoService.PesquisarComTipoProcesso(viewModel.Filtro.CodTipoProcesso, viewModel.Filtro.DataInicio,
                                                                            viewModel.Filtro.DataFim, viewModel.Ascendente, viewModel.Ordenacao,
                                                                            viewModel.Pagina, viewModel.Quantidade);

                result.DefinirData(_mapper.Map<ICollection<JuroCorrecaoProcessoViewModel>>(listaOrdenada));
                result.Total = await _juroCorrecaoProcessoService.Pesquisar(new List<Expression<Func<JuroCorrecaoProcesso, bool>>>
                {
                    x => viewModel.Filtro.CodTipoProcesso == null || x.Id == viewModel.Filtro.CodTipoProcesso,
                    x => (!viewModel.Filtro.DataInicio.HasValue && !viewModel.Filtro.DataFim.HasValue) || x.DataVigencia >= viewModel.Filtro.DataInicio.Value && x.DataVigencia <= viewModel.Filtro.DataFim.Value
                }).CountAsync();

                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarJuroCorrecaoProcesso(VigenciaCivilFiltrosViewModel viewModel)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await _juroCorrecaoProcessoService.PesquisarParaExportacaoComTipoProcesso(viewModel.Filtro.CodTipoProcesso, viewModel.Filtro.DataInicio,
                                                                                  viewModel.Filtro.DataFim);
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
                    csv.WriteRecords(_mapper.Map<ICollection<JuroCorrecaoProcessoViewModel>>(lista));
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

        public async Task<IResultadoApplication> CadastrarJurosCorrecaoProcesso(JuroCorrecaoProcessoInputViewModel viewModel)
        {
            var application = new ResultadoApplication();

            try
            {
                if (viewModel == null)
                {
                    throw new Exception("ViewModel é inválido.");
                }

                if(!viewModel.CodTipoProcesso.HasValue)
                {
                    throw new Exception("Código do tipo processo é obrigatório.");
                }

                var entidade = _mapper.Map<JuroCorrecaoProcesso>(viewModel);
                entidade.TipoProcesso = await _tipoProcessoService.RecuperarPorId(viewModel.CodTipoProcesso.Value);

                var resultado = await _juroCorrecaoProcessoService.Inserir(entidade);

                // TODO: Verificar se existe data maior e validar
                var EhDataMaior = _juroCorrecaoProcessoService.VerificarSeDataInseridaEMenorQueACadastrada(viewModel.CodTipoProcesso.Value, 
                                                                                       viewModel.DataVigencia.Value);

                if (resultado.IsValid)
                {
                    if(!EhDataMaior)
                    {
                        _juroCorrecaoProcessoService.Commit();

                        application.ExecutadoComSuccesso();
                    }
                    else
                    {
                        application.ExecutadoComErro("Não é permitido incluir vigências, com data menor ou igual a de maior data já cadastrada.");
                    }
                }
                else
                    application.ExecutadoComErro(resultado.ToString());
            }
            catch (Exception excecao)
            {
                application.ExecutadoComErro(excecao);
            }

            return application;
        }

        public async Task<IResultadoApplication> EditarJuroCorrecaoProcesso(JuroCorrecaoProcessoInputViewModel viewModel)
        {
            var result = new ResultadoApplication();

            try
            {
                if (!viewModel.CodTipoProcesso.HasValue || !viewModel.DataVigencia.HasValue)
                {
                    throw new Exception(Textos.Geral_Mensagem_Dado_Invalido);
                }

                var entidade = await _juroCorrecaoProcessoService.ObterPorChavesCompostas(viewModel.CodTipoProcesso.Value, viewModel.DataVigencia.Value);

                if (entidade == null)
                {
                    throw new Exception("Não foi possivel localizar o dado solicitado.");
                }

                entidade.PreencherDados(_mapper.Map<JuroCorrecaoProcesso>(viewModel));

                var validate = entidade.Validar();

                if (validate.IsValid)
                {
                    await _juroCorrecaoProcessoRepository.Atualizar(entidade);
                    _juroCorrecaoProcessoService.Commit();

                    result.ExecutadoComSuccesso();
                }
                else
                    result.ExecutadoComErro(validate.ToString());

            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> ExcluirJuroCorrecaoProcesso(long? codigo, DateTime? dataVigencia)
        {
            var result = new ResultadoApplication();

            try
            {
                if (!codigo.HasValue || !dataVigencia.HasValue)
                {
                    throw new Exception(Textos.Geral_Mensagem_Dado_Invalido);
                }

                var entidade = await _juroCorrecaoProcessoService.ObterPorChavesCompostas(codigo.Value, dataVigencia.Value);

                if (entidade == null)
                {
                    throw new Exception("Não foi possivel localizar o dado solicitado.");
                }

                await _juroCorrecaoProcessoService.Remover(entidade);

                _juroCorrecaoProcessoService.Commit();

                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExibirMensagem(ex.Message);
                result.ExecutadoComErro(ex);
            }

            return result;
        }
    }
}
