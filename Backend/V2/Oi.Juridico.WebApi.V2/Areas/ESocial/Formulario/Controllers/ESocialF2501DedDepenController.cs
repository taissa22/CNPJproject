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
    public class ESocialF2501DedDepenController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501DedDepenController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/deddepen/{codigoInfocrirrf}/{codigoDedDepen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501DedDepenDTO>> ListaIdeAdv2501dAsync([FromRoute] int codigoInfocrirrf, [FromRoute] long codigoDedDepen, [FromServices] ESocialF2501DedDepenService service, [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (infoCrIrrf != null)
                {
                    var dedDepen = await service.RetornaDedDepenPorIdAsync(codigoInfocrirrf, codigoDedDepen, ct);
                    if (dedDepen is not null)
                    {
                        EsF2501DedDepenDTO ideAdvDTO = service.PreencheDedDepenDTO(ref dedDepen);

                        return Ok(ideAdvDTO);
                    }

                    return NotFound($"Nenhuma informação de advogado encontrado para o id: {codigoDedDepen} ");
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id: {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de deduções de dependentes.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/deddepen/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501DedDepenDTO>>> ListaDedDepenF2501dAsync([FromRoute] int codigoInfocrirrf,
                                                                                                              [FromServices] ESocialF2501DedDepenService service,
                                                                                                              [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                                                                              [FromQuery] int pagina,
                                                                                                              [FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var existeInfoCrIrrf = await serviceIrrf.ExisteInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (existeInfoCrIrrf)
                {
                    IQueryable<EsF2501DedDepenDTO> listaDedDepen = service.RecuperaListaDedDepen(codigoInfocrirrf);


                    listaDedDepen = ascendente ? listaDedDepen.OrderBy(x => x.IdEsF2501Deddepen) : listaDedDepen.OrderByDescending(x => x.IdEsF2501Deddepen);

                    var total = await listaDedDepen.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501DedDepenDTO>
                    {
                        Total = total,
                        Lista = await listaDedDepen.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de deduções de dependentes.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/deddepen/{codigoFormulario}/{codigoInfocrirrf}/{codigoDedDepen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraDedDepenF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfocrirrf,
                                                                    [FromRoute] int codigoDedDepen,
                                                                    [FromBody] EsF2501DedDepenRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501DedDepenService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma dedução de dependente.");
                        }

                        if (await service.ExisteDedDepenPorTipoCpfAlteraAsync(codigoInfocrirrf, requestDTO.DeddepenCpfdep!, requestDTO.DeddepenTprend!.Value, codigoDedDepen, ct))
                        {
                            return BadRequest("O Tipo Rendimento e o CPF Dependente já existem.");
                        }

                        var dedDepen = await service.RetornaDedDepenEditavelPorIdAsync(codigoInfocrirrf, codigoDedDepen, ct);

                        if (dedDepen is not null)
                        {
                            service.PreencheDedDepen(ref dedDepen, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoDedDepen(requestDTO, infoCrIrrf).ToList());

                        infoAdvInvalido = listaErros.Count > 0;

                        if (infoAdvInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma dedução de dependente.");
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
                return BadRequest(new { mensagem = "Falha ao alterar deduções de dependentes.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/deddepen/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraDedDepenF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfocrirrf,
                                                                        [FromBody] EsF2501DedDepenRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501DedDepenService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma dedução de dependente.");
                        }

                        if (await service.ExisteDedDepenPorTipoCpfAsync(codigoInfocrirrf, requestDTO.DeddepenCpfdep!, requestDTO.DeddepenTprend!.Value, ct))
                        {
                            return BadRequest("O Tipo Rendimento e o CPF Dependente já existem.");
                        }

                        if (await service.QuantidadeMaximaDeDedDepensExcedida(999, codigoInfocrirrf, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 999 registros de deduções de dependentes.");
                        };

                        EsF2501Deddepen dedDepen = new EsF2501Deddepen();

                        service.PreencheDedDepen(ref dedDepen, requestDTO, User, codigoInfocrirrf);

                        service.AdicionaDedDepenAoContexto(ref dedDepen);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir uma dedução de dependente.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaInclusaoDedDepen(requestDTO, infoCrIrrf).ToList());

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
                return BadRequest(new { mensagem = "Falha ao incluir informações de deduções de dependentes.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/deddepen/{codigoFormulario}/{codigoInfocrirrf}/{codigoDedDepen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiDedDepenF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfocrirrf,
                                                              [FromRoute] long codigoDedDepen,
                                                              [FromServices] ESocialF2501DedDepenService service,
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

                if (formulario is not null)
                {
                    if (infoCrIrrf != null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir  uma dedução de dependente.");
                        }

                        var dedDepen = await service.RetornaDedDepenEditavelPorIdAsync(codigoInfocrirrf, codigoDedDepen, ct);

                        if (dedDepen is not null)
                        {
                            dedDepen.LogCodUsuario = User!.Identity!.Name;
                            dedDepen.LogDataOperacao = DateTime.Now;
                            service.RemoveDedDepenAoContexto(ref dedDepen);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir  uma dedução de dependente.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Informações de imposta de renda retido na fonte não encontradas para o id: {infoCrIrrf} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir deduções de dependentes.", erro = e.Message });
            }
        }

        #endregion
    }
}
