using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.Processos.Adapters;
using Perlink.Oi.Juridico.Application.Processos.Interface;
using Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.Processos.Interface.AdoRepository;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.Processos;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Globalization;

namespace Perlink.Oi.Juridico.Application.Processos.Impl
{
    public class AudienciaProcessoAppService : BaseCrudAppService<AudienciaProcessoUpdateVM, AudienciaProcesso, long>, IAudienciaProcessoAppService
    {
        private readonly IAdvogadoEscritorioService advogadoService;
        private readonly IComarcaService comarcaService;
        private readonly IEmpresaDoGrupoService empresaService;
        private readonly IEscritorioService escritorioService;
        private readonly IEstadoService estadoService;
        private readonly IPrepostoService prepostoService;
        private readonly IParteService parteService;
        private readonly IPedidoService pedidoService;
        private readonly IUsuarioService usuarioService;
        private readonly IConfiguration configuration;


        private readonly IAudienciaProcessoService _audienciaProcessoService;
        private readonly IMapper mapper;

        IAudienciaProcessoRepository _audienciaProcessoRepository;
        private readonly IAudienciaProcessoAdoRepository _audienciaProcessoAdoRepository;

        public AudienciaProcessoAppService(IAdvogadoEscritorioService advogadoService, IMapper mapper, IComarcaService comarcaService,
                                           IEmpresaDoGrupoService empresaService, IEscritorioService escritorioService,
                                           IEstadoService estadoService, IPrepostoService prepostoService, IParteService parteService, 
                                           IPedidoService pedidoService, IAudienciaProcessoService audienciaProcessoService, IUsuarioService usuarioService,
                                           IAudienciaProcessoRepository audienciaProcessoRepository,
                                           IAudienciaProcessoAdoRepository audienciaProcessoAdoRepository,
                                           IConfiguration configuration) : base(audienciaProcessoService, mapper)
        {
            this.advogadoService = advogadoService;
            this.comarcaService = comarcaService;
            this.mapper = mapper;
            this.empresaService = empresaService;
            this.escritorioService = escritorioService;
            this.estadoService = estadoService;
            this.prepostoService = prepostoService;
            this.parteService = parteService;
            this.pedidoService = pedidoService;
            this.usuarioService = usuarioService;
            this.configuration = configuration;

            _audienciaProcessoService = audienciaProcessoService;

            _audienciaProcessoRepository = audienciaProcessoRepository;
            _audienciaProcessoAdoRepository = audienciaProcessoAdoRepository;
        }

