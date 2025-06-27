using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialLogContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Enums.Functions;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Shared.Tools;
using Microsoft.OpenApi.Any;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Controllers
{
    [Route("api/esocial/v1/[controller]")]
    [ApiController]
    public class ESocialPartesProcessoController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ESocialLogDbContext _eSocialLogDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialPartesProcessoController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ESocialLogDbContext eSocialLogDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _eSocialLogDbContext = eSocialLogDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region endpoints

        #region Geral

        [HttpGet("consulta-processo/{codigoInterno}/{campoProcesso}/{criterioBuscaProcesso}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<ProcessoDTO>>> ObterESocialObterDadosProcesso([FromRoute] string codigoInterno, [FromRoute] string campoProcesso, [FromRoute] string criterioBuscaProcesso, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var loginUsuario = User.Identity!.Name;
                IQueryable<ProcessoDTO> queryResultado;

                if (campoProcesso == "CI")
                {
                    var query = from processo in _eSocialDbContext.Processo.AsNoTracking()
                                join tipovara in _eSocialDbContext.TipoVara on processo.CodTipoVara equals tipovara.CodTipoVara
                                where processo.CodProcesso == int.Parse(codigoInterno) && processo.CodTipoProcesso == (byte)TiposProcessos.Trabalhista
                                select new { processo, tipovara };

                    if (!await query.AnyAsync(ct))
                    {
                        return BadRequest("Processo trabalhista não existente com esse código!");
                    }

                    if (!await service.EscritorioEmpresaPodeAcessarAsync(query.Select(x => x.processo), User, ct))
                    {
                        return BadRequest("Acesso negado a esse processo!");
                    }

                    queryResultado = query.Select(p => new ProcessoDTO
                    {
                        CodProcesso = p.processo.CodProcesso,
                        NomeComarca = p.processo.CodComarcaNavigation.NomComarca,
                        NomeVara = $"{p.processo.CodVara}ª VARA {p.tipovara.NomTipoVara}",
                        NroProcessoCartorio = p.processo.NroProcessoCartorio,
                        UfVara = p.processo.CodComarcaNavigation.CodEstado,
                        IndAtivo = p.processo.IndProcessoAtivo == "S" ? "ATIVO" : "INATIVO",
                        NomeEmpresaGrupo = p.processo.CodParteEmpresaNavigation.NomParte,
                        IndProprioTerceiro = p.processo.IndProprioTerceiro == "P" ? "PRÓPRIO" : "TERCEIRO",
                    });
                }
                else
                {
                    if (criterioBuscaProcesso == "I")
                    {
                        codigoInterno = codigoInterno.FormataNumeroProcesso();
                        var query = from processo in _eSocialDbContext.Processo.AsNoTracking()
                                    join tipovara in _eSocialDbContext.TipoVara on processo.CodTipoVara equals tipovara.CodTipoVara
                                    where processo.NroProcessoCartorio == codigoInterno && processo.CodTipoProcesso == (byte)TiposProcessos.Trabalhista && processo.IndProcessoAtivo == "S"
                                    select new { processo, tipovara };

                        if (!await query.AnyAsync(ct))
                        {
                            return BadRequest("Processo trabalhista não existente com esse número!");
                        }

                        if (!await service.EscritorioEmpresaPodeAcessarAsync(query.Select(x => x.processo), User, ct))
                        {
                            return BadRequest("Acesso negado a esse processo!");
                        }

                        queryResultado = query.Select(p => new ProcessoDTO
                        {
                            CodProcesso = p.processo.CodProcesso,
                            NomeComarca = p.processo.CodComarcaNavigation.NomComarca,
                            NomeVara = $"{p.processo.CodVara}ª VARA {p.tipovara.NomTipoVara}",
                            NroProcessoCartorio = p.processo.NroProcessoCartorio,
                            UfVara = p.processo.CodComarcaNavigation.CodEstado,
                            IndAtivo = p.processo.IndProcessoAtivo == "S" ? "ATIVO" : "INATIVO",
                            NomeEmpresaGrupo = p.processo.CodParteEmpresaNavigation.NomParte,
                            IndProprioTerceiro = p.processo.IndProprioTerceiro == "P" ? "PRÓPRIO" : "TERCEIRO",
                        });
                    }
                    else
                    {
                        var query = from processo in _eSocialDbContext.Processo.AsNoTracking()
                                    join tipovara in _eSocialDbContext.TipoVara on processo.CodTipoVara equals tipovara.CodTipoVara
                                    where processo.NroProcessoCartorio.Replace("-","").Replace(".","").Contains(codigoInterno.RemoverCaracteres()) 
                                    && processo.CodTipoProcesso == (byte)TiposProcessos.Trabalhista
                                    && processo.IndProcessoAtivo == "S"
                                    select new { processo, tipovara };

                        if (!await query.AnyAsync(ct))
                        {
                            return BadRequest("Processo trabalhista não existente com esse número!");
                        }

                        if (!await service.EscritorioEmpresaPodeAcessarAsync(query.Select(x => x.processo), User, ct))
                        {
                            return BadRequest("Acesso negado a esse processo!");
                        }

                        queryResultado = query.Select(p => new ProcessoDTO
                        {
                            CodProcesso = p.processo.CodProcesso,
                            NomeComarca = p.processo.CodComarcaNavigation.NomComarca,
                            NomeVara = $"{p.processo.CodVara}ª VARA {p.tipovara.NomTipoVara}",
                            NroProcessoCartorio = p.processo.NroProcessoCartorio,
                            UfVara = p.processo.CodComarcaNavigation.CodEstado,
                            IndAtivo = p.processo.IndProcessoAtivo == "S" ? "ATIVO" : "INATIVO",
                            NomeEmpresaGrupo = p.processo.CodParteEmpresaNavigation.NomParte,
                            IndProprioTerceiro = p.processo.IndProprioTerceiro == "P" ? "PRÓPRIO" : "TERCEIRO",
                        });

                    }

                }

                return Ok(queryResultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("lista/{codigoInterno}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
#pragma warning disable CA1068 // CancellationToken parameters must come last
        public async Task<ActionResult<RetornoPaginadoDTO<ESocialPartesProcessoDTO>>> ObterPaginadoESocialPartesProcessoAsync(CancellationToken ct, [FromRoute] int codigoInterno,
#pragma warning restore CA1068 // CancellationToken parameters must come last
                                                                                                                        [FromServices] ESocialPartesProcessoService service,
                                                                                                                        [FromQuery] int? pagina = 1,
                                                                                                                        [FromQuery] int? quantidade = 8,
                                                                                                                        [FromQuery] int? statusReclamente = -1,
                                                                                                                        [FromQuery] int? statusFormulario = -1,
                                                                                                                        [FromQuery] string? filtro = "")
        {
            try
            {
                var loginUsuario = User.Identity!.Name;
                var sql = $"BEGIN  JUR.SP_INS_ES_PARTE_PROCESSO({codigoInterno}, '{loginUsuario}');  END;";
                await _eSocialDbContext.Database.ExecuteSqlRawAsync(sql);

                var queryParteProcesso = from espp in _eSocialDbContext.EsParteProcesso.AsNoTracking()
                                         where espp.CodProcesso == codigoInterno
                                         select espp;


                if (!await service.EscritorioEmpresaPodeAcessarAsync(queryParteProcesso.Select(x => x.CodProcessoNavigation), User, ct))
                {
                    return BadRequest("Acesso negado a esse processo!");
                }

                var queryResultado = queryParteProcesso.Select(espp => new ESocialPartesProcessoDTO
                {
                    CodParte = espp.CodParte,
                    CodProcesso = espp.CodProcesso,
                    NomeParte = espp.CodParteNavigation.NomParte,
                    CpfParte = espp.CodParteNavigation.CpfParte,
                    StatusReclamante = espp.StatusReclamante
                });

                queryResultado = statusReclamente > -1 ? queryResultado.Where(x => x.StatusReclamante == statusReclamente) : queryResultado;

                queryResultado = filtro != string.Empty ? filtro!.All(char.IsDigit) ? queryResultado = queryResultado.Where(x => x.CpfParte == filtro) : queryResultado.Where(x => x.NomeParte.ToLower().Contains(filtro!.ToLower())) : queryResultado;

                List<ESocialPartesProcessoDTO> listaParteProcesso = await service.ListaFormulariosParteProcesso(statusFormulario, queryResultado, ct);

                var total = listaParteProcesso.Count;

                var skip = PaginationHelper.PagesToSkip(quantidade!.Value, total, pagina!.Value);

                var resultado = new RetornoPaginadoDTO<ESocialPartesProcessoDTO>
                {
                    Total = total,
                    Lista = listaParteProcesso.Skip(skip).Take(quantidade!.Value)
                };

                return Ok(resultado);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("ORA-20001"))
                {

                    return BadRequest("Processo não encontrado.");
                }

                return BadRequest(e.Message);
            }
        }


        [HttpPost("altera-status/{codigoInterno}/{codigoParte}/{status}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlterarStatusParteProcessoAsync([FromRoute] int codigoInterno, [FromRoute] int codigoParte, [FromRoute] byte status, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                if (_controleDeAcessoService.TemPermissao(Permissoes.ALTERAR_STATUS_RECLAMANTE))
                {
                    var fomulariosIniciados = await service.ValidaStatusFormularios(codigoInterno, User, ct);


                    if (status < EsocialStatusReclamante.PendenteAnalise.ToByte() || status > EsocialStatusReclamante.ElegivelESocial.ToByte())
                    {
                        return BadRequest("Status inválido!");
                    }

                    var loginUsuario = User.Identity!.Name;
                    var dataOperacao = DateTime.Now;

                    var esParteProcesso = await _eSocialDbContext.EsParteProcesso.FirstOrDefaultAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte, ct);
                    if (esParteProcesso is null)
                    {
                        return NotFound($"Registro não encontrado. Processo: {codigoInterno}, Parte: {codigoParte}");
                    }

                    var processoFiltro = _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == codigoInterno);

                    if (!await service.EscritorioEmpresaPodeAcessarAsync(processoFiltro, User, ct))
                    {
                        return BadRequest("Acesso negado a esse processo!");
                    }

                    if (!byte.TryParse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ESOCIAL_TIPO_AMBIENTE"), out var eSocialCodigoTipoAmbiente))
                    {
                        return BadRequest("Parâmetro Tipo Ambiente não configurado.");
                    }

                    var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusReclamante();

                    if (esParteProcesso.StatusReclamante != status)
                    {
                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.CodParte == esParteProcesso.CodParte
                        && x.CodProcesso == esParteProcesso.CodProcesso
                        && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct))
                        {
                            return BadRequest($"Status não pode ser alterado pois o Processo: {codigoInterno}, Parte: {codigoParte} contém formulários tramitados.");
                        }
                    }

                    if ((esParteProcesso.StatusReclamante == EsocialStatusReclamante.PendenteAnalise.ToByte()
                        || esParteProcesso.StatusReclamante == EsocialStatusReclamante.NaoElegivelESocial.ToByte())
                        && status == EsocialStatusReclamante.ElegivelESocial.ToByte())
                    {

                        var dadosIdentificacaoFormulario = await service.RecuperaDadosIdentificacaoNovoFormulario(esParteProcesso!, eSocialCodigoTipoAmbiente, ct);

                        var formulario2500 = service.CriarNovoFormularioF2500(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao);

                        _eSocialDbContext.Add(formulario2500);
                    }

                    if (esParteProcesso.StatusReclamante == EsocialStatusReclamante.ElegivelESocial.ToByte() && (status == EsocialStatusReclamante.PendenteAnalise.ToByte()
                        || status == EsocialStatusReclamante.NaoElegivelESocial.ToByte()))
                    {
                        if (_controleDeAcessoService.TemPermissao(Permissoes.CANC_STATUS_ELEGI_RECLAMANTE))
                        {
                            await service.RemoveFormulario2500(esParteProcesso, ct);

                            await service.RemoveFormularios2501(esParteProcesso, ct);
                        }
                        else
                        {
                            return BadRequest("O usuário não tem permissão para retirar o status \"Elegível para o eSocial\"");
                        }

                    }

                    esParteProcesso.StatusReclamante = status;
                    esParteProcesso.LogCodUsuario = loginUsuario;
                    esParteProcesso.LogDataOperacao = dataOperacao;

                    await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                    return Ok("Status alterado com sucesso!");
                }
                else
                {
                    return BadRequest("O usuário não pode alterar o status de elegibilidade dos reclamante!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region 2500

        [HttpPost("novo/f2500/{codigoInterno}/{codigoParte}")]
        [TemPermissao(Permissoes.CRIAR_NOVO_2500)]
        public async Task<ActionResult> CriarNovoF2500Async([FromRoute] int codigoInterno, [FromRoute] int codigoParte, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var loginUsuario = User.Identity!.Name;
                var dataOperacao = DateTime.Now;

                var esParteProcesso = await _eSocialDbContext.EsParteProcesso.FirstOrDefaultAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte, ct);
                if (esParteProcesso is null)
                {
                    return NotFound($"Registro não encontrado. Processo: {codigoInterno}, Parte: {codigoParte}");
                }

                if (esParteProcesso.StatusReclamante != EsocialStatusReclamante.ElegivelESocial.ToByte())
                {
                    return BadRequest("o status do reclamante não permite criação de novo formulário 2500!");
                }

                var processoFiltro = _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == codigoInterno);

                if (!await service.EscritorioEmpresaPodeAcessarAsync(processoFiltro, User, ct))
                {
                    return BadRequest("Acesso negado a esse processo!");
                }

                if (!byte.TryParse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ESOCIAL_TIPO_AMBIENTE"), out var eSocialCodigoTipoAmbiente))
                {
                    return BadRequest("Parâmetro Tipo Ambiente não configurado.");
                }

                var formulario2500Ativo = await service.RetornaFormulario2500Ativo(codigoParte, codigoInterno, ct);

                if (formulario2500Ativo.StatusFormulario != EsocialStatusFormulario.Excluido3500.ToByte())
                {
                    return BadRequest("Cada parte do processo pode ter apenas um formulário 2500.");
                }

                var dadosIdentificacaoFormulario = await service.RecuperaDadosIdentificacaoNovoFormulario(esParteProcesso!, eSocialCodigoTipoAmbiente, ct);

                var formulario2500 = service.CriarNovoFormularioF2500(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao);

                _eSocialDbContext.Add(formulario2500);

                if (!await _eSocialDbContext.EsF2501.AnyAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte && x.StatusFormulario == EsocialStatusFormulario.NaoIniciado.ToByte()))
                {
                    var formulario2501 = service.AdicionaFormulario2501(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao);
                    _eSocialDbContext.Add(formulario2501);
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Formulário 2501 adicionado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("retificar/f2500/{codigoFormulario}")]
        [TemPermissao(Permissoes.RETIFICAR_FORM2500)]
        public async Task<ActionResult<RetornoRetificacaoDTO>> RetificarF2500Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var f2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (f2500 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                if (f2500.StatusFormulario != EsocialStatusFormulario.RetornoESocialOk.ToByte())
                {
                    return BadRequest("O status do formulário não permite retificação.");
                }

                var loginUsuario = User.Identity!.Name;
                var dataOperacao = DateTime.Now;

                var retornoRetificacao = await service.CriaNovoFormulario2500Retificacao(f2500, loginUsuario, dataOperacao, ct);

                await scope.CommitAsync(ct);

                return Ok(retornoRetificacao);
            }
            catch (Exception e)
            {
                await scope.RollbackAsync(ct);

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("remover/f2500/{codigoFormulario}")]
        [TemPermissao(Permissoes.RETIFICAR_FORM2500)]
        public async Task<ActionResult> RemoverF2500Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (f2500 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2500.StatusFormulario;

                var listaStatusFormularioNaoPermiteExclusao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemExclusaoFormulario();

                var formularioPermiteExclusao = !listaStatusFormularioNaoPermiteExclusao.Select(x => x.ToByte()).Contains(f2500.StatusFormulario)
                                                && f2500.IdeeventoIndretif == ESocialIndRetif.Retificacao.ToByte();

                if (!formularioPermiteExclusao)
                {
                    return BadRequest("O status do formulário não permite retificação.");
                }

                await service.RemoveFormulario2500(f2500.IdF2500, ct);

                if (await service.Formulario2500ProcessadoAsync(f2500.IdF2500, statusFormulario, ct))
                {
                    return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Formulário excluído com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("excluir/f2500/{codigoFormulario}")]
        [TemPermissao(Permissoes.EXCLUIR_FORM2500)]
        public async Task<ActionResult> ExcluirF2500Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2500 = await _eSocialDbContext.EsF2500.FirstAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (f2500 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2500.StatusFormulario;

                var formularioPermiteExclusao = f2500.StatusFormulario == EsocialStatusFormulario.RetornoESocialOk.ToByte();

                if (!formularioPermiteExclusao)
                {
                    return BadRequest("O status do formulário não permite solicitar exclusão.");
                }

                var listaStatusNaoPermiteSolicitacaoExclusaoS3500 = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermiteSolicitarExclusaoS3500();

                var listaFormulario2501Ativos = await service.ListaFormularios2501Ativos(f2500.CodParte, f2500.CodProcesso, ct);

                if (listaFormulario2501Ativos.Any(x => x.StatusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte() &&
                                                        x.StatusFormulario != EsocialStatusFormulario.Processando.ToByte() &&
                                                        x.StatusFormulario != EsocialStatusFormulario.Exclusao3500Solicitada.ToByte() &&
                                                        x.StatusFormulario != EsocialStatusFormulario.Exclusao3500Enviada.ToByte() &&
                                                        x.StatusFormulario != EsocialStatusFormulario.Excluido3500.ToByte()))
                {
                    return BadRequest("Antes de excluir um formulário 2500, você deve solicitar a exclusão 3500 de todos os formulários 2501 vinculados a ele.");
                }

                f2500.StatusFormulario = EsocialStatusFormulario.Exclusao3500Solicitada.ToByte();
                f2500.LogCodUsuario = loginUsuario;
                f2500.LogDataOperacao = DateTime.Now;

                if (await service.Formulario2500ExclusaoProcessadoAsync(f2500.IdF2500, statusFormulario, ct))
                {
                    return BadRequest("Operação não está mais disponível, pois a solicitação da exclusão 3500 já foi enviada ao eSocial.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Solicitação de Exclusão efetuada com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPatch("desfazer-exclusao/f2500/{codigoFormulario}")]
        [TemPermissao(Permissoes.EXCLUIR_FORM2500)]
        public async Task<ActionResult> DesfazerExclusaoF2500Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2500 = await _eSocialDbContext.EsF2500.FirstAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (f2500 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2500.StatusFormulario;

                var formularioPermiteDesfazerExclusao = f2500.StatusFormulario == EsocialStatusFormulario.Exclusao3500Solicitada.ToByte()
                    || f2500.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte();

                if (!formularioPermiteDesfazerExclusao)
                {
                    return BadRequest("Operação não está mais disponível, pois a solicitação da exclusão 3500 já foi enviada ao eSocial.");
                }

                f2500.StatusFormulario = EsocialStatusFormulario.RetornoESocialOk.ToByte();
                f2500.LogCodUsuario = loginUsuario;
                f2500.LogDataOperacao = DateTime.Now;

                if (await service.Formulario2500CancelarExclusaoProcessadoAsync(f2500.IdF2500, statusFormulario, ct))
                {
                    return BadRequest("Operação não está mais disponível, pois a solicitação da exclusão 3500 já foi enviada ao eSocial.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Cancelamento da solicitação de exclusão efetuada com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("exportar-retorno/f2500")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExportarF2500RetornoAsync([FromQuery] int codigoFormulario, [FromServices] ESocialDownloadRetornoService service, CancellationToken ct)
        {
            try
            {
                var responseService = await service.ExportarRetorno(_eSocialDbContext, codigoFormulario, true, ct);

                string inicioNomeArquivo = responseService.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte() ? "Retorno_FPW" : "Retorno_eSocial";

                return File(responseService.Dados!, "text/csv", $"{inicioNomeArquivo}_{responseService.InfoprocessoNrproctrab}_{responseService.CpfParte!.Replace(".", "").Replace("-", "")}_{responseService.TipoFormulario}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region 2501

        [HttpPost("retificar/f2501/{codigoFormulario}")]
        [TemPermissao(Permissoes.RETIFICAR_FORM2501)]
        public async Task<ActionResult<RetornoRetificacaoDTO>> RetificarF2501Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            using var scope = await _eSocialDbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var f2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (f2501 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                if (f2501.StatusFormulario != EsocialStatusFormulario.RetornoESocialOk.ToByte())
                {
                    return BadRequest("O status do formulário não permite retificação.");
                }


                var loginUsuario = User.Identity!.Name;
                var dataOperacao = DateTime.Now;

                var retornoRetificacao = await service.CriaNovoFormulario2501Retificacao(f2501, loginUsuario, dataOperacao, ct);

                await scope.CommitAsync(ct);

                return Ok(retornoRetificacao);
            }
            catch (Exception e)
            {
                await scope.RollbackAsync(ct);

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("remover/f2501/{codigoFormulario}")]
        [TemPermissao(Permissoes.REMOVER_FORM2501)]
        public async Task<ActionResult> RemoverF2501Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2501 = await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (f2501 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2501.StatusFormulario;

                var listaStatusFormularioNaoPermiteExclusao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemExclusaoFormulario();

                var formularioPermiteExclusao = !listaStatusFormularioNaoPermiteExclusao.Select(x => x.ToByte()).Contains(f2501.StatusFormulario)
                                                 || (f2501.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte() || f2501.StatusFormulario == EsocialStatusFormulario.ProntoParaEnvio.ToByte())
                                                    && f2501.IdeeventoIndretif == ESocialIndRetif.Retificacao.ToByte();

                if (!formularioPermiteExclusao)
                {
                    return BadRequest("O status do formulário não permite retificação.");
                }

                await service.RemoveFormulario2501(f2501.IdF2501, ct);

                if (await service.Formulario2501ProcessadoAsync(f2501.IdF2501, statusFormulario, ct))
                {
                    return BadRequest("Não é possível alterar o formulário pois ele já foi processado.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Formulário excluído com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("incluir/f2501/{codigoInterno}/{codigoParte}")]
        [TemPermissao(Permissoes.INCLUIR_FORM2501)]
        public async Task<ActionResult> IncluirF2501Async([FromRoute] int codigoInterno, [FromRoute] int codigoParte, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var loginUsuario = User.Identity!.Name;
                var dataOperacao = DateTime.Now;

                var esParteProcesso = await _eSocialDbContext.EsParteProcesso.FirstOrDefaultAsync(x => x.CodProcesso == codigoInterno && x.CodParte == codigoParte, ct);
                if (esParteProcesso is null)
                {
                    return NotFound($"Registro não encontrado. Processo: {codigoInterno}, Parte: {codigoParte}");
                }

                if (esParteProcesso.StatusReclamante != EsocialStatusReclamante.ElegivelESocial.ToByte())
                {
                    return BadRequest("o status do reclamante não permite inclusão de novo formulário 2501!");
                }

                var processoFiltro = _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == codigoInterno);

                if (!await service.EscritorioEmpresaPodeAcessarAsync(processoFiltro, User, ct))
                {
                    return BadRequest("Acesso negado a esse processo!");
                }

                if (!byte.TryParse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ESOCIAL_TIPO_AMBIENTE"), out var eSocialCodigoTipoAmbiente))
                {
                    return BadRequest("Parâmetro Tipo Ambiente não configurado.");
                }

                var dadosIdentificacaoFormulario = await service.RecuperaDadosIdentificacaoNovoFormulario(esParteProcesso!, eSocialCodigoTipoAmbiente, ct);

                var formulario2501 = service.AdicionaFormulario2501(dadosIdentificacaoFormulario, loginUsuario!, dataOperacao);

                _eSocialDbContext.Add(formulario2501);

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Formulário 2501 adicionado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("excluir/f2501/{codigoFormulario}")]
        [TemPermissao(Permissoes.EXCLUIR_FORM2501)]
        public async Task<ActionResult> ExcluirF2501Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2501 = await _eSocialDbContext.EsF2501.FirstAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (f2501 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2501.StatusFormulario;

                var formularioPermiteExclusao = f2501.StatusFormulario == EsocialStatusFormulario.RetornoESocialOk.ToByte();

                if (!formularioPermiteExclusao)
                {
                    return BadRequest("O status do formulário não permite solicitar exclusão.");
                }

                f2501.StatusFormulario = EsocialStatusFormulario.Exclusao3500Solicitada.ToByte();
                f2501.LogCodUsuario = loginUsuario;
                f2501.LogDataOperacao = DateTime.Now;

                if (await service.Formulario2501ExclusaoProcessadoAsync(f2501.IdF2501, statusFormulario, ct))
                {
                    return BadRequest("Operação não está mais disponível, pois a solicitação da exclusão 3500 já foi enviada ao eSocial.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Solicitação de Exclusão efetuada com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPatch("desfazer-exclusao/f2501/{codigoFormulario}")]
        [TemPermissao(Permissoes.EXCLUIR_FORM2501)]
        public async Task<ActionResult> DesfazerExclusaoF2501Async([FromRoute] int codigoFormulario, [FromServices] ESocialPartesProcessoService service, CancellationToken ct)
        {
            try
            {
                var f2501 = await _eSocialDbContext.EsF2501.FirstAsync(x => x.IdF2501 == codigoFormulario, ct);
                if (f2501 is null)
                {
                    return BadRequest("Formulário não encontrado");
                }

                var loginUsuario = User.Identity!.Name;
                var statusFormulario = f2501.StatusFormulario;

                var formulario2500Ativo = await service.RetornaFormulario2500Ativo(f2501.CodParte, f2501.CodProcesso, ct);

                if (formulario2500Ativo.StatusFormulario == EsocialStatusFormulario.Processando.ToByte() ||
                    formulario2500Ativo.StatusFormulario == EsocialStatusFormulario.Exclusao3500Solicitada.ToByte() ||
                    formulario2500Ativo.StatusFormulario == EsocialStatusFormulario.Exclusao3500Enviada.ToByte() ||
                    formulario2500Ativo.StatusFormulario == EsocialStatusFormulario.Excluido3500.ToByte() ||
                    formulario2500Ativo.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte())
                {
                    return BadRequest("Exclusão 3500 não pode ser desfeita em função do status atual do 2500. Favor verificar.");
                }

                var formularioPermiteDesfazerExclusao = f2501.StatusFormulario == EsocialStatusFormulario.Exclusao3500Solicitada.ToByte()
                    || f2501.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte();

                if (!formularioPermiteDesfazerExclusao)
                {
                    return BadRequest("O status do formulário não permite desfazer solicitação de exclusão.");
                }

                f2501.StatusFormulario = EsocialStatusFormulario.RetornoESocialOk.ToByte();
                f2501.LogCodUsuario = loginUsuario;
                f2501.LogDataOperacao = DateTime.Now;

                if (await service.Formulario2501CancelarExclusaoProcessadoAsync(f2501.IdF2501, statusFormulario, ct))
                {
                    return BadRequest("Operação não está mais disponível, pois a solicitação da exclusão 3500 já foi enviada ao eSocial.");
                }

                await _eSocialDbContext.SaveChangesAsync(loginUsuario, true, ct);

                return Ok("Cancelamento da solicitação de exclusão efetuada com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("exportar-retorno/f2501")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExportarF2501RetornoAsync([FromQuery] int codigoFormulario, [FromServices] ESocialDownloadRetornoService service, CancellationToken ct)
        {
            try
            {
                var responseService = await service.ExportarRetorno(_eSocialDbContext, codigoFormulario, false, ct);

                string inicioNomeArquivo = responseService.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte() ? "Retorno_FPW" : "Retorno_eSocial";

                return File(responseService.Dados!, "text/csv", $"{inicioNomeArquivo}_{responseService.InfoprocessoNrproctrab}_{responseService.CpfParte!.Replace(".", "").Replace("-", "")}_{responseService.TipoFormulario}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #endregion

    }
}
