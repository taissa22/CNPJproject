using Oi.Juridico.Contextos.V2.EmpresaContratadaContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.Controller;
using System.Data.Entity;
using Oi.Juridico.Contextos.V2.EmpresaContratadaContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.CsvHelperMap;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaContratadaController : ControllerBase
    {
        private readonly EmpresaContratadaDbContext _db;
        private readonly ILogger<ContratoEscritorioController> _logger;

        public EmpresaContratadaController(EmpresaContratadaDbContext context, ILogger<ContratoEscritorioController> logger)
        {
            _db = context;
            _logger = logger;
        }

        [HttpGet("obter/lista-empresas")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<ActionResult<ListaEmpresasContratadasResponse>> ObterListaEmpresaContratadaAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                IQueryable<ListaEmpresasContratadasResponse> query = QueryListar(filtro, ordem, asc);

                // retorna a lista da página
                var lista = query
                    .Skip(page * size).Take(size)
                    .ToArray();

                // conta o total de lista da página
                var total = query.Count();

                return Ok(new { lista, total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados das empresas.", erro = ex.Message });
            }
        }


        [HttpGet("obter/empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<ActionResult<EmpresaContratadaResponse>> ObterEmpresaContratadaAsync(CancellationToken ct, int codEmpresa, string nomEmpresa)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var queryTerceiro = _db.EmpresaTerceiroContratada
                .Where(x => x.CodEmpresaContratada == codEmpresa)
                .Select(x => new TerceiroContratadoResponse
                {
                    CodTerceiroContratado = x.CodTerceiroContratado,
                    LoginTerceiro = x.CodTerceiroContratadoNavigation.LoginTerceiro
                }).ToList();

                EmpresaContratadaResponse empresaContratadaResponse = new EmpresaContratadaResponse()
                {
                    CodEmpresaContratada = codEmpresa,
                    NomEmpresaContratada = nomEmpresa,
                    Matriculas = queryTerceiro
                };

                return Ok(empresaContratadaResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da empresa.", erro = ex.Message });
            }
        }


        [HttpPost("salvar-empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<IActionResult> SalvarEmpresaContratadaAsync(CancellationToken ct, EmpresaContratadaRequest model)
        {
            try
            {
                Contextos.V2.EmpresaContratadaContext.Entities.EmpresaContratada empresaContratada = new Contextos.V2.EmpresaContratadaContext.Entities.EmpresaContratada();
                empresaContratada.NomEmpresaContratada = model.NomEmpresaContratada;

                await _db.EmpresaContratada.AddAsync(empresaContratada, ct);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                foreach (var item in model.Matriculas)
                {
                    TerceiroContratado terceiroContratado = new TerceiroContratado();
                    EmpresaTerceiroContratada empresaTerceiroContratada = new EmpresaTerceiroContratada();


                    //Salva os Terceiros Contratados

                    var codMatricula = _db.TerceiroContratado.Where(x => item.LoginTerceiro.Contains(x.LoginTerceiro)).FirstOrDefault();

                    if (codMatricula == null)
                    {
                        terceiroContratado.LoginTerceiro = item.LoginTerceiro;
                        await _db.TerceiroContratado.AddAsync(terceiroContratado, ct);
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                    else
                        terceiroContratado.CodTerceiroContratado = codMatricula.CodTerceiroContratado;

                    // Registra o relacionamento das tabelas
                    empresaTerceiroContratada.CodEmpresaContratada = empresaContratada.CodEmpresaContratada;
                    empresaTerceiroContratada.CodTerceiroContratado = terceiroContratado.CodTerceiroContratado;
                    empresaTerceiroContratada.NomEmpresaContratada = empresaContratada.NomEmpresaContratada;
                    empresaTerceiroContratada.LoginTerceiro = terceiroContratado.LoginTerceiro;
                    await _db.EmpresaTerceiroContratada.AddAsync(empresaTerceiroContratada, ct);
                    await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                }

                return Ok("Empresa Contratada registrada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha na inclusão da empresa contratada.", erro = ex.Message });
            }
        }


        [HttpPut("editar-empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<IActionResult> EditarEmpresaContratadaAsync(CancellationToken ct, EmpresaContratadaRequest model)
        {
            try
            {

                var empresa = _db.EmpresaContratada.FirstOrDefault(x => x.CodEmpresaContratada == model.CodEmpresaContratada);

                if (empresa == null)
                    return BadRequest("Não foi possivel encontrar a empresa contratada.");

                var nomEmpresaA = empresa.NomEmpresaContratada;

                empresa.NomEmpresaContratada = model.NomEmpresaContratada;

                _db.EmpresaContratada.Update(empresa);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                var listaTerceiros = _db.EmpresaTerceiroContratada
                                           .Where(x => x.CodEmpresaContratada == model.CodEmpresaContratada)
                                           .Select(x => new TerceiroContratadoResponse
                                           {
                                               CodTerceiroContratado = x.CodTerceiroContratado,
                                               LoginTerceiro = x.CodTerceiroContratadoNavigation.LoginTerceiro
                                           }).ToList();


                // Encontra os registros em listaTerceiros que não existem em model
                var terceirosParaRemover = listaTerceiros.Where(lt => !model.Matriculas.Any(md => md.CodTerceiroContratado == lt.CodTerceiroContratado && md.LoginTerceiro == lt.LoginTerceiro)).ToList();

                foreach (var remover in terceirosParaRemover)
                {
                    var terceiroRemover = _db.EmpresaTerceiroContratada.First(x => x.CodEmpresaContratada == model.CodEmpresaContratada && x.CodTerceiroContratado == remover.CodTerceiroContratado);
                    _db.EmpresaTerceiroContratada.Remove(terceiroRemover);
                    await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                    if (!_db.EmpresaTerceiroContratada.Any(x => x.CodTerceiroContratado == remover.CodTerceiroContratado))
                    {
                        _db.TerceiroContratado.Remove(_db.TerceiroContratado.First(x => x.CodTerceiroContratado == remover.CodTerceiroContratado));
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                }

                // Encontra os registros em model que não existem em listaTerceiros
                var terceirosParaIncluir = model.Matriculas.Where(md => !listaTerceiros.Any(lt => lt.CodTerceiroContratado == md.CodTerceiroContratado && lt.LoginTerceiro == md.LoginTerceiro)).ToList();

                foreach (var incluir in terceirosParaIncluir)
                {
                    TerceiroContratado terceiroContratado = new TerceiroContratado();
                    EmpresaTerceiroContratada empresaTerceiroContratada = new EmpresaTerceiroContratada();


                    //Salva os Terceiros Contratados
                    var codMatricula = _db.TerceiroContratado.Where(x => incluir.LoginTerceiro.Contains(x.LoginTerceiro)).FirstOrDefault();

                    if (codMatricula == null)
                    {
                        terceiroContratado.LoginTerceiro = incluir.LoginTerceiro;
                        await _db.TerceiroContratado.AddAsync(terceiroContratado, ct);
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                    else
                        terceiroContratado.CodTerceiroContratado = codMatricula.CodTerceiroContratado;

                    // Registra o relacionamento das tabelas
                    empresaTerceiroContratada.CodEmpresaContratada = model.CodEmpresaContratada;
                    empresaTerceiroContratada.CodTerceiroContratado = terceiroContratado.CodTerceiroContratado;
                    empresaTerceiroContratada.NomEmpresaContratada = model.NomEmpresaContratada;
                    empresaTerceiroContratada.LoginTerceiro = terceiroContratado.LoginTerceiro;
                    await _db.EmpresaTerceiroContratada.AddAsync(empresaTerceiroContratada, ct);
                    await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                }


                if (nomEmpresaA != model.NomEmpresaContratada && terceirosParaRemover.Count == 0 && terceirosParaIncluir.Count == 0)
                {
                    foreach (var item in listaTerceiros)
                    {
                        //EmpresaTerceiroContratada empresaTerceiroContratada = new EmpresaTerceiroContratada();
                        var empresaTerceiroContratada = _db.EmpresaTerceiroContratada.First(x => x.CodEmpresaContratada == model.CodEmpresaContratada && x.CodTerceiroContratado == item.CodTerceiroContratado);

                        empresaTerceiroContratada.CodEmpresaContratada = model.CodEmpresaContratada;
                        empresaTerceiroContratada.NomEmpresaContratada = model.NomEmpresaContratada;
                        empresaTerceiroContratada.CodTerceiroContratado = item.CodTerceiroContratado;
                        empresaTerceiroContratada.LoginTerceiro = item.LoginTerceiro;
                        _db.EmpresaTerceiroContratada.Update(empresaTerceiroContratada);
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                }


                return Ok("Empresa Contratada registrada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha na alteração da empresa contratada.", erro = ex.Message });
            }
        }


        [HttpDelete("excluir-empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<IActionResult> ExcluirEmpresaContratadaAsync(CancellationToken ct, int codEmpresa)
        {
            try
            {
                var empresa = _db.EmpresaContratada.FirstOrDefault(x => x.CodEmpresaContratada == codEmpresa);

                if (empresa == null)
                    return BadRequest("Não foi possivel encontrar a empresa contratada.");


                var listaTerceiros = _db.EmpresaTerceiroContratada.Where(x => x.CodEmpresaContratada == codEmpresa).ToList();

                _db.EmpresaTerceiroContratada.RemoveRange(listaTerceiros);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                _db.EmpresaContratada.Remove(empresa);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                // Verifica se as matriculas da empresa está relacionado com mais alguma outra empresa

                foreach (var terceiroEmpresa in listaTerceiros)
                {
                    if (!_db.EmpresaTerceiroContratada.Any(x => x.CodTerceiroContratado == terceiroEmpresa.CodTerceiroContratado))
                    {
                        _db.TerceiroContratado.Remove(_db.TerceiroContratado.First(x => x.CodTerceiroContratado == terceiroEmpresa.CodTerceiroContratado));
                        await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);
                    }
                }

                return Ok("Empresa Contratada excluída com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao excluir empresa contratada.", erro = ex.Message });
            }
        }


        #region Download Methods

        [HttpGet("download-lista-empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<FileContentResult> DownloadListaEmpresaContratadaAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            var queryResult = QueryListarDownload(filtro, ordem, asc).ToArray();

            var nomeArquivoListaEmpresaContratada = $"Lista_Empresas_Contratadas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadListaEmpresasContratadasResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoListaEmpresaContratada);
        }


        [HttpGet("download-log-empresa-contratada")]
        [TemPermissao(Permissoes.ACESSAR_EMPRESA_CONTRATADA)]
        public async Task<FileContentResult> DownloadLogEmpresaContratadaAsync()
        {
            var queryResult = QueryLogDownload().ToArray();

            var nomeArquivoLogEmpresaContratada = $"Log_Empresas_Contratadas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadLogEmpresasContratadasResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoLogEmpresaContratada);
        }

        #endregion


        #region Private Methods

        private IQueryable<ListaEmpresasContratadasResponse> QueryListar(string? filtro, string? ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            // monta a query
            var query = _db.EmpresaContratada
                .WhereIfNotEmpty(x => x.NomEmpresaContratada.Contains(filtro!), filtro)
                .Select(x => new ListaEmpresasContratadasResponse
                {
                    CodEmpresaContratada = x.CodEmpresaContratada,
                    NomEmpresaContratada = x.NomEmpresaContratada
                });

            // monta a ordenação
            switch (ordem)
            {
                case "razao":
                    if (asc)
                        query = query.OrderBy(x => x.NomEmpresaContratada);
                    else
                        query = query.OrderByDescending(a => a.NomEmpresaContratada);
                    break;
                default:
                    query = query.OrderBy(x => x.NomEmpresaContratada);
                    break;
            }

            return query;
        }

        private IQueryable<DownloadListaEmpresasContratadasResponse> QueryListarDownload(string? filtro, string? ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            var queryBase = from ec in _db.EmpresaContratada
                            join etc in _db.EmpresaTerceiroContratada
                            on ec.CodEmpresaContratada equals etc.CodEmpresaContratada
                            where (filtro == null || ec.NomEmpresaContratada.Contains(filtro))
                            select new
                            {
                                empresaContratada = ec,
                                empresaTerceiroContratada = etc
                            };

            var queryGroup = queryBase.GroupBy(x => x.empresaContratada.CodEmpresaContratada)
                            .Select(s => new
                            {
                                CodEmpresaContratada = s.Key,
                                NomEmpresaContratada = s.Select(x => x.empresaContratada.NomEmpresaContratada).First(),
                                Matriculas = s.Select(x => x.empresaTerceiroContratada.CodTerceiroContratadoNavigation.LoginTerceiro).OrderBy(o => o).ToList()
                            });



            var query = queryGroup.Select(x => new DownloadListaEmpresasContratadasResponse
            {
                CodEmpresaContratada = x.CodEmpresaContratada,
                NomEmpresaContratada = x.NomEmpresaContratada,
                Matriculas = string.Join(" | ", x.Matriculas)
            });

            // monta a ordenação
            switch (ordem)
            {
                case "razao":
                    if (asc)
                        query = query.OrderBy(x => x.NomEmpresaContratada);
                    else
                        query = query.OrderByDescending(a => a.NomEmpresaContratada);
                    break;
                default:
                    query = query.OrderBy(x => x.NomEmpresaContratada);
                    break;
            }

            return query;
        }

        private IQueryable<DownloadLogEmpresasContratadasResponse> QueryLogDownload()
        {
            return _db.LogEmpTerceiroContratada.Select(x => new DownloadLogEmpresasContratadasResponse
            {
                CodEmpTerceiroContratada = x.CodEmpTerceiroContratada,
                Operacao = x.Operacao,
                DatLog = x.DatLog,
                CodUsuario = x.CodUsuario,
                CodEmpresaContratadaA = x.CodEmpresaContratadaA,
                CodEmpresaContratadaD = x.CodEmpresaContratadaD,
                CodTerceiroContratadoA = x.CodTerceiroContratadoA,
                CodTerceiroContratadoD = x.CodTerceiroContratadoD,
                NomEmpresaContratadaA = x.NomEmpresaContratadaA,
                NomEmpresaContratadaD = x.NomEmpresaContratadaD,
                LoginTerceiroA = x.LoginTerceiroA,
                LoginTerceiroD = x.LoginTerceiroD
            }).OrderByDescending(x => x.DatLog);
        }
        #endregion
    }
}