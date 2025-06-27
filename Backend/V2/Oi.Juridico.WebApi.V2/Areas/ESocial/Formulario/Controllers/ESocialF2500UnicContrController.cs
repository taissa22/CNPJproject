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
    public class ESocialF2500UnicContrController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500UnicContrController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/unicidade/{codigoContrato}/{codigoUnicidade}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500UniccontrDTO>> ListaUnicidadesF2500dAsync([FromRoute] int codigoContrato, [FromRoute] long codigoUnicidade, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var unicidade = await _eSocialDbContext.EsF2500Uniccontr.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Uniccontr == codigoUnicidade, ct);
                    if (unicidade is not null)
                    {
                        EsF2500UniccontrDTO dependenteDTO = PreencheUniccontr(ref unicidade);

                        return Ok(dependenteDTO);
                    }
                    return NotFound($"Nenhuma informação de unicidade contratual encontrada para o id: {codigoUnicidade}");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da unicidade contratual.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/unicidade/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500UniccontrDTO>>> ListaUnicidadesF2500dAsync([FromRoute] int codigoContrato,
                                                                                                     [FromQuery] int pagina,
                                                                                                     [FromQuery] string coluna,
                                                                                                     [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (contrato is not null)
                {
                    IQueryable<EsF2500UniccontrDTO> listaUnicidades = RecuperaListadeUnicidade(codigoContrato);

                    switch (coluna.ToLower())
                    {
                        case "matricula":
                        default:
                            listaUnicidades = ascendente ? listaUnicidades.OrderBy(x => x.UniccontrMatunic != null ? 1 : 0).ThenBy(x => x.UniccontrMatunic)
                                                        : listaUnicidades.OrderByDescending(x => x.UniccontrMatunic != null ? 1 : 0).ThenByDescending(x => x.UniccontrMatunic);
                            break;
                    }

                    var total = await listaUnicidades.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500UniccontrDTO>
                    {
                        Total = total,
                        Lista = await listaUnicidades.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de unicidades contratuais.", erro = e.Message });
            }
        }



        #endregion

        #region Alteração 

        [HttpPut("alteracao/unicidade/{codigoFormulario}/{codigoContrato}/{codigoUnicidade}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraUnicidadeF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] int codigoUnicidade,
                                                                  [FromBody] EsF2500UniccontrRequestDTO requestDTO, [FromServices] ESocialF2500UnicContrService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma unicidade contratual.");
                        }

                        var unicidade = await _eSocialDbContext.EsF2500Uniccontr.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Uniccontr == codigoUnicidade, ct);

                        if (unicidade is not null)
                        {
                            PreencheUniccontr(ref unicidade, requestDTO);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma unicidade contratual.");
                        }

                        var listaErros = listaErrosDTO.ToList();
                        listaErros.AddRange(service.ValidaAlteracaoUnicidadeContratual(_eSocialDbContext, requestDTO, contrato, codigoUnicidade).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest("O contrato informado não foi encontrado.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar unicidade contratual.", erro = e.Message });
            }
        }


        #endregion

        #region Inclusao 

        [HttpPost("inclusao/unicidade/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraUnicidadeF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato,
                                                                    [FromBody] EsF2500UniccontrRequestDTO requestDTO, [FromServices] ESocialF2500UnicContrService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de unicidade contratual.");
                        }

                        if (await _eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= 99)
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de Vínculos/Contratos incorporados.");
                        };

                        var unicidade = new EsF2500Uniccontr();

                        PreencheUniccontr(ref unicidade, requestDTO, codigoContrato);

                        _eSocialDbContext.Add(unicidade);

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de unicidade contratual.");
                        }

                        var listaErros = listaErrosDTO.ToList();
                        listaErros.AddRange(service.ValidaInclusaoUnicidadeContratual(_eSocialDbContext, requestDTO, contrato).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest("O contrato informado não foi encontrado.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de unicidade contratual.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/unicidade/{codigoFormulario}/{codigoContrato}/{codigoUnicidade}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiUnicidadeF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] long codigoUnicidade, CancellationToken ct)
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma unicidade contratual.");
                        }

                        var unicidade = await _eSocialDbContext.EsF2500Uniccontr.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Uniccontr == codigoUnicidade, ct);

                        if (unicidade is not null)
                        {
                            unicidade.LogCodUsuario = User!.Identity!.Name;
                            unicidade.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(unicidade);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma unicidade contratual.");
                        }

                        await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest("O contrato informado não foi encontrado.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir unicidade contratual.", erro = e.Message });
            }
        }


        #endregion

        #region Métodos privados
        private void PreencheUniccontr(ref EsF2500Uniccontr unicidade, EsF2500UniccontrRequestDTO requestDTO, int? codigoContrato = null)
        {

            if (codigoContrato.HasValue)
            {
                unicidade.IdEsF2500Infocontrato = codigoContrato.Value;
            }
            unicidade.UniccontrCodcateg = requestDTO.UniccontrCodcateg;
            unicidade.UniccontrDtinicio = requestDTO.UniccontrDtinicio;
            unicidade.UniccontrMatunic = requestDTO.UniccontrMatunic;

            unicidade.LogCodUsuario = User!.Identity!.Name;
            unicidade.LogDataOperacao = DateTime.Now;

        }

        private static EsF2500UniccontrDTO PreencheUniccontr(ref EsF2500Uniccontr? unicidade)
        {
            return new EsF2500UniccontrDTO()
            {
                IdEsF2500Uniccontr = unicidade!.IdEsF2500Uniccontr,
                IdEsF2500Infocontrato = unicidade!.IdEsF2500Infocontrato,
                LogCodUsuario = unicidade!.LogCodUsuario,
                LogDataOperacao = unicidade!.LogDataOperacao,
                UniccontrCodcateg = unicidade!.UniccontrCodcateg,
                UniccontrDtinicio = unicidade!.UniccontrDtinicio,
                UniccontrMatunic = unicidade!.UniccontrMatunic
            };
        }

        private IQueryable<EsF2500UniccontrDTO> RecuperaListadeUnicidade(int codigoContrato)
        {
            var query = from uni in _eSocialDbContext.EsF2500Uniccontr.AsNoTracking()
                        join cat in _eSocialDbContext.EsTabela01 on uni.UniccontrCodcateg equals cat.CodEsTabela01 into grouping
                        from cat in grouping.DefaultIfEmpty()
                        where uni.IdEsF2500Infocontrato == codigoContrato
                        select new EsF2500UniccontrDTO()
                        {
                            IdEsF2500Infocontrato = uni.IdEsF2500Infocontrato,
                            IdEsF2500Uniccontr = uni.IdEsF2500Uniccontr,
                            LogCodUsuario = uni.LogCodUsuario,
                            LogDataOperacao = uni.LogDataOperacao,
                            UniccontrCodcateg = uni.UniccontrCodcateg,
                            UniccontrDtinicio = uni.UniccontrDtinicio,
                            UniccontrMatunic = uni.UniccontrMatunic,
                            UniccontrDesccateg = uni.UniccontrCodcateg.HasValue ? $"{uni.UniccontrCodcateg} - {cat.DscEsTabela01}" : string.Empty
                        };

            return query;
        }

        #endregion

    }
}
