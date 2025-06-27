using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/ESocialF2500")]
    [ApiController]
    public class ESocialF2500MudancaCategoriaController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500MudancaCategoriaController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/mudanca-categoria/{codigoContrato}/{codigoMudCategoria}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500MudcategativDTO>> ListaMudcategativF2500dAsync([FromRoute] int codigoContrato, [FromRoute] long codigoMudCategoria, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var mudancaCatAtiv = await _eSocialDbContext.EsF2500Mudcategativ.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Mudcategativ == codigoMudCategoria, ct);
                    if (mudancaCatAtiv is not null)
                    {
                        EsF2500MudcategativDTO mudancaCatAtivDTO = PreencheMudcategativDTO(ref mudancaCatAtiv);

                        return Ok(mudancaCatAtivDTO);
                    }

                    return NotFound($"Nenhuma informação de mudança de categoria encontrada para o id: {codigoMudCategoria} ");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da mudança de categoria de atividade.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/mudanca-categoria/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500MudcategativDTO>>> ListaMudancasCategoriaF2500dAsync([FromRoute] int codigoContrato,
                                                                                                              [FromQuery] int pagina,
                                                                                                              [FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    IQueryable<EsF2500MudcategativDTO> listaMudancasCategoriaAtividade = RecuperaListaCategoria(codigoContrato);

                    switch (coluna.ToLower())
                    {
                        case "categoria":
                        default:
                            listaMudancasCategoriaAtividade = ascendente ? listaMudancasCategoriaAtividade.OrderBy(x => x.MudcategativCodcateg) : listaMudancasCategoriaAtividade.OrderByDescending(x => x.MudcategativCodcateg);
                            break;
                    }

                    var total = await listaMudancasCategoriaAtividade.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500MudcategativDTO>
                    {
                        Total = total,
                        Lista = await listaMudancasCategoriaAtividade.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de mudanças de categoria de atividade.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/mudanca-categoria/{codigoFormulario}/{codigoContrato}/{codigoMudCategoria}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraMudcategativF2500Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoContrato,
                                                                    [FromRoute] int codigoMudCategoria,
                                                                    [FromBody] EsF2500MudcategativRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2500MudancaCategoriaService service,
                                                                    CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }
                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar(contrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma mudança de categoria de atividade.");
                        }

                        var mudancaCatAtiv = await _eSocialDbContext.EsF2500Mudcategativ.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Mudcategativ == codigoMudCategoria, ct);

                        if (mudancaCatAtiv is not null)
                        {
                            PreencheMudcategativ(ref mudancaCatAtiv, requestDTO);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma mudança de categoria de atividade.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaAlteracaoMudancaCategoria(_eSocialDbContext, requestDTO, contrato, codigoMudCategoria).ToList());

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
                return BadRequest(new { mensagem = "Falha ao alterar mudança de categoria de atividade.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/mudanca-categoria/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraMudcategativF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromBody] EsF2500MudcategativRequestDTO requestDTO, [FromServices] ESocialF2500MudancaCategoriaService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar(contrato!);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de mudança de categoria e/ou natureza da atividade.");
                        }

                        if (await _eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= 99)
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de Mudanças de Categoria.");
                        };

                        var mudancaCatAtiv = new EsF2500Mudcategativ();

                        PreencheMudcategativ(ref mudancaCatAtiv, requestDTO, codigoContrato);

                        _eSocialDbContext.Add(mudancaCatAtiv);

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de mudança de categoria e/ou natureza da atividade.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaInclusaoMudancaCategoria(_eSocialDbContext, requestDTO, contrato).ToList());

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
                return BadRequest(new { mensagem = "Falha ao incluir informações de mudança de categoria e/ou natureza da atividade.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/mudanca-categoria/{codigoFormulario}/{codigoContrato}/{codigoMudCategoria}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiMudcategativF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] long codigoMudCategoria, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma mudança de categoria de atividade.");
                        }

                        var mudancaCatAtiv = await _eSocialDbContext.EsF2500Mudcategativ.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Mudcategativ == codigoMudCategoria, ct);

                        if (mudancaCatAtiv is not null)
                        {
                            mudancaCatAtiv.LogCodUsuario = User!.Identity!.Name;
                            mudancaCatAtiv.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(mudancaCatAtiv);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma mudança de categoria de atividade.");
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
                return BadRequest(new { mensagem = "Falha ao excluir mudança de categoria de atividade.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos Privados

        private void PreencheMudcategativ(ref EsF2500Mudcategativ mudanca, EsF2500MudcategativRequestDTO requestDTO, int? codigoContrato = null)
        {
            if (codigoContrato.HasValue)
            {
                mudanca.IdEsF2500Infocontrato = codigoContrato.Value;
            }

            mudanca.MudcategativCodcateg = requestDTO.MudcategativCodcateg;
            mudanca.MudcategativDtmudcategativ = requestDTO.MudcategativDtmudcategativ;
            mudanca.MudcategativNatatividade = requestDTO.MudcategativNatatividade;

            mudanca.LogCodUsuario = User!.Identity!.Name;
            mudanca.LogDataOperacao = DateTime.Now;

        }

        private static EsF2500MudcategativDTO PreencheMudcategativDTO(ref EsF2500Mudcategativ? mudancaCatAtiv)
        {
            return new EsF2500MudcategativDTO()
            {
                IdEsF2500Mudcategativ = mudancaCatAtiv!.IdEsF2500Mudcategativ,
                IdEsF2500Infocontrato = mudancaCatAtiv!.IdEsF2500Infocontrato,
                LogCodUsuario = mudancaCatAtiv!.LogCodUsuario,
                LogDataOperacao = mudancaCatAtiv!.LogDataOperacao,
                MudcategativCodcateg = mudancaCatAtiv!.MudcategativCodcateg,
                MudcategativDtmudcategativ = mudancaCatAtiv!.MudcategativDtmudcategativ,
                MudcategativNatatividade = mudancaCatAtiv!.MudcategativNatatividade
            };
        }

        private IQueryable<EsF2500MudcategativDTO> RecuperaListaCategoria(int codigoContrato)
        {
            return from mc in _eSocialDbContext.EsF2500Mudcategativ.AsNoTracking()
                   join ct in _eSocialDbContext.EsTabela01 on mc.MudcategativCodcateg! equals ct.CodEsTabela01 into grouping
                   from ct in grouping.DefaultIfEmpty()
                   where mc.IdEsF2500Infocontrato == codigoContrato
                   select new EsF2500MudcategativDTO()
                   {
                       IdEsF2500Infocontrato = mc.IdEsF2500Infocontrato,
                       IdEsF2500Mudcategativ = mc.IdEsF2500Mudcategativ,
                       LogCodUsuario = mc.LogCodUsuario,
                       LogDataOperacao = mc.LogDataOperacao,
                       MudcategativCodcateg = mc.MudcategativCodcateg,
                       MudcategativDtmudcategativ = mc.MudcategativDtmudcategativ,
                       MudcategativNatatividade = mc.MudcategativNatatividade,
                       DescricaoNaturezaDeAtividade = mc.MudcategativNatatividade.HasValue ? mc.MudcategativNatatividade.Value.ToEnum<ESocialNaturezaAtividade>().Descricao() : string.Empty,
                       DescricaoCodCategoria = mc.MudcategativCodcateg.HasValue ? $"{mc.MudcategativCodcateg.Value} - {ct.DscEsTabela01}" : null
                   };

        }

        #endregion
    }
}
