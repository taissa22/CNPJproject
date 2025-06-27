using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/[controller]")]
    [ApiController]
    public partial class ESocialF2500Controller : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialF2500Controller(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }


        #region Consulta

        [HttpGet("consulta/f2500/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500DTO>> ConsultaFormulario2500PorIdAsync([FromRoute] int codigoFormulario, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {     
                var formulario2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {
                    var formularioDTO = service.PreencheFormulario2500DTO(ref formulario2500);

                    return Ok(formularioDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do formulário.", erro = e.Message });
            }
        }

        [HttpGet("consulta/f2500/header/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500HeaderDTO>> ConsultaFormulario2500HeaderPorIdAsync([FromRoute] int codigoFormulario, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                EsF2500HeaderDTO? formulario2500 = await service.ConsultaHeaderF2500(codigoFormulario, ct);

                if (formulario2500 is not null)
                {

                    return Ok(formulario2500);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do formulário.", erro = e.Message });
            }
        }        

        [HttpGet("consulta/f2500/finaliza-escritorio/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<bool?>> AlteraFormulario2500FinalizaEscritorioAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {                  
                    return Ok(formulario2500.FinalizadoEscritorio is null ? null : formulario2500.FinalizadoEscritorio == "S" ? true : false);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar escritório.", erro = e.Message });
            }
        }

        [HttpGet("consulta/f2500/finaliza-contador/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraFormulario2500FinalizaContadorAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {
                    return Ok(formulario2500.FinalizadoContador is null ? null : formulario2500.FinalizadoContador == "S" ? true : false);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar contador.", erro = e.Message });
            }
        }


        #endregion

        #region Edição

        [HttpPut("alteracao/{codigoFormulario}/{statusFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraFormulario2500Async([FromRoute] int codigoFormulario, [FromRoute] byte statusFormulario, [FromBody] EsF2500RequestDTO requestDTO, [FromServices] ESocialF2500Service service,[FromServices] ESocialF2500IdePeriodoService serviceIdePeriodo, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && statusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                {
                    return BadRequest("O usuário não tem permissão para enviar um formulário S-2500 para o eSocial");
                }
                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();
                var (formularioRascunhoInvalido, listaErrosRascunho) = requestDTO.ValidarRascunho();

                if (statusFormulario != EsocialStatusFormulario.Rascunho.ToByte() && statusFormulario != EsocialStatusFormulario.ProntoParaEnvio.ToByte())
                {
                    return BadRequest("Staus informado diferente dos status esperados.");
                }

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (formulario2500 is not null)
                {
                    var statusInicial = formulario2500!.StatusFormulario;

                    service.PreencheEntidadeFormulario2500(requestDTO, ref formulario2500, User, statusFormulario);
              
                    if (await service.FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    if (formularioRascunhoInvalido)
                    {
                        return BadRequest(listaErrosRascunho);
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    if (statusFormulario == EsocialStatusFormulario.ProntoParaEnvio.ToByte())
                    {
                        
                        var listaErros = listaErrosDTO.ToList();                       

                        listaErros.AddRange(await service.ValidaAlteracaoF2500(formulario2500!, ct));                        

                        listaErrosDTO = listaErros;

                        formularioInvalido = listaErrosDTO.Any();

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErrosDTO);
                        }

                        await serviceIdePeriodo.LimpaPeriodo(codigoFormulario,_eSocialDbContext, User, ct);

                        formulario2500!.StatusFormulario = statusFormulario;
                        formulario2500!.LogCodUsuario = User!.Identity!.Name;
                        formulario2500!.LogDataOperacao = DateTime.Now;

                        if (await service.FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                        {
                            return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                        }
                        await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);
                    }

                    return Ok("Formulário alterado com sucesso!");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar formulário.", erro = e.Message });
            }

        }

        [HttpPatch("cadastro/alteracao/f2500/status-rascunho/{codigoFormulario}")]
        public async Task<ActionResult> AlteraFormulario2500StatusRascunhoAsync([FromRoute] int codigoFormulario, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.CADASTRAR_FORM2500))
                {
                    return BadRequest("Usuário não autorizado!");
                }

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500!.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte())
                {
                    return BadRequest("Status não alterado. O status informado é igual ao atual.");
                }

                if (formulario2500 is not null)
                {
                    formulario2500.StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte();
                    formulario2500.LogCodUsuario = User!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                }

                if (await service.FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                {
                    return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                }

                await _eSocialDbContext.SaveChangesAsync(ct);

                return Ok("Status alterado com sucesso.");

            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao alterar formulário.", erro = e.Message });
            }
        }

        [HttpPatch("cadastro/alteracao/f2500/altera-versao/{codigoFormulario}")]
        public async Task<ActionResult> AlteraFormulario2500VersaoAsync([FromRoute] int codigoFormulario, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.CADASTRAR_FORM2500))
                {
                    return BadRequest("Usuário não autorizado!");
                }
                string parametroVersaoAtualEsocial = await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("VRS_ATUAL_ESOCIAL");

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {
                    formulario2500.VersaoEsocial = parametroVersaoAtualEsocial;
                    formulario2500.LogCodUsuario = User!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                }

                if (await service.FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                {
                    return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                }

                await _eSocialDbContext.SaveChangesAsync(ct);

                return Ok();

            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao alterar formulário.", erro = e.Message });
            }
        }

        [HttpPatch("cadastro/alteracao/f2500/retorna-rascunho/{codigoFormulario}")]
        [TemPermissao(Permissoes.ESOCIAL_RETORNA_STATUS_RASCUNHO)]
        public async Task<ActionResult> RetornaFormulario2500StatusRascunhoAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.CADASTRAR_FORM2500))
                {
                    return BadRequest("Usuário não autorizado!");
                }

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500!.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte())
                {
                    return BadRequest("Status não alterado. O status informado é igual ao atual.");
                }

                if (formulario2500 is not null)
                {
                    formulario2500.StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte();
                    formulario2500.LogCodUsuario = User!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                }
                
                await _eSocialDbContext.SaveChangesAsync(ct);

                return Ok("Status alterado com sucesso.");

            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao alterar formulário.", erro = e.Message });
            }
        }

        [HttpPost("cadastro/alteracao/f2500/finaliza-escritorio/{codigoFormulario}")]
        [TemPermissao(Permissoes.CADASTRAR_FORM2500)]
        public async Task<ActionResult> AlteraFormulario2500FinalizaEscritorioAsync([FromRoute] int codigoFormulario, [FromBody] bool? statusCancelamento, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_ESCRITORIO_FORM2500) && !_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && statusCancelamento.HasValue && statusCancelamento.Value)
                {
                    return BadRequest("O usuário não tem permissão para \"Finalizar Preenchimento Escritório\".");
                }

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {

                    formulario2500.LogCodUsuario = User!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                    formulario2500.FinalizadoEscritorio = !statusCancelamento.HasValue ? null : statusCancelamento.Value ? "S" : "N";

                    if (await service.FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Status alterado com sucesso.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar escritório.", erro = e.Message });
            }
        }

        [HttpPost("cadastro/alteracao/f2500/finaliza-contador/{codigoFormulario}")]
        [TemPermissao(Permissoes.CADASTRAR_FORM2500)]
        public async Task<ActionResult> AlteraFormulario2500FinalizaContadorAsync([FromRoute] int codigoFormulario, [FromBody] bool? statusCancelamento, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario2500 is not null)
                {
                    if (!_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_CONTADOR_FORM2500) && !_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && statusCancelamento.HasValue && statusCancelamento.Value)
                    {
                        return BadRequest("O usuário não tem permissão para \"Finalizar Preenchimento Contador\".");
                    }

                    formulario2500.LogCodUsuario = User!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                    formulario2500.FinalizadoContador = !statusCancelamento.HasValue ? null : statusCancelamento.Value ? "S" : "N";

                    if (await service.FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Status alterado com sucesso.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar contador.", erro = e.Message });
            }
        }

        [HttpPatch("cadastro/alteracao/f2500/atualiza-recibo/{codigoFormulario}")]
        [TemPermissao(Permissoes.ESOCIAL_ATUALIZAR_RECIBO)]
        public async Task<ActionResult> AlterarNumeroReciboAsync([FromRoute] int codigoFormulario, [FromBody] string? numeroRecibo, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                (bool alteradoComSucesso, string MensagemErro) = await service.AlteraNumeroReciboAsync(codigoFormulario, numeroRecibo, User, ct);

                if ( !alteradoComSucesso ) { return BadRequest(MensagemErro); }

                return Ok("Número do recibo alterado com sucesso.");

            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao alterar formulário.", erro = e.Message });
            }
        }

        #endregion

        #region Validação Formulário 2500

        [HttpPut("validacao/{codigoFormulario}/{statusFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidarFormulario2500Async([FromRoute] int codigoFormulario, [FromBody] EsF2500RequestDTO requestDTO, [FromServices] ESocialF2500Service service, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (formulario2500 is not null)
                {
                    service.PreencheEntidadeFormulario2500(requestDTO, ref formulario2500, User, EsocialStatusFormulario.Rascunho.ToByte());

                    if (await service.FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("Não é possível validar o formulário pois ele já foi processado.");
                    }   

                    var listaErros = listaErrosDTO.ToList();

                    listaErros.AddRange(await service.ValidaAlteracaoF2500(formulario2500!, ct));

                    listaErrosDTO = listaErros;

                    formularioInvalido = listaErrosDTO.Any();

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErrosDTO);
                    }

                    return Ok("Formulário validado com sucesso!");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao validar formulário.", erro = e.Message });
            }

        }


        #endregion

        #region Limpar formulário

        [HttpPut("limpar/{codigoFormulario}/{statusFormulario}")]
        [TemPermissao(Permissoes.LIMPAR_FORMULARIO_ESOCIAL)]
        public async Task<ActionResult> LimparFormulario2500Async([FromRoute] int codigoFormulario, [FromRoute] byte statusFormulario, 
                                                                  [FromBody] EsF2500RequestDTO requestDTO, 
                                                                  [FromServices] ESocialF2500Service service,
                                                                  [FromServices] ESocialPartesProcessoService partesProcessoService,
                                                                  CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();
                var (formularioRascunhoInvalido, listaErrosRascunho) = requestDTO.ValidarRascunho();

                if (statusFormulario != EsocialStatusFormulario.Rascunho.ToByte() && statusFormulario != EsocialStatusFormulario.ProntoParaEnvio.ToByte() && statusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte())
                {
                    return BadRequest("Staus informado diferente dos status esperados.");
                }

                var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (formulario2500 is not null)
                {
                    var statusInicial = formulario2500!.StatusFormulario;

                    var codigoInterno = formulario2500.CodProcesso;
                    var codigoParte = formulario2500.CodParte;
                    var loginUsuario = User.Identity!.Name;
                    var dataOperacao = DateTime.Now;

                    var esParteProcesso = await _eSocialDbContext.EsParteProcesso.FirstOrDefaultAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte, ct);

                    if (!byte.TryParse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ESOCIAL_TIPO_AMBIENTE"), out var eSocialCodigoTipoAmbiente))
                    {
                        return BadRequest("Parâmetro Tipo Ambiente não configurado.");
                    }

                    if (await service.FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    service.PreencheEntidadeFormulario2500(requestDTO, ref formulario2500, User, EsocialStatusFormulario.NaoIniciado.ToByte());

                    await service.LimpaFilhosFormulario2500(codigoFormulario, ct);

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);   

                    var dadosIdentificacaoFormulario = await partesProcessoService.RecuperaDadosIdentificacaoNovoFormulario(esParteProcesso!, eSocialCodigoTipoAmbiente, ct);

                    partesProcessoService.LimparFormularioF2500(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao, ref formulario2500!);                    

                    if (await service.FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Formulário limpo com sucesso!");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao limpar formulário.", erro = e.Message });
            }

        }

        #endregion

    }

}
