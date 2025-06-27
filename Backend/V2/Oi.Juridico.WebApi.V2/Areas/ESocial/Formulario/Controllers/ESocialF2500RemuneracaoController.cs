using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using System.Globalization;
using System.IO;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/ESocialF2500")]
    [ApiController]
    public class ESocialF2500RemuneracaoController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500RemuneracaoController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/remuneracao/{codigoContrato}/{codigoRemuneracao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500RemuneracaoDTO>> ListaRemuneracoesF2500dAsync([FromRoute] int codigoContrato, [FromRoute] long codigoRemuneracao, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var remuneracao = await _eSocialDbContext.EsF2500Remuneracao.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Remuneracao == codigoRemuneracao, ct);
                    if (remuneracao is not null)
                    {
                        EsF2500RemuneracaoDTO remuneracaoDTO = PreencheRemuneracaoDTO(ref remuneracao);

                        return Ok(remuneracaoDTO);
                    }

                    return NotFound($"Nenhuma informação de remuneração encontrada para o id: {codigoRemuneracao} ");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da remuneração.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/remuneracao/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500RemuneracaoDTO>>> ListaRemuneracoesF2500dAsync([FromRoute] int codigoContrato,
                                                                                                        [FromQuery] int pagina,
                                                                                                        [FromQuery] string coluna,
                                                                                                        [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var listaRecuperacao = RecuperaListaRemuneracao(codigoContrato);

                    switch (coluna.ToLower())
                    {
                        case "datavigencia":
                        default:
                            listaRecuperacao = ascendente ? listaRecuperacao.OrderBy(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenBy(x => x.RemuneracaoDtremun)
                                                          : listaRecuperacao.OrderByDescending(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenByDescending(x => x.RemuneracaoDtremun);
                            break;
                    }

                    var total = await listaRecuperacao.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500RemuneracaoDTO>
                    {
                        Total = total,
                        Lista = await listaRecuperacao.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de remunerações.", erro = e.Message });
            }
        }

        #endregion

        #region Alteração 
        [HttpPut("alteracao/remuneracao/{codigoFormulario}/{codigoContrato}/{codigoRemuneracao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraRemuneracaoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] int codigoRemuneracao, [FromBody] EsF2500RemuneracaoRequestDTO requestDTO, [FromServices] ESocialF2500RemuneracaoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma remuneração.");
                        }

                        var remuneracao = await _eSocialDbContext.EsF2500Remuneracao.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Remuneracao == codigoRemuneracao, ct);

                        if (remuneracao is not null)
                        {
                            PreencheRemuneracao(ref remuneracao, requestDTO);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma remuneração.");
                        }

                        var listaErros = listaErrosDTO.ToList();
                        listaErros.AddRange(service.ValidaAlteracaoRemuneracao(_eSocialDbContext, requestDTO, contrato, codigoRemuneracao).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario}");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar remuneração.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusao 

        [HttpPost("inclusao/remuneracao/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraRemuneracaoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromBody] EsF2500RemuneracaoRequestDTO requestDTO, [FromServices] ESocialF2500RemuneracaoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de remuneração.");
                        }

                        if (await _eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= 99)
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de Remunerações.");
                        };

                        var remuneracao = new EsF2500Remuneracao();

                        PreencheRemuneracao(ref remuneracao, requestDTO, codigoContrato);

                        _eSocialDbContext.Add(remuneracao);

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de remuneração.");
                        }

                        var listaErros = listaErrosDTO.ToList();
                        listaErros.AddRange(service.ValidaInclusaoRemuneracao(_eSocialDbContext, requestDTO, contrato).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario}.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de remuneração.", erro = e.Message });
            }
        }

        #endregion

        #region ARQUIVOS

        [HttpPost("upload/remuneracao/{codigoFormulario}/{codigoContrato}/{opcaoCarga}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> IncluirAnexoAsync([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromForm] IFormFile arquivo, [FromServices] ESocialF2500RemuneracaoService service, CancellationToken ct, [FromRoute] int opcaoCarga = 1)
        {
            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem parmissão para alterar os blocos G e K");
                }

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
                        csv.Context.RegisterClassMap(new ImportarPlanilhaRemuneracaoMap());

                        var linhas = csv.GetRecords<EsF2500RemuneracaoRequestDTO>().ToList();

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

                        var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                        var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                        if (formulario is not null)
                        {
                            if (contrato is not null)
                            {
                                if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                                {
                                    return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de remuneração.");
                                }

                                if (opcaoCarga == ESocialTipoCarga.ApagarRegistros.ToByte())
                                {
                                    await ApagaListaRemneracao(codigoContrato, User!.Identity!.Name!, ct);
                                }

                                int contadorLinhas = 1;
                                var linhasComErro = new List<int>();

                                foreach (var linha in linhas)
                                {
                                    if (!linha.RemuneracaoDtremun.HasValue && linha.RemuneracaoVrsalfx == null)
                                    {
                                        contadorLinhas++;
                                        continue;
                                    }

                                    var (linhaInvalida, listaErrosDTO) = linha.Validar();

                                    var listaErros = listaErrosDTO.ToList();
                                    listaErros.AddRange(service.ValidaInclusaoRemuneracao(_eSocialDbContext, linha, contrato).ToList());

                                    contadorLinhas++;

                                    linhaInvalida = listaErros.Count > 0;

                                    if (linhaInvalida)
                                    {
                                        linhasComErro.Add(contadorLinhas);
                                        continue;
                                    }

                                    var remuneracaoUpdate = await _eSocialDbContext.EsF2500Remuneracao.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.RemuneracaoDtremun == linha.RemuneracaoDtremun, ct);

                                    if (remuneracaoUpdate != null)
                                    {
                                        PreencheRemuneracao(ref remuneracaoUpdate, linha);
                                    }
                                    else
                                    {
                                        var remuneracao = new EsF2500Remuneracao();
                                        PreencheRemuneracao(ref remuneracao, linha, codigoContrato);
                                        _eSocialDbContext.Add(remuneracao);
                                    }

                                    await _eSocialDbContext.SaveChangesAsync(ct);
                                }


                                if (linhasComErro.Any())
                                {
                                    scope.Rollback();

                                    return BadRequest(linhasComErro);
                                }

                                if (await _eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) > 99)
                                {
                                    scope.Rollback();
                                    return BadRequest("O sistema só permite a inclusão de até 99 registros de Remunerações.");
                                };

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

        [HttpGet("download/remuneracao/{codigoContrato}")]
        public async Task<ActionResult> ExportarCsvLista([FromRoute] int codigoContrato, [FromQuery] bool ascendente, CancellationToken ct)
        {


            var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
            if (contrato is not null)
            {
                IQueryable<EsF2500RemuneracaoDTO> periodoDTO = RecuperaListaRemuneracao(codigoContrato);

                periodoDTO = ascendente ? periodoDTO.OrderBy(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenBy(x => x.RemuneracaoDtremun)
                                                          : periodoDTO.OrderByDescending(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenByDescending(x => x.RemuneracaoDtremun);

                var arquivo = GerarCSV(periodoDTO.ToList(), ascendente);
                return arquivo;
            }

            return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
        }
        #endregion ARQUIVOS

        #region Exclusão 

        [HttpDelete("exclusao/remuneracao/{codigoFormulario}/{codigoContrato}/{codigoRemuneracao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiRemuneracaoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] long codigoRemuneracao, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma remuneração.");
                        }

                        var remuneracao = await _eSocialDbContext.EsF2500Remuneracao.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Remuneracao == codigoRemuneracao, ct);

                        if (remuneracao is not null)
                        {
                            remuneracao.LogCodUsuario = User!.Identity!.Name;
                            remuneracao.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(remuneracao);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma remuneração.");
                        }

                        await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario}.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir remuneração.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos privados

        private void PreencheRemuneracao(ref EsF2500Remuneracao remuneracao, EsF2500RemuneracaoRequestDTO requestDTO, int? codigoFormulario = null)
        {

            if (codigoFormulario.HasValue)
            {
                remuneracao.IdEsF2500Infocontrato = codigoFormulario.Value;
            }
            remuneracao.RemuneracaoDscsalvar = requestDTO.RemuneracaoDscsalvar;
            remuneracao.RemuneracaoDtremun = requestDTO.RemuneracaoDtremun;
            remuneracao.RemuneracaoUndsalfixo = requestDTO.RemuneracaoUndsalfixo;
            remuneracao.RemuneracaoVrsalfx = requestDTO.RemuneracaoVrsalfx;

            remuneracao.LogCodUsuario = User!.Identity!.Name;
            remuneracao.LogDataOperacao = DateTime.Now;

        }

        private static EsF2500RemuneracaoDTO PreencheRemuneracaoDTO(ref EsF2500Remuneracao? remuneracao)
        {
            return new EsF2500RemuneracaoDTO()
            {
                IdEsF2500Remuneracao = remuneracao!.IdEsF2500Remuneracao,
                IdEsF2500Infocontrato = remuneracao!.IdEsF2500Infocontrato,
                LogDataOperacao = remuneracao!.LogDataOperacao,
                LogCodUsuario = remuneracao!.LogCodUsuario,
                RemuneracaoDscsalvar = remuneracao!.RemuneracaoDscsalvar,
                RemuneracaoDtremun = remuneracao!.RemuneracaoDtremun,
                RemuneracaoUndsalfixo = remuneracao!.RemuneracaoUndsalfixo
            };
        }

        private IQueryable<EsF2500RemuneracaoDTO> RecuperaListaRemuneracao(int codigoContrato)
        {
            return _eSocialDbContext.EsF2500Remuneracao
                                    .Where(x => x.IdEsF2500Infocontrato == codigoContrato).AsNoTracking()
                                    .Select(x => new EsF2500RemuneracaoDTO()
                                    {
                                        IdEsF2500Infocontrato = x.IdEsF2500Infocontrato,
                                        IdEsF2500Remuneracao = x.IdEsF2500Remuneracao,
                                        LogCodUsuario = x.LogCodUsuario,
                                        LogDataOperacao = x.LogDataOperacao,
                                        RemuneracaoDscsalvar = x.RemuneracaoDscsalvar,
                                        RemuneracaoDtremun = x.RemuneracaoDtremun,
                                        RemuneracaoUndsalfixo = x.RemuneracaoUndsalfixo,
                                        RemuneracaoVrsalfx = x.RemuneracaoVrsalfx,
                                        DescricaoUnidadePagamento = x.RemuneracaoUndsalfixo != null ? $"{x.RemuneracaoUndsalfixo.Value} - {x.RemuneracaoUndsalfixo.Value.ToEnum<ESocialUnidadePagamento>().Descricao()}" : string.Empty
                                    });
        }

        private async Task ApagaListaRemneracao(int codigoContrato, string usuario, CancellationToken ct)
        {
            var qryRemuneracao = await _eSocialDbContext.EsF2500Remuneracao
                                    .Where(x => x.IdEsF2500Infocontrato == codigoContrato).AsNoTracking().ToListAsync(ct);

            foreach (var remneracao in qryRemuneracao)
            {
                _eSocialDbContext.Remove(remneracao);
            }

            await _eSocialDbContext.SaveChangesExternalScopeAsync(usuario, true, ct);
        }

        private FileResult GerarCSV(List<EsF2500RemuneracaoDTO> resultado, bool ascendente)
        {
            var lista = ascendente ? resultado.ToArray().OrderBy(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenBy(x => x.RemuneracaoDtremun)
                                                         : resultado.ToArray().OrderByDescending(x => x.RemuneracaoDtremun != null ? 1 : 0).ThenByDescending(x => x.RemuneracaoDtremun);

            var csv = lista.ToCsvByteArray(typeof(ExportaF2500RemuneracaoMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"BlocoG_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        }

        #endregion
    }
}
