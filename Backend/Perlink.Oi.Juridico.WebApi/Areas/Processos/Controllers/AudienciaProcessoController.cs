using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Processos.Interface;
using Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Processos.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AudienciaProcessoController : JuridicoControllerBase
    {
        private readonly IAudienciaProcessoAppService appService;

        public AudienciaProcessoController(IAudienciaProcessoAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet("CarregarFiltros")]
        public async Task<IResultadoApplication> CarregarFiltros()
        {
            var resultado = await appService.CarregarFiltros();
            return resultado;
        }

        [HttpGet("RecuperarPartes")]
        public async Task<IResultadoApplication> RecuperarPartes(long codigoProcesso)
        {
            var resultado = await appService.RecuperarPartes(codigoProcesso);
            return resultado;
        }

        [HttpGet("RecuperarPedidos")]
        public async Task<IResultadoApplication> RecuperarPedidos(long codigoProcesso)
        {
            var resultado = await appService.RecuperarPedidos(codigoProcesso);
            return resultado;
        }


        [HttpGet("RecuperarCombosEdicao")]
        public async Task<IResultadoApplication> RecuperarCombosEdicao()
        {
            var resultado = await appService.RecuperarCombosEdicao();
            return resultado;
        }

        [HttpGet("RecuperarAdvogadoEscritorio")]
        public async Task<IResultadoApplication> RecuperarAdvogadoEscritorio(long codigoEscritorio)
        {
            var resultado = await appService.RecuperarAdvogadoEscritorio(codigoEscritorio);
            return resultado;
        }

        [HttpPost("pesquisar")]
        public IActionResult Pesquisar(PesquisarFiltroVM viewmodel)
        {
            try
            {
                var resultado = appService.ObterAudienciasPorFiltro(viewmodel.Filters, viewmodel.PageNumber, viewmodel.PageSize, viewmodel.SortOrders, viewmodel.IsExportMethod);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new[] { $"Ocorreu uma falha interna no servidor. Erro: {ex.Message}" }
                });
            }
        }

        [HttpPost("exportar")]
        public IResultadoApplication<byte[]> ExportarCSV(PesquisarFiltroVM viewmodel)
        {
            var result = appService.Exportar(viewmodel.Filters, viewmodel.SortOrders, viewmodel.IsExportMethod);
            return result;
        }

        [HttpPost("exportarCompleto")]
        public IResultadoApplication<byte[]> ExportarCSVCompleto(PesquisarFiltroVM viewmodel)
        {
            var result = appService.ExportarCompleto(viewmodel.Filters, viewmodel.SortOrders, viewmodel.IsExportMethod);
            return result;
        }

        [HttpPost("download")]
        public async Task<IResultadoApplication<byte[]>> Download(PesquisarFiltroVM viewmodel)
        {
            var result = await appService.Download(viewmodel.Filters, viewmodel.SortOrders);
            return result;
        }

        [HttpPost("editar")]
        public async Task<IResultadoApplication> Editar(AudienciaProcessoUpdateVM viewmodel)
        {
            var result = await appService.Atualizar(viewmodel);
            return result;
        }
    }
}
