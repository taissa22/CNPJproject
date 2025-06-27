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
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2501")]
    [ApiController]
    public class ESocialF2501InfoCrIrrfController : ControllerBase
    {
        //private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ILogger _logger;

        private List<RetornoListaDTO> _listaRetorno = new List<RetornoListaDTO>()
        {
            new RetornoListaDTO() { Id = 188951, Descricao = "IRRF - RRA - Decisão da Justiça do Trabalho" },
            new RetornoListaDTO() { Id = 593656, Descricao = "IRRF - Decisão da Justiça do Trabalho" },
        };


        private const int QuantidadePorPagina = 10;

        public ESocialF2501InfoCrIrrfController(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
            //_logger = logger;
        }



        #region Consulta

        [HttpGet("consulta/info-cr-irrf/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfocrirrfDTO>> RetornaInfocrirrfF2501Async([FromRoute] int codigoFormulario, [FromRoute] long codigoInfocrirrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario && x.IdEsF2501Infocrirrf == codigoInfocrirrf, ct);
                if (infoCrIrrf is not null)
                {
                    EsF2501InfocrirrfDTO infoCrIrrfDTO = PreencheInfocrirrfDTO(ref infoCrIrrf);

                    return Ok(infoCrIrrfDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da informação de imposto de renda retído na fonte.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/info-cr-irrf/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501InfocrirrfDTO>>> ListaInfocrirrfF2501Async([FromRoute] int codigoFormulario,
                                                                                                           [FromQuery] int pagina,
                                                                                                           [FromQuery] string coluna,
                                                                                                           [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario, ct))
                {
                    IQueryable<EsF2501InfocrirrfDTO> listaInfocrirrf = RecuperaListaInfocrirrf(codigoFormulario);

                    switch (coluna.ToLower())
                    {
                        case "tipo":
                        default:
                            listaInfocrirrf = ascendente ? listaInfocrirrf.OrderBy(x => x.InfocrcontribTpcr) : listaInfocrirrf.OrderByDescending(x => x.InfocrcontribTpcr);
                            break;
                    }

                    var total = await listaInfocrirrf.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var lista = await listaInfocrirrf.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct);

                    foreach (var item in lista)
                    {
                        item.DescricaoTpcr = $"{item.InfocrcontribTpcr!.Value} - {_listaRetorno.First(x => x.Id == item.InfocrcontribTpcr).Descricao}";
                    }

                    var resultado = new RetornoPaginadoDTO<EsF2501InfocrirrfDTO>
                    {
                        Total = total,
                        Lista = lista
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de imposto de renda retído na fonte.", erro = e.Message });
            }
        }


        #endregion

        #region Alteração

        [HttpPut("alteracao/info-cr-irrf/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> AlteraInfocrirrfF2501Async([FromRoute] int codigoFormulario, [FromRoute] int codigoInfocrirrf, [FromBody] EsF2501InfocrirrfRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de imposto de renda retído na fonte.");
                    }

                    var infoCrIrrf = await _eSocialDbContext.EsF2501Infocrirrf.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario && x.IdEsF2501Infocrirrf == codigoInfocrirrf, ct);

                    if (infoCrIrrf is not null)
                    {
                        PreencheInfocrirrf(ref infoCrIrrf, requestDTO);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de imposto de renda retído na fonte.");
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
                return BadRequest(new { mensagem = "Falha ao alterar informações de imposto de renda retído na fonte.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusão

        [HttpPost("inclusao/info-cr-irrf/{codigoFormulario}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> CadastraInfocrirrfF2501Async([FromRoute] int codigoFormulario, [FromBody] EsF2501InfocrirrfRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de imposto de renda retído na fonte.");
                    }

                    if (await _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == codigoFormulario).CountAsync(ct) >= 99)
                    {
                        return BadRequest("O sistema só permite a inclusão de até 99 registros de Imposto de Renda.");
                    };

                    var infoCrIrrf = new EsF2501Infocrirrf();

                    PreencheInfocrirrf(ref infoCrIrrf, requestDTO, codigoFormulario);

                    _eSocialDbContext.Add(infoCrIrrf);

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma informação de imposto de renda retído na fonte.");
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
                return BadRequest(new { mensagem = "Falha ao incluir uma informação de imposto de renda retído na fonte.", erro = e.Message });
            }
        }

        #endregion


        #region ARQUIVOS
        [HttpPost("upload/info-cr-irrf/{codigoFormulario}/{opcaoCarga}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codigoFormulario, [FromRoute] int opcaoCarga, [FromForm] IFormFile arquivo, CancellationToken ct)
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
                        csv.Context.RegisterClassMap(new ImportarPlanilhaIdeImpostoMap());

                        var linhas = csv.GetRecords<EsF2501InfocrirrfRequestDTO>().ToList();

                        double tamanhoArquivo = reader.BaseStream.Length / 1024 / 1024;

                        if (linhas.Count == 0)
                        {
                            return BadRequest("Arquivo não contém dados para importação.");
                        }

                        if (linhas.Count > 99)
                        {
                            return BadRequest("Quantidade de linhas no arquivo excedida. Quantidade máxima 99 linhas.");
                        }

                        if (tamanhoArquivo > limiteArquivoEmMB)
                        {
                            return BadRequest($"O arquivo '{arquivo.FileName}' excede o tamanho permitido: {limiteArquivoEmMB}MB");
                        }

                        var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                        if (formulario is not null)
                        {
                            if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                            {
                                return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um Imposto de Renda Retido na Fonte.");
                            }

                            if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
                            {
                                ExcluirLinhas(codigoFormulario);                                
                            }

                            int contadorLinhas = 1;
                            var linhasComErro = new List<int>();

                            foreach (var linha in linhas)
                            {
                                if (linha.InfocrcontribTpcr == null & linha.InfocrcontribVrcr == null)
                                {
                                    contadorLinhas++;
                                    continue;
                                }

                                var (linhaInvalida, listaErros) = linha.Validar();

                                var listaErrosTemp = listaErros.ToList();

                                if (linha.InfocrcontribTpcr.HasValue && !_listaRetorno.Any(x => x.Id == linha.InfocrcontribTpcr.Value))
                                {
                                    listaErrosTemp.Add("O código informado no campo \"Código Receita (CR) IRRF\" não exite na tabela de \"Códigos de Receita - Reclamatória Trabalhista\".");
                                }

                                contadorLinhas++;

                                var infoCrIrrf = new EsF2501Infocrirrf();

                                PreencheInfocrirrf(ref infoCrIrrf, linha, codigoFormulario);

                                _eSocialDbContext.Add(infoCrIrrf);
                               
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

                            if (await _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == codigoFormulario).CountAsync(ct) > 99)
                            {
                                scope.Rollback();
                                return BadRequest("O sistema só permite a inclusão de até 99 registros de Imposto de Renda.");
                            };
                            
                            scope.Commit();
                            return Ok();
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
                scope.Rollback();
                return BadRequest("Não foi possível incluir o anexo");
            }
        }

        [HttpGet("download/info-cr-irrf/{codigoFormulario}")]
        public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoFormulario, [FromQuery] bool ascendente, CancellationToken ct)
        {
            if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario, ct))
            {
                IQueryable<EsF2501InfocrirrfDTO> listaInfocrirrf = RecuperaListaInfocrirrf(codigoFormulario);

                var arquivo = GerarCSV(listaInfocrirrf.ToList(), ascendente);
                return arquivo;
            }

            return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
        }
        
        #endregion


        #region Exclusão

        [HttpDelete("exclusao/info-cr-irrf/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> ExcluiInfoCrIrrfF2500Async([FromRoute] int codigoFormulario, [FromRoute] long codigoInfocrirrf, CancellationToken ct)
        {
            try
            {
                var formulario = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de imposto de renda retído na fonte.");
                    }

                    var infoCrIrrf = await _eSocialDbContext.EsF2501Infocrirrf.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario && x.IdEsF2501Infocrirrf == codigoInfocrirrf, ct);

                    if (infoCrIrrf is not null)
                    {
                        infoCrIrrf.LogCodUsuario = User!.Identity!.Name;
                        infoCrIrrf.LogDataOperacao = DateTime.Now;
                        _eSocialDbContext.Remove(infoCrIrrf);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de imposto de renda retído na fonte.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir uma informação de imposto de renda retído na fonte.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos privados

        private void PreencheInfocrirrf(ref EsF2501Infocrirrf infoCrIrrf, EsF2501InfocrirrfRequestDTO requestDTO, int? codigoFormulario = null)
        {
            if (codigoFormulario.HasValue)
            {
                infoCrIrrf.IdF2501 = codigoFormulario.Value;
            }
            infoCrIrrf.InfocrcontribTpcr = requestDTO.InfocrcontribTpcr;
            infoCrIrrf.InfocrcontribVrcr = requestDTO.InfocrcontribVrcr;

            infoCrIrrf.LogCodUsuario = User!.Identity!.Name;
            infoCrIrrf.LogDataOperacao = DateTime.Now;

        }

        private static EsF2501InfocrirrfDTO PreencheInfocrirrfDTO(ref EsF2501Infocrirrf? infoContrato)
        {
            return new EsF2501InfocrirrfDTO()
            {
                IdEsF2501Infocrirrf = infoContrato!.IdEsF2501Infocrirrf,
                IdF2501 = infoContrato!.IdF2501,
                LogDataOperacao = infoContrato!.LogDataOperacao,
                LogCodUsuario = infoContrato!.LogCodUsuario,
                InfocrcontribTpcr = infoContrato!.InfocrcontribTpcr,
                InfocrcontribVrcr = infoContrato!.InfocrcontribVrcr,
                DescricaoTpcr = string.Empty,
            };
        }

        private IQueryable<EsF2501InfocrirrfDTO> RecuperaListaInfocrirrf(int codigoFormulario)
        {
            return from es2501 in _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking()
                   where es2501.IdF2501 == codigoFormulario
                   select new EsF2501InfocrirrfDTO()
                   {
                       IdF2501 = es2501.IdF2501,
                       IdEsF2501Infocrirrf = es2501.IdEsF2501Infocrirrf,
                       InfocrcontribTpcr = es2501.InfocrcontribTpcr,
                       InfocrcontribVrcr = es2501.InfocrcontribVrcr,
                       LogCodUsuario = es2501.LogCodUsuario,
                       LogDataOperacao = es2501.LogDataOperacao,
                   };
        }

        private void ExcluirLinhas(int codigoFormulario)
        {
            var infoCrIrrf = _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == codigoFormulario).ToList();

            foreach (var item in infoCrIrrf)
            {
                item.LogCodUsuario = User!.Identity!.Name;
                item.LogDataOperacao = DateTime.Now;
                _eSocialDbContext.Remove(item);

            }

        }

        private FileResult GerarCSV(List<EsF2501InfocrirrfDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.InfocrcontribTpcr) : resultado.ToArray().OrderByDescending(x => x.InfocrcontribTpcr);

            foreach (var item in lista)
            {
                item.DescricaoTpcr = $"{item.InfocrcontribTpcr!.Value} - {_listaRetorno.First(x => x.Id == item.InfocrcontribTpcr).Descricao}";
            }

            var csv = lista.ToCsvByteArray(typeof(ExportaF2501InfocrirrfMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoE_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        #endregion
    }
}
