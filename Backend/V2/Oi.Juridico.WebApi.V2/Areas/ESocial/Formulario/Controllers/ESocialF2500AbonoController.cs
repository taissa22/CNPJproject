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
    [Route("api/esocial/formulario/ESocialF2500")]
    [ApiController]
    public class ESocialF2500AbonoController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500AbonoController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/abono/{codigoContrato}/{codigoAbono}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500AbonoDTO>> ListaAbonoF2500dAsync([FromRoute] long codigoContrato, [FromRoute] long codigoAbono, [FromServices] ESocialF2500AbonoService service, [FromServices] ESocialF2500InfoContratoService contratoService, CancellationToken ct)
        {
            try
            {
                var existeContrato = await contratoService.ExisteContratoPorIdAsync(codigoContrato, ct);
                if (existeContrato)
                {
                    var abono = await service.RetornaAbonoPorIdAsync(codigoContrato, codigoAbono, ct);
                    if (abono is not null)
                    {
                        EsF2500AbonoDTO abonoDTO = service.PreencheAbonoDTO(ref abono);

                        return Ok(abonoDTO);
                    }

                    return NotFound($"Nenhuma informação de abono encontrado para o id: {codigoAbono} ");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do abono.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/abono/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500AbonoDTO>>> ListaAbonosF2500dAsync([FromRoute] long codigoContrato,
                                                                                                              [FromServices] ESocialF2500AbonoService service,
                                                                                                              [FromServices] ESocialF2500InfoContratoService contratoService,
                                                                                                              [FromQuery] int pagina,
                                                                                                              [FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var existeContrato = await contratoService.ExisteContratoPorIdAsync(codigoContrato, ct);
                if (existeContrato)
                {
                    IQueryable<EsF2500AbonoDTO> listaAbonos = service.RecuperaListaAbono(codigoContrato);


                    listaAbonos = ascendente ? listaAbonos.OrderBy(x => x.AbonoAnobase) : listaAbonos.OrderByDescending(x => x.AbonoAnobase);

                    var total = await listaAbonos.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500AbonoDTO>
                    {
                        Total = total,
                        Lista = await listaAbonos.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista abonos.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/abono/{codigoFormulario}/{codigoContrato}/{codigoAbono}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraAbonoF2500Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoContrato,
                                                                    [FromRoute] int codigoAbono,
                                                                    [FromBody] EsF2500AbonoRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2500AbonoService service,
                                                                    [FromServices] ESocialF2500InfoContratoService contratoService,
                                                                    [FromServices] ESocialF2500Service f2500Service,
                                                                    [FromServices] DBContextService dbContextService,
                                                                    CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOJ_VALORES))
                {
                    return BadRequest("O usuário não tem permissão para alterar o bloco J");
                }
                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var contrato = await contratoService.RetornaContratoPorIdAsync(codigoContrato, ct);

                var (abonoInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um abono.");
                        }

                        var abono = await service.RetornaAbonoUpdatPorIdAsync(codigoContrato, codigoAbono, ct);

                        if (abono is not null)
                        {
                            service.PreencheAbono(ref abono, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaAlteracaoAbono(requestDTO, codigoAbono, contrato).ToList());

                        abonoInvalido = listaErros.Count > 0;

                        if (abonoInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar um abono.");
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar abono.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/abono/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraAbonoF2500Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoContrato,
                                                                        [FromBody] EsF2500AbonoRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2500AbonoService service,
                                                                        [FromServices] ESocialF2500InfoContratoService contratoService,
                                                                        [FromServices] ESocialF2500Service f2500Service,
                                                                        [FromServices] DBContextService dbContextService,
                                                                        CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOJ_VALORES))
                {
                    return BadRequest("O usuário não tem permissão para alterar o bloco J");
                }

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var contrato = await contratoService.RetornaContratoPorIdAsync(codigoContrato, ct);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de mudança de abono.");
                        }

                        if (await service.QuantidadeMaximaDeAbonosExcedida(9, codigoContrato, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 9 registros de Abono.");
                        };

                        EsF2500Abono abono = new EsF2500Abono();

                        service.PreencheAbono(ref abono, requestDTO, User, codigoContrato);

                        service.AdicionaAbonoAoContexto(ref abono);

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de mudança de abono.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaInclusaoAbono(requestDTO, contrato).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

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

        [HttpDelete("exclusao/abono/{codigoFormulario}/{codigoContrato}/{codigoAbono}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiAbonoF2500Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoContrato,
                                                              [FromRoute] long codigoAbono,
                                                              [FromServices] ESocialF2500AbonoService service,
                                                              [FromServices] ESocialF2500InfoContratoService contratoService,
                                                              [FromServices] ESocialF2500Service f2500Service,
                                                              [FromServices] DBContextService dbContextService,
                                                              CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOJ_VALORES))
                {
                    return BadRequest("O usuário não tem permissão para alterar o bloco J");
                }

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var existeContrato = await contratoService.ExisteContratoPorIdAsync(codigoContrato, ct);

                if (formulario is not null)
                {
                    if (existeContrato)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um abono.");
                        }

                        var abono = await service.RetornaAbonoUpdatPorIdAsync(codigoContrato, codigoAbono, ct);

                        if (abono is not null)
                        {
                            abono.LogCodUsuario = User!.Identity!.Name;
                            abono.LogDataOperacao = DateTime.Now;
                            service.RemoveAbonoAoContexto(ref abono);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um abono.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir abono.", erro = e.Message });
            }
        }

        #endregion


    }
}
