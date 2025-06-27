using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.WebApi.Areas.Contingencia.Fechamento
{
    // TODO: João Pedro Menezes
    [Route("contingencia/fechamento/provisao-trabalhista")]
    [ApiController]
    [Authorize]
    public class ProvisaoTrabalhistaController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult ObterTodos([FromServices] IDatabaseContext database,
            [FromQuery] bool semExclusao = true, [FromQuery] bool percentual = true, [FromQuery] bool desvioPadrao = true)
        {
            var fechamentos = database.FechamentosProvisaoTrabalhista
                .AsNoTracking()
                .Where(x => (x.TipoDeOutlierId == TipoDeOutliers.SEM_EXCLUSAO.Id && semExclusao) || (x.TipoDeOutlierId == TipoDeOutliers.PERCENTUAL.Id && percentual) || (x.TipoDeOutlierId == TipoDeOutliers.DESVIO_PADRAO.Id && desvioPadrao))
                .Select(x => new { x.Id, x.DataFechamento, x.NumeroDeMeses, x.TipoDeOutliers })
                .GroupBy(x => new { x.DataFechamento, x.TipoDeOutliers, x.NumeroDeMeses })
                .Select(x => x.First())
                .OrderByDescending(x => x.DataFechamento)
                .ToArray();

            return Ok(fechamentos);
        }

        [HttpGet("{fechamentoId}")]
        public IActionResult ObterPorId([FromServices] IDatabaseContext database, [FromRoute] int fechamentoId)
        {
            var fechamento = database.FechamentosProvisaoTrabalhista.FirstOrDefault(x => x.Id == fechamentoId);

            var centralizadoras = database.FechamentosProvisaoTrabalhista
                .AsNoTracking()
                .Include(x => x.EmpresaCentralizadora)
                .Where(x => x.TipoDeOutlierId == fechamento.TipoDeOutlierId)
                .Where(x => x.DataFechamento == fechamento.DataFechamento)
                .Where(x => x.NumeroDeMeses == fechamento.NumeroDeMeses)
                .Where(x => x.EmpresaCentralizadoraId != null)
                .Select(x => x.EmpresaCentralizadora)
                .Select(x => new
                {
                    x.Codigo,
                    x.Nome
                })
                .OrderBy(x => x.Nome)
                .ToArray();

            if (centralizadoras.Count() == 0)
            {
                centralizadoras = database.EmpresasCentralizadoras
                    .Select(x => new
                    {
                        x.Codigo,
                        x.Nome
                    })
                    .OrderBy(x => x.Nome)
                    .ToArray();
            }

            return Ok(new
            {
                fechamento.Id,
                fechamento.DataFechamento,
                fechamento.NumeroDeMeses,
                fechamento.TipoDeOutliers,
                Centralizadoras = centralizadoras
            });
        }

        [HttpGet("empresas-da-centralizadora/{centralizadora}")]
        public IActionResult EmpresasDaCentralizadora([FromServices] IDatabaseContext database, [FromRoute] int centralizadora)
        {
            var empresas = database.EmpresasDoGrupo.Where(x => x.EmpresaCentralizadora.Codigo == centralizadora).OrderBy(x => x.Nome);
            return Ok(empresas.Select(x => new { x.Id, x.Nome }));
        }

        [HttpGet("anteriores/{fechamentoId}")]
        public IActionResult ObterAnteriores([FromServices] IDatabaseContext database, [FromRoute] int fechamentoId,
            [FromQuery] IEnumerable<int> centralizadoras,
            [FromQuery] bool semExclusao = true, [FromQuery] bool percentual = true, [FromQuery] bool desvioPadrao = true)
        {
            var fechamento = database.FechamentosProvisaoTrabalhista.FirstOrDefault(x => x.Id == fechamentoId);

            var fechamentosAnteriores = database.FechamentosProvisaoTrabalhista
                .AsNoTracking()
                .Where(x => (x.TipoDeOutlierId == TipoDeOutliers.SEM_EXCLUSAO.Id && semExclusao) || (x.TipoDeOutlierId == TipoDeOutliers.PERCENTUAL.Id && percentual) || (x.TipoDeOutlierId == TipoDeOutliers.DESVIO_PADRAO.Id && desvioPadrao))
                .Where(x => x.DataFechamento < fechamento.DataFechamento)
                .Where(x => centralizadoras.Contains(x.EmpresaCentralizadoraId.GetValueOrDefault()) || x.EmpresaCentralizadoraId == null)
                .Select(x => new { x.Id, x.DataFechamento, x.NumeroDeMeses, x.TipoDeOutliers })
                .GroupBy(x => new { x.DataFechamento, x.TipoDeOutliers, x.NumeroDeMeses })
                .Select(x => x.First())
                .OrderByDescending(x => x.DataFechamento)
                .ToArray();

            return Ok(fechamentosAnteriores);
        }
    }
}