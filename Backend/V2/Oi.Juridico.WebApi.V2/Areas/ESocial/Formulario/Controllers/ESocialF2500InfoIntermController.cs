using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.SequenceContext.Data;
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
    public class ESocialF2500InfoIntermController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;
        private readonly SequenceDbContext _sequenceDbContext;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500InfoIntermController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService, SequenceDbContext sequenceDbContext)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
            _sequenceDbContext = sequenceDbContext;
        }

        #region Consulta
        [HttpGet("consulta/infoInterm/{codigoIdeperiodo}/{codigoInfoInterm}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500InfoIntermDTO>> ListaInfoIntermF2500IdAsync([FromRoute] int codigoIdeperiodo, 
                                                                                            [FromRoute] long codigoInfoInterm, 
                                                                                            [FromServices] ESocialF2500InfoIntermService service, 
                                                                                            CancellationToken ct)
        {
            try
            {
                var infoInterm = await service.RetornaInfoIntermPorIdAsync(codigoIdeperiodo, codigoInfoInterm, ct);
                if (infoInterm is not null)
                {
                    EsF2500InfoIntermDTO infoIntermDTO = service.PreencheInfoIntermDTO(ref infoInterm);

                    return Ok(infoIntermDTO);
                }

                return BadRequest($"Período não encontrado para o id: {codigoIdeperiodo} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do trabalho intermitente.", erro = e.Message });
            }
        }

        [HttpGet("consulta/verifica/infoInterm/{codigoIdeperiodo}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<bool>> VerificaInfoIntermF2500IdAsync([FromRoute] int codigoIdeperiodo,
                                                                                           [FromRoute] int codigoContrato,
                                                                                           [FromServices] ESocialF2500InfoIntermService service,
                                                                                           CancellationToken ct)
        {
            try
            {
                var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);
                var existeInfoInterm = true;
                if (periodo is not null)
                {
                    if (contrato?.InfocontrIndcontr == "N"
                            && contrato?.InfocontrCodcateg == 111)
                    {
                        existeInfoInterm = await service.ExisteInfoIntermPorPeriodoAsync(periodo.IdEsF2500Ideperiodo, ct);
                    }

                    return Ok(existeInfoInterm);
                }

                return BadRequest($"Período não encontrado para o id: {codigoIdeperiodo} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do trabalho intermitente.", erro = e.Message });
            }
        }
        #endregion

        #region Lista paginado
        [HttpGet("lista/infoInterm/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<RetornoPaginadoDTO<EsF2500InfoIntermDTO>>> ListaInfoIntermF2500dAsync([FromRoute] int codigoIdeperiodo,
                                                                                                               [FromQuery] int pagina,
                                                                                                               //[FromQuery] string coluna,
                                                                                                               //[FromQuery] bool ascendente,
                                                                                                               [FromServices] ESocialF2500InfoIntermService service,
                                                                                                               CancellationToken ct)
        {
            try
            {
                var periodo = await _eSocialDbContext.EsF2500Ideperiodo.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                if (periodo is not null)
                {
                    IQueryable<EsF2500InfoIntermDTO> listaInfoInterm = service.RecuperaListaInfoInterm(codigoIdeperiodo);

                    var total = await listaInfoInterm.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500InfoIntermDTO>
                    {
                        Total = total,
                        Lista = await listaInfoInterm.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Período não encontrado para o id: {codigoIdeperiodo} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista dos trabalhos intermitente.", erro = e.Message });
            }
        }
        #endregion

        #region Alteração 
        [HttpPut("alteracao/infoInterm/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}/{codigoInfoInterm}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoIntermF2500Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoContrato,
                                                                    [FromRoute] int codigoIdeperiodo,
                                                                    [FromRoute] long codigoInfoInterm,
                                                                    [FromBody] EsF2500InfoIntermRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2500Service f2500Service,
                                                                    [FromServices] ESocialF2500IdePeriodoService servicePeriodo,
                                                                    [FromServices] ESocialF2500InfoIntermService service,
                                                                    [FromServices] DBContextService dbContextService,
                                                                    CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                if (formulario is not null)
                {
                    if (periodo is not null)
                    {
                        if (contrato?.InfocontrIndcontr != "N"
                            || contrato?.InfocontrCodcateg != 111)
                        {
                            var dtPeriodo = new DateTime(int.Parse(periodo.IdeperiodoPerref.Substring(0, 4)), int.Parse(periodo.IdeperiodoPerref.Substring(5, 2)), 1);
                            return BadRequest($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for diferente de \"Não\" e a Categoria informada (Bloco D) for diferente de \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" não deve ser preenchido para o periodo {dtPeriodo.ToString("MM/yyyy")}.");

                        }
                        var diasPeriodo = DateTime.DaysInMonth(int.Parse(periodo.IdeperiodoPerref.Substring(0, 4)), int.Parse(periodo.IdeperiodoPerref.Substring(5, 2)));

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar Trabalho intermitente.");
                        }

                        if (await service.QuantidadeMaximaDeInfoIntermExcedida(diasPeriodo, codigoIdeperiodo, ct))
                        {
                            return BadRequest($"O sistema só permite a inclusão de até {diasPeriodo} registros de trabalho intermitente para o periodo {periodo.IdeperiodoPerref}.");
                        }

                        if (await service.VerificaDiaUpdate(requestDTO.InfointermDia, codigoIdeperiodo, requestDTO.IdEsF2500Infointerm, ct))
                        {
                            return BadRequest($"Este dia já foi informado para este período!");
                        }

                        var infoInterm = await service.RetornaInfoIntermUpdatPorIdAsync(codigoIdeperiodo, codigoInfoInterm, ct);

                        if (infoInterm is not null)
                        {
                            service.PreencheInfoInterm(ref infoInterm, requestDTO, User);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar Trabalho intermitente.");
                        }

                        var listaErrosTemp = listaErros.ToList();

                        listaErrosTemp.AddRange(service.ValidaAlteracaoInfoInterm(requestDTO, codigoInfoInterm).ToList());

                        listaErros = listaErrosTemp;

                        formularioInvalido = listaErros.Any();

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await dbContextService.SalvaAlteracoesAsync(ct); //_eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }
                    return BadRequest($"Período não encontrado para o id: {codigoIdeperiodo} ");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar trabalho intermitente.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusao 
        [HttpPost("inclusao/infoInterm/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoIntermF2500Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoContrato,
                                                                    [FromRoute] int codigoIdeperiodo,
                                                                    [FromBody] EsF2500InfoIntermRequestDTO requestDTO,
                                                                    [FromServices] ESocialF2500Service f2500Service,
                                                                    [FromServices] ESocialF2500InfoIntermService service,
                                                                    [FromServices] DBContextService dbContextService,
                                                                        CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                if (formulario is not null)
                {
                    if (periodo is not null)
                    {
                        if (contrato?.InfocontrIndcontr != "N"
                            || contrato?.InfocontrCodcateg != 111)
                        {
                            var dtPeriodo = new DateTime(int.Parse(periodo.IdeperiodoPerref.Substring(0, 4)), int.Parse(periodo.IdeperiodoPerref.Substring(5, 2)), 1);
                            return BadRequest($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for diferente de \"Não\" e a Categoria informada (Bloco D) for diferente de \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" não deve ser preenchido para o periodo {dtPeriodo.ToString("MM/yyyy")}.");

                        }

                        var diasPeriodo = DateTime.DaysInMonth(int.Parse(periodo.IdeperiodoPerref.Substring(0, 4)), int.Parse(periodo.IdeperiodoPerref.Substring(5, 2)));

                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir Trabalho intermitente.");
                        }

                        if (await service.QuantidadeMaximaDeInfoIntermExcedida(diasPeriodo, codigoIdeperiodo, ct))
                        {
                            return BadRequest($"O sistema só permite a inclusão de até {diasPeriodo} registros de trabalho intermitente para o periodo.");
                        }

                        if (await service.VerificaDia(requestDTO.InfointermDia, codigoIdeperiodo, ct))
                        {
                            return BadRequest($"Este dia já foi informado para este período!");
                        }

                        var infoInterm = new EsF2500Infointerm();

                        service.PreencheInfoInterm(ref infoInterm, requestDTO, User, codigoIdeperiodo);

                        service.AdicionaInfoIntermAoContexto(ref infoInterm);

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir Trabalho intermitente.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        listaErros.AddRange(service.ValidaInclusaoInfoInterm(requestDTO).ToList());

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
                return BadRequest(new { mensagem = "Falha ao incluir informações de trabalho intermitente.", erro = e.Message });
            }
        }

        [HttpPost("inclusao/zerado/infoInterm/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoIntermZeradoF2500Async([FromRoute] int codigoFormulario,
                                                                   [FromRoute] int codigoContrato,
                                                                   [FromRoute] int codigoIdeperiodo,
                                                                   [FromServices] ESocialF2500Service f2500Service,
                                                                   [FromServices] ESocialF2500InfoIntermService service,
                                                                       CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var periodo = await _eSocialDbContext.EsF2500Ideperiodo.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);


                if (formulario is not null)
                {
                    if (periodo is not null)
                    {
                        if (contrato?.InfocontrIndcontr == "N"
                            && contrato?.InfocontrCodcateg == 111)
                        {
                            if (!await service.ExisteInfoIntermPorPeriodoAsync(periodo.IdEsF2500Ideperiodo, ct))
                            {
                                service.InsereInfoIntermZerado(User, ct, periodo.IdEsF2500Ideperiodo);
                            }
                        }

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de trabalho intermitente.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão
        [HttpDelete("exclusao/infoInterm/{codigoFormulario}/{codigoContrato}/{codigoIdeperiodo}/{codigoInfoInterm}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiInfoIntermF2500Async([FromRoute] int codigoFormulario,
                                                                    [FromRoute] int codigoContrato,
                                                                    [FromRoute] int codigoIdeperiodo,
                                                                    [FromRoute] long codigoInfoInterm,
                                                                    [FromServices] ESocialF2500Service f2500Service,
                                                                    [FromServices] ESocialF2500InfoIntermService service,
                                                                    [FromServices] DBContextService dbContextService,
                                                              CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOGK))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos G e K");
                }

                var formulario = await f2500Service.RetornaFormularioPorIdAsync(codigoFormulario, ct);
                var existePeriodo = await _eSocialDbContext.EsF2500Ideperiodo.AnyAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Ideperiodo == codigoIdeperiodo, ct);

                if (formulario is not null)
                {
                    if (existePeriodo)
                    {
                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de trabalho intermitente.");
                        }

                        var infoInterm = await service.RetornaInfoIntermPorIdAsync(codigoIdeperiodo, codigoInfoInterm, ct);

                        if (infoInterm is not null)
                        {
                            infoInterm.LogCodUsuario = User!.Identity!.Name;
                            infoInterm.LogDataOperacao = DateTime.Now;
                            service.RemoveInfoIntermAoContexto(ref infoInterm);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (!await f2500Service.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir um trabalho intermitente.");
                        }

                        await dbContextService.SalvaAlteracoesRegistraLogAsync(User.Identity!.Name!, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Periodo não encontrado para o id: {codigoIdeperiodo} ");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir trabalho intermitente.", erro = e.Message });
            }
        }
        #endregion



    }
}
