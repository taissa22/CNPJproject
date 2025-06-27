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
    public class ESocialF2501PenAlimController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501PenAlimController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/penalim/{codigoInfocrirrf}/{codigoPenAlim}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501PenAlimDTO>> ListaPenAlim2501dAsync([FromRoute] int codigoInfocrirrf, [FromRoute] long codigoPenAlim, [FromServices] ESocialF2501PenAlimService service, [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf, CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await serviceIrrf.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (infoCrIrrf != null)
                {
                    var penAlim = await service.RetornaPenAlimPorIdAsync(codigoInfocrirrf, codigoPenAlim, ct);
                    if (penAlim is not null)
                    {
                        EsF2501PenAlimDTO penAlimDTO = service.PreenchePenAlimDTO(ref penAlim);

                        return Ok(penAlimDTO);
                    }

                    return NotFound($"Nenhuma informação de penção alimentícia encontrado para o id: {codigoPenAlim} ");
                }

                return BadRequest($"Informação de Imposto de renda retido na fonte não encontrada para o id: {codigoInfocrirrf} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de penções alimentícias.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/penalim/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501PenAlimDTO>>> ListaPenAlimF2501dAsync([FromRoute] int codigoInfocrirrf,
                                                                                                              [FromServices] ESocialF2501PenAlimService service,
                                                                                                              [FromServices] ESocialF2501InfoCrIrrfService serviceIrrf,
                                                                                                              [FromQuery] int pagina,
                                                                                                              [FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var existeContrato = await serviceIrrf.ExisteInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (existeContrato)
                {
                    IQueryable<EsF2501PenAlimDTO> listaPenALim = service.RecuperaListaPenAlim(codigoInfocrirrf);


                    listaPenALim = ascendente ? listaPenALim.OrderBy(x => x.IdEsF2501Penalim) : listaPenALim.OrderByDescending(x => x.IdEsF2501Penalim);

                    var total = await listaPenALim.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501PenAlimDTO>
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
                return BadRequest(new { mensagem = "Falha ao recuperar lista de penções alimentícias.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/penalim/{codigoFormulario}/{codigoInfocrirrf}/{codigoPenAlim}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraPenAlimF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoInfocrirrf,
                                                                    [FromRoute] int codigoPenAlim,
                                                                    [FromBody] EsF2501PenAlimRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501PenAlimService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma penção alimentícia.");
                        }

                        if (await service.ExistePenAlimPorTpCpfAlteraAsync(codigoInfocrirrf, requestDTO.PenalimCpfdep!, requestDTO.PenalimTprend!.Value, codigoPenAlim, ct))
                        {
                            return BadRequest("O Tipo Rendimento e o CPF Beneficiário já existem.");
                        }

                        var penAlim = await service.RetornaPenAlimEditavelPorIdAsync(codigoInfocrirrf, codigoPenAlim, ct);

                        if (penAlim is not null)
                        {
                            service.PreenchePenAlim(ref penAlim, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoPenAlim(requestDTO, infoCrIrrf).ToList());

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de uma penção alimentícia.");
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
                return BadRequest(new { mensagem = "Falha ao alterar Penções Alimentícias.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/penalim/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraPenAlimF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoInfocrirrf,
                                                                        [FromBody] EsF2501PenAlimRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501PenAlimService service,
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
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir penções alimentícias.");
                        }

                        if (await service.ExistePenAlimPorTpCpfAsync(codigoInfocrirrf, requestDTO.PenalimCpfdep!, requestDTO.PenalimTprend!.Value, ct))
                        {
                            return BadRequest("O Tipo Rendimento e o CPF Beneficiário já existem.");
                        }

                        if (await service.QuantidadeMaximaDePenAlimsExcedida(99, codigoInfocrirrf, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de penções alimentícias.");
                        };

                        EsF2501Penalim penAlim = new EsF2501Penalim();

                        service.PreenchePenAlim(ref penAlim, requestDTO, User, codigoInfocrirrf);

                        service.AdicionaPenAlimAoContexto(ref penAlim);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir penções alimentícias.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaAlteracaoPenAlim(requestDTO, infoCrIrrf).ToList());

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
                return BadRequest(new { mensagem = "Falha ao incluir informações de penções alimentícias.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/penAlim/{codigoFormulario}/{codigoInfocrirrf}/{codigoPenAlim}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiPenAlimF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoInfocrirrf,
                                                              [FromRoute] long codigoPenAlim,
                                                              [FromServices] ESocialF2501PenAlimService service,
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

                        var penAlim = await service.RetornaPenAlimEditavelPorIdAsync(codigoInfocrirrf, codigoPenAlim, ct);

                        if (penAlim is not null)
                        {
                            penAlim.LogCodUsuario = User!.Identity!.Name;
                            penAlim.LogDataOperacao = DateTime.Now;
                            service.RemovePenAlimDoContexto(ref penAlim);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma penção alimentícia.");
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
                return BadRequest(new { mensagem = "Falha ao excluir Pensão Alimentícia.", erro = e.Message });
            }
        }

        #endregion
    }
}
