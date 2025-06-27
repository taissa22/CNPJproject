using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.FechamentoTrabalhistaContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Contingencia.DTOs;
using Oi.Juridico.WebApi.V2.Areas.Contingencia.DTOs.CsvHelperMap;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Controllers
{
    [Route("contingencia/[controller]")]
    [ApiController]
    public class FechamentoTrabalhistaController : ControllerBase
    {
        private readonly FechamentoTrabalhistaContext _db;
        private readonly ILogger<FechamentoTrabalhistaController> _logger;

        public FechamentoTrabalhistaController(FechamentoTrabalhistaContext db, ILogger<FechamentoTrabalhistaController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("ExportaGrupamentoEmpresasOutliers")]
        public async Task<FileContentResult> ExportaGrupamentoEmpresasOutliersAsync(CancellationToken ct)
        {
            var lista = await _db.GrupoEmpresasOutliersParte
                .AsNoTracking()
                .OrderBy(x => x.CodGrupoEmpresasOutliersNavigation.NomGrupoEmpresasOutliers)
                .ThenBy(x => x.CodParteNavigation.NomParte)
                .Select(x => new ExportaGrupamentoEmpresasOutliersResponse(x.CodGrupoEmpresasOutliersNavigation.CodGrupoEmpresasOutliers, x.CodGrupoEmpresasOutliersNavigation.NomGrupoEmpresasOutliers, x.CodParteNavigation.CodParte, x.CodParteNavigation.NomParte))
                .ToArrayAsync(ct);

            var csv = lista.ToCsvByteArray(typeof(ExportaGrupamentoEmpresasOutliersResponseMap), sanitizeForInjection: false);

            return File(csv, "application/csv", "Grupamento_Empresas_Outliers.csv");
        }
    }
}
