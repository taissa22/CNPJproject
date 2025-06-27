using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Interface;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JuroCorrecaoProcessoController : JuridicoControllerBase
    {
        private readonly IJuroCorrecaoProcessoAppService _appService;

        public JuroCorrecaoProcessoController(IJuroCorrecaoProcessoAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("ConsultarJuroCorrecaoProcesso")]        
        public async Task<IResultadoApplication<ICollection<JuroCorrecaoProcessoViewModel>>> Consultar(VigenciaCivilFiltrosViewModel viewModel)
        {
            var resultado = await _appService.ObterJuroCorrecaoProcessoPorFiltro(viewModel);
            return resultado;
        }

        [HttpPost("ExportarJuroCorrecaoProcesso")]
        public async Task<IResultadoApplication<byte[]>> Exportar(VigenciaCivilFiltrosViewModel viewModel)
        {
            var result = await _appService.ExportarJuroCorrecaoProcesso(viewModel);
            return result;
        }

        [HttpPost("SalvarJuroCorrecaoProcesso")]
        public async Task<IResultadoApplication> SalvarJuroCorrecaoProcesso([FromBody] JuroCorrecaoProcessoInputViewModel viewModel)
        {
            var resultado = await _appService.CadastrarJurosCorrecaoProcesso(viewModel);

            return resultado;
        }

        [HttpPost("EditarJuroCorrecaoProcesso")]
        public async Task<IResultadoApplication> EditarJuroCorrecaoProcesso([FromBody] JuroCorrecaoProcessoInputViewModel viewModel)
        {
            var resultado = await _appService.EditarJuroCorrecaoProcesso(viewModel);

            return resultado;
        }

        [HttpGet("ExcluirJuroCorrecaoProcesso")]
        public async Task<IResultadoApplication> ExcluirJuroCorrecaoProcesso(long codigo, DateTime dataVigencia)
        {
            var result = await _appService.ExcluirJuroCorrecaoProcesso(codigo, dataVigencia);
            return result;
        }
    }
}
