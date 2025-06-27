using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Cadastro.Controllers
{
    [Route("api/esocial/formulario/ESocialF2501")]
    [ApiController]
    public class ESocialF2501DedSuspController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501DedSuspController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/dedsusp/{codigoInfoValores}/{codigoDedSusp}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501DedSuspDTO>> ListaDedSusp2501dAsync([FromRoute] int codigoInfoValores,
                                                                                          [FromRoute] long codigoDedSusp,
                                                                                          [FromServices] ESocialF2501DedSuspService service,
                                                                                          [FromServices] ESocialF2501InfoValoresService serviceInfoValores,
                                                                                          CancellationToken ct)
        {
            try
            {
                var existeDedSusp = await serviceInfoValores.ExisteInfoValoresPorIdAsync(codigoInfoValores, ct);

                if (existeDedSusp)
                {
                    var dedSusp = await service.RetornaDedSuspPorIdAsync(codigoDedSusp, ct);
                    if (dedSusp is not null)
                    {
                        EsF2501DedSuspDTO benefPenDTO = service.PreencheDedSuspDTO(ref dedSusp);

                        return Ok(benefPenDTO);
                    }

                    return NotFound($"Nenhuma informação de deduções suspensas encontrada para o id: {codigoDedSusp} ");
                }

                return BadRequest($"Informações de valores não encontrada para o id: {codigoInfoValores} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de informações de deduções suspensas.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/dedsusp/{codigoInfoValores}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501DedSuspDTO>>> ListaDedSuspF2501dAsync([FromRoute] long codigoInfoValores,
                                                                                                              [FromServices] ESocialF2501DedSuspService service,
                                                                                                              [FromServices] ESocialF2501InfoValoresService serviceInfoValores,
                                                                                                              [FromQuery] int pagina,
                                                                                                              //[FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var exiteDedSusp = await serviceInfoValores.ExisteInfoValoresPorIdAsync(codigoInfoValores, ct);
                if (exiteDedSusp)
                {
                    IQueryable<EsF2501DedSuspDTO> listaBenefPen = service.RecuperaListaDedSusp(codigoInfoValores);


                    listaBenefPen = ascendente ? listaBenefPen.OrderBy(x => x.IdEsF2501Dedsusp) : listaBenefPen.OrderByDescending(x => x.IdEsF2501Dedsusp);

                    var total = await listaBenefPen.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501DedSuspDTO>
                    {
                        Total = total,
                        Lista = await listaBenefPen.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informações de Valores não encontrada para o id {codigoInfoValores} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de deduções suspensas .", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/dedsusp/{codigoFormulario}/{codigoInfoValores}/{codigoDedSusp}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraDedSuspF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfoValores,
                                                                    [FromRoute] int codigoDedSusp,
                                                                    [FromBody] EsF2501DedSuspRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501DedSuspService service,
                                                                    [FromServices] ESocialF2501InfoValoresService serviceInfoValores,
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
                var infoValores = await serviceInfoValores.RetornaInfoValoresPorIdAsync(codigoInfoValores, ct);

                var (dedSuspInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoValores != null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de deduções suspensas.");
                        }

                        if (await service.ExisteDedSuspAlteraAsync(codigoDedSusp, codigoInfoValores, requestDTO.DedsuspIndtpdeducao!.Value, ct))
                        {
                            return BadRequest("Tipo Dedução já existe.");
                        }

                        var benefPen = await service.RetornaDedSuspnEditavelPorIdAsync(codigoDedSusp, ct);

                        if (benefPen is not null)
                        {
                            service.PreencheDedSusp(ref benefPen, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaAlteracaoDedSusp(requestDTO, infoValores, codigoDedSusp).ToList());

                        dedSuspInvalido = listaErros.Count > 0;

                        if (dedSuspInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de deduções suspensas.");
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Informações de Valores não encontrado para o id: {codigoInfoValores} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informação de deduções suspensas.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/dedsusp/{codigoFormulario}/{codigoInfoValores}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraDedSuspF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfoValores,
                                                                        [FromBody] EsF2501DedSuspRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501DedSuspService service,
                                                                        [FromServices] ESocialF2501InfoValoresService serviceInfoValores,
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
                var infoValores = await serviceInfoValores.RetornaInfoValoresPorIdAsync(codigoInfoValores, ct);

                var (benefPenInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (infoValores != null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de deduções suspensas.");
                        }

                        if (await service.ExisteDedSuspAsync(codigoInfoValores, requestDTO.DedsuspIndtpdeducao!.Value, ct))
                        {
                            return BadRequest("Tipo Dedução já existe.");
                        }

                        if (await service.QuantidadeMaximaDeDedSuspExcedida(25, codigoInfoValores, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 25 registros de informações de deduções suspensas.");
                        };

                        EsF2501Dedsusp dedSusp = new EsF2501Dedsusp();

                        service.PreencheDedSusp(ref dedSusp, requestDTO, User, codigoInfoValores);

                        service.AdicionaDedSuspAoContexto(ref dedSusp);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de deduções suspensas.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaInclusaoDedSusp(requestDTO, infoValores).ToList());

                        benefPenInvalido = listaErros.Count > 0;

                        if (benefPenInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Informações de valores não encontradas para o id: {codigoInfoValores} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de informações de deduções suspensas.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/dedsusp/{codigoFormulario}/{codigoInfoValores}/{codigoDedSusp}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiDedSuspF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfoValores,
                                                              [FromRoute] long codigoDedSusp,
                                                              [FromServices] ESocialF2501DedSuspService service,
                                                              [FromServices] ESocialF2501InfoValoresService serviceInfoValores,
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
                var existeInfoValores = await serviceInfoValores.ExisteInfoValoresPorIdAsync(codigoInfoValores, ct);

                if (formulario is not null)
                {
                    if (existeInfoValores)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de deduções suspensas.");
                        }

                        var dedsusp = await service.RetornaDedSuspnEditavelPorIdAsync(codigoDedSusp, ct);

                        if (dedsusp is not null)
                        {
                            dedsusp.LogCodUsuario = User!.Identity!.Name;
                            dedsusp.LogDataOperacao = DateTime.Now;
                            service.RemoveDedSuspDoContexto(ref dedsusp, codigoDedSusp);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de deduções suspensas.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Informações de valores não encontradas para o id: {existeInfoValores} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir informação de deduções suspensas.", erro = e.Message });
            }
        }

        #endregion
    }
}
