using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.Extensions;
using Oi.Juridico.Contextos.V2.PermissaoContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Entities;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.DTOs.Permissao;
using Oi.Juridico.WebApi.V2.DTOs.Permissao.CsvHelperMap;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers
{
    /// <summary>
    /// Api associação das permissões de módulos
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PermissaoController : ControllerBase
    {
        private readonly PermissaoContext _db;
        private readonly ILogger<PermissaoController> _logger;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        public PermissaoController(PermissaoContext db, ILogger<PermissaoController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint que trata da atualização da permissão
        /// </summary>
        /// <param name="model">Model com a permissão a ser alterada</param>
        /// <returns>200 para sucesso</returns>
        [HttpPost]
        public async Task<ActionResult> SalvarAsync(PermissaoRequest model, CancellationToken ct)
        {
            try
            {
                var permissao = _db.AcaMenu
                    .Include(x => x.CodModuloNavigation)
                    .Where(x => x.CodMenu.Contains(model.PermissaoId))
                    .FirstOrDefault();

                if (permissao is null)
                    return Problem($"Não foi possível localizar a permissão com o id {model.PermissaoId}");

                var remover = permissao.CodModuloNavigation.Select(x => (int)x.CodModulo).Except(model.ListaModulos).ToArray();
                var incluir = model.ListaModulos.Except(permissao.CodModuloNavigation.Select(x => (int)x.CodModulo)).ToArray();

                permissao.DscCaminhoMenuTela = model.Caminho;
                permissao.DescricaoMenu = model.Descricao;

                permissao.CodModuloNavigation.RemoveAll(x => remover.Contains(x.CodModulo));

                var listModulos = await ObterModulosAsync(incluir, ct);

                permissao.CodModuloNavigation.AddAll(listModulos);
                await _db.SaveChangesAsync(User.Identity!.Name!, true);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém os dados do usuário logado
        /// </summary>
        /// <param name="moduloId">Id do módulo</param>
        /// <param name="ct"></param>
        /// <returns>Dados do usuário logado</returns>
        [HttpGet, Route("Modulos")]
        public async Task<ActionResult<ModuloResponse>> ObterModulosAsync(int? moduloId, CancellationToken ct)
        {
            return Ok(await _db.ModuloSisjur
                  .AsNoTracking()
                  .WhereIfNotNull(x => x.CodModulo == moduloId, moduloId)
                  .OrderBy(x => x.DscModulo)
                  .Select(x => new ModuloResponse(x.CodModulo, x.DscModulo))
                  .ToListAsync(ct));
        }

        /// <summary>
        /// Obtém a lista de permissões e gera o arquivo de exportação
        /// </summary>
        /// <param name="filtro">Filtro com paginanação e ordenação</param>
        /// <param name="coluna">Coluna pela qual será feita a ordenação</param>
        /// <param name="direcao">Direção pela qual será feita a ordenação</param>
        /// <param name="ct"></param>
        /// <returns>Lista de permissões</returns>
        [HttpGet, Route("Exportar")]
        public async Task<ActionResult> ExportarAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? modulo,
             [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc")
        {
            var permissoes = await this.ObterBaseConsulta(filtro, modulo, QueryableExtensions.ConverterOuPadrao(coluna, PermissaoOrder.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc")).ToListAsync(ct);

            var csv = permissoes.Where(x => modulo == null || (x.Modulos != null && x.Modulos.Split("; ").Any(y => y == modulo!))).ToList()
                                .ToCsvByteArray(typeof(PermissaoResponseMap), header: ">>> PERMISSÕES", textoComAspas: true);

            DateTime dtExportacao = DateTime.Now;
            string nomeArquivo = $"relatorio_juridico_permissoes_{dtExportacao:yyyy_MM_dd}_{dtExportacao.Hour}h{dtExportacao.Minute}m{3}s_{dtExportacao.Second}_{this.GeraValorAleatorioParaNomeDeArquivo()}.csv";
            return File(csv, "application/csv", nomeArquivo);
        }

        /// <summary>
        /// Obtém a lista de permissões
        /// </summary>
        /// <returns>Lista de permissões</returns>
        [HttpGet]
        public async Task<ActionResult<PermissaoRequest>> ObterPermissoesAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? modulo,
             [FromQuery] int pagina = 1, [FromQuery] int quantidade = 8, [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc")
        {
            var qryPermissoes = ObterBaseConsulta(filtro, modulo, QueryableExtensions.ConverterOuPadrao(coluna, PermissaoOrder.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));            
            var lstPermissoes = await qryPermissoes.ToListAsync(ct);

            var listaPermissoesFiltrada = lstPermissoes.Where(x => modulo == null || (x.Modulos != null && x.Modulos.Split("; ").Any(y => y == modulo!))).ToList();
            var totalRegistros = listaPermissoesFiltrada.Count;

            var skip = PaginationHelper.PagesToSkip(quantidade, totalRegistros, pagina);

            listaPermissoesFiltrada = listaPermissoesFiltrada.Skip(skip).Take(quantidade).ToList();


            try
            {
                return Ok(new
                {
                    quantidade = totalRegistros,
                    permissoes = listaPermissoesFiltrada
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gera a query base com o filtro
        /// </summary>
        /// <param name="filtro">Termo buscado</param>
        /// <param name="ordem">Coluna pela qual será ordenado</param>
        /// <param name="asc">Direção pela qual será ordenado</param>
        /// <returns>Consulta sql pronta para ser executada</returns>
        private IQueryable<PermissaoResponse> ObterBaseConsulta(string? filtro, string? modulo, PermissaoOrder ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            var consulta = _db.VPermissoesModulos
               .AsNoTracking()
               .WhereIfNotEmpty(x => x.DescricaoMenu.Contains(filtro!) || x.DscCaminhoMenuTela.Contains(filtro!) || x.CodMenu.Contains(filtro!), filtro)
               .WhereIfNotEmpty(x => x.Modulos.Contains(modulo!), modulo)
               .Select(x => new PermissaoResponse
               {
                   PermissaoId = x.CodMenu,
                   Descricao = x.DescricaoMenu,
                   Caminho = x.DscCaminhoMenuTela,
                   Modulos = x.Modulos
               });

            return IncluirOrdenacao(consulta, ordem, asc);
        }

        /// <summary>
        /// Inclui ordenação de acordo com o informado pleo usuário
        /// </summary>
        /// <param name="consulta">Query com a consulta</param>
        /// <param name="ordem">Coluna pela qual será ordenado</param>
        /// <param name="asc">Direção pela qual será ordenado</param>
        /// <returns></returns>
        private IQueryable<PermissaoResponse> IncluirOrdenacao(IQueryable<PermissaoResponse> consulta, PermissaoOrder ordem, bool asc)
        {
            switch (ordem)
            {
                case PermissaoOrder.Modulo:
                    if (asc)
                        consulta = consulta.OrderBy(x => x.Modulos != null).ThenBy(a => a.Modulos);
                    else
                        consulta = consulta.OrderByDescending(a => a.Modulos);
                    break;

                case PermissaoOrder.Caminho:
                    if (asc)
                        consulta = consulta.OrderBy(x => x.Caminho != null).ThenBy(a => a.Caminho);
                    else
                        consulta = consulta.OrderByDescending(a => a.Caminho).ThenByDescending(a => a.Caminho);
                    break;

                case PermissaoOrder.Nome:
                default:
                    if (asc)
                        consulta = consulta.OrderBy(x => x.Descricao != null).ThenBy(a => a.Descricao);
                    else
                        consulta = consulta.OrderByDescending(a => a.Descricao);
                    break;
            }
            return consulta;
        }

        /// <summary>
        /// Recupera o módulos por id ou todos
        /// </summary>
        /// <param name="modulosId">Id dos modulos</param>
        /// <param name="ct"></param>
        /// <returns>Array com os módulos da permissão</returns>
        private async Task<List<ModuloSisjur>> ObterModulosAsync(int[]? modulosId, CancellationToken ct)
        {
            List<ModuloSisjur> lstModulos = new List<ModuloSisjur>();

            if (modulosId != null && modulosId.Length > 0)
                lstModulos = await _db.ModuloSisjur
                    .AsNoTracking()
                    .Where(x => modulosId.Any(id => x.CodModulo == id))
                    .ToListAsync(ct);

            return lstModulos;
        }

        /// <summary>
        /// Rotina que gera valores aleatórios para compor nome do arquivo de exportação
        /// </summary>
        /// <returns>String com valores aleatórios</returns>
        private string GeraValorAleatorioParaNomeDeArquivo()
        {
            var random = new Random(DateTime.Now.Millisecond);
            return $"{random.Next(1, 9999):0000}";
        }
    }
}