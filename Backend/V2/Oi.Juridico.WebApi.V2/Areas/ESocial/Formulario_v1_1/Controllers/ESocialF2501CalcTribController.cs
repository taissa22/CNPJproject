using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;
using System.Globalization;
using System.IO;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2501")]
    [ApiController]
    public class ESocialF2501CalcTribController : ControllerBase
    {
        //private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;

        private readonly ILogger _logger;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501CalcTribController(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
            //_logger = logger;
        }

        #region Consulta

        [HttpGet("consulta/calc-trib/{codigoFormulario}/{codigoCalctrib}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501CalctribDTO>> RetornaCalctribF2501Async([FromRoute] int codigoFormulario, [FromRoute] long codigoCalctrib, CancellationToken ct)
        {
            try
            {
                var calctrib = await _eSocialDbContext.EsF2501Calctrib.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501 == codigoFormulario && x.IdEsF2501Calctrib == codigoCalctrib, ct);
                if (calctrib is not null)
                {
                    EsF2501CalctribDTO calctribDTO = PreencheCalctribDTO(ref calctrib);

                    return Ok(calctribDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do dependente.", erro = e.Message });
            }
        }

        #endregion Consulta

        #region Lista paginado

        [HttpGet("lista/calc-trib/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<RetornoPaginadoDTO<EsF2501CalctribDTO>>> ListaCalctribF2501Async([FromRoute] int codigoFormulario,
                                                                                                               [FromQuery] int pagina,
                                                                                                               [FromQuery] string coluna,
                                                                                                               [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    IQueryable<EsF2501CalctribDTO> listaCalcTrib = RecuperaListaCalctrib(codigoFormulario);

                    switch (coluna.ToLower())
                    {
                        case "periodo":
                        default:
                            listaCalcTrib = ascendente ? listaCalcTrib.OrderBy(x => x.CalctribPerref) : listaCalcTrib.OrderByDescending(x => x.CalctribPerref);
                            break;
                    }

                    var total = await listaCalcTrib.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501CalctribDTO>
                    {
                        Total = total,
                        Lista = await listaCalcTrib.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista dos Cálculos de tributos.", erro = e.Message });
            }
        }

        #endregion Lista paginado

        #region Alteração

        [HttpPut("alteracao/calc-trib/{codigoFormulario}/{codigoCalctrib}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> AlteraCalcTribF2501Async([FromRoute] int codigoFormulario, [FromRoute] int codigoCalctrib, [FromBody] EsF2501CalctribRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {                    
                    var ExistePeriodo = await _eSocialDbContext.EsF2501Calctrib.AnyAsync(p => p.IdEsF2501 == codigoFormulario && p.IdEsF2501Calctrib != codigoCalctrib && p.CalctribPerref == requestDTO!.CalctribPerref.ToString("yyyy-MM"), ct);

                    if (ExistePeriodo)
                    {
                        return BadRequest("O periodo de referencia já existe");
                    }

                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um cálculo de tributo");
                    }

                    var calcTrib = await _eSocialDbContext.EsF2501Calctrib.FirstOrDefaultAsync(x => x.IdEsF2501 == codigoFormulario && x.IdEsF2501Calctrib == codigoCalctrib, ct);

                    if (calcTrib is not null)
                    {
                        PreencheCalctrib(ref calcTrib, requestDTO);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um cálculo de tributo");
                    }

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Registro alterado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar cálculo de tributo.", erro = e.Message });
            }
        }

        #endregion Alteração

        #region Inclusão

        [HttpPost("inclusao/calc-trib/{codigoFormulario}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> CadastraCalcribF2501Async([FromRoute] int codigoFormulario, [FromBody] EsF2501CalctribRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {                    
                    var ExistePeriodo = await _eSocialDbContext.EsF2501Calctrib.AnyAsync(p => p.IdEsF2501 == codigoFormulario && p.CalctribPerref == requestDTO!.CalctribPerref.ToString("yyyy-MM"), ct);

                    if (ExistePeriodo)
                    {
                        return BadRequest("O periodo de referencia já existe");
                    }

                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir cálculo de tributo");
                    }

                    if (await _eSocialDbContext.EsF2501Calctrib.Where(x => x.IdEsF2501 == codigoFormulario).CountAsync(ct) >= 360)
                    {
                        return BadRequest("O sistema só permite a inclusão de até 360 Bases de Cálculos.");
                    };

                    var calcTrib = new EsF2501Calctrib();

                    PreencheCalctrib(ref calcTrib, requestDTO, codigoFormulario);

                    _eSocialDbContext.Add(calcTrib);

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir cálculos de tributos");
                    }

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Registro incluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao cadastrar cálculo de tributo.", erro = e.Message });
            }
        }

        #endregion Inclusão

        #region ARQUIVOS

        [HttpPost("upload/calc-trib/{codF2501}/{opcaoCarga}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codF2501, [FromRoute] int opcaoCarga, [FromForm] IFormFile arquivo, CancellationToken ct)
        {
            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);

            try
            {
                if (arquivo is null) return BadRequest("Nenhum arquivo para anexar.");

                double limiteArquivoEmMB = 10;

                if (FileHelper.ExtensaoArquivoInvalida(arquivo.FileName, ".csv"))
                {
                    return BadRequest("Extensão do arquivo inválida. Deve ser um arquivo \".csv\".");
                }

                using (var reader = new StreamReader(arquivo.OpenReadStream()))
                {
                    var configuration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                    {
                        Delimiter = ";",
                        TrimOptions = TrimOptions.Trim,
                        HasHeaderRecord = true,
                        IgnoreBlankLines = false,
                        MissingFieldFound = null
                    };

                    using (var csv = new CsvReader(reader, configuration))
                    {
                        csv.Context.RegisterClassMap(new ImportarPlanilhaCalcTribMap());

                        var linhas = csv.GetRecords<EsF2501CalctribRequestDTO>().ToList();

                        double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

                        if (linhas.Count == 0)
                        {
                            return BadRequest("Arquivo não contém dados para importação.");
                        }

                        if (linhas.Count > 361)
                        {
                            return BadRequest("Quantidade de linhas no arquivo excedida. Quantidade máxima 360 linhas.");
                        }

                        if (tamanhoArquivo > limiteArquivoEmMB)
                        {
                            return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
                        }

                        var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codF2501, ct);

                        if (formulario is not null)
                        {
                            if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                            {
                                return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                            }

                            if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
                            {
                                await ApagaListaCalcTrib(codF2501, ct);
                            }

                            int contadorLinhas = 1;
                            var linhasComErro = new List<int>();

                            foreach (var linha in linhas)
                            {
                                if (linha.CalctribPerref == new DateTime(1901, 01, 01) && linha.CalctribVrbccpmensal == null)
                                {
                                    contadorLinhas++;
                                    continue;
                                }

                                var (linhaInvalida, listaErros) = linha.Validar();

                                contadorLinhas++;

                                if (!linhaInvalida)
                                {
                                    
                                    var listaErrosTemp = listaErros.ToList();

                                    var calcTribUpdate = await _eSocialDbContext.EsF2501Calctrib.FirstOrDefaultAsync(x => x.IdEsF2501 == codF2501 && x.CalctribPerref == linha.CalctribPerref.ToString("yyyy-MM"), ct);

                                    if (calcTribUpdate != null)
                                    {
                                        PreencheCalctrib(ref calcTribUpdate, linha);
                                    }
                                    else
                                    {                                        
                                        var calcTrib = new EsF2501Calctrib();
                                        PreencheCalctrib(ref calcTrib, linha, codF2501);
                                        _eSocialDbContext.Add(calcTrib);
                                    }
                                }
                                linhaInvalida = listaErros.Any();

                                if (linhaInvalida)
                                {
                                    linhasComErro.Add(contadorLinhas);
                                    continue;
                                } 

                                await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity!.Name, true, ct);
                                                              
                            }

                            if (linhasComErro.Any())
                            {
                                scope.Rollback();
                                return BadRequest(linhasComErro);
                            }

                            if (await _eSocialDbContext.EsF2501Calctrib.Where(x => x.IdEsF2501 == codF2501).CountAsync(ct) > 360)
                            {
                                scope.Rollback();
                                return BadRequest("O sistema só permite a inclusão de até 360 Bases de Cálculos.");
                            };

                            scope.Commit();
                            return Ok();
                        }

                        scope.Rollback();
                        return BadRequest($"Formulário não encontrado para o id: {codF2501} ");
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

                scope.Rollback();
                return BadRequest("Não foi possível incluir o anexo");
            }
        }

        [HttpGet("download/calc-trib/{codigoFormulario}")]
        public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoFormulario, [FromQuery] bool ascendente, CancellationToken ct)
        {


            var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
            if (formulario2501 is not null)
            {
                    IQueryable<EsF2501CalctribDTO> listaCalcTrib = RecuperaListaCalctrib(codigoFormulario);

                var arquivo = GerarCSV(listaCalcTrib.ToList(), ascendente);
                return arquivo;
            }

            return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
        }

        #endregion ARQUIVOS

        #region Exclusão

        [HttpDelete("exclusao/calc-trib/{codigoFormulario}/{codigoCalctrib}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> ExcluiCalctribF2501Async([FromRoute] int codigoFormulario, [FromRoute] long codigoCalctrib, CancellationToken ct)
        {
            try
            {
                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir cálculo de tributo");
                    }

                    var calcTrib = await _eSocialDbContext.EsF2501Calctrib.FirstOrDefaultAsync(x => x.IdEsF2501 == codigoFormulario && x.IdEsF2501Calctrib == codigoCalctrib, ct);

                    if (calcTrib is not null)
                    {
                        await RemoveCalcTrib(codigoCalctrib, ct);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir cálculos de tributos");
                    }

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir cálculo de tributo.", erro = e.Message });
            }
        }

        #endregion Exclusão

        #region Métodos privados

        private void PreencheCalctrib(ref EsF2501Calctrib calcTrib, EsF2501CalctribRequestDTO requestDTO, int? codigoFormulario = null)
        {
            if (codigoFormulario.HasValue)
            {
                calcTrib.IdEsF2501 = codigoFormulario.Value;
            }
            calcTrib.CalctribPerref = requestDTO.CalctribPerref.ToString("yyyy-MM");
            calcTrib.CalctribVrbccp13 = requestDTO!.CalctribVrbccp13;
            calcTrib.CalctribVrbccpmensal = requestDTO!.CalctribVrbccpmensal;
            calcTrib.CalctribVrrendirrf = requestDTO!.CalctribVrrendirrf;
            calcTrib.CalctribVrrendirrf13 = requestDTO!.CalctribVrrendirrf13;
            calcTrib.LogCodUsuario = User!.Identity!.Name;
            calcTrib.LogDataOperacao = DateTime.Now;
        }

        private static EsF2501CalctribDTO PreencheCalctribDTO(ref EsF2501Calctrib? calctrib)
        {
            return new EsF2501CalctribDTO()
            {
                IdEsF2501Calctrib = calctrib!.IdEsF2501Calctrib,
                IdEsF2501 = calctrib!.IdEsF2501,
                CalctribPerref = calctrib!.CalctribPerref,
                CalctribVrbccp13 = calctrib!.CalctribVrbccp13,
                CalctribVrbccpmensal = calctrib!.CalctribVrbccpmensal,
                CalctribVrrendirrf = calctrib!.CalctribVrrendirrf,
                CalctribVrrendirrf13 = calctrib!.CalctribVrrendirrf13,
                LogCodUsuario = calctrib!.LogCodUsuario,
                LogDataOperacao = calctrib!.LogDataOperacao
            };
        }

        private IQueryable<EsF2501CalctribDTO> RecuperaListaCalctrib(int codigoFormulario)
        {
            return _eSocialDbContext.EsF2501Calctrib
                .Where(x => x.IdEsF2501 == codigoFormulario).AsNoTracking()
                .Select(x => new EsF2501CalctribDTO()
                {
                    IdEsF2501 = x.IdEsF2501,
                    IdEsF2501Calctrib = x.IdEsF2501Calctrib,
                    CalctribPerref = x.CalctribPerref,
                    CalctribVrbccp13 = x.CalctribVrbccp13,
                    CalctribVrbccpmensal = x.CalctribVrbccpmensal,
                    CalctribVrrendirrf = x.CalctribVrrendirrf,
                    CalctribVrrendirrf13 = x.CalctribVrrendirrf13,
                    LogCodUsuario = x.LogCodUsuario,
                    LogDataOperacao = x.LogDataOperacao
                });
        }

        private async Task RemoveCalcTrib(long codigoCalcTrib, CancellationToken ct)
        {
            var formulario2501CalcTribExclusao = await _eSocialDbContext.EsF2501Calctrib.FirstOrDefaultAsync(x => x.IdEsF2501Calctrib == codigoCalcTrib, ct);
            if (formulario2501CalcTribExclusao is not null)
            {
                var listaFormulario2501InfocrcontribExclusao = await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == formulario2501CalcTribExclusao.IdEsF2501Calctrib).ToListAsync(ct);

                if (listaFormulario2501InfocrcontribExclusao.Any())
                {
                    foreach (var infoCrContrib in listaFormulario2501InfocrcontribExclusao)
                    {
                        _eSocialDbContext.Remove(infoCrContrib);
                    }
                }
                _eSocialDbContext.Remove(formulario2501CalcTribExclusao);
            }
        }

        private async Task ApagaListaCalcTrib(int codigoFormulario, CancellationToken ct)
        {
            var qryCalcTrib = await _eSocialDbContext.EsF2501Calctrib
                                    .Where(x => x.IdEsF2501 == codigoFormulario).AsNoTracking().ToListAsync(ct);

            foreach (var remneracao in qryCalcTrib)
            {
                var listaFormulario2501InfocrcontribExclusao = await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == remneracao.IdEsF2501Calctrib).ToListAsync(ct);

                if (listaFormulario2501InfocrcontribExclusao.Any())
                {
                    foreach (var infoCrContrib in listaFormulario2501InfocrcontribExclusao)
                    {
                        _eSocialDbContext.Remove(infoCrContrib);
                    }
                }
                _eSocialDbContext.Remove(remneracao);
            }

            await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity.Name, true, ct);
        }

        private FileResult GerarCSV(List<EsF2501CalctribDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.CalctribPerref) : resultado.ToArray().OrderByDescending(x => x.CalctribPerref);

            var csv = lista.ToCsvByteArray(typeof(ExportaF2501PlanilhaCalcTribMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoC_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }
        #endregion Métodos privados
    }
}