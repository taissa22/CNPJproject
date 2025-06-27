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
    public class ESocialF2501BefPenController : ControllerBase
    {
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501BefPenController(ControleDeAcessoService controleDeAcessoService)
        {
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/benefpen/{codigoDedSusp}/{codigoBenefPen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501BenefPenDTO>> ListaBenefPen2501dAsync([FromRoute] int codigoDedSusp,
                                                                                          [FromRoute] long codigoBenefPen,
                                                                                          [FromServices] ESocialF2501BenefPenService service,
                                                                                          [FromServices] ESocialF2501DedSuspService serviceDedSusp,
                                                                                          CancellationToken ct)
        {
            try
            {
                var existeDedSusp = await serviceDedSusp.ExisteDedSuspPorIdAsync(codigoDedSusp, ct);

                if (existeDedSusp)
                {
                    var benefPen = await service.RetornaBenefPenPorIdAsync(codigoBenefPen, ct);
                    if (benefPen is not null)
                    {
                        EsF2501BenefPenDTO benefPenDTO = service.PreencheBenefPenDTO(ref benefPen);

                        return Ok(benefPenDTO);
                    }

                    return NotFound($"Nenhuma informação de deduções suspensas encontrada para o id: {codigoBenefPen} ");
                }

                return BadRequest($"Informações de deduções suspensas não encontrada para o id: {codigoDedSusp} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de informações de deduções suspensas de beneficiários da pensão alimentícia.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/benefpen/{codigoDedSusp}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2501BenefPenDTO>>> ListaBenefPenF2501dAsync([FromRoute] long codigoDedSusp,
                                                                                                              [FromServices] ESocialF2501BenefPenService service,
                                                                                                              [FromServices] ESocialF2501DedSuspService serviceDedSusp,
                                                                                                              [FromQuery] int pagina,
                                                                                                              //[FromQuery] string coluna,
                                                                                                              [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var exiteDedSusp = await serviceDedSusp.ExisteDedSuspPorIdAsync(codigoDedSusp, ct);
                if (exiteDedSusp)
                {
                    IQueryable<EsF2501BenefPenDTO> listaBenefPen = service.RecuperaListaBenefPen(codigoDedSusp);


                    listaBenefPen = ascendente ? listaBenefPen.OrderBy(x => x.IdEsF2501Benefpen) : listaBenefPen.OrderByDescending(x => x.IdEsF2501Benefpen);

                    var total = await listaBenefPen.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2501BenefPenDTO>
                    {
                        Total = total,
                        Lista = await listaBenefPen.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Informações de deduções suspensas não encontrada para o id {codigoDedSusp} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de informações de deduções suspensas de beneficiários da pensão alimentícia.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 

        [HttpPut("alteracao/benefpen/{codigoFormulario}/{codigoDedSusp}/{codigoBenefPen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraBenefPenF2501Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoDedSusp,
                                                                    [FromRoute] int codigoBenefPen,
                                                                    [FromBody] EsF2501BenefPenRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2501BenefPenService service,
                                                                    [FromServices] ESocialF2501DedSuspService serviceDedSusp,
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
                var dedSusp = await serviceDedSusp.RetornaDedSuspPorIdAsync(codigoDedSusp, ct);

                var (benefPenInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (dedSusp != null)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        if (await service.ExisteBenefPenAlteraAsync(codigoBenefPen, codigoDedSusp, requestDTO.BenefpenCpfdep!, ct))
                        {
                            return BadRequest("CPF Dependente/Beneficiário já existe.");
                        }

                        var benefPen = await service.RetornaBenefPenEditavelPorIdAsync(codigoBenefPen, ct);

                        if (benefPen is not null)
                        {
                            service.PreencheBenefPen(ref benefPen, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: implementar validações
                        listaErros.AddRange(service.ValidaInclusaoBenefPen(requestDTO, dedSusp).ToList());

                        benefPenInvalido = listaErros.Count > 0;

                        if (benefPenInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar os dados de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Informações de deduções suspensas não encontrado para o id: {codigoDedSusp} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informação de deduções suspensas de beneficiários da pensão alimentícia.", erro = e.Message });
            }
        }
        #endregion

        #region Inclusao 

        [HttpPost("inclusao/benefpen/{codigoFormulario}/{codigoDedSusp}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraBenefPenF2501Async([FromRoute] int codigoFormulario,
                                                                        [FromRoute] int codigoDedSusp,
                                                                        [FromBody] EsF2501BenefPenRequestDTO requestDTO,
                                                                        [FromServices] ESocialF2501BenefPenService service,
                                                                        [FromServices] ESocialF2501DedSuspService serviceDedSusp,
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
                var dedSusp = await serviceDedSusp.RetornaDedSuspPorIdAsync(codigoDedSusp, ct);

                var (benefPenInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (dedSusp != null)
                    {

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        if (await service.ExisteBenefPenAsync(codigoDedSusp, requestDTO.BenefpenCpfdep!, ct))
                        {
                            return BadRequest("CPF Dependente/Beneficiário já existe.");
                        }

                        if (await service.QuantidadeMaximaDeBenefPensExcedida(99, codigoDedSusp, ct))
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 registros de informações de deduções suspensas de beneficiários da pensão alimentícia.");
                        };

                        EsF2501Benefpen benefPen = new EsF2501Benefpen();

                        service.PreencheBenefPen(ref benefPen, requestDTO, User, codigoDedSusp);

                        service.AdicionaBenefPenAoContexto(ref benefPen);

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        //TODO: Adicionar Validações
                        listaErros.AddRange(service.ValidaAlteracaoBenefPen(requestDTO, dedSusp).ToList());

                        benefPenInvalido = listaErros.Count > 0;

                        if (benefPenInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Informações de deduções suspensas não encontradas para o id: {codigoDedSusp} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de informações de deduções suspensas de beneficiários da pensão alimentícia.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/benefPen/{codigoFormulario}/{codigoDedSusp}/{codigoBenefPen}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiBenefPenF2501Async([FromRoute] int codigoFormulario,
                                                              [FromRoute] int codigoDedSusp,
                                                              [FromRoute] long codigoBenefPen,
                                                              [FromServices] ESocialF2501BenefPenService service,
                                                              [FromServices] ESocialF2501DedSuspService serviceDedSusp,
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
                var existeDedSusp = await serviceDedSusp.ExisteDedSuspPorIdAsync(codigoDedSusp, ct);

                if (formulario is not null)
                {
                    if (existeDedSusp)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        var benefPen = await service.RetornaBenefPenEditavelPorIdAsync(codigoBenefPen, ct);

                        if (benefPen is not null)
                        {
                            benefPen.LogCodUsuario = User!.Identity!.Name;
                            benefPen.LogDataOperacao = DateTime.Now;
                            service.RemoveBenefPenDoContexto(ref benefPen);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2501Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma informação de deduções suspensas de beneficiários da pensão alimentícia.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Informações de deduções suspensas não encontradas para o id: {existeDedSusp} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir informação de deduções suspensas de beneficiários da pensão alimentícia.", erro = e.Message });
            }
        }

        #endregion
    }
}
