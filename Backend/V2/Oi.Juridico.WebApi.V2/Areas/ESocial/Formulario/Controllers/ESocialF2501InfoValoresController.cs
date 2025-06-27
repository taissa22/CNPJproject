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
    public class ESocialF2501InfoValoresController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501InfoValoresController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/infovalores/{codigoInfoProcRet}/{codigoInfoValores}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfoValoresDTO>> ListaInfoValores2501dAsync([FromRoute] int codigoInfoProcRet,
                                                                                          [FromRoute] long codigoInfoValores,
                                                                                          [FromServices] ESocialF2501InfoValoresService service,
                                                                                          [FromServices] ESocialF2501InfoProcRetService serviceInfoProcRet,
                                                                                          CancellationToken ct)
        {
            try
            {
                var existeInfoProcRet = await serviceInfoProcRet.ExisteInfoProcRetPorIdAsync(codigoInfoProcRet, ct);

                if (existeInfoProcRet)
                {
                    var infoValores = await service.RetornaInfoValoresPorIdAsync(codigoInfoValores, ct);
                    if (infoValores is not null)
                    {
                        EsF2501InfoValoresDTO infoValoresDTO = service.PreencheInfoValoresDTO(ref infoValores);

                        return Ok(infoValoresDTO);
                    }

                    return NotFound($"Nenhuma informação de valores de retenção de tributos encontrada para o id: {codigoInfoValores} ");
                }

                return BadRequest($"Informações de processos de retenção de tributos não encontrada para o id: {codigoInfoProcRet} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de informações de valores de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/infovalores/{codigoInfoProcRet}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501InfoValoresDTO>>> ListaInfoValoresF2501dAsync([FromRoute] long codigoInfoProcRet,
                                                                                                              [FromServices] ESocialF2501InfoValoresService service,
                                                                                                              [FromServices] ESocialF2501InfoProcRetService serviceInfoProcRet,
                                                                                                              [FromQuery] int pagina,
                                                                                                              //[FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var existeInfoProcRet = await serviceInfoProcRet.ExisteInfoProcRetPorIdAsync(codigoInfoProcRet, ct);
                if (existeInfoProcRet)
                {
                    IQueryable<EsF2501InfoValoresDTO> listaInfoValores = service.RecuperaListaInfoValores(codigoInfoProcRet);


                    listaInfoValores = ascendente ? listaInfoValores.OrderBy(x => x.IdEsF2501Infovalores) : listaInfoValores.OrderByDescending(x => x.IdEsF2501Infovalores);

                    var total = await listaInfoValores.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501InfoValoresDTO>
                    {
                        Total = total,
                        Lista = await listaInfoValores.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informações de processos de retenção de tributos não encontrada para o id {codigoInfoProcRet} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de valores de retenção de tributos.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/infovalores/{codigoFormulario}/{codigoInfoProcRet}/{codigoInfoValores}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoValoresF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfoProcRet,
                                                                    [FromRoute] int codigoInfoValores,
                                                                    [FromBody] EsF2501InfoValoresRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501InfoValoresService service,
                                                                    [FromServices] ESocialF2501InfoProcRetService infoProcRetService,
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
                var infoProcRet = await infoProcRetService.RetornaInfoProcRetPorIdAsync(codigoInfoProcRet, ct);

                var (infoValorInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoProcRet != null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma informação de valores de retenção de tributos.");
                        }

                        if (await service.ExisteInfoValoresAlteraAsync(codigoInfoProcRet, requestDTO.InfovaloresIndapuracao!.Value, codigoInfoValores, ct))
                        {
                            return BadRequest("O Periodo Apuração já existe.");
                        }

                        var infoValores = await service.RetornaInfoValoresEditavelPorIdAsync(codigoInfoValores, ct);

                        if (infoValores is not null)
                        {
                            service.PreencheInfoValores(ref infoValores, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoInfoValores(requestDTO, infoProcRet).ToList());

                        infoValorInvalido = listaErros.Count > 0;

                        if (infoValorInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma informação de valores de retenção de tributos.");
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Informações de processos de retenção de tributos não encontrado para o id: {codigoInfoProcRet} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informação de valores de retenção de tributos.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/infovalores/{codigoFormulario}/{codigoInfoProcRet}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoValoresF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfoProcRet,
                                                                        [FromBody] EsF2501InfoValoresRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501InfoValoresService service,
                                                                        [FromServices] ESocialF2501InfoProcRetService serviceInfoProcRet,
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
                var infoProcRet = await serviceInfoProcRet.RetornaInfoProcRetPorIdAsync(codigoInfoProcRet, ct);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoProcRet != null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de valores de retenção de tributos.");
                        }

                        if (await service.ExisteInfoValoresAsync(codigoInfoProcRet, requestDTO.InfovaloresIndapuracao!.Value, ct))
                        {
                            return BadRequest("O Periodo Apuração já existe.");
                        }

                        if (await service.QuantidadeMaximaDeInfoValoressExcedida(2, codigoInfoProcRet, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 2 registros de informações de valores de retenção de tributos.");
                        };

                        EsF2501Infovalores infoValores = new EsF2501Infovalores();

                        service.PreencheInfoValores(ref infoValores, requestDTO, User, codigoInfoProcRet);

                        service.AdicionaInfoValoresAoContexto(ref infoValores);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de valores de retenção de tributos.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaAlteracaoInfoValores(requestDTO, infoProcRet).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Informações de processos de retenção de tributos não encontradas para o id: {codigoInfoProcRet} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de informações de valores de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/infoValores/{codigoFormulario}/{codigoInfoProcRet}/{codigoInfoValores}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiInfoValoresF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfoProcRet,
                                                              [FromRoute] long codigoInfoValores,
                                                              [FromServices] ESocialF2501InfoValoresService service,
                                                              [FromServices] ESocialF2501InfoProcRetService serviceInfoProcRet,
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
                var existeInfoProcRet = await serviceInfoProcRet.ExisteInfoProcRetPorIdAsync(codigoInfoProcRet, ct);

                if (formulario is not null)
                {
                    if (existeInfoProcRet)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de valores de retenção de tributos.");
                        }

                        var infoValores = await service.RetornaInfoValoresEditavelPorIdAsync(codigoInfoValores, ct);

                        if (infoValores is not null)
                        {
                            infoValores.LogCodUsuario = User!.Identity!.Name;
                            infoValores.LogDataOperacao = DateTime.Now;
                            service.RemoveInfoValoresDoContexto(ref infoValores, codigoInfoValores);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de valores de retenção de tributos.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Informações de imposta de renda retido na fonte não encontradas para o id: {existeInfoProcRet} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir informação de valores de retenção de tributos.", erro = e.Message });
            }
        }

        #endregion
    }
}
