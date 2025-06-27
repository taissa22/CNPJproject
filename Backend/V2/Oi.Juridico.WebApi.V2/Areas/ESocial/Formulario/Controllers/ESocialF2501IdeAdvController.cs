using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
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
    [Route("api/esocial/formulario/ESocialF2501")]
    [ApiController]
    public class ESocialF2501IdeAdvController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501IdeAdvController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/ideadv/{codigoInfocrirrf}/{codigoIdeAdv}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501IdeAdvDTO>> ListaIdeAdv2501dAsync([FromRoute] int codigoInfocrirrf, [FromRoute] long codigoIdeAdv, [FromServices] ESocialF2501IdeAdvService service, [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (infoCrIrrf != null)
                {
                    var ideAdv = await service.RetornaIdeAdvPorIdAsync(codigoInfocrirrf, codigoIdeAdv, ct);
                    if (ideAdv is not null)
                    {
                        EsF2501IdeAdvDTO ideAdvDTO = service.PreencheIdeAdvDTO(ref ideAdv);

                        return Ok(ideAdvDTO);
                    }

                    return NotFound($"Nenhuma informação de advogado encontrado para o id: {codigoIdeAdv} ");
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id: {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de informações de advogados.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/ideadv/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501IdeAdvDTO>>> ListaIdeAdvsF2501dAsync([FromRoute] int codigoInfocrirrf,
                                                                                                              [FromServices] ESocialF2501IdeAdvService service,
                                                                                                              [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                                                                              [FromQuery] int pagina,
                                                                                                              //[FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var existeContrato = await serviceIrrf.ExisteInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (existeContrato)
                {
                    IQueryable<EsF2501IdeAdvDTO> listaIdeAdv = service.RecuperaListaIdeAdv(codigoInfocrirrf);


                    listaIdeAdv = ascendente ? listaIdeAdv.OrderBy(x => x.IdEsF2501Ideadv) : listaIdeAdv.OrderByDescending(x => x.IdEsF2501Ideadv);

                    var total = await listaIdeAdv.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501IdeAdvDTO>
                    {
                        Total = total,
                        Lista = await listaIdeAdv.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de advogados.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/ideadv/{codigoFormulario}/{codigoInfocrirrf}/{codigoIdeAdv}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoAdvF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfocrirrf,
                                                                    [FromRoute] int codigoIdeAdv,
                                                                    [FromBody] EsF2501IdeAdvRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501IdeAdvService service,
                                                                    [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                                    [FromServices] ESocialF2501Service f2501Service,
                                                                    [FromServices] DBContextService dbContextService,
                                                                    CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos C, D, E");
                }
                var formulario = await f2501Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);

                var (infoAdvInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoCrIrrf != null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de advogado.");
                        }

                        if (await service.ExisteIdeAdvPorTpInscAlteraAsync(codigoInfocrirrf, requestDTO.IdeadvNrinsc!, requestDTO.IdeadvTpinsc!.Value, codigoIdeAdv, ct))
                        {
                            return BadRequest("O Tipo e Número de Inscrição já existem.");
                        }

                        var ideAdv = await service.RetornaIdeAdvEditavelPorIdAsync(codigoInfocrirrf, codigoIdeAdv, ct);

                        if (ideAdv is not null)
                        {
                            service.PreencheIdeAdv(ref ideAdv, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoIdeAdv(requestDTO, infoCrIrrf).ToList());

                        infoAdvInvalido = listaErros.Count > 0;

                        if (infoAdvInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de advogado.");
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Informação de imposto de renda retido na fonte não encontrado para o id: {codigoInfocrirrf} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar Informações de Advogados.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/ideadv/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoAdvF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfocrirrf,
                                                                        [FromBody] EsF2501IdeAdvRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501IdeAdvService service,
                                                                        [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                                        [FromServices] ESocialF2501Service f2501Service,
                                                                        [FromServices] DBContextService dbContextService,
                                                                        CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos C, D, E");
                }

                var formulario = await f2501Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoCrIrrf != null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de advogados.");
                        }

                        if (await service.ExisteIdeAdvPorTpInscAsync(codigoInfocrirrf, requestDTO.IdeadvNrinsc!, requestDTO.IdeadvTpinsc!.Value, ct))
                        {
                            return BadRequest("O Tipo e Número de Inscrição já existem.");
                        }

                        if (await service.QuantidadeMaximaDeIdeAdvsExcedida(99, codigoInfocrirrf, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de informações de advogados.");
                        };

                        EsF2501Ideadv ideAdv = new EsF2501Ideadv();

                        service.PreencheIdeAdv(ref ideAdv, requestDTO, User, codigoInfocrirrf);

                        service.AdicionaIdeAdvAoContexto(ref ideAdv);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de advogados.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaAlteracaoIdeAdv(requestDTO, infoCrIrrf).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Informações de imposto de renda retido na fonte não encontradas para o id: {codigoInfocrirrf} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de informações de advogados.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/infoadv/{codigoFormulario}/{codigoInfocrirrf}/{codigoIdeAdv}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiInfoAdvF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfocrirrf,
                                                              [FromRoute] long codigoIdeAdv,
                                                              [FromServices] ESocialF2501IdeAdvService service,
                                                              [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                              [FromServices] ESocialF2501Service f2501Service,
                                                              [FromServices] DBContextService dbContextService,
                                                              CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos C, D, E");
                }

                var formulario = await f2501Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var existeInfoCrIrrf = await serviceIrrf.ExisteInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);

                if (formulario is not null)
                {
                    if (existeInfoCrIrrf)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um abono.");
                        }

                        var ideAdv = await service.RetornaIdeAdvEditavelPorIdAsync(codigoInfocrirrf, codigoIdeAdv, ct);

                        if (ideAdv is not null)
                        {
                            ideAdv.LogCodUsuario = User!.Identity!.Name;
                            ideAdv.LogDataOperacao = DateTime.Now;
                            service.RemoveIdeAdvAoContexto(ref ideAdv);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de advogado.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Informações de imposta de renda retido na fonte não encontradas para o id: {existeInfoCrIrrf} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir Informação de advogado.", erro = e.Message });
            }
        }

        #endregion
    }
}
