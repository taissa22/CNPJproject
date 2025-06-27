using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using System.Globalization;
using System.IO;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2500")]
    [ApiController]
    public class ESocialF2500IdePeriodoController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        //private readonly ILogger _logger;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500IdePeriodoController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/ide-periodo/{codigoContrato}/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500IdeperiodoDTO>> ListaPeriodosF2500dAsync([FromRoute] int codigoContrato, [FromRoute] long codigoIdeperiodo, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var periodo = await _eSocialDbContext.EsF2500Ideperiodo.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                    if (periodo is not null)
                    {
                        EsF2500IdeperiodoDTO periodoDTO = PreencheIdePeriodoDTO(ref periodo);

                        return Ok(periodoDTO);
                    }

                    return NotFound($"Nenhuma informação de período de base de cálculo encontrada para o id: {codigoIdeperiodo} ");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do período de base de cálculo.", erro = e.Message });
            }
        }

        #endregion Consulta

        #region Lista paginado

        [HttpGet("lista/ide-periodo/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500IdeperiodoDTO>>> ListaPeriodosF2500dAsync([FromRoute] int codigoContrato,
                                                                                                    [FromQuery] int pagina,
                                                                                                    [FromQuery] string coluna,
                                                                                                    [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);
                if (contrato is not null)
                {
                    IQueryable<EsF2500IdeperiodoDTO> listaPeriodos = RecuperaListaPeriodos(codigoContrato);

                    switch (coluna.ToLower())
                    {
                        case "periodoref":
                        default:
                            listaPeriodos = ascendente ? listaPeriodos.OrderBy(x => x.IdeperiodoPerref) : listaPeriodos.OrderByDescending(x => x.IdeperiodoPerref);
                            break;
                    }

                    var total = await listaPeriodos.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500IdeperiodoDTO>
                    {
                        Total = total,
                        Lista = await listaPeriodos.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista dos períodos de base de cálculo.", erro = e.Message });
            }
        }

        #endregion Lista paginado

        #region Alteração

        [HttpPut("alteracao/ide-periodo/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraIdePeriodoF2500Async([FromRoute] int codigoFormulario,
                                                                  [FromRoute] int codigoContrato,
                                                                  [FromRoute] int codigoIdeperiodo,
                                                                  [FromBody] EsF2500IdeperiodoRequestDTO requestDTO,
                                                                  [FromServices] ESocialF2500IdePeriodoService service,
                                                                  CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                        }
                        
                        var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);

                        if (periodo is not null)
                        {
                            PreencheIdePeriodo(ref periodo, requestDTO);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaAlteracaoIdePeriodo(requestDTO, contrato).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar o período de base de cálculo.", erro = e.Message });
            }
        }

        #endregion Alteração

        #region Inclusao

        [HttpPost("inclusao/ide-periodo/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraIdePeriodoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromBody] EsF2500IdeperiodoRequestDTO requestDTO, [FromServices] ESocialF2500IdePeriodoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de perído de bases de cálculo de contribuição previdenciária e FGTS.");
                        }

                        //if (await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= 360)
                        //{
                        //    return BadRequest("O sistema só permite a inclusão de até 360 Bases de Cálculos.");
                        //};

                        var periodo = new EsF2500Ideperiodo();

                        PreencheIdePeriodo(ref periodo, requestDTO, codigoContrato);

                        _eSocialDbContext.Add(periodo);

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de perído de bases de cálculo de contribuição previdenciária e FGTS.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaInclusaoIdePeriodo(requestDTO, contrato).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }
                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de perído de bases de cálculo de contribuição previdenciária e FGTS.", erro = e.Message });
            }
        }

        #endregion Inclusao

        #region ARQUIVOS

        [HttpPost("upload/ide-periodo/{codigoFormulario}/{codigoContrato}/{opcaoCarga}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromForm] IFormFile arquivo,
                                                                  [FromServices] ESocialF2500IdePeriodoService service, CancellationToken ct, [FromRoute] int opcaoCarga = 1)
        {
            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                if (arquivo is null) return BadRequest("Nenhum arquivo para anexar.");

                double limiteArquivoEmMB = 10;
                //var limiteArquivoEmMB = _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("criar parametro juridico"); //TODO criar parametro

                if (FileHelper.ExtensaoArquivoInvalida(arquivo.FileName, ".csv"))
                {
                    return BadRequest("Extensão do arquivo inválida. Deve ser um arquivo \".csv\".");
                }

                using (var reader = new StreamReader(arquivo.OpenReadStream()))
                {
                    var csvConfiguration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                    {
                        Delimiter = ";",
                        TrimOptions = TrimOptions.Trim,
                        HasHeaderRecord = true,
                        IgnoreBlankLines = false,
                        MissingFieldFound = null                       
                    };

                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        csv.Context.RegisterClassMap(new ImportarPlanilhaIdePeriodoMap());

                        var linhas = csv.GetRecords<EsF2500IdeperiodoRequestDTO>().ToList();

                        double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

                        if (linhas.Count == 0)
                        {
                            return BadRequest("Arquivo não contém dados para importação.");
                        }

                        //if (linhas.Count > 361)
                        //{
                        //    return BadRequest("Quantidade de linhas no arquivo excedida. Quantidade máxima 360 linhas.");
                        //}

                        if (tamanhoArquivo > limiteArquivoEmMB)
                        {
                            return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
                        }

                        var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                        var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                        if (formulario is not null)
                        {
                            if (contrato is not null)
                            {
                                if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                                {
                                    return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                                }                               

                                if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
                                {
                                    await ApagaListaListaPeriodos(codigoContrato, ct);
                                }

                                int contadorLinhas = 1;
                                var linhasComErro = new List<int>();

                                foreach (var linha in linhas)
                                {
                                    if (linha.IdeperiodoPerref == new DateTime(1901,01,01) && linha.BasecalculoVrbccpmensal == null)
                                    {
                                        contadorLinhas++;
                                        continue;
                                    }

                                    var (linhaInvalida, listaErrosDTO) = linha.Validar();
                                    

                                    var listaErros = listaErrosDTO.ToList();

                                    listaErros.AddRange(service.ValidaInclusaoIdePeriodo(linha, contrato).ToList());

                                    contadorLinhas++;

                                    linhaInvalida = listaErros.Count > 0;

                                    if (linhaInvalida)
                                    {
                                        linhasComErro.Add(contadorLinhas);
                                        continue;
                                    }

                                    var periodoUpdate = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdeperiodoPerref == linha.IdeperiodoPerref.ToString("yyyy-MM"), ct);

                                    if (periodoUpdate != null)
                                    {
                                        PreencheIdePeriodo(ref periodoUpdate, linha);
                                    }
                                    else
                                    {
                                        var periodo = new EsF2500Ideperiodo();
                                        PreencheIdePeriodo(ref periodo, linha, codigoContrato);
                                        _eSocialDbContext.Add(periodo);
                                    }

                                    await _eSocialDbContext.SaveChangesAsync(ct);                                    
                                }

                                if (linhasComErro.Any())
                                {
                                    scope.Rollback();
                                    return BadRequest(linhasComErro);
                                }

                                //if (await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) > 360)
                                //{
                                //    scope.Rollback();
                                //    return BadRequest("O sistema só permite a inclusão de até 360 Bases de Cálculos.");
                                //};

                                scope.Commit();

                                return Ok();
                            }
                            scope.Rollback();
                            return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");

                        }
                        scope.Rollback();
                        return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The conversion cannot be performed") || ex.Message.Contains("An unexpected error occurred"))
                {
                    var indiceInicial = ex.Message.IndexOf("Row: ") + 5;
                    var indiceFinal = ex.Message.IndexOf("\r", indiceInicial);
                    var tamanhoLinha = indiceFinal - indiceInicial;
                    var linha = ex.Message.Substring(indiceInicial, tamanhoLinha);
                    scope.Rollback();
                    return BadRequest($"O arquivo importado contem dados que não podem ser lidos na linha: {linha}. Corrija e verifique se existem outras linhas com o mesmo problema.");
                }
                //  _logger.LogError(ex.ToString());
                scope.Rollback();
                return BadRequest("Não foi possível incluir o anexo");
            }
        }

        [HttpGet("download/ide-periodo/{codigoContrato}")]
        public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoContrato, [FromQuery] bool ascendente, CancellationToken ct)
        {
            var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
            if (contrato is not null)
            {
                    IQueryable<EsF2500IdeperiodoDTO> periodoDTO = RecuperaListaPeriodos(codigoContrato);
                periodoDTO = ascendente ? periodoDTO.OrderBy(x => x.IdeperiodoPerref) : periodoDTO.OrderByDescending(x => x.IdeperiodoPerref);
                    var arquivo = GerarCSV(periodoDTO.ToList(), ascendente);
                    return arquivo;
            }

            return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
        }

        #endregion ARQUIVOS

        #region Exclusão

        [HttpDelete("exclusao/ide-periodo/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiIdePeriodoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] long codigoIdeperiodo, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um perído de base de cálculo.");
                        }

                        var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);

                        if (periodo is not null)
                        {
                            periodo.LogCodUsuario = User!.Identity!.Name;
                            periodo.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(periodo);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um perído de base de cálculo.");
                        }

                        await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir o perído de base de cálculo.", erro = e.Message });
            }
        }

        #endregion Exclusão

        #region Métodos privados

        private void PreencheIdePeriodo(ref EsF2500Ideperiodo periodo, EsF2500IdeperiodoRequestDTO requestDTO, int? codigoContrato = null)
        {
            if (codigoContrato.HasValue)
            {
                periodo.IdEsF2500Infocontrato = codigoContrato.Value;
            }
            periodo.IdeperiodoPerref = requestDTO.IdeperiodoPerref.ToString("yyyy-MM");
            periodo.BasecalculoVrbccpmensal = requestDTO.BasecalculoVrbccpmensal;
            periodo.BasecalculoVrbccp13 = requestDTO.BasecalculoVrbccp13;
            periodo.BasecalculoVrbcfgts = requestDTO.BasecalculoVrbcfgts;
            periodo.BasecalculoVrbcfgts13 = requestDTO.BasecalculoVrbcfgts13;
            periodo.InfoagnocivoGrauexp = requestDTO.InfoagnocivoGrauexp;
            periodo.InfofgtsVrbcfgtsguia = requestDTO.InfofgtsVrbcfgtsguia;
            periodo.InfofgtsVrbcfgts13guia = requestDTO.InfofgtsVrbcfgts13guia;
            periodo.InfofgtsPagdireto = requestDTO.InfofgtsPagdireto;
            periodo.BasemudcategCodcateg = requestDTO.BasemudcategCodcateg;
            periodo.BasemudcategVrbccprev = requestDTO.BasemudcategVrbccprev;

            periodo.LogCodUsuario = User!.Identity!.Name;
            periodo.LogDataOperacao = DateTime.Now;
        }

        private static EsF2500IdeperiodoDTO PreencheIdePeriodoDTO(ref EsF2500Ideperiodo? periodo)
        {
            return new EsF2500IdeperiodoDTO()
            {
                IdEsF2500Ideperiodo = periodo!.IdEsF2500Ideperiodo,
                IdEsF2500Infocontrato = periodo!.IdEsF2500Infocontrato,
                BasecalculoVrbccp13 = periodo!.BasecalculoVrbccp13,
                BasecalculoVrbccpmensal = periodo!.BasecalculoVrbccpmensal,
                BasecalculoVrbcfgts = periodo!.BasecalculoVrbcfgts,
                BasecalculoVrbcfgts13 = periodo!.BasecalculoVrbcfgts13,
                BasemudcategCodcateg = periodo!.BasemudcategCodcateg,
                BasemudcategVrbccprev = periodo!.BasemudcategVrbccprev,
                IdeperiodoPerref = periodo!.IdeperiodoPerref,
                InfoagnocivoGrauexp = periodo!.InfoagnocivoGrauexp,
                InfofgtsPagdireto = periodo!.InfofgtsPagdireto,
                InfofgtsVrbcfgts13guia = periodo!.InfofgtsVrbcfgts13guia,
                InfofgtsVrbcfgtsguia = periodo!.InfofgtsVrbcfgtsguia,

                LogCodUsuario = periodo!.LogCodUsuario,
                LogDataOperacao = periodo!.LogDataOperacao
            };
        }

        private IQueryable<EsF2500IdeperiodoDTO> RecuperaListaPeriodos(int codigoContrato)
        {
            return from esf2500 in _eSocialDbContext.EsF2500Ideperiodo.AsNoTracking()
                   join t02 in _eSocialDbContext.EsTabela02.Where(x => x.IndAtivo == "S") on esf2500.InfoagnocivoGrauexp equals t02.CodEsTabela02 into grouping2
                   from t02 in grouping2.DefaultIfEmpty()
                   join t01 in _eSocialDbContext.EsTabela01 on esf2500.BasemudcategCodcateg! equals t01.CodEsTabela01 into grouping
                   from t01 in grouping.DefaultIfEmpty()
                   where esf2500.IdEsF2500Infocontrato == codigoContrato
                                    select new EsF2500IdeperiodoDTO()
                                    {
                                        IdEsF2500Infocontrato = esf2500.IdEsF2500Infocontrato,
                                        IdEsF2500Ideperiodo = esf2500.IdEsF2500Ideperiodo,
                                        LogDataOperacao = esf2500.LogDataOperacao,
                                        LogCodUsuario = esf2500.LogCodUsuario,
                                        BasecalculoVrbccp13 = esf2500.BasecalculoVrbccp13,
                                        BasecalculoVrbccpmensal = esf2500.BasecalculoVrbccpmensal,
                                        BasecalculoVrbcfgts = esf2500.BasecalculoVrbcfgts,
                                        BasecalculoVrbcfgts13 = esf2500.BasecalculoVrbcfgts13,
                                        BasemudcategCodcateg = esf2500.BasemudcategCodcateg,
                                        BasemudcategVrbccprev = esf2500.BasemudcategVrbccprev,
                                        IdeperiodoPerref = esf2500.IdeperiodoPerref,
                                        InfoagnocivoGrauexp = esf2500.InfoagnocivoGrauexp,
                                        InfofgtsPagdireto = esf2500.InfofgtsPagdireto,
                                        InfofgtsVrbcfgts13guia = esf2500.InfofgtsVrbcfgts13guia,
                                        InfofgtsVrbcfgtsguia = esf2500.InfofgtsVrbcfgtsguia,
                                        BasemudcategCodcategDesc = esf2500.BasemudcategCodcateg.HasValue ? $"{esf2500.BasemudcategCodcateg} - {t01.DscEsTabela01}" : string.Empty,
                                        InfoagnocivoGrauexpDesc = esf2500.InfoagnocivoGrauexp.HasValue ? $"{esf2500.InfoagnocivoGrauexp} - {t02.DscEsTabela02}": string.Empty,
                                    };
        }

        private async Task ApagaListaListaPeriodos(int codigoContrato, CancellationToken ct)
        {
            var qryListaPeriodos = await _eSocialDbContext.EsF2500Ideperiodo
                                    .Where(x => x.IdEsF2500Infocontrato == codigoContrato).AsNoTracking().ToListAsync(ct);

            foreach (var periodo in qryListaPeriodos)
            {
                periodo.LogCodUsuario = User!.Identity!.Name;
                periodo.LogDataOperacao = DateTime.Now;
                _eSocialDbContext.Remove(periodo);
            }
            await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity!.Name, true, ct);
        }

        private FileResult GerarCSV(List<EsF2500IdeperiodoDTO> resultado, bool ascendente)
        {

            var lista = ascendente ?  resultado.ToArray().OrderBy(x => x.IdeperiodoPerref) : resultado.ToArray().OrderByDescending(x => x.IdeperiodoPerref);

            var csv = lista.ToCsvByteArray(typeof(ExportaF2500IdePeriodoMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoK_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        #endregion Métodos privados
    }
}