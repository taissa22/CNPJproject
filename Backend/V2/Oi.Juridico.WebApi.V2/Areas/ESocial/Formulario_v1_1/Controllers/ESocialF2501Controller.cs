using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Enums.Functions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/[controller]")]
    [ApiController]
    public class ESocialF2501Controller : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2501Controller(ParametroJuridicoContext parametroJuridico, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridico;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta Formulário 2501

        [HttpGet("consulta/f2501/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501DTO>> ConsultaFormulario2501PorIdAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);


                if (formulario2501 is not null)
                {
                    var formularioDTO = PreencheFormulario2501DTO(ref formulario2501);

                    return Ok(formularioDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do formulário.", erro = e.Message });
            }
        }

        [HttpGet("consulta/f2501/header/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501HeaderDTO>> ConsultaFormulario2501HeaderPorIdAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {                          

                var formulario2501 = await (from es2501 in _eSocialDbContext.EsF2501.AsNoTracking()
                                            from pt in _eSocialDbContext.Parte.Where(x => x.CodParte == es2501.CodParte).Select(x => new { x.CodParte, x.CpfParte, x.NomParte })
                                            join p in _eSocialDbContext.Processo on es2501.CodProcesso equals p.CodProcesso
                                            join tv in _eSocialDbContext.TipoVara on p.CodTipoVara equals tv.CodTipoVara
                                            where es2501.IdF2501 == codigoFormulario
                                            select new EsF2501HeaderDTO()
                                            {
                                                CodProcesso = es2501.CodProcesso,
                                                NroProcessoCartorio = p.NroProcessoCartorio,
                                                NomeComarca = p.CodComarcaNavigation.NomComarca,
                                                NomeVara = $"{p.CodVara}ª VARA {tv.NomTipoVara}",
                                                UfVara = p.CodComarcaNavigation.CodEstado,
                                                IndAtivo = p.IndProcessoAtivo == "S" ? "ATIVO" : "INATIVO",
                                                NomeEmpresaGrupo = p.CodParteEmpresaNavigation.NomParte,
                                                IndProprioTerceiro = p.IndProprioTerceiro == "P" ? "PRÓPRIO" : "TERCEIRO",
                                                LogCodUsuario = es2501.LogCodUsuario,
                                                NomeUsuario = es2501.LogCodUsuarioNavigation.NomeUsuario,
                                                LogDataOperacao = es2501.LogDataOperacao,
                                                StatusFormulario = es2501.StatusFormulario,
                                                CodParte = pt.CodParte,
                                                NomeParte = pt.NomParte,
                                                CpfParte = pt.CpfParte,
                                                IdeeventoNrrecibo = es2501.IdeeventoNrrecibo,
                                                ExclusaoNrrecibo = es2501.ExclusaoNrrecibo
                                            }).FirstOrDefaultAsync(ct);

                if (formulario2501 is not null)
                {
                    return Ok(formulario2501);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do formulário.", erro = e.Message });
            }
        }

        [HttpGet("consulta/f2501/finaliza-escritorio/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<bool?>> AlteraFormulario2501FinalizaEscritorioAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario2501 is not null)
                {
                    return Ok(formulario2501.FinalizadoEscritorio is null ? null : formulario2501.FinalizadoEscritorio == "S" ? true : false);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar escritório.", erro = e.Message });
            }
        }

        [HttpGet("consulta/f2501/finaliza-contador/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraFormulario2501FinalizaContadorAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario2501 is not null)
                {
                    return Ok(formulario2501.FinalizadoContador is null ? null : formulario2501.FinalizadoContador == "S" ? true : false);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar contador.", erro = e.Message });
            }
        }

        #endregion

        #region Edição Formulário 2501

        [HttpPut("cadastro/alteracao/f2501/{codigoFormulario}/{statusFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraFormulario2501Async([FromRoute] int codigoFormulario, [FromRoute] byte statusFormulario, [FromBody] EsF2501RequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();
                var (formularioRascunhoInvalido, listaErrosRascunho) = requestDTO.ValidarRascunho();

                if (statusFormulario != EsocialStatusFormulario.Rascunho.ToByte() && statusFormulario != EsocialStatusFormulario.ProntoParaEnvio.ToByte())
                {
                    return BadRequest("Staus informado diferente dos status esperados.");
                }

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    var statusInicial = formulario2501!.StatusFormulario;

                    PreencheEntidadeFormulario2501(requestDTO, ref formulario2501);

                    if (await FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
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

                        if (!_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_2501_PARA_ESOCIAL))
                        {
                            return new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                        }

                        var listaErrosFilhos = FilhosInvalido(_eSocialDbContext, codigoFormulario);

                         var listaErrosTemp = listaErros.ToList();

                        listaErrosTemp.AddRange(listaErrosFilhos.ToList());

                        listaErros = listaErrosTemp;

                        formularioInvalido = listaErros.Any();

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        formulario2501!.StatusFormulario = statusFormulario;
                        formulario2501!.LogCodUsuario = User!.Identity!.Name;
                        formulario2501!.LogDataOperacao = DateTime.Now;

                        if (await FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                        {
                            return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                        }
                        await _eSocialDbContext.SaveChangesAsync(ct);
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

        [HttpPatch("cadastro/alteracao/f2501/status-rascunho/{codigoFormulario}")]
        public async Task<ActionResult> AlteraFormulario2501StatusRascunhoAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.CADASTRAR_FORM2501))
                {
                    return BadRequest("Usuário não autorizado!");
                }

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario2501!.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte())
                {
                    return BadRequest("Status não alterado. O status informado é igual ao atual.");
                }

                if (formulario2501 is not null)
                {
                    formulario2501.StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte();
                    formulario2501.LogCodUsuario = User!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                }

                if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
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

        [HttpPatch("cadastro/alteracao/f2501/altera-versao/{codigoFormulario}")]
        public async Task<ActionResult> AlteraFormulario2501VersaoAsync([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.CADASTRAR_FORM2501))
                {
                    return BadRequest("Usuário não autorizado!");
                }

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                string parametroVersaoAtualEsocial = await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("VRS_ATUAL_ESOCIAL");

                if (formulario2501 is not null)
                {
                    formulario2501.VersaoEsocial = parametroVersaoAtualEsocial;
                    formulario2501.LogCodUsuario = User!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                }

                if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
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


        [HttpPost("cadastro/alteracao/f2501/finaliza-escritorio/{codigoFormulario}")]
        [TemPermissao(Permissoes.CADASTRAR_FORM2501)]
        public async Task<ActionResult> AlteraFormulario2501FinalizaEscritorioAsync([FromRoute] int codigoFormulario, [FromBody] bool? statusCancelamento, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_ESCRITORIO_FORM2501) && !_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && statusCancelamento.HasValue && statusCancelamento.Value)
                {
                    return BadRequest("O usuário não tem permissão para \"Finalizar Preenchimento Escritório\".");
                }

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario2501 is not null)
                {

                    formulario2501.LogCodUsuario = User!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                    formulario2501.FinalizadoEscritorio = !statusCancelamento.HasValue ? null : statusCancelamento.Value ? "S" : "N";

                    if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
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

        [HttpPost("cadastro/alteracao/f2501/finaliza-contador/{codigoFormulario}")]
        [TemPermissao(Permissoes.CADASTRAR_FORM2501)]
        public async Task<ActionResult> AlteraFormulario2501FinalizaContadorAsync([FromRoute] int codigoFormulario, [FromBody] bool? statusCancelamento, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

                if (formulario2501 is not null)
                {
                    if (!_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_CONTADOR_FORM2501) && !_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && statusCancelamento.HasValue && statusCancelamento.Value)
                    {
                        return BadRequest("O usuário não tem permissão para \"Finalizar Preenchimento Contador\".");
                    }

                    formulario2501.LogCodUsuario = User!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                    formulario2501.FinalizadoContador = !statusCancelamento.HasValue ? null : statusCancelamento.Value ? "S" : "N";

                    if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
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


        [HttpPost("cadastro/alteracao/f2501/valida-dados/")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidaDadosFormulariosAsync([FromBody] EsF2501RequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                List<string> ListaErros = new List<string>();
                var listaFormularios2500 = await _eSocialDbContext.EsF2500.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == requestDTO.CodParte && x.CodProcesso == requestDTO.CodProcesso).ToListAsync(ct);

                if (listaFormularios2500.Any())
                {
                    var ultimoItem2500 = listaFormularios2500.MaxBy(x => x.IdF2500)!;

                    if (requestDTO.IdeempregadorNrinsc!.Replace(".","").Replace("-", "").Replace("/","") != ultimoItem2500.IdeempregadorNrinsc.Replace(".", "").Replace("-", "").Replace("/", ""))
                    {
                        ListaErros.Add("Tipo e Número de Inscrição do Empregador ou Contribuinte");
                    }

                    if (requestDTO.IdetrabCpftrab!.Replace(".", "").Replace("-", "") != ultimoItem2500.IdetrabCpftrab.Replace(".", "").Replace("-", ""))
                    {
                        ListaErros.Add("CPF do Trabalhador");
                    }

                    if (requestDTO.IdeprocNrproctrab != ultimoItem2500.InfoprocessoNrproctrab)
                    {
                        ListaErros.Add("Número do Processo/Demanda");
                    }

                }
                    return Ok(ListaErros);
            }
            catch (Exception e)
            {

                return BadRequest(new { mensagem = "Falha ao finalizar contador.", erro = e.Message });
            }
        }
        #endregion

        #region Limpar
        [HttpPut("cadastro/limpar/f2501/{codigoFormulario}/{statusFormulario}")]
        [TemPermissao(Permissoes.LIMPAR_FORMULARIO_ESOCIAL)]
        public async Task<ActionResult> LimparFormulario2501Async([FromRoute] int codigoFormulario, [FromRoute] byte statusFormulario, [FromServices] ESocialPartesProcessoService partesProcessoService, [FromBody] EsF2501RequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();
                var (formularioRascunhoInvalido, listaErrosRascunho) = requestDTO.ValidarRascunho();

                if (statusFormulario != EsocialStatusFormulario.Rascunho.ToByte() && statusFormulario != EsocialStatusFormulario.ProntoParaEnvio.ToByte() && statusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte())
                {
                    return BadRequest("Staus informado diferente dos status esperados.");
                }

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    //var statusInicial = formulario2501!.StatusFormulario;

                    var codigoInterno = formulario2501.CodProcesso;
                    var codigoParte = formulario2501.CodParte;
                    var loginUsuario = User.Identity!.Name;
                    var dataOperacao = DateTime.Now;

                    var esParteProcesso = await _eSocialDbContext.EsParteProcesso.FirstOrDefaultAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte, ct);


                    if (!byte.TryParse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ESOCIAL_TIPO_AMBIENTE"), out var eSocialCodigoTipoAmbiente))
                    {
                        return BadRequest("Parâmetro Tipo Ambiente não configurado.");
                    }

                    if (await FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
                    {
                        return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                    }

                    PreencheEntidadeFormulario2501(requestDTO, ref formulario2501);

                    await LimparFilhosFormularios2501(codigoFormulario, ct);

                    formulario2501!.StatusFormulario = EsocialStatusFormulario.NaoIniciado.ToByte();

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                    var dadosIdentificacaoFormulario = await partesProcessoService.RecuperaDadosIdentificacaoNovoFormulario(esParteProcesso!, eSocialCodigoTipoAmbiente, ct);

                    partesProcessoService.LimpaFormulario2501(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao, ref formulario2501!);                    

                    if (await FormularioProcessadoAsync(codigoFormulario, statusFormulario, ct))
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

        #region Validação Escritório

        [HttpPost("validacao/f2501/finalizar-escritorio/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidaFinalizacaoEscritorioF2501Async([FromRoute] int codigoFormulario, [FromBody] EsF2501RequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    var statusInicial = formulario2501!.StatusFormulario;

                    PreencheEntidadeFormulario2501(requestDTO, ref formulario2501);

                    if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("Não é possível validar o formulário pois ele já foi processado.");
                    }

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
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

        [HttpGet("validacao/f2501/finalizar-contador/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidaFinalizacaoContadorF2501Async([FromRoute] int codigoFormulario, CancellationToken ct)
        {
            try
            {
                var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (formulario2501 is not null)
                {
                    if (await FormularioProcessadoAsync(codigoFormulario, EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("Não é possível validar o formulário pois ele já foi processado.");
                    }

                    var listaErrosFilhos = FilhosInvalido(_eSocialDbContext, codigoFormulario);

                    if (listaErrosFilhos.Any())
                    {
                        return BadRequest(listaErrosFilhos);
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

        #region Métodos privados formulário 2501

        private async Task<bool> FormularioProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusFormulario();

            return (await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct));
        }

        private void PreencheEntidadeFormulario2501(EsF2501RequestDTO requestDTO, ref EsF2501? formulario2501)
        {
            formulario2501!.StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte();
            formulario2501!.LogCodUsuario = User!.Identity!.Name;
            formulario2501!.LogDataOperacao = DateTime.Now;

            formulario2501!.CodParte = requestDTO.CodParte;
            formulario2501!.CodProcesso = requestDTO.CodProcesso;


            formulario2501!.IdeempregadorNrinsc = requestDTO.IdeempregadorNrinsc is not null ? Regex.Replace(requestDTO.IdeempregadorNrinsc, "[^0-9]+", "") : requestDTO.IdeempregadorNrinsc;
            formulario2501!.IdeprocNrproctrab = requestDTO.IdeprocNrproctrab is not null ? Regex.Replace(requestDTO.IdeprocNrproctrab, "[^0-9]+", "") : requestDTO.IdeprocNrproctrab;
            formulario2501!.IdeprocObs = requestDTO.IdeprocObs;
            formulario2501!.IdeprocPerapurpgto = requestDTO.IdeprocPerapurpgto.HasValue ? requestDTO.IdeprocPerapurpgto.Value.ToString("yyyyMM") : null;
            formulario2501!.IdetrabCpftrab = requestDTO.IdetrabCpftrab is not null ? Regex.Replace(requestDTO.IdetrabCpftrab, "[^0-9]+", "") : requestDTO.IdetrabCpftrab;
        }

        private EsF2501DTO PreencheFormulario2501DTO(ref EsF2501? formulario2501)
        {
            var formularioDTO = new EsF2501DTO()
            {
                IdF2501 = formulario2501!.IdF2501,
                CodParte = formulario2501!.CodParte,
                CodProcesso = formulario2501!.CodProcesso,
                StatusFormulario = formulario2501!.StatusFormulario,
                LogCodUsuario = formulario2501!.LogCodUsuario,
                LogDataOperacao = formulario2501!.LogDataOperacao,
                IdeempregadorTpinsc = formulario2501!.IdeempregadorTpinsc,
                IdeeventoIndretif = formulario2501!.IdeeventoIndretif,
                IdeprocNrproctrab = formulario2501!.IdeprocNrproctrab,
                IdeprocObs = formulario2501!.IdeprocObs,
                IdeprocPerapurpgto = formulario2501!.IdeprocPerapurpgto,
                ParentIdF2501 = formulario2501!.ParentIdF2501,
                IdetrabCpftrab = formulario2501!.IdetrabCpftrab,
                IdeempregadorNrinsc = formulario2501!.IdeempregadorNrinsc,
                IdeeventoNrrecibo = formulario2501!.IdeeventoNrrecibo,
                ExclusaoNrrecibo = formulario2501!.ExclusaoNrrecibo
        };

            return formularioDTO;
        }

        private async Task RemoveFilhosFormularios2501(int codigoFormulario, CancellationToken ct)
        {
            var formulario2501Exclusao = await _eSocialDbContext.EsF2501.AsNoTracking().Where(x => x.IdF2501 == codigoFormulario).ToListAsync(ct);
            if (formulario2501Exclusao.Count > 0)
            {
                foreach (var formulario2501 in formulario2501Exclusao)
                {
                    if (formulario2501 is not null)
                    {
                        var listaFormulario2501CalcTribExclusao = await _eSocialDbContext.EsF2501Calctrib.Where(x => x.IdEsF2501 == formulario2501.IdF2501).ToListAsync(ct);
                        var listaFormulario2501InfocrirrfExclusao = await _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == formulario2501.IdF2501).ToListAsync(ct);


                        if (listaFormulario2501CalcTribExclusao.Any())
                        {
                            foreach (var calcTrib in listaFormulario2501CalcTribExclusao)
                            {
                                var listaFormulario2501InfocrcontribExclusao = await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == calcTrib.IdEsF2501Calctrib).ToListAsync(ct);

                                if (listaFormulario2501InfocrcontribExclusao.Any())
                                {
                                    foreach (var infoCrContrib in listaFormulario2501InfocrcontribExclusao)
                                    {
                                        _eSocialDbContext.Remove(infoCrContrib);
                                    }
                                }
                                _eSocialDbContext.Remove(calcTrib);
                            }
                        }

                        if (listaFormulario2501InfocrirrfExclusao.Any())
                        {
                            foreach (var infoCrIrrf in listaFormulario2501InfocrirrfExclusao)
                            {
                                _eSocialDbContext.Remove(infoCrIrrf);
                            }
                        }
                    }
                }
            }
        }

        private async Task LimparFilhosFormularios2501(int codigoFormulario, CancellationToken ct)
        {
            var formulario2501Exclusao = await _eSocialDbContext.EsF2501.AsNoTracking().Where(x => x.IdF2501 == codigoFormulario).ToListAsync(ct);
            if (formulario2501Exclusao.Count > 0)
            {
                foreach (var formulario2501 in formulario2501Exclusao)
                {
                    if (formulario2501 is not null)
                    {
                        var listaFormulario2501CalcTribExclusao = await _eSocialDbContext.EsF2501Calctrib.Where(x => x.IdEsF2501 == formulario2501.IdF2501).ToListAsync(ct);
                        var listaFormulario2501InfocrirrfExclusao = await _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == formulario2501.IdF2501).ToListAsync(ct);


                        if (listaFormulario2501CalcTribExclusao.Any())
                        {
                            foreach (var calcTrib in listaFormulario2501CalcTribExclusao)
                            {
                                var listaFormulario2501InfocrcontribExclusao = await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == calcTrib.IdEsF2501Calctrib).ToListAsync(ct);

                                if (listaFormulario2501InfocrcontribExclusao.Any())
                                {
                                    foreach (var infoCrContrib in listaFormulario2501InfocrcontribExclusao)
                                    {
                                        _eSocialDbContext.Remove(infoCrContrib);
                                    }
                                }
                                _eSocialDbContext.Remove(calcTrib);
                            }
                        }

                        if (listaFormulario2501InfocrirrfExclusao.Any())
                        {
                            foreach (var infoCrIrrf in listaFormulario2501InfocrirrfExclusao)
                            {
                                _eSocialDbContext.Remove(infoCrIrrf);
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> FilhosInvalido(ESocialDbContext eSocialDbContext, int codigoFormulario)
        {
            var mensagensErro = new List<string>();

            if (!eSocialDbContext.EsF2501Calctrib.Any(x => x.IdEsF2501 == codigoFormulario))
            {
                mensagensErro.Add("É necessário que o formulário tenha pelo menos um registro de Período e Base de Cálculo dos Tributos.");
            }

            return mensagensErro;
        }
        #endregion

    }
}
