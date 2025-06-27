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
    public class ESocialF2501InfoProcRetController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501InfoProcRetController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/infoprocret/{codigoInfocrirrf}/{codigoInfoProcRet}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfoProcRetDTO>> ListaInfoProcRet2501dAsync([FromRoute] int codigoInfocrirrf, [FromRoute] long codigoInfoProcRet, [FromServices] ESocialF2501InfoProcRetService service, [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (infoCrIrrf != null)
                {
                    var infoProcRet = await service.RetornaInfoProcRetPorIdAsync(codigoInfoProcRet, ct);
                    if (infoProcRet is not null)
                    {
                        EsF2501InfoProcRetDTO infoProcRetDTO = service.PreencheInfoProcRetDTO(ref infoProcRet);

                        return Ok(infoProcRetDTO);
                    }

                    return NotFound($"Nenhuma informação de informação de processo de retenção de tributos encontrada para o id: {codigoInfoProcRet} ");
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id: {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de informações de processos de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/infoprocret/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501InfoProcRetDTO>>> ListaInfoProcRetF2501dAsync([FromRoute] int codigoInfocrirrf,
                                                                                                              [FromServices] ESocialF2501InfoProcRetService service,
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
                    IQueryable<EsF2501InfoProcRetDTO> listaPenALim = service.RecuperaListaInfoProcRet(codigoInfocrirrf);


                    listaPenALim = ascendente ? listaPenALim.OrderBy(x => x.IdEsF2501Infoprocret) : listaPenALim.OrderByDescending(x => x.IdEsF2501Infoprocret);

                    var total = await listaPenALim.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501InfoProcRetDTO>
                    {
                        Total = total,
                        Lista = await listaPenALim.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de processos de retenção de tributos.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/infoprocret/{codigoFormulario}/{codigoInfocrirrf}/{codigoInfoProcRet}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoProcRetF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfocrirrf,
                                                                    [FromRoute] int codigoInfoProcRet,
                                                                    [FromBody] EsF2501InfoProcRetRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501InfoProcRetService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma informação de processo de retenção de tributos.");
                        }

                        if (await service.ExisteInfoProcRetAlteraAsync(codigoInfocrirrf, requestDTO, codigoInfoProcRet, ct))
                        {
                            if (requestDTO.InfoprocretCodsusp.HasValue)
                            {
                                return BadRequest("O Tipo de Processo, Número Processo e Código Indicativo Suspensão já existem.");
                            }
                            else
                            {
                                return BadRequest("O Tipo de Processo e Número Processo Suspensão já existem.");
                            }
                        }

                        var infoProcRet = await service.RetornaInfoProcRetEditavelPorIdAsync(codigoInfoProcRet, ct);

                        if (infoProcRet is not null)
                        {
                            service.PreencheInfoProcRet(ref infoProcRet, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoInfoProcRet(requestDTO, infoCrIrrf).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma informação de processo de retenção de tributos.");
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
                return BadRequest(new { mensagem = "Falha ao alterar informação de processo de retenção de tributos.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/infoprocret/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoProcRetF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfocrirrf,
                                                                        [FromBody] EsF2501InfoProcRetRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501InfoProcRetService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de processos de retenção de tributos.");
                        }

                        if (await service.ExisteInfoProcRetAsync(codigoInfocrirrf, requestDTO, ct))
                        {
                            if (requestDTO.InfoprocretCodsusp.HasValue)
                            {
                                return BadRequest("O Tipo de Processo, Número do Processo e Código Indicativo Suspensão já existem.");
                            }
                            else
                            {
                                return BadRequest("O Tipo de Processo e Número do Processo Suspensão já existem.");
                            }
                        }

                        if (await service.QuantidadeMaximaDeInfoProcRetsExcedida(50, codigoInfocrirrf, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 50 registros de informações de processos de retenção de tributos.");
                        };

                        EsF2501Infoprocret infoProcRet = new EsF2501Infoprocret();

                        service.PreencheInfoProcRet(ref infoProcRet, requestDTO, User, codigoInfocrirrf);

                        service.AdicionaInfoProcRetAoContexto(ref infoProcRet);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de processos de retenção de tributos.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaAlteracaoInfoProcRet(requestDTO, infoCrIrrf).ToList());

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
                return BadRequest(new { mensagem = "Falha ao incluir informações de informações de processos de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/infoProcRet/{codigoFormulario}/{codigoInfocrirrf}/{codigoInfoProcRet}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiInfoProcRetF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfocrirrf,
                                                              [FromRoute] long codigoInfoProcRet,
                                                              [FromServices] ESocialF2501InfoProcRetService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de processo de retenção de tributos.");
                        }

                        var infoProcRet = await service.RetornaInfoProcRetEditavelPorIdAsync(codigoInfoProcRet, ct);

                        if (infoProcRet is not null)
                        {
                            infoProcRet.LogCodUsuario = User!.Identity!.Name;
                            infoProcRet.LogDataOperacao = DateTime.Now;
                            service.RemoveInfoProcRetDoContexto(ref infoProcRet, codigoInfoProcRet);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de processo de retenção de tributos.");
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
                return BadRequest(new { mensagem = "Falha ao excluir informação de processo de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion
    }
}
