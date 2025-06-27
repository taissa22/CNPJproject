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
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Services;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/ESocialF2501")]
    [ApiController]
    public class ESocialF2501InfoCrIrrfController : ControllerBase
    {
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ILogger _logger;


        private const int QuantidadePorPagina = 10;

        public ESocialF2501InfoCrIrrfController(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
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

        [HttpGet("consulta/info-cr-irrf/tributaveis/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfocrirrfTributaveisDTO>> RetornaInfocrirrfTributaveisF2501Async([FromRoute] int codigoFormulario, [FromRoute] long codigoInfocrirrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario && x.IdEsF2501Infocrirrf == codigoInfocrirrf, ct);
                if (infoCrIrrf is not null)
                {
                    EsF2501InfocrirrfTributaveisDTO infoCrIrrfDTO = PreencheInfocrirrTributaveisfDTO(ref infoCrIrrf);

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

                    //foreach (var item in lista)
                    //{
                    //    item.DescricaoTpcr = $"{item.InfocrcontribTpcr!.Value} - {_listaRetorno.First(x => x.Id == item.InfocrcontribTpcr).Descricao}";
                    //}

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
                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

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

                    var listaErros = listaErrosDTO.ToList();

                    if (await _eSocialDbContext.EsF2501Infocrirrf.AnyAsync(x => x.IdF2501 == codigoFormulario && x.IdEsF2501Infocrirrf != codigoInfocrirrf && x.InfocrcontribTpcr == requestDTO.InfocrcontribTpcr, ct))
                    {
                        listaErros.Add("O formulário pode conter apenas um registro para cada código de receita de informações de imposto de renda retido na fonte.");
                    }

                    formularioInvalido = listaErros.Count > 0;

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

        [HttpPut("alteracao/info-cr-irrf/tributaveis/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> AlteraInfocrirrfTributaveisF2501Async([FromRoute] int codigoFormulario, [FromRoute] int codigoInfocrirrf, [FromBody] EsF2501InfocrirrfTributaveisRequestDTO requestDTO, CancellationToken ct)
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
                        PreencheInfocrirrfTributaveis(ref infoCrIrrf, requestDTO);
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
                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

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

                    var listaErros = listaErrosDTO.ToList();

                    if (await _eSocialDbContext.EsF2501Infocrirrf.AnyAsync(x => x.IdF2501 == codigoFormulario && x.InfocrcontribTpcr == requestDTO.InfocrcontribTpcr, ct))
                    {
                        listaErros.Add("O formulário pode conter apenas um registro para cada código de receita de informações de imposto de renda retido na fonte.");
                    }

                    formularioInvalido = listaErros.Count > 0;

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
                               await ExcluirLinhas(codigoFormulario, ct);
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

                                if (linhas.Where(x => x.InfocrcontribTpcr.Equals(linha.InfocrcontribTpcr)).ToList().Count() > 1)
                                {
                                    listaErrosTemp.Add("O formulário pode conter apenas um registro para cada código de receita de informações de imposto de renda retido na fonte.");
                                }

                                if (await _eSocialDbContext.EsF2501Infocrirrf.AnyAsync(x => x.IdF2501 == codigoFormulario && x.InfocrcontribTpcr == linha.InfocrcontribTpcr, ct))
                                {
                                    listaErrosTemp.Add("O formulário pode conter apenas um registro para cada código de receita de informações de imposto de renda retido na fonte.");
                                }
                                var listaCodigoReceita = EnumExtension.ToList<EsocialCodigoIrrf>()
                                    .Select(x => new RetornoListaDTO()
                                    {
                                        Id = x.ToInt(),
                                        Descricao = x.Descricao()
                                    }
                                            );

                                if (linha.InfocrcontribTpcr.HasValue && !listaCodigoReceita.Any(x => x.Id == linha.InfocrcontribTpcr.Value))
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

                        var listaFormulario2501IdeadvExclusao = await _eSocialDbContext.EsF2501Ideadv.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501DeddepenExclusao = await _eSocialDbContext.EsF2501Deddepen.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501PenAlimExclusao = await _eSocialDbContext.EsF2501Penalim.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501ProcRetExclusao = await _eSocialDbContext.EsF2501Infoprocret.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);

                        if (listaFormulario2501IdeadvExclusao.Any())
                        {
                            foreach (var Ideadv in listaFormulario2501IdeadvExclusao)
                            {
                                _eSocialDbContext.Remove(Ideadv);
                            }
                        }

                        if (listaFormulario2501DeddepenExclusao.Any())
                        {
                            foreach (var Deddepen in listaFormulario2501DeddepenExclusao)
                            {
                                _eSocialDbContext.Remove(Deddepen);
                            }
                        }

                        if (listaFormulario2501PenAlimExclusao.Any())
                        {
                            foreach (var PenAlim in listaFormulario2501PenAlimExclusao)
                            {
                                _eSocialDbContext.Remove(PenAlim);
                            }
                        }

                        if (listaFormulario2501ProcRetExclusao.Any())
                        {
                            foreach (var ProcRet in listaFormulario2501ProcRetExclusao)
                            {
                                var listaFormulario2501InfoValoresExclusao = _eSocialDbContext.EsF2501Infovalores.AsNoTracking().Where(x => x.IdEsF2501Infoprocret == ProcRet.IdEsF2501Infoprocret);

                                if (listaFormulario2501InfoValoresExclusao.Any())
                                {
                                    foreach (var InfoValores in listaFormulario2501InfoValoresExclusao)
                                    {
                                        var listaFormulario2501DedSuspExclusao = _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == InfoValores.IdEsF2501Infovalores);
                                        if (listaFormulario2501DedSuspExclusao.Any())
                                        {
                                            foreach (var DedSusp in listaFormulario2501DedSuspExclusao)
                                            {
                                                var listaFormulario2501BenefPenExclusao = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == DedSusp.IdEsF2501Dedsusp);

                                                if (listaFormulario2501BenefPenExclusao.Any())
                                                {
                                                    foreach (var BenefPen in listaFormulario2501BenefPenExclusao)
                                                    {
                                                        _eSocialDbContext.Remove(BenefPen);
                                                    }
                                                }
                                                _eSocialDbContext.Remove(DedSusp);
                                            }
                                        }
                                        _eSocialDbContext.Remove(InfoValores);
                                    }
                                }
                                _eSocialDbContext.Remove(ProcRet);
                            }
                        }

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

            if (requestDTO?.InfocrcontribTpcr == 188951)
            {
                infoCrIrrf.InfoirVrrendisenntrib = null;
                infoCrIrrf.InfoirDescisenntrib = null;
            }

            infoCrIrrf.InfocrcontribTpcr = requestDTO?.InfocrcontribTpcr;
            infoCrIrrf.InfocrcontribVrcr = requestDTO?.InfocrcontribVrcr;
            infoCrIrrf.InfocrcontribVrcr13 = requestDTO?.InfocrcontribVrcr13;

            if (requestDTO?.InfocrcontribTpcr != 56152)
            {
                // CR 0561
                infoCrIrrf.InfoirVlrDiarias = null;
                infoCrIrrf.InfoirVlrAjudaCusto = null;
                infoCrIrrf.InfoirVlrIndResContrato = null;
                infoCrIrrf.InfoirVlrAbonoPec = null;
                infoCrIrrf.InfoirVlrAuxMoradia = null;

            }

            if (requestDTO?.InfocrcontribTpcr == 188951)
            {
                infoCrIrrf.InfoirVrrendtrib13 = null;   
            }

            infoCrIrrf.LogCodUsuario = User!.Identity!.Name;
            infoCrIrrf.LogDataOperacao = DateTime.Now;

        }

        private void PreencheInfocrirrfTributaveis(ref EsF2501Infocrirrf infoCrIrrf, EsF2501InfocrirrfTributaveisRequestDTO requestDTO, int? codigoFormulario = null)
        {
            if (codigoFormulario.HasValue)
            {
                infoCrIrrf.IdF2501 = codigoFormulario.Value;
            }
            //infoCrIrrf.InfocrcontribTpcr = requestDTO?.InfocrcontribTpcr;
            //infoCrIrrf.InfocrcontribVrcr = requestDTO?.InfocrcontribVrcr;

            infoCrIrrf.LogCodUsuario = User!.Identity!.Name;
            infoCrIrrf.LogDataOperacao = DateTime.Now;

            infoCrIrrf.InfoirVrrendtrib = requestDTO?.InfoirVrrendtrib;
            infoCrIrrf.InfoirVrrendtrib13 = requestDTO?.InfoirVrrendtrib13;
            infoCrIrrf.InfoirVrrendmolegrave = requestDTO?.InfoirVrrendmolegrave;
            infoCrIrrf.InfoirVrrendisen65 = requestDTO?.InfoirVrrendisen65;
            infoCrIrrf.InfoirVrjurosmora = requestDTO?.InfoirVrjurosmora;
            infoCrIrrf.InfoirVrrendisenntrib = requestDTO?.InfoirVrrendisenntrib;
            infoCrIrrf.InfoirDescisenntrib = requestDTO?.InfoirVrrendisenntrib > 0 ? requestDTO?.InfoirDescisenntrib : null;
            infoCrIrrf.InfoirVrprevoficial = requestDTO?.InfoirVrprevoficial;

            infoCrIrrf.InfoirVrrendmolegrave13 = requestDTO?.InfoirVrrendmolegrave13;
            infoCrIrrf.InfoirRrendisen65dec = requestDTO?.InfoirRrendisen65dec;
            infoCrIrrf.InfoirVrjurosmora13 = requestDTO?.InfoirVrjurosmora13;
            infoCrIrrf.InfoirVrprevoficial13 = requestDTO?.InfoirVrprevoficial13;

            // CR 0561
            infoCrIrrf.InfoirVlrDiarias = requestDTO?.InfoirVlrDiarias;
            infoCrIrrf.InfoirVlrAjudaCusto = requestDTO?.InfoirVlrAjudaCusto;
            infoCrIrrf.InfoirVlrIndResContrato = requestDTO?.InfoirVlrIndResContrato;
            infoCrIrrf.InfoirVlrAbonoPec = requestDTO?.InfoirVlrAbonoPec;
            infoCrIrrf.InfoirVlrAuxMoradia = requestDTO?.InfoirVlrAuxMoradia;
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
                InfocrcontribVrcr13 = infoContrato!.InfocrcontribVrcr13,
            };
        }

        private static EsF2501InfocrirrfTributaveisDTO PreencheInfocrirrTributaveisfDTO(ref EsF2501Infocrirrf? infoContrato)
        {
            return new EsF2501InfocrirrfTributaveisDTO()
            {
                IdEsF2501Infocrirrf = infoContrato!.IdEsF2501Infocrirrf,
                IdF2501 = infoContrato!.IdF2501,
                LogDataOperacao = infoContrato!.LogDataOperacao,
                LogCodUsuario = infoContrato!.LogCodUsuario,
                InfocrcontribTpcr = infoContrato!.InfocrcontribTpcr,
                InfocrcontribVrcr = infoContrato!.InfocrcontribVrcr,
                DescricaoTpcr = string.Empty,
                InfoirVrrendtrib13 = infoContrato!.InfoirVrrendtrib13,
                InfoirVrrendtrib = infoContrato!.InfoirVrrendtrib,
                InfoirDescisenntrib = infoContrato!.InfoirDescisenntrib,
                InfoirVrjurosmora = infoContrato!.InfoirVrjurosmora,
                InfoirVrprevoficial = infoContrato!.InfoirVrprevoficial,
                InfoirVrrendisen65 = infoContrato!.InfoirVrrendisen65,
                InfoirVrrendisenntrib = infoContrato!.InfoirVrrendisenntrib,
                InfoirVrrendmolegrave = infoContrato!.InfoirVrrendmolegrave,
                InfocrcontribVrcr13 = infoContrato!.InfocrcontribVrcr13,

                InfoirVrrendmolegrave13 = infoContrato!.InfoirVrrendmolegrave13,
                InfoirRrendisen65dec = infoContrato!.InfoirRrendisen65dec,
                InfoirVrjurosmora13 = infoContrato!.InfoirVrjurosmora13,
                InfoirVrprevoficial13 = infoContrato!.InfoirVrprevoficial13,

                // CR 0561
                InfoirVlrDiarias = infoContrato!.InfoirVlrDiarias,
                InfoirVlrAjudaCusto = infoContrato!.InfoirVlrAjudaCusto,
                InfoirVlrIndResContrato = infoContrato!.InfoirVlrIndResContrato,
                InfoirVlrAbonoPec = infoContrato!.InfoirVlrAbonoPec,
                InfoirVlrAuxMoradia = infoContrato!.InfoirVlrAuxMoradia
            };
        }

        private IQueryable<EsF2501InfocrirrfDTO> RecuperaListaInfocrirrf(int codigoFormulario)
        {
            return from es2501 in _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking()
                   join crIrrf in _eSocialDbContext.EsTabelaCrIrrf on es2501.InfocrcontribTpcr!.Value equals crIrrf.CodCrirrf into LeftJoincrIrrf
                   from crIrrf in LeftJoincrIrrf.DefaultIfEmpty()
                   where es2501.IdF2501 == codigoFormulario
                   select new EsF2501InfocrirrfDTO()
                   {
                       IdF2501 = es2501.IdF2501,
                       IdEsF2501Infocrirrf = es2501.IdEsF2501Infocrirrf,
                       InfocrcontribTpcr = es2501.InfocrcontribTpcr,
                       InfocrcontribVrcr = es2501.InfocrcontribVrcr,
                       LogCodUsuario = es2501.LogCodUsuario,
                       LogDataOperacao = es2501.LogDataOperacao,
                       DescricaoTpcr = es2501.InfocrcontribTpcr.HasValue ? $"{crIrrf.CodCrirrf.ToString().PadLeft(6, '0')} - {crIrrf.NomCrirrf}" : string.Empty,
                       InfocrcontribVrcr13 = es2501.InfocrcontribVrcr13,
                   };
        }

        private async Task ExcluirLinhas(int codigoFormulario, CancellationToken ct)
        {
            var infoCrIrrf = _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == codigoFormulario).ToList();

            foreach (var item in infoCrIrrf)
            {
                item.LogCodUsuario = User!.Identity!.Name;
                item.LogDataOperacao = DateTime.Now;
                _eSocialDbContext.Remove(item);

            }

            await _eSocialDbContext.SaveChangesExternalScopeAsync(User!.Identity!.Name, true, ct);
        }

        private FileResult GerarCSV(List<EsF2501InfocrirrfDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.InfocrcontribTpcr) : resultado.ToArray().OrderByDescending(x => x.InfocrcontribTpcr);

            //foreach (var item in lista)
            //{
            //    item.DescricaoTpcr = $"{item.InfocrcontribTpcr!.Value} - {_listaRetorno.First(x => x.Id == item.InfocrcontribTpcr).Descricao}";
            //}

            var csv = lista.ToCsvByteArray(typeof(ExportaF2501InfocrirrfMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoE_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        #endregion
    }
}
