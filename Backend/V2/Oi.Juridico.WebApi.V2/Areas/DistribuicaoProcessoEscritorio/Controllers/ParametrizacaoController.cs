using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Immutable;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.Controllers
{
    [Route("parametrizacao-distribuicao-escritorio")]
    [ApiController]
    public class ParametrizacaoDistribuicaoEscritorioController : ControllerBase
    {
        private readonly DistribuicaoProcessoEscritorioContext _db;
        private readonly ParametroJuridicoContext _pjdb;
        private readonly ILogger<ParametrizacaoDistribuicaoEscritorioController> _logger;

        const string TAM_MAX_ANEXO_DIST_ESCRIT = "TAM_MAX_ANEXO_DIST_ESCRIT";
        const string DIR_NAS_ANEXO_DIST_ESCRIT = "DIR_NAS_ANEXO_DIST_ESCRIT";

        public ParametrizacaoDistribuicaoEscritorioController(DistribuicaoProcessoEscritorioContext db, ILogger<ParametrizacaoDistribuicaoEscritorioController> logger, ParametroJuridicoContext pjdb)
        {
            _db = db;
            _logger = logger;
            _pjdb = pjdb;
        }

        #region COMBOS

        [HttpGet("obter-empresas-centralizadoras")]
        public async Task<List<ObterEmpresasContralizadorasResponse>> ObterEmpresasContralizadorasAsync(CancellationToken ct)
        {
            var lista = await _db.EmpresasCentralizadoras
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .Select(x => new ObterEmpresasContralizadorasResponse
                {
                    Codigo = x.Codigo,
                    Nome = x.Nome
                })
                .ToListAsync(ct);

            lista.Insert(0, new ObterEmpresasContralizadorasResponse { Codigo = -1, Nome = "TODAS" });

            return lista;
        }

        [HttpGet("obter-comarcas")]
        public async Task<List<ObterComarcasResponse>> ObterComarcasAsync(CancellationToken ct, string? uf)
        {
            var lista = await _db.Comarca
                .AsNoTracking()
                .WhereIfNotNull(x => x.CodEstado == uf, uf)
                .OrderBy(x => x.NomComarca)
                .Select(x => new ObterComarcasResponse
                {
                    Codigo = x.CodComarca,
                    Nome = x.NomComarca
                })
                .ToListAsync(ct);

            lista.Insert(0, new ObterComarcasResponse { Codigo = -1, Nome = "TODAS" });

            return lista;
        }

        [HttpGet("obter-varas")]
        public async Task<List<ObterVarasResponse>> ObterVarasAsync(CancellationToken ct, short codComarca, int? codTipoProcesso)
        {
            List<ObterVarasResponse> lista = new();

            if (codTipoProcesso.HasValue)
            {
                lista = await _db.Vara
                    .AsNoTracking()
                    .Where(x => x.CodComarca == codComarca &&
                        (
                            (codTipoProcesso == 1 && x.CodTipoVaraNavigation.IndCivel == "S") ||
                            (codTipoProcesso == 7 && x.CodTipoVaraNavigation.IndJuizado == "S") ||
                            (codTipoProcesso == 17 && x.CodTipoVaraNavigation.IndProcon == "S")
                        ))
                    .OrderBy(x => x.CodTipoVaraNavigation.NomTipoVara)
                    .Select(x => new ObterVarasResponse
                    {
                        Codigos = x.CodVara + "|" + x.CodTipoVara,
                        Nome = x.CodVara + "ª " + x.CodTipoVaraNavigation.NomTipoVara
                    })
                    .ToListAsync(ct);
            }

            lista.Insert(0, new ObterVarasResponse { Codigos = "-1|-1", Nome = "TODAS" });

            return lista;
        }

        [HttpGet("obter-solicitantes")]
        public async Task<List<ObterSolicitantesResponse>> ObterSolicitantesAsync(CancellationToken ct)
        {
            var lista = await _db.Solicitante
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .Select(x => new ObterSolicitantesResponse
                {
                    Codigo = x.CodSolicitante,
                    Nome = x.Nome
                })
                .ToListAsync(ct);

            return lista;
        }

        [HttpGet("obter-escritorios")]
        public async Task<List<ObterEscritoriosResponse>> ObterEscritoriosAsync(CancellationToken ct, [FromQuery] int[] codTipoProcesso)
        {
            // se nenhum codTipoProcesso for informado, retorna uma lista sem escritório
            if (codTipoProcesso.Length == 0)
                return new List<ObterEscritoriosResponse>();

            var query = _db.Profissional
                .AsNoTracking()
                .Where(x => x.IndEscritorio == "S" && x.IndAtivo == "S");

            // se ativo no cível consumidor
            if (codTipoProcesso.Contains(1))
            {
                query = query.Where(x => x.IndAreaCivel == "S");
            }

            // se ativo no juizado especial
            if (codTipoProcesso.Contains(7))
            {
                query = query.Where(x => x.IndAreaJuizado == "S");
            }

            // se ativo no procon
            if (codTipoProcesso.Contains(17))
            {
                query = query.Where(x => x.IndProcon == "S");
            }

            var lista = await query
                .OrderBy(x => x.NomProfissional)
                .Select(x => new ObterEscritoriosResponse
                {
                    Codigo = x.CodProfissional,
                    Nome = x.NomProfissional
                })
                .ToListAsync(ct);

            return lista;
        }

        [HttpGet("obter-uf")]
        public async Task<List<ObterEstadosResponse>> ObterUfAsync(CancellationToken ct)
        {
            var lista = await _db.Estado
                .AsNoTracking()
                .OrderBy(x => x.CodEstado)
                .Select(x => new ObterEstadosResponse
                {
                    Codigo = x.CodEstado,
                    Nome = x.CodEstado
                })
                .ToListAsync(ct);

            return lista;
        }

        [HttpGet("obter-natureza")]
        public async Task<List<ObterNaturezasResponse>> ObterNaturezasAsync(CancellationToken ct)
        {
            var lista = await _db.TipoProcesso
                .AsNoTracking()
                .Where(x => x.IndDistribuicaoEscritorio == "S")
                .OrderBy(x => x.DscTipoProcesso)
                .Select(x => new ObterNaturezasResponse
                {
                    Codigo = x.CodTipoProcesso,
                    Nome = x.DscTipoProcesso
                })
                .ToListAsync(ct);

            return lista;
        }

        #endregion

        #region DISTRIBUIÇÃO E ESCRITORIO

        [HttpGet("obter-parametrizacao")]
        public async Task<IActionResult> ObterParametrizacaoAsync(CancellationToken ct, [FromQuery] ObterParametrizacaoRequest parametrizacao, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            var query = QueryParametrizacao(parametrizacao, ordem, asc);

            var lista = await query
                .Skip(page * size).Take(size)
                .ToArrayAsync(ct);

            var total = await query.CountAsync(ct);

            var totalLista = lista.Length;

            return Ok(new { lista, total, totalLista });
        }

        [HttpGet("obter-parametrizacao-escritorio")]
        public async Task<IActionResult> ObterParametrizacaoEscritorioAsync(CancellationToken ct, int CodParametrizacao, string? NomeEscritorio, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var query = QueryParametrizacaoEscritorio(new[] { CodParametrizacao }, NomeEscritorio, ordem, asc);

            var lista = await query
                .Skip(page * size).Take(size)
                .ToArrayAsync(ct);

            var total = await query.CountAsync(ct);

            var totalLista = lista.Length;

            return Ok(new { lista, total, totalLista });
        }

        [HttpPost("validar-parametrizacao")]
        public async Task<ActionResult<bool>> ValidaSeParametroDistribuicaoJaExisteNoBanco(CancellationToken ct, [FromBody] SalvarParametrizacaoRequest parametrizacao)
        {
            ParamDistribuicao paramDistribuicao = new();
            paramDistribuicao.CodEstado = parametrizacao.CodEstado;
            paramDistribuicao.CodComarca = parametrizacao.CodComarca == -1 ? null : parametrizacao.CodComarca;
            paramDistribuicao.CodVara = parametrizacao.CodVara == -1 ? null : parametrizacao.CodVara;
            paramDistribuicao.CodTipoVara = parametrizacao.CodTipoVara == -1 ? null : parametrizacao.CodTipoVara;
            paramDistribuicao.CodTipoProcesso = parametrizacao.CodTipoProcesso;
            paramDistribuicao.CodEmpresaCentralizadora = parametrizacao.CodEmpresaCentralizadora == -1 ? null : parametrizacao.CodEmpresaCentralizadora;
            paramDistribuicao.IndAtivo = parametrizacao.IndAtivo ? "S" : "N";

            // Verifica se já existe um Parametro igual no banco.
            var existe = await VerificaExisteParametroDistribuicaoAsync(paramDistribuicao, ct);

            return existe;
        }

        [HttpPost("validar-escritorio")]
        public async Task<bool> ValidaSeParametrizacaoDistribuicaoEscritorioJaExisteNoBanco(CancellationToken ct, [FromBody] SalvarEscritorioRequest model)
        {
            // Verifica se já existe um escritório igual para o parâmetro no banco.
            var existe = await _db.ParamDistribEscritorio.AsNoTracking()
                .AnyAsync(x => x.CodParamDistribuicao == model.CodParamDistribuicao &&
                               x.CodProfissional == model.CodProfissional &&
                               x.CodParamDistribEscrit != model.CodParamDistribEscrit, ct);

            return existe;
        }

        [HttpPost("salvar-parametrizacao-em-lote")]
        public async Task<IActionResult> SalvarParametrizacaoEmLoteAsync(CancellationToken ct, [FromBody] SalvarParametrizacaoEmLoteRequest model)
        {
            try
            {
                // valida se a soma do percentual dos processos dos escritórios dá 100%
                if (model.Escritorios.Sum(x => x.PorcentagemProcessos) != 100)
                    return BadRequest("A soma do % de processos dos escritórios deve totalizar 100%.");

                foreach (var parametrizacao in model.Parametrizacoes)
                {
                    ParamDistribuicao? paramDistribuicao = null;
                    // inclusão
                    if (parametrizacao.CodParamDistribuicao == 0)
                    {
                        paramDistribuicao = new();
                        _db.ParamDistribuicao.Add(paramDistribuicao);
                    }
                    // alteração
                    else
                    {
                        paramDistribuicao = await _db.ParamDistribuicao
                            .Include(x => x.ParamDistribEscritorio)
                            .ThenInclude(x => x.DistribEscritContador)
                            .FirstOrDefaultAsync(x => x.CodParamDistribuicao == parametrizacao.CodParamDistribuicao, ct);

                        if (paramDistribuicao == null)
                            return NotFound("Parâmetro de Distribuição não encontrado.");
                    }

                    paramDistribuicao.CodEstado = parametrizacao.CodEstado;
                    paramDistribuicao.CodComarca = parametrizacao.CodComarca == -1 ? null : parametrizacao.CodComarca;
                    paramDistribuicao.CodVara = parametrizacao.CodVara == -1 ? null : parametrizacao.CodVara;
                    paramDistribuicao.CodTipoVara = parametrizacao.CodTipoVara == -1 ? null : parametrizacao.CodTipoVara;
                    paramDistribuicao.CodTipoProcesso = parametrizacao.CodTipoProcesso;
                    paramDistribuicao.CodEmpresaCentralizadora = parametrizacao.CodEmpresaCentralizadora == -1 ? null : parametrizacao.CodEmpresaCentralizadora;
                    paramDistribuicao.IndAtivo = parametrizacao.IndAtivo ? "S" : "N";

                    // Verifica se já existe um Parametro igual no banco.
                    var existe = await VerificaExisteParametroDistribuicaoAsync(paramDistribuicao, ct);
                    if (existe)
                        return BadRequest("Parametro Distribuição já existe!");

                    // verifica se o escritório está cadastrado mais de uma vez
                    if (model.Escritorios.GroupBy(x => new { x.CodParamDistribuicao, x.CodProfissional }).Any(x => x.Count() > 1))
                        return BadRequest("Existe escritório cadastrado mais de uma vez.");

                    // exclui os escritórios removidos da tela
                    var removerCodigos = paramDistribuicao.ParamDistribEscritorio.Select(x => x.CodParamDistribEscrit).Except(model.Escritorios.Select(x => x.CodParamDistribEscrit)).ToArray();
                    foreach (var r in removerCodigos)
                    {
                        var escritorio = paramDistribuicao.ParamDistribEscritorio.First(x => x.CodParamDistribEscrit == r);
                        _db.DistribEscritContador.Remove(escritorio.DistribEscritContador);
                        _db.ParamDistribEscritorio.Remove(paramDistribuicao.ParamDistribEscritorio.First(x => x.CodParamDistribEscrit == r));
                    }

                    // carrega a lista com os escritórios
                    foreach (var escritorio in model.Escritorios)
                    {
                        // verifica se o profissional está ativo para o tipo de processo
                        var profissional = _db.Profissional.First(x => x.CodProfissional == escritorio.CodProfissional);

                        if (profissional.IndAtivo == "N")
                            return BadRequest("O escritório não está ativo.");

                        if ((paramDistribuicao.CodTipoProcesso == 1 && profissional.IndAreaCivel == "N") ||
                            (paramDistribuicao.CodTipoProcesso == 7 && profissional.IndAreaJuizado == "N") ||
                            (paramDistribuicao.CodTipoProcesso == 17 && profissional.IndProcon == "N"))
                            return BadRequest("O escritório não esta ativo para a natureza informada.");

                        ParamDistribEscritorio? paramDistribEscr = null;
                        // inclusão
                        if (escritorio.CodParamDistribEscrit == 0)
                        {
                            paramDistribEscr = new();
                            paramDistribuicao.ParamDistribEscritorio.Add(paramDistribEscr);
                        }
                        // alteração
                        else
                        {
                            // verifica se pode alterar
                            existe = await ValidaSeParametrizacaoDistribuicaoEscritorioJaExisteNoBanco(ct, escritorio);

                            if (existe)
                                return BadRequest("O escritório já está cadastrado para o parâmetro de distribuição.");

                            paramDistribEscr = paramDistribuicao.ParamDistribEscritorio.FirstOrDefault(x => x.CodParamDistribEscrit == escritorio.CodParamDistribEscrit);

                            if (paramDistribEscr == null)
                                return NotFound("Escritório não encontrado.");
                        }

                        paramDistribEscr.CodParamDistribuicao = escritorio.CodParamDistribuicao;
                        paramDistribEscr.CodProfissional = escritorio.CodProfissional;
                        paramDistribEscr.CodSolicitante = escritorio.CodSolicitante;
                        paramDistribEscr.PorcentagemProcessos = escritorio.PorcentagemProcessos;
                        paramDistribEscr.DatVigenciaInicial = escritorio.DatVigenciaInicial;
                        paramDistribEscr.DatVigenciaFinal = escritorio.DatVigenciaFinal;
                        paramDistribEscr.Prioridade = escritorio.Prioridade;
                    }

                    // salva o anexo
                    foreach (var anexo in model.Anexos.Where(x => x != 0))
                    {
                        paramDistribuicao.ParamDistribuicaoAnexos.Add(new ParamDistribuicaoAnexos() { IdAnexoDistEscritorio = anexo });
                    }

                }

                // se houve alguma alteração, é necessário reiniciar a tabela contadora
                if (_db.ChangeTracker.HasChanges())
                {
                    var entries = _db.ChangeTracker.Entries<ParamDistribEscritorio>().GroupBy(x => x.Entity.CodParamDistribuicao);

                    foreach (var item in entries)
                    {
                        // as duas verificações de alteração desconsidera as alteração do campo 'IndAtivo'
                        bool houveAlteracaoNaChave = _db.ChangeTracker.Entries<ParamDistribuicao>().Any(x => x.State == EntityState.Modified && x.Entity.CodParamDistribuicao == item.First().Entity.CodParamDistribuicao
                                                                                                             && x.Properties.Any(p => p.Metadata.Name != "IndAtivo" && p.IsModified));
                        bool houveAlteracao = item.Any(x => x.State == EntityState.Added) || item.Any(x => x.Properties.Any(p => new string[] { "PorcentagemProcessos", "CodParamDistribuicao", "CodProfissional" }.Contains(p.Metadata.Name) && p.IsModified)) || item.Any(x => x.State == EntityState.Deleted
                                                                                                           && x.Properties.Any(p => p.Metadata.Name != "IndAtivo" && p.IsModified));

                        // para os escritórios incluídos, deve incluir na tabela contadora
                        foreach (var entry in item.Where(x => x.State == EntityState.Added))
                            entry.Entity.DistribEscritContador = new DistribEscritContador { Contador = 0 };

                        // para os escritórios alterados a porcentagem, deve zerar o contador
                        foreach (var entry in item.Where(x => x.Properties.Any(p => new string[] { "PorcentagemProcessos", "CodParamDistribuicao", "CodProfissional" }.Contains(p.Metadata.Name) && p.IsModified)))
                            entry.Entity.DistribEscritContador.Contador = 0;

                        // para os escritórios excluídos, deve excluir da tabela contadora
                        foreach (var entry in item.Where(x => x.State == EntityState.Deleted))
                            _db.DistribEscritContador.Remove(entry.Entity.DistribEscritContador);

                        if (houveAlteracaoNaChave || houveAlteracao)
                        {
                            // se houve alguma alteração, deve zerar o contador dos registros não modificados
                            foreach (var entry in item.Where(x => x.State == EntityState.Unchanged))
                                entry.Entity.DistribEscritContador.Contador = 0;
                        }
                    }
                }

                await _db.SaveChangesAsync(User.Identity!.Name, true, ct);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem("Ocorreu um erro ao salvar");
            }
        }

        [HttpDelete("excluir-parametrizacao")]
        public async Task<IActionResult> ExcluirParametrizacaoAsync(CancellationToken ct, int codParamDistribuicao)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();
                _db.ExecutarProcedureDeLog(User.Identity!.Name, true);

                var qtdeExcluidos = await _db.DistribEscritContador
                    .Where(x => x.CodParamDistribEscritNavigation.CodParamDistribuicao == codParamDistribuicao)
                    .ExecuteDeleteAsync(ct);

                qtdeExcluidos = await _db.ParamDistribEscritorio
                    .Where(x => x.CodParamDistribuicao == codParamDistribuicao)
                    .ExecuteDeleteAsync(ct);

                qtdeExcluidos = await _db.ParamDistribuicaoAnexos
                    .Where(x => x.CodParamDistribuicao == codParamDistribuicao)
                    .ExecuteDeleteAsync(ct);

                qtdeExcluidos = await _db.ParamDistribuicao
                    .Where(x => x.CodParamDistribuicao == codParamDistribuicao)
                    .ExecuteDeleteAsync(ct);

                scope.Commit();

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Não foi possível excluir a parametrização de distribuição, verifique os dados e tente novamente!");
            }
        }

        [HttpGet("download-arquivos-log")]
        public async Task<FileContentResult> DownloadArquivosLog(CancellationToken ct)
        {
            // monta o zip para download
            using var memoryStream = new MemoryStream();
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var zipFile = new ZipArchive(memoryStream, ZipArchiveMode.Create, false);

            var arqParametrizacao = await _db.VLogParamDistribuicao
            .AsNoTracking()
            .ToListAsync(ct);

            var nomeArquivoParametrizacao = $"Log_Parametrizacoes_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            // gera o csv para incluir no zip
            var csv = arqParametrizacao.ToCsv(typeof(VLogParamDistribuicaoResponseMap), false);

            // adiciona ao zip
            var zipEntry = zipFile.CreateEntry(nomeArquivoParametrizacao);
            using (var zipEntryStream = new StreamWriter(zipEntry.Open(), Encoding.UTF8))
            {
                await zipEntryStream.WriteAsync(csv);
            }

            // retira a lista da memória
            arqParametrizacao.Clear();

            var arqDistribuicaoEscritorio = await _db.VLogParamDistribEscritorio
                .AsNoTracking()
                .ToListAsync(ct);

            var nomeArquivoParametrizacaoEscritorio = $"Log_Escritorios_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            // gera o csv para incluir no zip
            csv = arqDistribuicaoEscritorio.ToCsv(typeof(VLogParamDistribEscritorioResponseMap), false);

            // adiciona ao zip
            zipEntry = zipFile.CreateEntry(nomeArquivoParametrizacaoEscritorio);
            using (var zipEntryStream = new StreamWriter(zipEntry.Open(), Encoding.UTF8))
            {
                await zipEntryStream.WriteAsync(csv);
            }

            // retira a lista da memória
            arqDistribuicaoEscritorio.Clear();

            zipFile.Dispose();

            return File(memoryStream.ToArray(), "application/zip", $"Log_DistribuicaoEscritorios_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
        }

        [HttpGet("download-lista-parametrizacao")]
        public async Task<FileContentResult> ListaParametrizacaoAsync(CancellationToken ct, [FromQuery] ObterParametrizacaoRequest parametrizacao, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            var listaParametrizacao = await QueryParametrizacao(parametrizacao, ordem, asc).ToArrayAsync(ct);
            var listaEscritorios = await QueryParametrizacaoEscritorio(listaParametrizacao.Select(x => x.Codigo).ToArray()).ToArrayAsync(ct);

            var listaFinal = from p in listaParametrizacao
                             join pe in listaEscritorios on p.Codigo equals pe.CodParamDistribuicao
                             select new ListarParametrizacaoEscritorioResponse
                             {
                                 Codigo = p.Codigo,
                                 CodEstado = p.CodEstado,
                                 CodComarca = p.CodComarca == -1 ? null : p.CodComarca,
                                 NomComarca = p.NomComarca,
                                 CodVara = p.CodVara == -1 ? null : p.CodVara,
                                 CodTipoVara = p.CodTipoVar == -1 ? null : p.CodTipoVar,
                                 NomTipoVara = p.NomTipoVara,
                                 CodTipoProcesso = p.CodTipoProcesso,
                                 DscTipoProcesso = p.DscTipoProcesso,
                                 CodEmpresaCentralizadora = p.CodEmpresaCentralizadora == -1 ? null : p.CodEmpresaCentralizadora,
                                 NomEmpresaCentralizadora = p.NomEmpresaCentralizadora,
                                 Status = p.Status,
                                 CodProfissional = pe.CodProfissional,
                                 NomProfissional = pe.NomProfissional,
                                 CodSolicitante = pe.CodSolicitante,
                                 NomSolicitante = pe.NomSolicitante,
                                 DatVigenciaInicial = pe.DatVigenciaInicial,
                                 DatVigenciaFinal = pe.DatVigenciaFinal,
                                 PorcentagemProcessos = decimal.Parse(pe.PorcentagemProcessos),
                                 Prioridade = pe.Prioridade
                             };

            var nomeArquivoParametrizacao = $"Lista_DistribuicaoEscritorios_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = listaFinal.ToCsvByteArray(typeof(ListaParametrizacaoEscritorioResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoParametrizacao);
        }

        #endregion

        #region METODOS PRIVADOS

        private IQueryable<ObterParametrizacaoResponse> QueryParametrizacao(ObterParametrizacaoRequest parametrizacao, string? ordem, bool asc)
        {
            var query = _db.ParamDistribuicao
                .AsNoTracking()
                .Where(x => (parametrizacao.CodEstado == "-1" || x.CodEstado == parametrizacao.CodEstado) &&
                            (parametrizacao.CodComarca == -1 || x.CodComarca == parametrizacao.CodComarca) &&
                            (parametrizacao.CodVara == -1 || x.CodVara == parametrizacao.CodVara) &&
                            (parametrizacao.CodTipoVara == -1 || x.CodTipoVara == parametrizacao.CodTipoVara) &&
                            (parametrizacao.CodTipoProcesso == -1 || x.CodTipoProcesso == parametrizacao.CodTipoProcesso) &&
                            (parametrizacao.CodEmpresaCentralizadora == -1 || x.CodEmpresaCentralizadora == parametrizacao.CodEmpresaCentralizadora) &&
                            (parametrizacao.CodProfissional == -1 || x.ParamDistribEscritorio.Any(p => p.CodProfissional == parametrizacao.CodProfissional)))
                .Select(x => new ObterParametrizacaoResponse
                {
                    Codigo = x.CodParamDistribuicao,
                    CodEstado = x.CodEstado,
                    CodComarca = x.CodComarca == null ? -1 : x.CodComarca,
                    NomComarca = x.CodComarca == null ? "TODAS" : x.CodComarcaNavigation.NomComarca,
                    CodVara = x.CodVara == null ? -1 : x.CodVara,
                    CodTipoVar = x.CodTipoVara == null ? -1 : x.CodTipoVara,
                    Codigos = (x.CodVara == null ? -1 : x.CodVara) + "|" + (x.CodTipoVara == null ? -1 : x.CodTipoVara),
                    NomTipoVara = x.CodVara == null ? "TODAS" : x.CodVara + "ª " + x.Vara.CodTipoVaraNavigation.NomTipoVara,
                    CodTipoProcesso = x.CodTipoProcesso,
                    DscTipoProcesso = x.CodTipoProcessoNavigation.DscTipoProcesso,
                    CodEmpresaCentralizadora = x.CodEmpresaCentralizadora == null ? -1 : x.CodEmpresaCentralizadora,
                    NomEmpresaCentralizadora = x.CodEmpresaCentralizadora == null ? "TODAS" : x.CodEmpresaCentralizadoraNavigation.Nome,
                    Status = x.IndAtivo == "S" ? "Ativo" : "Inativo"
                });

            #region ordenar
            switch (ordem)
            {
                case "Codigo":
                    if (asc)
                        query = query.OrderBy(x => x.Codigo);
                    else
                        query = query.OrderByDescending(a => a.Codigo);
                    break;

                case "UF":
                    if (asc)
                        query = query.OrderBy(x => x.CodEstado);
                    else
                        query = query.OrderByDescending(a => a.CodEstado);
                    break;

                case "Comarca":
                    if (asc)
                        query = query.OrderBy(x => x.NomComarca);
                    else
                        query = query.OrderByDescending(a => a.NomComarca);
                    break;
                case "Vara":
                    if (asc)
                        query = query.OrderBy(x => x.NomTipoVara);
                    else
                        query = query.OrderByDescending(a => a.NomTipoVara);
                    break;
                case "Natureza":
                    if (asc)
                        query = query.OrderBy(x => x.DscTipoProcesso);
                    else
                        query = query.OrderByDescending(a => a.DscTipoProcesso);
                    break;
                case "Empresa":
                    if (asc)
                        query = query.OrderBy(x => x.NomEmpresaCentralizadora);
                    else
                        query = query.OrderByDescending(a => a.NomEmpresaCentralizadora);
                    break;
                case "Status":
                    if (asc)
                        query = query.OrderBy(x => x.Status);
                    else
                        query = query.OrderByDescending(a => a.Status);
                    break;
                default:
                    query = query.OrderBy(x => x.CodEstado).ThenBy(c => c.NomComarca);
                    break;
            }
            #endregion

            return query;
        }

        private IQueryable<ObterEscritorioResponse> QueryParametrizacaoEscritorio(int[] CodigosParametrizacao, string? NomeEscritorio = null, string? ordem = null, bool asc = true)
        {
            var query = _db.ParamDistribEscritorio
                .Where(x => CodigosParametrizacao.Contains(x.CodParamDistribuicao!.Value) &&
                 (string.IsNullOrEmpty(NomeEscritorio) || x.CodProfissionalNavigation.NomProfissional.ToUpper().Contains(NomeEscritorio.ToUpper())))
                .Select(x => new ObterEscritorioResponse
                {
                    CodParamDistribEscrit = x.CodParamDistribEscrit,
                    CodParamDistribuicao = x.CodParamDistribuicao!.Value,
                    CodProfissional = x.CodProfissional!.Value,
                    NomProfissional = x.CodProfissionalNavigation.NomProfissional,
                    CodSolicitante = x.CodSolicitante,
                    NomSolicitante = x.CodSolicitanteNavigation.Nome,
                    DatVigenciaInicial = x.DatVigenciaInicial,
                    DatVigenciaFinal = x.DatVigenciaFinal,
                    PorcentagemProcessos = x.PorcentagemProcessos.ToString(),
                    PorcentagemProcessosNumerico = x.PorcentagemProcessos,
                    Prioridade = x.Prioridade
                });

            #region ordenar
            switch (ordem)
            {
                case "Nome":
                    if (asc)
                        query = query.OrderBy(x => x.NomProfissional);
                    else
                        query = query.OrderByDescending(a => a.NomProfissional);
                    break;

                case "Solicitante":
                    if (asc)
                        query = query.OrderBy(x => x.NomSolicitante);
                    else
                        query = query.OrderByDescending(a => a.NomSolicitante);
                    break;

                case "Vigencia":
                    if (asc)
                        query = query.OrderBy(x => x.DatVigenciaInicial);
                    else
                        query = query.OrderByDescending(a => a.DatVigenciaInicial);
                    break;
                case "Processos":
                    if (asc)
                        query = query.OrderBy(x => x.PorcentagemProcessosNumerico);
                    else
                        query = query.OrderByDescending(a => a.PorcentagemProcessosNumerico);
                    break;
                case "Prioridade":
                    if (asc)
                        query = query.OrderBy(x => x.Prioridade);
                    else
                        query = query.OrderByDescending(a => a.Prioridade);
                    break;
                default:
                    query = query.OrderBy(x => x.NomProfissional);
                    break;
            }
            #endregion

            return query;
        }

        private async Task<bool> VerificaExisteParametroDistribuicaoAsync(ParamDistribuicao paramDistribuicao, CancellationToken ct)
        {
            var existe = await _db.ParamDistribuicao
                        .AsNoTracking()
                        .Where(x => x.CodEstado == paramDistribuicao.CodEstado &&
                                    x.CodComarca == paramDistribuicao.CodComarca &&
                                    x.CodVara == paramDistribuicao.CodVara &&
                                    x.CodTipoVara == paramDistribuicao.CodTipoVara &&
                                    x.CodTipoProcesso == paramDistribuicao.CodTipoProcesso &&
                                    x.CodEmpresaCentralizadora == paramDistribuicao.CodEmpresaCentralizadora &&
                                    (paramDistribuicao.CodParamDistribuicao == 0 || x.CodParamDistribuicao != paramDistribuicao.CodParamDistribuicao)).AnyAsync(ct);
            return existe;
        }

        #endregion

        #region ANEXO

        [HttpPost("listar-anexos/{codParamDistribuicao:int}")]
        public async Task<ActionResult<List<ListarAnexosResponse>>> ListarAnexosAsync(CancellationToken ct, int codParamDistribuicao, [FromBody] int[] idAnexos)
        {

            var lista = await (from a in _db.AnexosDistEscritorio.AsNoTracking()
                               join p in _db.ParamDistribuicaoAnexos on a.IdAnexoDistEscritorio equals p.IdAnexoDistEscritorio into leftJoin
                               from p in leftJoin.DefaultIfEmpty()
                               where (idAnexos.Contains(a.IdAnexoDistEscritorio) || p.CodParamDistribuicao == codParamDistribuicao)
                               select new ListarAnexosResponse
                               {
                                   CodParamDistribuicao = p == null ? 0 : p.CodParamDistribuicao,
                                   IdAnexoDistEscritorio = a.IdAnexoDistEscritorio,
                                   NomeArquivo = a.NomeArquivo,
                                   Comentario = a.Comentario,
                                   DataUpload = a.LogDataOperacao,
                                   NomeUsuario = a.LogCodUsuarioNavigation.NomeUsuario
                               })
                               .ToListAsync(ct);

            return Ok(lista);
        }

        [HttpDelete("excluir-anexo/{codParamDistribuicao:int}/{idAnexoDistEscritorio:int}")]
        public async Task<IActionResult> ExcluirAnexoAsync(CancellationToken ct, int codParamDistribuicao, int idAnexoDistEscritorio)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();
                _db.ExecutarProcedureDeLog(User.Identity!.Name, true);

                var qtdeExcluidos = await _db.ParamDistribuicaoAnexos
                    .Where(x => x.CodParamDistribuicao == codParamDistribuicao && x.IdAnexoDistEscritorio == idAnexoDistEscritorio)
                    .ExecuteDeleteAsync(ct);

                // verifica se o anexo está atrelado a outra distribuição
                if (!await _db.ParamDistribuicaoAnexos.AnyAsync(x => x.IdAnexoDistEscritorio == idAnexoDistEscritorio))
                {
                    // exclui o anexo da tabela de anexos
                    await _db.AnexosDistEscritorio
                        .Where(x => x.IdAnexoDistEscritorio == idAnexoDistEscritorio)
                        .ExecuteDeleteAsync(ct);
                }

                scope.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Não foi possível excluir o anexo");
            }
        }

        [HttpPost("upload/incluir-anexo/{codParamDistribuicao:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ListarAnexosResponse>> IncluirAnexoAsync(CancellationToken ct, int codParamDistribuicao, [FromForm] IncluirAnexoRequest model)
        {
            try
            {
                if (model.Arquivo is null)
                    return BadRequest("Nenhum arquivo para anexar.");

                var limiteArquivoEmMB = (await _pjdb.ParametroJuridico.FirstAsync(x => x.CodParametro == TAM_MAX_ANEXO_DIST_ESCRIT, ct)).DscConteudoParametro;
                var caminhoNas = await _pjdb.TratarCaminhoDinamicoArrayAsync(DIR_NAS_ANEXO_DIST_ESCRIT, ct: ct);

                // valida o tamanho dos arquivos

                if (model.Arquivo.Length > int.Parse(limiteArquivoEmMB) * 1024 * 1024)
                    return BadRequest($"O arquivo '{model.Arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");

                // cria o objeto para ser salvo no banco
                var anexo = new AnexosDistEscritorio();
                anexo.NomeArquivo = model.Arquivo.FileName;
                anexo.NomeArquivoNas = $"{Guid.NewGuid()}{Path.GetExtension(model.Arquivo.FileName)}";
                anexo.Comentario = model.Comentario;
                anexo.LogDataOperacao = DateTime.Now;
                anexo.LogCodUsuario = User.Identity!.Name;

                // se o CodParamDistribuicao for informado, faz a ligação entre o anexo e o parâmetro
                if (codParamDistribuicao > 0)
                    anexo.ParamDistribuicaoAnexos.Add(new ParamDistribuicaoAnexos { CodParamDistribuicao = codParamDistribuicao });

                // salva o arquivo na rede
                Directory.CreateDirectory(caminhoNas[0]);
                using (var stream = System.IO.File.Create(Path.Combine(caminhoNas[0], anexo.NomeArquivoNas)))
                    await model.Arquivo.CopyToAsync(stream, ct);

                // salva os anexos no banco
                _db.AnexosDistEscritorio.AddRange(anexo);
                await _db.SaveChangesAsync(User.Identity!.Name, true, ct);

                // monta a lista de retorno
                var lar = new ListarAnexosResponse();
                lar.CodParamDistribuicao = codParamDistribuicao;
                lar.Comentario = anexo.Comentario;
                lar.IdAnexoDistEscritorio = anexo.IdAnexoDistEscritorio;
                lar.NomeArquivo = anexo.NomeArquivo;
                lar.DataUpload = anexo.LogDataOperacao;
                lar.NomeUsuario = (await _db.AcaUsuario.FirstAsync(x => x.CodUsuario == anexo.LogCodUsuario)).NomeUsuario;

                return Ok(lar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Não foi possível incluir o anexo");
            }
        }

        [HttpGet("download-anexo/{idAnexoDistEscritorio:int}")]
        public async Task<IActionResult> DownloadAnexoAsync(CancellationToken ct, int idAnexoDistEscritorio)
        {
            // procura pelo anexo
            var anexo = await _db.AnexosDistEscritorio.AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdAnexoDistEscritorio == idAnexoDistEscritorio, ct);

            if (anexo == null)
                return BadRequest("Anexo não encontrado.");

            // obtém o parâmetro jurídico com o caminho do NAS
            var caminhoNas = await _pjdb.TratarCaminhoDinamicoArrayAsync(DIR_NAS_ANEXO_DIST_ESCRIT, anexo.NomeArquivoNas, ct: ct);

            foreach (var arquivo in caminhoNas)
            {
                if (System.IO.File.Exists(arquivo))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(arquivo, ct);
                    return File(bytes, "application/zip", anexo.NomeArquivo);
                }
            }

            return BadRequest("Anexo não encontrado no NAS.");
        }

        [HttpGet("download-todos-anexos")]
        public async Task<IActionResult> DownloadTodosAnexosAsync(CancellationToken ct, [FromQuery] int[] idAnexos)
        {
            // procura pelos anexos
            var anexos = await (_db.AnexosDistEscritorio.AsNoTracking()
                .Where(x => idAnexos.Contains(x.IdAnexoDistEscritorio)))
                .ToListAsync(ct);

            if (!anexos.Any())
                return BadRequest("Anexo não encontrado.");

            // obtém o parâmetro jurídico com o caminho do NAS
            var caminhoNas = await _pjdb.TratarCaminhoDinamicoArrayAsync(DIR_NAS_ANEXO_DIST_ESCRIT, ct: ct);

            // monta o zip para download
            using var memoryStream = new MemoryStream();
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var zipFile = new ZipArchive(memoryStream, ZipArchiveMode.Create, false);

            foreach (var anexo in anexos)
            {
                foreach (var dir in caminhoNas)
                {
                    var arquivo = Path.Combine(dir, anexo.NomeArquivoNas);

                    if (System.IO.File.Exists(arquivo))
                    {
                        var bytes = await System.IO.File.ReadAllBytesAsync(arquivo, ct);

                        // adiciona ao zip
                        var zipEntry = zipFile.CreateEntry(anexo.NomeArquivo);
                        using (var zipEntryStream = new StreamWriter(zipEntry.Open(), Encoding.UTF8))
                        {
                            await zipEntryStream.BaseStream.WriteAsync(bytes, ct);
                            break;
                        }
                    }
                }
            }

            zipFile.Dispose();

            return File(memoryStream.ToArray(), "application/zip", $"Anexos_{DateTime.Now:yyyyMMdd_HHmmss}.zip");

        }

        #endregion

        #region PROCEDURE DE DISTRIBUICAO

        [HttpGet("executa-procedure-distribuicao")]
        public async Task<int> ExecutaProcedureDistribuicaoAsync(CancellationToken ct, string CodEstado, int CodComarca, int CodVara, int CodTipoVara, int CodTipoProcesso, int CodEmpresaGrupo)
        {
            using var scope = await _db.Database.BeginTransactionAsync(ct);

            _db.ExecutarProcedureDeLog(User.Identity!.Name, true);

            var parameters = new OracleParameter[]
            {
                new OracleParameter("P_COD_ESTADO", OracleDbType.NVarchar2, CodEstado, ParameterDirection.Input),
                new OracleParameter("P_COD_COMARCA", OracleDbType.Int64, CodComarca,ParameterDirection.Input),
                new OracleParameter("P_COD_VARA", OracleDbType.Int64, CodVara,ParameterDirection.Input),
                new OracleParameter("P_COD_TIPO_VARA", OracleDbType.Int64, CodTipoVara,ParameterDirection.Input),
                new OracleParameter("P_COD_TIPO_PROCESSO", OracleDbType.Int64, CodTipoProcesso,ParameterDirection.Input),
                new OracleParameter("P_COD_EMPRESA_GRUPO", OracleDbType.Int64, CodEmpresaGrupo,ParameterDirection.Input),
                new OracleParameter("P_COD_PROFISSIONAL", OracleDbType.Int64, ParameterDirection.Output),
            };

            var command = "BEGIN jur.SP_DISTRIBUI_PROCESSO(:P_COD_ESTADO,:P_COD_COMARCA,:P_COD_VARA,:P_COD_TIPO_VARA,:P_COD_TIPO_PROCESSO,:P_COD_EMPRESA_GRUPO,:P_COD_PROFISSIONAL); END;";

            await _db.Database.ExecuteSqlRawAsync(command, parameters);

            return (int)((OracleDecimal)parameters.First(x => x.ParameterName == "P_COD_PROFISSIONAL").Value).Value;
        }

        #endregion

        #region VERIFICA ESCRITORIO ATIVO


        [HttpGet("validar-escritorio-ativo")]
        public async Task<bool> ValidaEscritorioAtivo(CancellationToken ct, int id)
        {
            return await _db.Profissional.AsNoTracking().AnyAsync(x => x.CodProfissional == id && x.IndAtivo == "S", ct);
        }
        #endregion

    }
}
