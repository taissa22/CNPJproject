using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Controllers
{
    [Route("api/esocial/v1/[controller]")]
    [ApiController]
    public class ESocialDashboardController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialDashboardController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        [HttpPost("obter-dados-dashboard")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ObterDadosDashboard([FromBody] ESocialDashboardRequestDTO requestDTO, [FromServices] ESocialDashboardService service, CancellationToken ct)
        {
            try
            {
                var (dadosDashboad, mensagemErro) = await service.ObtemDadosDashboard(requestDTO, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(dadosDashboad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("obter-erros")]
        public async Task<ActionResult<int>> ObterQuantidadeErrosEnvioAsync([FromBody] ESocialDashboardRequestDTO requestDTO, [FromServices] ESocialDashboardService service, CancellationToken ct)
        {
            try
            {
                var (quantidadeErrosEnvio, mensagemErro) = await service.ObetemQuantidadeErrosEnvio(requestDTO, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(quantidadeErrosEnvio);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("filtro-empresas")]
        public async Task<ActionResult<IEnumerable<RetornoListaDefaultDTO>>> ObterListaEmpresasFiltroAsync([FromServices] ESocialDashboardService service, CancellationToken ct)
        {
            try
            {
                var loginUsuario = User.Identity!.Name!;

                var (listaEmpresas, mensagemErro) = await service.ObtemFiltroEmpresaAgrupadora(loginUsuario, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(listaEmpresas);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpGet("filtro-estados")]
        public async Task<ActionResult<IEnumerable<RetornoListaUFDTO>>> ObterListaUFsFiltroAsync([FromServices] ESocialDashboardService service, CancellationToken ct)
        {
            try
            {
                var (listaUFs, mensagemErro) = await service.ObtemFiltroUF(ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(listaUFs);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }


    }
        
}