        public async Task<IResultadoApplication<AgendaAudienciaFiltrosViewModel>> CarregarFiltros()
        {
            var result = new ResultadoApplication<AgendaAudienciaFiltrosViewModel>();

            try
            {
                var usuarioLogado = await usuarioService.ObterUsuarioLogado();

                var model = new AgendaAudienciaPedidosDTO()
                {
                    ListaComarca = await comarcaService.RecuperarTodasComarca(),
                    ListaEmpresa = await empresaService.RecuperarEmpresaDoGrupo(),
                    ListaEstado = await estadoService.RecuperarListaEstados(),
                    ListaPreposto = await prepostoService.ConsultarPreposto(),
                    ListaEscritorio = await escritorioService.RecuperarEscritorioTrabalhistaFiltro(usuarioLogado.EhEscritorio, usuarioLogado.Id),
                    ListaAdvogado = await advogadoService.ConsultarAdvogadoEscritorio(usuarioLogado.EhEscritorio, usuarioLogado.Id)
                };            

                result.DefinirData(mapper.Map<AgendaAudienciaFiltrosViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<AgendaAudienciaPartesViewModel>> RecuperarPartes(long codigoProcesso)
        {
            var result = new ResultadoApplication<AgendaAudienciaPartesViewModel>();

            try
            {
                var model = new AgendaAudienciaPartesDTO()
                {
                    Autores = await parteService.RecuperarPartesTrabalhistaPorCodigoProcesso(codigoProcesso, true),
                    Reus = await parteService.RecuperarPartesTrabalhistaPorCodigoProcesso(codigoProcesso, false)
                };

                result.DefinirData(mapper.Map<AgendaAudienciaPartesViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<AgendaAudienciaPedidosViewModel>> RecuperarPedidos(long codigoProcesso)
        {
            var result = new ResultadoApplication<AgendaAudienciaPedidosViewModel>();

            try
            {
                var model = new AgendaAudienciaPedidoDTO()
                {
                    Pedidos = await pedidoService.ConsultarPedido((long) TipoProcessoEnum.Trabalhista, codigoProcesso)                    
                };

                result.DefinirData(mapper.Map<AgendaAudienciaPedidosViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<AgendaAudienciaComboEdicaoViewModel>> RecuperarCombosEdicao()
        {
            var result = new ResultadoApplication<AgendaAudienciaComboEdicaoViewModel>();

            try
            {
                var usuarioLogado = await usuarioService.ObterUsuarioLogado();

                var model = new AgendaAudienciaComboEdicaoDTO()
                {
                    ListaPrepostos = await prepostoService.ListarPreposto((long)TipoProcessoEnum.Trabalhista),
                    ListaEscritorios = await escritorioService.RecuperarEscritorioTrabalhistaFiltro(usuarioLogado.EhEscritorio, usuarioLogado.Id)
                };

                result.DefinirData(mapper.Map<AgendaAudienciaComboEdicaoViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<AgendaAudienciaAdvogadoViewModel>> RecuperarAdvogadoEscritorio(long codigoEscritorio)
        {
            var result = new ResultadoApplication<AgendaAudienciaAdvogadoViewModel>();

            try
            {
                var model = new AgendaAudienciaAdvogadoDTO()
                {
                    ListaAdvogados = await advogadoService.RecuperarAdvogadoEscritorioPorCodigoProfissional(codigoEscritorio)
                };

                result.DefinirData(mapper.Map<AgendaAudienciaAdvogadoViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public AudienciaProcessoBuscaResultadoVM ObterAudienciasPorFiltro(IEnumerable<FilterVM> lstFilters, int pageNumber, int pageSize, IEnumerable<SortOrderVM> orders, bool IsExportMethod)
        {
            var filters = lstFilters.Select(vm => FilterAdapter.ToDTO(vm)).ToList();
            var sortOrders = orders.Select(vm => SortOrderAdapter.ToDTO(vm)).ToList();

            var listaFiltrada = _audienciaProcessoAdoRepository.ObterTodosPorFiltro(filters, pageNumber, pageSize, sortOrders, IsExportMethod)
                                                               .Select(dto => AudienciaProcessoAdapter.ToViewModel(dto));

            var totalElementos = _audienciaProcessoAdoRepository.GetTotalCount(filters);

            return new AudienciaProcessoBuscaResultadoVM(listaFiltrada, totalElementos);
        }

        public IResultadoApplication<byte[]> Exportar(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders, bool IsExportMethod)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var filters = lstFilters.Select(vm => FilterAdapter.ToDTO(vm)).ToList();
                var sortOrders = orders.Select(vm => SortOrderAdapter.ToDTO(vm)).ToList();

                var lista = _audienciaProcessoAdoRepository.ObterTodosPorFiltro(filters, 0, 0, sortOrders, IsExportMethod)
                                                           .Select(dto => AudienciaProcessoAdapter.ToViewModel(dto));                

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
                    csv.WriteRecords(lista);
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

        public IResultadoApplication<byte[]> ExportarCompleto(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders, bool IsExportMethod)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var filters = lstFilters.Select(vm => FilterAdapter.ToDTO(vm)).ToList();
                var sortOrders = orders.Select(vm => SortOrderAdapter.ToDTO(vm)).ToList();

                var lista = _audienciaProcessoAdoRepository.ObterTodosCompletoPorFiltro(filters, 0, 0, sortOrders, IsExportMethod)
                                                           .Select(dto => AudienciaProcessoAdapter.ToExportViewModel(dto));

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
                    csv.WriteRecords(lista);
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


        public async Task<IResultadoApplication<byte[]>> Download(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var filters = lstFilters.Select(vm => FilterAdapter.ToDTO(vm)).ToList();
                var sortOrders = orders.Select(vm => SortOrderAdapter.ToDTO(vm)).ToList();

                var lista = _audienciaProcessoAdoRepository.ObterTodosPorFiltro(filters, 0, 0, sortOrders, true)
                                                           .Select(dto => AudienciaProcessoAdapter.ToDownloadViewModel(dto));

                if (lista.Count() > 0)
                {
                    var json = JsonSerializer.Serialize(lista);

                    //WebClient client = new WebClient();
                    //client.Headers.Add("Content-Type", "application/json");
                    var host = global::Oi.Juridico.AddOn.Configuracoes.Relatorios.EndpointIPE;
                    var token = global::Oi.Juridico.AddOn.Configuracoes.Relatorios.Token;
                    var url = $"{host}/AgendaAudienciaTrabalhista?token={token}";
                    //var pdf = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(json));
                   
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                  
                    using var client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(20);

                    var response = await client.PostAsync(url, data);

                    var pdf = await response.Content.ReadAsByteArrayAsync();

                    result.DefinirData(pdf);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();
                }
                else
                {
                    result.ExecutadoComErro(Textos.Mensagem_Erro_Arquivo_Vazio);
                }  
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> Atualizar(AudienciaProcessoUpdateVM viewmodel)
        {
            var result = new ResultadoApplication();

            try
            {
                var entidade = await _audienciaProcessoService.ObterPorChavesCompostas(viewmodel.CodProcesso, viewmodel.SequenciaAudiencia);

                if (entidade == null)
                {
                    throw new Exception("Não foi possivel localizar o dado solicitado.");
                }

                entidade.PreencherDados(mapper.Map<AudienciaProcesso>(viewmodel));

                var validate = entidade.Validar();

                if (validate.IsValid)
                {
                    await _audienciaProcessoRepository.Atualizar(entidade);
                    _audienciaProcessoService.Commit();

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
    }
}
