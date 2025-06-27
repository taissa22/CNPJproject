using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;
using System.Globalization;
using System.IO;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2501")]
    [ApiController]
    public class ESocialF2501InfoCrContribController : ControllerBase
    {
        //private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        //private readonly ILogger _logger;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501InfoCrContribController(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
            //_logger = logger;
        }



        #region Consulta

        [HttpGet("consulta/info-cr-contrib/{codigoCalctrib}/{codigoInfocrcontrib}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfocrcontribDTO>> RetornaInfocrcontribF2501Async([FromRoute] int codigoCalctrib, [FromRoute] long codigoInfocrcontrib, CancellationToken ct)
        {
            try
            {
                var infoCrContrib = await _eSocialDbContext.EsF2501Infocrcontrib.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Calctrib == codigoCalctrib && x.IdEsF2501Infocrcontrib == codigoInfocrcontrib, ct);
                if (infoCrContrib is not null)
                {
                    EsF2501InfocrcontribDTO infoCrContribDTO = PreencheInfocrcontribDTO(ref infoCrContrib);

                    return Ok(infoCrContribDTO);
                }

                return BadRequest($"Cálculo de tributos não encontrado para o id: {codigoCalctrib} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da informação de contribuições.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/info-cr-contrib/{codigoFormulario}/{codigoCalcTrib}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501InfocrcontribDTO>>> ListaInfocrcontribF2501Async([FromRoute] int codigoFormulario,
                                                                                                            [FromRoute] int codigoCalcTrib,
                                                                                                            [FromQuery] int pagina,
                                                                                                            [FromQuery] string coluna,
                                                                                                            [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    IQueryable<EsF2501InfocrcontribDTO> listaInfocrcontrib = RecuperaListaInfocrcontrib(codigoCalcTrib);

                    switch (coluna.ToLower())
                    {
                        case "tipo":
                        default:
                            listaInfocrcontrib = ascendente ? listaInfocrcontrib.OrderBy(x => x.InfocrcontribTpcr) : listaInfocrcontrib.OrderByDescending(x => x.InfocrcontribTpcr);
                            break;
                    }

                    var total = await listaInfocrcontrib.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501InfocrcontribDTO>
                    {
                        Total = total,
                        Lista = await listaInfocrcontrib.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Cálculo de tributos não encontrado para o id: {codigoCalcTrib} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista das informações de contrinbuições.", erro = e.Message });
            }
        }


        #endregion

        #region Alteração

        [HttpPut("alteracao/info-cr-contrib/{codigoFormulario}/{codigoCalcTrib}/{codigoInfocrcontrib}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> AlteraInfocrcontribF2501Async([FromRoute] int codigoFormulario,
                                                                      [FromRoute] int codigoCalcTrib,
                                                                      [FromRoute] int codigoInfocrcontrib,
                                                                      [FromBody] EsF2501InfocrcontribRequestDTO requestDTO,
                                                                      CancellationToken ct)

        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma informação de contribuição.");
                    }

                    var infoCrContrib = await _eSocialDbContext.EsF2501Infocrcontrib.FirstOrDefaultAsync(x => x.IdEsF2501Calctrib == codigoCalcTrib && x.IdEsF2501Infocrcontrib == codigoInfocrcontrib, ct);

                    if (infoCrContrib is not null)
                    {
                        PreencheInfocrcontrib(ref infoCrContrib, requestDTO);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma informação de contribuição.");
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
                return BadRequest(new { mensagem = "Falha ao alterar a informação de contribuição.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusão

        [HttpPost("inclusao/info-cr-contrib/{codigoFormulario}/{codigoCalctrib}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> CadastraInfocrcontribF2501Async([FromRoute] int codigoFormulario, [FromRoute] int codigoCalctrib, [FromBody] EsF2501InfocrcontribRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de contribuição.");
                    }

                    if (await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == codigoCalctrib).CountAsync(ct) >= 99)
                    {
                        return BadRequest("O sistema só permite a inclusão de até 99 registros de Contribuições Sociais.");
                    };


                    var periodo = new EsF2501Infocrcontrib();

                    PreencheInfocrcontrib(ref periodo, requestDTO, codigoCalctrib);

                    _eSocialDbContext.Add(periodo);

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de contribuição.");
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
                return BadRequest(new { mensagem = "Falha ao incluir uma informação de contribuição.", erro = e.Message });
            }
        }

        #endregion


        #region ARQUIVOS
        [HttpPost("upload/info-cr-contrib/{codigoFormulario}/{opcaoCarga}/{tipoCarga}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codigoFormulario, [FromRoute] int opcaoCarga, [FromRoute] int tipoCarga, [FromForm] IFormFile arquivo, CancellationToken ct)
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

                if (tipoCarga == ESocialTipoPlanilha.Emlinhas.ToByte())
                {
                    return await IncluirEmLinha(codigoFormulario, opcaoCarga, arquivo, scope, limiteArquivoEmMB, ct);
                }
                else
                {
                    return await IncluirEmColunas(codigoFormulario, opcaoCarga, arquivo, scope, limiteArquivoEmMB, ct);
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

        [HttpGet("download/info-cr-contrib/{codigoFormulario}/{codigoCalcTrib}")]
        public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoFormulario, [FromRoute] int codigoCalcTrib, [FromQuery] bool ascendente, CancellationToken ct)
        {
            var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
            if (formulario2501 is not null)
            {
                IQueryable<EsF2501InfocrcontribDTO> listaInfocrcontrib = RecuperaListaInfocrcontrib(codigoCalcTrib);

                var arquivo = GerarCSV(listaInfocrcontrib.ToList(), ascendente);
                return arquivo;
            }

            return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
        }

        [HttpGet("download/info-cr-contrib/{codigoFormulario}")]
        public async Task<ActionResult> ExportarCsvListaPeriodo([FromRoute] int codigoFormulario, [FromQuery] bool ascendente, CancellationToken ct)
        {
            var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
            if (formulario2501 is not null)
            {
                IQueryable<EsF2501InfocrcontribExportarDTO> listaInfocrcontrib = RecuperaListaInfocrcontribPeriodo(codigoFormulario);
                if (listaInfocrcontrib.Count() > 0)
                {
                    var arquivo = GerarCSV(listaInfocrcontrib.ToList(), ascendente);
                    return arquivo;
                }
                else
                {
                    return BadRequest(new { mensagem = $"Contribuições não encontradas para o formulário id: {codigoFormulario} " });

                }

            }

            return BadRequest(new { mensagem = $"Formulário não encontrado para o id: {codigoFormulario} " });
        }
        #endregion


        #region Exclusão

        [HttpDelete("exclusao/info-cr-contrib/{codigoFormulario}/{codigoCalctrib}/{codigoInfocrcontrib}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> ExcluiInfoCrContribF2501Async([FromRoute] int codigoFormulario, [FromRoute] int codigoCalctrib, [FromRoute] long codigoInfocrcontrib, CancellationToken ct)
        {
            try
            {
                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de contribuição.");
                    }

                    var infoCrContrib = await _eSocialDbContext.EsF2501Infocrcontrib.FirstOrDefaultAsync(x => x.IdEsF2501Calctrib == codigoCalctrib && x.IdEsF2501Infocrcontrib == codigoInfocrcontrib, ct);

                    if (infoCrContrib is not null)
                    {
                        infoCrContrib.LogCodUsuario = User!.Identity!.Name;
                        infoCrContrib.LogDataOperacao = DateTime.Now;
                        _eSocialDbContext.Remove(infoCrContrib);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de contribuição.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir uma informação de contribuição.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos privados

        private void PreencheInfocrcontrib(ref EsF2501Infocrcontrib infoCrContrib, EsF2501InfocrcontribRequestDTO requestDTO, int? codigoCalcTrib = null)
        {
            if (codigoCalcTrib.HasValue)
            {
                infoCrContrib.IdEsF2501Calctrib = codigoCalcTrib.Value;
            }

            infoCrContrib.InfocrcontribTpcr = requestDTO.InfocrcontribTpcr;
            infoCrContrib.InfocrcontribVrcr = requestDTO.InfocrcontribVrcr;

            infoCrContrib.LogCodUsuario = User!.Identity!.Name;
            infoCrContrib.LogDataOperacao = DateTime.Now;

        }

        private static EsF2501InfocrcontribDTO PreencheInfocrcontribDTO(ref EsF2501Infocrcontrib? infoCrContrib)
        {
            return new EsF2501InfocrcontribDTO()
            {
                IdEsF2501Infocrcontrib = infoCrContrib!.IdEsF2501Infocrcontrib,
                IdEsF2501Calctrib = infoCrContrib!.IdEsF2501Calctrib,
                InfocrcontribTpcr = infoCrContrib!.InfocrcontribTpcr,
                InfocrcontribVrcr = infoCrContrib!.InfocrcontribVrcr,
                DescricaoTpcr = string.Empty,

                LogCodUsuario = infoCrContrib!.LogCodUsuario,
                LogDataOperacao = infoCrContrib!.LogDataOperacao
            };
        }

        private IQueryable<EsF2501InfocrcontribDTO> RecuperaListaInfocrcontrib(int codigoCalcTrib)
        {
            return from es2501 in _eSocialDbContext.EsF2501Infocrcontrib.AsNoTracking()
                   join t29 in _eSocialDbContext.EsTabela29.Where(x => x.IndAtivo == "S") on es2501.InfocrcontribTpcr equals t29.CodEsTabela29
                   where es2501.IdEsF2501Calctrib == codigoCalcTrib
                   select new EsF2501InfocrcontribDTO()
                   {
                       IdEsF2501Calctrib = es2501.IdEsF2501Calctrib,
                       IdEsF2501Infocrcontrib = es2501.IdEsF2501Infocrcontrib,
                       InfocrcontribTpcr = es2501.InfocrcontribTpcr,
                       InfocrcontribVrcr = es2501.InfocrcontribVrcr,
                       LogDataOperacao = es2501.LogDataOperacao,
                       LogCodUsuario = es2501.LogCodUsuario,
                       DescricaoTpcr = es2501.InfocrcontribTpcr.HasValue ? $"{es2501.InfocrcontribTpcr.Value} - {t29.DscEsTabela29}" : string.Empty

                   };
        }

        private IQueryable<EsF2501InfocrcontribExportarDTO> RecuperaListaInfocrcontribPeriodo(int codigoFormulario)
        {


            return from es2501 in _eSocialDbContext.EsF2501Infocrcontrib.AsNoTracking()
                   join t29 in _eSocialDbContext.EsTabela29.Where(x => x.IndAtivo == "S") on es2501.InfocrcontribTpcr equals t29.CodEsTabela29
                   join ict in _eSocialDbContext.EsF2501Calctrib on es2501.IdEsF2501Calctrib equals ict.IdEsF2501Calctrib
                   where ict.IdEsF2501 == codigoFormulario
                   select new EsF2501InfocrcontribExportarDTO()
                   {
                       CalctribPerref = ict.CalctribPerref,
                       IdEsF2501Calctrib = es2501.IdEsF2501Calctrib,
                       IdEsF2501Infocrcontrib = es2501.IdEsF2501Infocrcontrib,
                       InfocrcontribTpcr = es2501.InfocrcontribTpcr,
                       InfocrcontribVrcr = es2501.InfocrcontribVrcr,
                       LogDataOperacao = es2501.LogDataOperacao,
                       LogCodUsuario = es2501.LogCodUsuario,
                       DescricaoTpcr = es2501.InfocrcontribTpcr.HasValue ? $"{es2501.InfocrcontribTpcr.Value} - {t29.DscEsTabela29}" : string.Empty

                   };
        }

        private async Task ApagaListaInfoCrContrib(int codigoFormulario, DateTime? PeriodoRef, CancellationToken ct)
        {
            var periodo = PeriodoRef!.Value.ToString("yyyy-MM");

            var qryCalcTrib = from ict in _eSocialDbContext.EsF2501Calctrib
                              where ict.IdEsF2501 == codigoFormulario
                              && ict.CalctribPerref == periodo
                              select ict.IdEsF2501Calctrib;

            var listaIdCalcTrib = await qryCalcTrib.ToListAsync(ct);

            foreach (var idCalcTrib in listaIdCalcTrib)
            {
                var listaInfoCrContrib = await (from icc in _eSocialDbContext.EsF2501Infocrcontrib
                                                where icc.IdEsF2501Calctrib == idCalcTrib
                                                select icc).ToListAsync(ct);

                foreach (var infoCrContrib in listaInfoCrContrib)
                {
                    _eSocialDbContext.Remove(infoCrContrib);
                }
            }
        }

        private FileResult GerarCSV(List<EsF2501InfocrcontribDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.InfocrcontribTpcr) : resultado.ToArray().OrderByDescending(x => x.InfocrcontribTpcr);

            var csv = lista.ToCsvByteArray(typeof(ExportaF2501InfocrcontribMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoD_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        private FileResult GerarCSV(List<EsF2501InfocrcontribExportarDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.CalctribPerref) : resultado.ToArray().OrderByDescending(x => x.CalctribPerref);
            var csv = lista.ToCsvByteArray(typeof(ExportaF2501InfocrcontribExportarMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoD_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        private async Task<IActionResult> IncluirEmLinha(int codigoFormulario, int opcaoCarga, IFormFile arquivo, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction scope, double limiteArquivoEmMB, CancellationToken ct)
        {
            using (var reader = new StreamReader(arquivo.OpenReadStream()))
            {
                var csvConfiguration = new CsvConfiguration(new CultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = false,
                    MissingFieldFound = null
                };

                using (var csv = new CsvReader(reader, csvConfiguration))
                {
                    csv.Context.RegisterClassMap(new ImportarPlanilhaInfoCrContribMap());

                    var linhas = csv.GetRecords<EsF2501InfocrcontribImportRequestDTO>().ToList();

                    double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

                    if (linhas.Count == 0)
                    {
                        return BadRequest("Arquivo não contém dados para importação.");
                    }

                    if (linhas.Count > 35641)
                    {
                        return BadRequest("Quantidade de linhas no arquivo excedida. A quantidade máxima é de 35.640 linhas, sendo 99 linhas para cada período de referência.");
                    }

                    if (tamanhoArquivo > limiteArquivoEmMB)
                    {
                        return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
                    }

                    var listaPeridos = linhas.Select(x => x.CalctribPerref).Distinct();

                    foreach (var periodo in listaPeridos)
                    {
                        if (linhas.Count(x => x.CalctribPerref == periodo) > 99)
                        {
                            return BadRequest("Quantidade de linhas para cada período de referência no arquivo excedida. A quantidade máxima é de 99 linhas por período.");
                        }
                    }

                    var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                    if (formulario is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                        }

                        int contadorLinhas = 1;
                        var linhasComErro = new List<int>();

                        foreach (var linha in linhas)
                        {
                            if (linha.CalctribPerref == new DateTime(1901, 01, 01) && linha.InfocrcontribTpcr == null)
                            {
                                contadorLinhas++;
                                continue;
                            }

                            var (linhaInvalida, listaErros) = linha.Validar();
                            var listaErrosTemp = listaErros.ToList();

                            contadorLinhas++;

                            var periodoReferencia = linha.CalctribPerref;

                            if (periodoReferencia == null)
                            {
                                listaErrosTemp.Add("\"Período e Base de Cálculo dos Tributos\" não informado.");
                                linhasComErro.Add(contadorLinhas);
                                continue;
                            }

                            if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
                            {
                                await ApagaListaInfoCrContrib(codigoFormulario, periodoReferencia, ct);
                            }

                            if (linha.InfocrcontribTpcr.HasValue && (!await _eSocialDbContext.EsTabela29.AnyAsync(x => x.CodEsTabela29 == linha.InfocrcontribTpcr.Value, ct)))
                            {
                                listaErrosTemp.Add("O código informado no campo \"Código Receita (CR) Contrib. Sociais\" não exite na tabela de \"Códigos de Receita - Reclamatória Trabalhista\".");
                            }

                            var calcTrib = await _eSocialDbContext.EsF2501Calctrib.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501 == codigoFormulario && x.CalctribPerref == periodoReferencia.Value.ToString("yyyy-MM"), ct);

                            if (calcTrib != null)
                            {
                                var infoCrContrib = new EsF2501Infocrcontrib();

                                PreencheInfocrcontrib(ref infoCrContrib, linha, (int)calcTrib.IdEsF2501Calctrib);

                                _eSocialDbContext.Add(infoCrContrib);

                            }
                            else
                            {
                                listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                            }

                            listaErros = listaErrosTemp;

                            linhaInvalida = listaErros.Any();

                            if (linhaInvalida)
                            {
                                linhasComErro.Add(contadorLinhas);
                            }
                        }

                        if (linhasComErro.Any())
                        {
                            scope.Rollback();
                            return BadRequest(linhasComErro);
                        }

                        await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity!.Name, true, ct);

                        foreach (var linha in linhas)
                        {
                            var periodo = linha.CalctribPerref!.Value.ToString("yyyy-MM");

                            var qryCalcTrib = from ict in _eSocialDbContext.EsF2501Calctrib
                                              where ict.IdEsF2501 == codigoFormulario
                                              && ict.CalctribPerref == periodo
                                              select ict.IdEsF2501Calctrib;

                            var listaIdCalcTrib = await qryCalcTrib.ToListAsync(ct);
                            var count = 0;
                            foreach (var idCalcTrib in listaIdCalcTrib)
                            {

                                if (await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == idCalcTrib).CountAsync(ct) > 99)
                                {
                                    count++;
                                };
                            }

                            if (count > 0)
                            {
                                scope.Rollback();
                                return BadRequest("O sistema só permite a inclusão de até 99 registros de Contribuições Sociais.");
                            }
                        }

                        scope.Commit();
                        return Ok();


                    }
                    scope.Rollback();
                    return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");

                }
            }
        }

        private async Task<IActionResult> IncluirEmColunas(int codigoFormulario, int opcaoCarga, IFormFile arquivo, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction scope, double limiteArquivoEmMB, CancellationToken ct)
        {
            using (var reader = new StreamReader(arquivo.OpenReadStream()))
            {
                var csvConfiguration = new CsvConfiguration(new CultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = false,
                    MissingFieldFound = null
                };

                using (var csv = new CsvReader(reader, csvConfiguration))
                {
                    csv.Context.RegisterClassMap(new ImportarPlanilhaInfoCrContribColunasMap());

                    var linhas = csv.GetRecords<EsF2501InfocrcontribImportColunaRequestDTO>().ToList();

                    double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

                    if (linhas.Count == 0)
                    {
                        return BadRequest("Arquivo não contém dados para importação.");
                    }

                    if (linhas.Count > 35641)
                    {
                        return BadRequest("Quantidade de linhas no arquivo excedida. A quantidade máxima é de 35.640 linhas, sendo 99 linhas para cada período de referência.");
                    }

                    if (tamanhoArquivo > limiteArquivoEmMB)
                    {
                        return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
                    }

                    var listaPeridos = linhas.Select(x => x.CalctribPerref).Distinct();

                    foreach (var periodo in listaPeridos)
                    {
                        if (linhas.Count(x => x.CalctribPerref == periodo) > 99)
                        {
                            return BadRequest("Quantidade de linhas para cada período de referência no arquivo excedida. A quantidade máxima é de 99 linhas por período.");
                        }
                    }

                    var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                    if (formulario is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um perído de base de cálculo.");
                        }

                        int contadorLinhas = 1;
                        var linhasComErro = new List<int>();
                        var Infocrcontrib = new List<EsF2501InfocrcontribImportRequestDTO>();

                        foreach (var item in linhas)
                        {
                            var (linhaInvalida, listaErros) = item.Validar();
                            var listaErrosTemp = listaErros.ToList();

                            if (item.CalctribPerref == new DateTime(1901, 01, 01))
                            {
                                contadorLinhas++;
                                continue;
                            }

                            contadorLinhas++;

                            var periodoReferencia = item.CalctribPerref;

                            if (periodoReferencia == null)
                            {
                                listaErrosTemp.Add("\"Período e Base de Cálculo dos Tributos\" não informado.");
                                linhasComErro.Add(contadorLinhas);
                                continue;
                            }

                            await ApagaListaInfoCrContrib(codigoFormulario, periodoReferencia, ct);

                            var calcTrib = await _eSocialDbContext.EsF2501Calctrib.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501 == codigoFormulario && x.CalctribPerref == periodoReferencia.Value.ToString("yyyy-MM"), ct);

                            var InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO();

                            if (item.ContribSocialSegurado.HasValue && item.ContribSocialSegurado > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.ContribSocialSegurado.ToInt(),
                                    InfocrcontribVrcr = item.ContribSocialSegurado,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.ContribSocialEmpregador.HasValue && item.ContribSocialEmpregador > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.ContribSocialEmpregador.ToInt(),
                                    InfocrcontribVrcr = item.ContribSocialEmpregador,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.RatSat.HasValue && item.RatSat > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.RatSat.ToInt(),
                                    InfocrcontribVrcr = item.RatSat,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.SalariaEducacao.HasValue && item.SalariaEducacao > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.SalariaEducacao.ToInt(),
                                    InfocrcontribVrcr = item.SalariaEducacao,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.INCRA.HasValue && item.INCRA > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.INCRA.ToInt(),
                                    InfocrcontribVrcr = item.INCRA,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.SENAI.HasValue && item.SENAI > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.SENAI.ToInt(),
                                    InfocrcontribVrcr = item.SENAI,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.SESI.HasValue && item.SESI > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.SESI.ToInt(),
                                    InfocrcontribVrcr = item.SESI,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            if (item.SEBRAE.HasValue && item.SEBRAE > 0)
                            {
                                InfocrcontribImport = new EsF2501InfocrcontribImportRequestDTO
                                {
                                    CalctribPerref = item.CalctribPerref,
                                    InfocrcontribTpcr = ESocialCRContribSocial.SEBRAE.ToInt(),
                                    InfocrcontribVrcr = item.SEBRAE,
                                };

                                if (calcTrib != null)
                                {
                                    var infoCrContrib = new EsF2501Infocrcontrib();

                                    PreencheInfocrcontrib(ref infoCrContrib, InfocrcontribImport, (int)calcTrib.IdEsF2501Calctrib);

                                    _eSocialDbContext.Add(infoCrContrib);

                                }
                                else
                                {
                                    listaErrosTemp.Add("O \"Período e Base de Cálculo dos Tributos\" informado não existe.");
                                }

                                Infocrcontrib.Add(InfocrcontribImport);
                            }

                            listaErros = listaErrosTemp;

                            linhaInvalida = listaErros.Any();

                            if (linhaInvalida)
                            {
                                linhasComErro.Add(contadorLinhas);
                            }

                        }

                        if (linhasComErro.Any())
                        {
                            scope.Rollback();
                            return BadRequest(linhasComErro);
                        }

                        await _eSocialDbContext.SaveChangesExternalScopeAsync(User.Identity!.Name, true, ct);

                        foreach (var linha in linhas)
                        {
                            var periodo = linha.CalctribPerref!.Value.ToString("yyyy-MM");

                            var qryCalcTrib = from ict in _eSocialDbContext.EsF2501Calctrib
                                              where ict.IdEsF2501 == codigoFormulario
                                              && ict.CalctribPerref == periodo
                                              select ict.IdEsF2501Calctrib;

                            var listaIdCalcTrib = await qryCalcTrib.ToListAsync(ct);
                            var count = 0;
                            foreach (var idCalcTrib in listaIdCalcTrib)
                            {

                                if (await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == idCalcTrib).CountAsync(ct) > 99)
                                {
                                    count++;
                                };
                            }

                            if (count > 0)
                            {
                                scope.Rollback();
                                return BadRequest("O sistema só permite a inclusão de até 99 registros de Contribuições Sociais.");
                            }
                        }

                        scope.Commit();
                        return Ok();


                    }
                    scope.Rollback();
                    return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");

                }
            }
        }


        #endregion
    }
}
