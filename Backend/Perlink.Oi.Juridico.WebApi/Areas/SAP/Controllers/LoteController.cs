using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Application.Security;
using Perlink.Oi.Juridico.CrossCutting.IoC;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
   
    public class LotesController : JuridicoControllerBase
    {
        private readonly ILoteAppService appService;
        private readonly ILancamentoProcessoAppService appLancService;
        private readonly ILancamentoProcessoAppService lancamentoProcessoAppService;
        private static ServiceProvider _serviceProvider;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        /// <param name="AppLancService"></param>

        public LotesController(ILancamentoProcessoAppService AppLancService, ILoteAppService AppService) {

            this.appLancService = AppLancService;
            this.appService = AppService;
        }

        /// <summary>
        /// Recupera um Lote por codigo Sap.
        /// </summary>
        /// <returns>Lote correspondente ao código.</returns>
        [HttpGet("recuperarPorNumeroSAPCodigoTipoProcesso")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IResultadoApplication<string>> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroSAP, long CodigoTipoProcesso)
        {
            var resultado = await appService.RecuperarPorNumeroSAPCodigoTipoProcesso(NumeroSAP, CodigoTipoProcesso);
            return resultado;
        }
        
        /// <summary>
        /// Recuperar Todos os Lotes de acordo com o filtro informado
        /// </summary>
        /// <returns>Lista paginada de LoteResultadoViewModel</returns>
        /// <param name="loteFiltroDTO"></param>
        [HttpPost("RecuperarPorFiltroPaginado")]
        public async Task<IPagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>> RecuperarPorFiltroPaginado([FromBody] LoteFiltroDTO loteFiltroDTO)
        {
            var resultado = await appService.RecuperarPorFiltro(loteFiltroDTO);
            if (resultado.Sucesso == true)
                if (loteFiltroDTO.Total <= 0) {
                    var retTotais = await appService.ObterQuantidadeTotalPorFiltro(loteFiltroDTO);
                    resultado.Total = retTotais.Total;
                    resultado.TotalLotes = retTotais.TotalLotes;
                    resultado.TotalValorLotes = retTotais.TotalValorLotes;
                    resultado.QuantidadesLancamentos = retTotais.QuantidadesLancamentos;
                } else {
                    if (Math.Floor(Convert.ToDecimal(loteFiltroDTO.Total / loteFiltroDTO.Quantidade)) == loteFiltroDTO.Pagina) {
                        var retTotais = await appService.ObterQuantidadeTotalPorFiltro(loteFiltroDTO);
                        resultado.Total = retTotais.Total;
                        resultado.TotalLotes = retTotais.TotalLotes;
                        resultado.TotalValorLotes = retTotais.TotalValorLotes;
                        resultado.QuantidadesLancamentos = retTotais.QuantidadesLancamentos;
                    } else
                        resultado.Total = loteFiltroDTO.Total;
                }
            return resultado;
        }


        /// <summary>
        /// Cancela o lote.
        /// </summary>
        /// <returns>Detalhe Lote.</returns>
        [HttpPost("CancelamentoLote")]
        public async Task<IResultadoApplication<LoteCanceladoViewModel>> CancelamentoLote([FromBody]LoteCancelamentoDTO lotecancelamentoDTO)
        {
            return await appService.CancelamentoLote(lotecancelamentoDTO);
        }

        /// <summary>
        /// Retorna o csv dos lotes de acordo com o filtro selecionado.
        /// </summary>
        /// <returns>Lista de LoteResultadoViewModel</returns>
        /// /// <param name="loteFiltroDTO"></param>
        /// <param name="CodigoTipoProcesso"></param>
        [HttpPost("ExportarLote")]
        public async Task<IResultadoApplication<byte[]>> Exportar(LoteFiltroDTO loteFiltroDTO, long CodigoTipoProcesso)
        {
            var result = await appService.Exportar(loteFiltroDTO, CodigoTipoProcesso);
            return result;
        }

        /// <summary>
        /// Recupera Detalhes do Lote Pelo seu Codigo.
        /// </summary>
        /// <returns>Detalhe Lote.</returns>
        [HttpGet("RecuperaDetalhes")]
        public async Task<IResultadoApplication<LoteDetalhesViewModel>> RecuperaDetalhes(long CodigoLote)
        {
            return await appService.RecuperaDetalhes(CodigoLote);
        }
        /// <summary>
        /// Recupera Detalhes do Lote Pelo seu Codigo.
        /// </summary>
        /// <returns>Detalhe Lote.</returns>
        [HttpGet("VerificarCompromisso")]
        public async Task<IResultadoApplication<CompromissoLoteViewModel>> VerificarCompromisso(long CodigoLote)
        {
            return await appService.VerificarCompromisso(CodigoLote);
        }



        /// <summary>
        /// Atualiza o Número do Lote BB
        /// </summary>
        /// <returns>Novo número do Lote BB</returns>
        [HttpGet("AtualizarNumeroLoteBB")]
        public async Task<IResultadoApplication<long?>> AtualizarNumeroLoteBB(long CodigoLote)
        {
            return await appService.AtualizarNumeroLoteBBAsync(CodigoLote);
        }

        #region Criação de Lote

        ///<summary>
        ///Filtrar Previa de Lotes agrupados por Empresa Centralizadora 
        ///</summary>
        ///<returns>Retorna lista de LoteCriacaoResultadoEmpresaCentralizadoraViewModel</returns>
        [HttpPost("ObterLotesAgrupadoPorEmpresaCentralizadora")]
        public async Task<IResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraViewModel>>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros) {                    
            var result = await appLancService.ObterLotesAgrupadoPorEmpresaCentralizadora(filtros);

            return result;    

        }


    
        ///<summary>
        ///Filtrar Previa de Lotes agrupados por Empresa do grupo 
        ///</summary>
        ///<returns>
        ///Retorna lista de LoteCriacaoResultadoEmpresaGrupoViewModel
        ///</returns>
        /// <param name="filtros"></param>
        [HttpPost("ObterLotesAgrupadoPorEmpresaDoGrupo")]
        public async Task<IPagingResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaGrupoViewModel>>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros) {            
            var result = await appLancService.ObterLotesAgrupadoPorEmpresadoGrupo(filtros);           
            return result;
        }

        ///<summary>
        ///Retorna os possiveis lançamentos para criação do lote
        ///</summary>
        ///<returns>Retorna lista de LoteCriacaoResultadoLancamentoViewModel</returns>
        [HttpPost("ObterLancamentosLoteCriacao")]
        public async Task<IResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoViewModel>>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros) {
            Stopwatch stopwatch = new Stopwatch();
            RegisterServices();
            ILogger<LotesController> logger = _serviceProvider.GetService<ILoggerFactory>().CreateLogger<LotesController>();
            stopwatch.Start();
            logger.LogInformation("Inicio da consulta (Controller)");
            var result = await appLancService.ObterLancamentosLoteCriacao(filtros, logger);
            stopwatch.Stop();
            logger.LogInformation("Fim da consulta (Controller)");
            logger.LogInformation("tempo gasto: " + stopwatch.Elapsed.TotalSeconds + " em segundos");
            DisposeServices();
            return result;
        }

        ///<summary>
        ///Criar o vinculo entre o lote e os lançamentos selecionados
        ///</summary>
        ///<returns>
        ///Retorna se o vinculo foi criado com sucesso
        ///</returns>
        /// <param name="loteCriacaoViewModel"></param>

        [HttpPost("CriarLote")]
        public async Task<IResultadoApplication> CriarLote([FromBody]LoteCriacaoViewModel loteCriacaoViewModel) {
            var resultado = await appService.CriarLote(loteCriacaoViewModel);
            return resultado;
        }
        #endregion Criação de Lote

        /// <summary>
        /// Recuperar Todos os parametros de um CodigoFiltroConsulta
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("CarregarFiltros")]
        public async Task<IResultadoApplication<LoteCriteriosFiltrosViewModel>> CarregarFiltros(long CodigoTipoProcesso)
        {
            var resultado = await appService.CarregarFiltros(CodigoTipoProcesso);
            return resultado;
        }

        /// <summary>
        /// Retorna o arquivo regerado de Lote BB.
        /// </summary>
        /// <returns>Lista de LoteResultadoViewModel</returns>
        [HttpPost("RegerarArquivoBB")]
        public IResultadoApplication<dynamic> RegerarArquivoBB(ArquivoBBDTO regerarArquivoBBDTO)
        {
            var result = appService.RegerarArquivoBB(regerarArquivoBBDTO);

            return result;
        }

        /// <summary>
        /// Retorna o numero do lote ou 0.
        /// </summary>
        /// <returns>Long</returns>
        [HttpGet("ValidarNumeroLote")]
        public async Task<IResultadoApplication<long>> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso) {
            var result = await appService.ValidarNumeroLote(numeroLote, codigoTipoProcesso);

            return result;
        }

        private static void RegisterServices() {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var services = new ServiceCollection();

            Configuracao.InjecaoDependencia(services, configuration.GetConnectionString("JuridicoConnection"));
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();


            services.AddLogging(builder => {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });

            _serviceProvider = services.BuildServiceProvider();
        }
        private static void DisposeServices() {
            if (_serviceProvider == null) {
                return;
            }
            if (_serviceProvider is IDisposable) {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}