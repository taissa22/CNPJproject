using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Contextos.V2.SequenceContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using System.Drawing;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2500")]
    [ApiController]
    public class ESocialF2500InfoContratoController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;
        private readonly SequenceDbContext _sequenceDbContext;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500InfoContratoController(ParametroJuridicoContext parametroJuridicoDbContext, ControleDeAcessoService controleDeAcessoService, ESocialDbContext eSocialDbContext, SequenceDbContext sequenceDbContext)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
            _sequenceDbContext = sequenceDbContext;
        }

        #region Consulta

        [HttpGet("consulta/info-contrato/{codigoFormulario}/{codigoInfoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500InfocontratoDTO>> ListaInfoContratosF2500dAsync([FromRoute] int codigoFormulario, [FromRoute] long codigoInfoContrato, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct);
                if (infoContrato is not null)
                {
                    EsF2500InfocontratoDTO infoContratoDTO = PreencheInfoContratoDTO(ref infoContrato);
                    var result = await _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == formulario2500!.CodProcesso).Select(y => y.IndProprioTerceiro).FirstOrDefaultAsync(ct);                               
                    infoContratoDTO.IndProprioTerceiro = result!;
                    return Ok(infoContratoDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do dependente.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/info-contrato/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500InfocontratoDTO>>> ListaInfoContratosF2500dAsync([FromRoute] int codigoFormulario,
                                                                                                         [FromQuery] int pagina,
                                                                                                         [FromQuery] string coluna,
                                                                                                         [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (formulario2500 is not null)
                {
                    IQueryable<EsF2500InfocontratoDTO> listaInfoContratos = RecuperaListaContrato(codigoFormulario, formulario2500.CodProcesso);

                    switch (coluna.ToLower())
                    {
                        case "id":
                        default:
                            listaInfoContratos = ascendente ? listaInfoContratos.OrderBy(x => x.IdEsF2500Infocontrato) : listaInfoContratos.OrderByDescending(x => x.IdEsF2500Infocontrato);
                            break;
                    }

                    var total = await listaInfoContratos.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500InfocontratoDTO>
                    {
                        Total = total,
                        Lista = await listaInfoContratos.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista das informações do cotrato.", erro = e.Message });
            }
        }

        #endregion

        #region Alteração 

        [HttpPut("alteracao/info-contrato/{codigoFormulario}/{codigoInfoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoContratoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoInfoContrato, [FromBody] EsF2500InfocontratoRequestDTO requestDTO, [FromServices] ESocialF2500InfoContratoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }                    

                    var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct);

                    if (infoContrato is not null)
                    {
                        PreencheInfoContrato(ref infoContrato, requestDTO, formulario);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }

                    var listaErrosTemp = listaErrosDTO.ToList();

                    if (requestDTO.InfocontrDtadmorig.HasValue && (await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato != infoContrato.IdEsF2500Infocontrato && (x.InfocontrDtadmorig.HasValue && x.InfocontrDtadmorig.Value.Date == requestDTO.InfocontrDtadmorig!.Value.Date), ct)))
                    {
                        listaErrosTemp.Add("Já existe um registro com a \"Data de Admissão Original\" informada.");
                    }

                    //Comentado para depois reimplementar de forma mais 
                    //if (await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => !string.IsNullOrEmpty(requestDTO.InfocontrMatricula) && x.InfocontrMatricula == requestDTO.InfocontrMatricula && x.IdF2500Navigation.CodProcesso != formulario.CodProcesso && x.IdF2500Navigation.CodParte != formulario.CodParte, ct))
                    //{
                    //    listaErrosTemp.Add("Matrícula informada já cadastrada.");
                    //}

                    var listaErros = listaErrosTemp.ToList();

                    listaErros.AddRange((await service.ValidaAlteracaoInfoContrato(_eSocialDbContext, requestDTO, infoContrato, formulario, ct)).ToList());

                    formularioInvalido = listaErros.Count > 0;

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Registro alterado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações do contrato de trabalho.", erro = e.Message });
            }
        }

        [HttpPut("alteracao/info-contrato-subgrupos/{codigoFormulario}/{codigoInfoContrato}/{statusFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraInfoContratoSubgruposF2500Async([FromRoute] int codigoFormulario, 
                                                                              [FromRoute] int codigoInfoContrato, 
                                                                              [FromRoute] int statusFormulario, 
                                                                              [FromBody] EsF2500InfocontratoRequestDTO requestDTO,
                                                                              [FromServices] ESocialF2500InfoContratoService service,
                                                                              CancellationToken ct)
        {
            try
            {                
                var (formularioInvalido, listaErros) = requestDTO.ValidarSubgrupo();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }

                    var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct);

                    if (infoContrato is not null)
                    {
                        PreencheInfoContrato(ref infoContrato, requestDTO, formulario, false, true);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    if (statusFormulario == EsocialStatusFormulario.ProntoParaEnvio.ToByte())
                    {                      
                        List<string> listaErrosTemp = new();

                        listaErrosTemp.AddRange(service.ValidaCamposEscritorio(codigoFormulario, requestDTO, listaErros, formulario, infoContrato, _eSocialDbContext));

                        listaErrosTemp.AddRange(service.ValidaCamposContador(requestDTO, listaErros, formulario, infoContrato, _eSocialDbContext));                        

                        listaErros = listaErrosTemp;

                        formularioInvalido = listaErros.Any();

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                    }

                    return Ok("Registro alterado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
               
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações do contrato de trabalho.", erro = e.Message });
            }
        }

        [HttpGet("alteracao/info-contrato/gerar-matricula")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> GerarMatriculaInfoContratoAsync(CancellationToken ct)
        {
            try
            {
                var sequencial = await _sequenceDbContext.GetNextSequence("ES_GERAMATRICULA_F2500", ct);
                if (sequencial > 0)
                {
                    var matriculaFormatada = sequencial.ToString().PadLeft(10, '0') + "ES";
                    var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.InfocontrMatricula == matriculaFormatada, ct);

                    while (infoContrato != null)
                    {
                        sequencial = await _sequenceDbContext.GetNextSequence("ES_GERAMATRICULA_F2500", ct);
                        matriculaFormatada = sequencial.ToString().PadLeft(10, '0') + "ES";
                        infoContrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.InfocontrMatricula == matriculaFormatada, ct);

                    }
                    return Ok(matriculaFormatada);
                }

                return BadRequest($"Não foi possível gerar uma nova matrícula.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao gerar uma nova matrícula.", erro = e.Message });
            }
        }

        #endregion

        #region Validação

        [HttpPost("validacao/info-contrato-subgrupos/contador/{codigoFormulario}/{codigoInfoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidaInfoContratoSubgruposF2500ContadorAsync([FromRoute] int codigoFormulario, [FromRoute] int codigoInfoContrato, [FromBody] EsF2500InfocontratoRequestDTO requestDTO, [FromServices] ESocialF2500InfoContratoService service, CancellationToken ct)
        {
            try
            {

                if (!_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) &&
                   !_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_CONTADOR_FORM2500))
                {
                    return BadRequest("O Usuário não tem permissão para finalizar o preenchimento do contador.");
                }

                var (formularioInvalido, listaErros) = requestDTO.ValidarSubgrupoContador();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }

                    var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct);

                    listaErros = service.ValidaCamposContador(requestDTO, listaErros, formulario, infoContrato, _eSocialDbContext);

                    formularioInvalido = listaErros.Any();

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    return Ok("Contrato validado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações do contrato de trabalho.", erro = e.Message });
            }
        }


        [HttpPost("validacao/info-contrato-subgrupos/escritorio/{codigoFormulario}/{codigoInfoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ValidaInfoContratoSubgruposF2500EscritorioAsync([FromRoute] int codigoFormulario, [FromRoute] int codigoInfoContrato, [FromBody] EsF2500InfocontratoRequestDTO requestDTO, [FromServices] ESocialF2500InfoContratoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ENVIAR_PARA_ESOCIAL) && !_controleDeAcessoService.TemPermissao(Permissoes.FINALIZAR_ESCRITORIO_FORM2500))
                {
                    return BadRequest("O Usuário não tem permissão para finalizar o preenchimento do escritório.");
                }

                var (formularioInvalido, listaErros) = requestDTO.ValidarSubgrupoEscritorio();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações do contrato de trabalho.");
                    }

                    var infoContrato = await _eSocialDbContext.EsF2500Infocontrato.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct);

                    listaErros = service.ValidaCamposEscritorio(codigoFormulario, requestDTO, listaErros, formulario, infoContrato, _eSocialDbContext);

                    formularioInvalido = listaErros.Any();

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    return Ok("Contrato validado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");

            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações do contrato de trabalho.", erro = e.Message });
            }
        }


        #endregion

        #region Inclusao 

        [HttpPost("inclusao/info-contrato/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraInfoContratoF2500Async([FromRoute] int codigoFormulario, [FromBody] EsF2500InfocontratoRequestDTO requestDTO, [FromServices] ESocialF2500InfoContratoService service, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                    var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                    
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de contratos de trabalho e término de TSVE.");
                    }

                    if (await _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == codigoFormulario).CountAsync(ct) >= 99)
                    {
                        return BadRequest("O sistema só permite a inclusão de até 99 Contratos de Trabalho.");
                    };

                    var infoContrato = new EsF2500Infocontrato();

                    PreencheInfoContrato(ref infoContrato, requestDTO, formulario, true);

                    _eSocialDbContext.Add(infoContrato);

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de contratos de trabalho e término de TSVE.");
                    }

                    var listaErrosTemp = listaErrosDTO.ToList();

                    var validaMatriculaInfoContrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => !string.IsNullOrEmpty(requestDTO.InfocontrMatricula) && x.InfocontrMatricula == requestDTO.InfocontrMatricula, ct);
                    if (validaMatriculaInfoContrato != null)
                    {
                        listaErrosTemp.Add("Matrícula informada já cadastrada.");
                    }

                    if (requestDTO.InfocontrDtadmorig.HasValue && (await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == codigoFormulario && (x.InfocontrDtadmorig.HasValue && x.InfocontrDtadmorig.Value.Date == requestDTO.InfocontrDtadmorig!.Value.Date), ct)))
                    {
                        listaErrosTemp.Add("Já existe um registro com a \"Data de Admissão Original\" informada.");
                    }

                    var listaErros = listaErrosTemp.ToList();

                    listaErros.AddRange((await service.ValidaInclusaoInfoContrato(_eSocialDbContext, requestDTO, infoContrato, formulario, ct)).ToList());

                    formularioInvalido = listaErros.Count > 0;

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await _eSocialDbContext.SaveChangesAsync(ct);

                    return Ok("Registro incluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
                
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de contratos de trabalho e término de TSVE.", erro = e.InnerException != null ? e.InnerException.Message : e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/info-contrato/{codigoFormulario}/{codigoInfoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiInfoContratoF2500Async([FromRoute] int codigoFormulario, [FromRoute] long codigoInfoContrato, CancellationToken ct)
        {
            try
            {
                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir informações do contrato de trabalho.");
                    }

                    if (await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Infocontrato == codigoInfoContrato, ct))
                    {
                        await RemoveInfoContrato(codigoInfoContrato, ct);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir informações do contrato de trabalho.");
                    }

                    await _eSocialDbContext.SaveChangesAsync(User!.Identity!.Name, true, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir informações do contrato de trabalho.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos Privados

        private void PreencheInfoContrato(ref EsF2500Infocontrato infoContrato, EsF2500InfocontratoRequestDTO requestDTO, EsF2500 formulario, bool novoContrato = false, bool salvandoSubgrupos = false)
        {
            var processo = (from p in _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == formulario.CodProcesso)
                            from pp in _eSocialDbContext.Parte.Where(x => x.CodParte == p.CodParteEmpresa)
                            select new
                            {
                                p.NroProcessoCartorio,
                                CnpjEmpresa = p.CodParteEmpresaNavigation.CgcParte,
                                pp.IdEsEmpresaAgrupadora
                            }).First();

            var empresaAgrupadora = _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking().FirstOrDefault(e => e.IdEsEmpresaAgrupadora == processo.IdEsEmpresaAgrupadora);

            if (novoContrato)
            {
                infoContrato.IdF2500 = formulario.IdF2500;
               
                if (requestDTO.InfocontrIndcontr != "S")
                {
                    infoContrato.InfovincTpregtrab = ESocialRegimeTrabalhista.CLT.ToByte();
                    infoContrato.InfovincTpregprev = ESocialRegimePrevidenciario.RegimeGeral.ToByte();
                }
                infoContrato.InfovincTmpparc = ESocialContratoTempoParcial.NaoParcial.ToByte();
                infoContrato.DuracaoTpcontr = ESocialTipoContrato.PrazoIndeterminado.ToByte();
                infoContrato.DuracaoClauassec = "N";
                infoContrato.IdeestabTpinsc = ESocialTipoInscricaoTabela05.CNPJ.ToByte();
                infoContrato.IdeestabNrinsc = empresaAgrupadora != null && empresaAgrupadora.CnpjEstabelecimentoPagto != null ? empresaAgrupadora.CnpjEstabelecimentoPagto : processo!.CnpjEmpresa;
            }
            infoContrato.LogCodUsuario = User!.Identity!.Name;
            infoContrato.LogDataOperacao = DateTime.Now;

            if (!salvandoSubgrupos)
            {
                if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    infoContrato.InfocontrCodcateg = requestDTO.InfocontrCodcateg;
                    infoContrato.InfocontrDtadmorig = requestDTO.InfocontrDtadmorig;
                    infoContrato.InfocontrDtinicio = requestDTO.InfocontrDtinicio;
                    infoContrato.InfocontrIndcateg = requestDTO.InfocontrIndcateg;
                    infoContrato.InfocontrIndcontr = requestDTO.InfocontrIndcontr;
                    infoContrato.InfocontrIndmotdeslig = requestDTO.InfocontrIndmotdeslig;
                    infoContrato.InfocontrIndnatativ = requestDTO.InfocontrIndnatativ;
                    infoContrato.InfocontrIndreint = requestDTO.InfocontrIndreint;
                    infoContrato.InfocontrIndunic = requestDTO.InfocontrIndunic;
                    infoContrato.InfocontrMatricula = requestDTO.InfocontrMatricula;
                    infoContrato.InfocontrTpcontr = requestDTO.InfocontrTpcontr;
                    infoContrato.InfocomplCodcbo = requestDTO.InfocomplCodcbo;
                    infoContrato.InfocomplNatatividade = requestDTO.InfocomplNatatividade;
                    infoContrato.InfotermDtterm = requestDTO.InfotermDtterm;
                    infoContrato.InfotermMtvdesligtsv = requestDTO.InfotermMtvdesligtsv;
                }
            }

            if (salvandoSubgrupos)
            {
                if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    if (!novoContrato)
                    {
                        infoContrato.InfovincTpregtrab = requestDTO.InfovincTpregtrab;
                        infoContrato.InfovincTpregprev = requestDTO.InfovincTpregprev;
                        infoContrato.InfovincTmpparc = requestDTO.InfovincTmpparc;
                        infoContrato.DuracaoTpcontr = requestDTO.DuracaoTpcontr;
                        infoContrato.DuracaoClauassec = requestDTO.DuracaoClauassec;
                    }

                    infoContrato.InfovincDtadm = requestDTO.InfovincDtadm;
                    infoContrato.DuracaoDtterm = requestDTO.DuracaoDtterm;
                    infoContrato.DuracaoObjdet = requestDTO.DuracaoObjdet;
                    infoContrato.SucessaovincTpinsc = requestDTO.SucessaovincTpinsc;
                    infoContrato.SucessaovincNrinsc = requestDTO.SucessaovincNrinsc is not null ? Regex.Replace(requestDTO.SucessaovincNrinsc, "[^0-9]+", "") : requestDTO.SucessaovincNrinsc;
                    infoContrato.SucessaovincMatricant = requestDTO.SucessaovincMatricant;
                    infoContrato.SucessaovincDttransf = requestDTO.SucessaovincDttransf;
                    infoContrato.InfodesligDtdeslig = requestDTO.InfodesligDtdeslig;
                    infoContrato.InfodesligMtvdeslig = requestDTO.InfodesligMtvdeslig;
                    infoContrato.InfodesligDtprojfimapi = requestDTO.InfodesligDtprojfimapi;
                }

                if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOJ_DADOSESTABE))
                {
                    if (!novoContrato)
                    {
                        infoContrato.IdeestabTpinsc = requestDTO.IdeestabTpinsc;
                        infoContrato.IdeestabNrinsc = requestDTO.IdeestabNrinsc is not null ? Regex.Replace(requestDTO.IdeestabNrinsc, "[^0-9]+", "") : requestDTO.IdeestabNrinsc;

                    }
                }

                if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOJ_VALORES))
                {
                    infoContrato.InfovlrRepercproc = requestDTO.InfovlrRepercproc;
                    infoContrato.InfovlrCompini = requestDTO.InfovlrCompini.HasValue ? requestDTO.InfovlrCompini.Value.ToString("yyyyMM") : null;
                    infoContrato.InfovlrCompfim = requestDTO.InfovlrCompfim.HasValue ? requestDTO.InfovlrCompfim.Value.ToString("yyyyMM") : null;
                    infoContrato.InfovlrVrremun = requestDTO.InfovlrVrremun;
                    infoContrato.InfovlrVrapi = requestDTO.InfovlrVrapi;
                    infoContrato.InfovlrVr13api = requestDTO.InfovlrVr13api;
                    infoContrato.InfovlrVrinden = requestDTO.InfovlrVrinden;
                    infoContrato.InfovlrPagdiretoresc = requestDTO.InfovlrPagdiretoresc;
                    infoContrato.InfovlrVrbaseindenfgts = requestDTO.InfovlrVrbaseindenfgts;
                }

            }
        }

        private static EsF2500InfocontratoDTO PreencheInfoContratoDTO(ref EsF2500Infocontrato? infoContrato)
        {
            return new EsF2500InfocontratoDTO()
            {
                IdEsF2500Infocontrato = infoContrato!.IdEsF2500Infocontrato,
                IdF2500 = infoContrato!.IdF2500,
                LogDataOperacao = infoContrato!.LogDataOperacao,
                LogCodUsuario = infoContrato!.LogCodUsuario,
                InfocontrCodcateg = infoContrato!.InfocontrCodcateg,
                InfocontrDtadmorig = infoContrato!.InfocontrDtadmorig,
                InfocontrDtinicio = infoContrato!.InfocontrDtinicio,
                InfocontrIndcateg = infoContrato!.InfocontrIndcateg,
                InfocontrIndcontr = infoContrato!.InfocontrIndcontr,
                InfocontrIndmotdeslig = infoContrato!.InfocontrIndmotdeslig,
                InfocontrIndnatativ = infoContrato!.InfocontrIndnatativ,
                InfocontrIndreint = infoContrato!.InfocontrIndreint,
                InfocontrIndunic = infoContrato!.InfocontrIndunic,
                InfocontrMatricula = infoContrato!.InfocontrMatricula,
                InfocomplCodcbo = infoContrato!.InfocomplCodcbo,
                InfocomplNatatividade = infoContrato!.InfocomplNatatividade,
                InfovincTpregtrab = infoContrato!.InfovincTpregtrab,
                InfovincTpregprev = infoContrato!.InfovincTpregprev,
                InfovincDtadm = infoContrato!.InfovincDtadm,
                InfovincTmpparc = infoContrato!.InfovincTmpparc,
                DuracaoTpcontr = infoContrato!.DuracaoTpcontr,
                DuracaoDtterm = infoContrato!.DuracaoDtterm,
                DuracaoClauassec = infoContrato!.DuracaoClauassec,
                DuracaoObjdet = infoContrato!.DuracaoObjdet,
                SucessaovincTpinsc = infoContrato!.SucessaovincTpinsc,
                SucessaovincNrinsc = infoContrato!.SucessaovincNrinsc,
                SucessaovincMatricant = infoContrato!.SucessaovincMatricant,
                SucessaovincDttransf = infoContrato!.SucessaovincDttransf,
                InfodesligDtdeslig = infoContrato!.InfodesligDtdeslig,
                InfodesligMtvdeslig = infoContrato!.InfodesligMtvdeslig,
                InfodesligDtprojfimapi = infoContrato!.InfodesligDtprojfimapi,
                InfotermDtterm = infoContrato!.InfotermDtterm,
                InfotermMtvdesligtsv = infoContrato!.InfotermMtvdesligtsv,
                IdeestabTpinsc = infoContrato!.IdeestabTpinsc,
                IdeestabNrinsc = infoContrato!.IdeestabNrinsc,
                InfovlrCompini = infoContrato!.InfovlrCompini,
                InfovlrCompfim = infoContrato!.InfovlrCompfim,
                InfovlrRepercproc = infoContrato!.InfovlrRepercproc,
                InfovlrVrremun = infoContrato!.InfovlrVrremun,
                InfovlrVrapi = infoContrato!.InfovlrVrapi,
                InfovlrVr13api = infoContrato!.InfovlrVr13api,
                InfovlrVrinden = infoContrato!.InfovlrVrinden,
                InfovlrVrbaseindenfgts = infoContrato!.InfovlrVrbaseindenfgts,
                InfovlrPagdiretoresc = infoContrato!.InfovlrPagdiretoresc

            };
        }

        private IQueryable<EsF2500InfocontratoDTO> RecuperaListaContrato(int codigoFormulario, long codProcesso)
        {
            var result = _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == codProcesso).Select(y => y.IndProprioTerceiro).FirstOrDefault();

            var query = from infoContrato in _eSocialDbContext.EsF2500Infocontrato.AsNoTracking()
                       join t01 in _eSocialDbContext.EsTabela01 on infoContrato.InfocontrCodcateg equals t01.CodEsTabela01 into LeftJoinT01
                       from t01 in LeftJoinT01.DefaultIfEmpty()
                       join tipoContrato in _eSocialDbContext.EsTabelaTipoContratoTsve on infoContrato.InfocontrTpcontr equals tipoContrato.CodEsTipoContrato into LeftJointipoContrato
                       from tipoContrato in LeftJointipoContrato.DefaultIfEmpty()
                        where (infoContrato.IdF2500 == codigoFormulario)
                       select new EsF2500InfocontratoDTO()
                       {
                           IdF2500 = infoContrato.IdF2500,
                           IdEsF2500Infocontrato = infoContrato.IdEsF2500Infocontrato,
                           InfocontrCodcateg = infoContrato.InfocontrCodcateg,
                           InfocontrDtadmorig = infoContrato.InfocontrDtadmorig,
                           InfocontrDtinicio = infoContrato.InfocontrDtinicio,
                           InfocontrIndcateg = infoContrato.InfocontrIndcateg,
                           InfocontrIndcontr = infoContrato.InfocontrIndcontr,
                           InfocontrIndmotdeslig = infoContrato.InfocontrIndmotdeslig,
                           InfocontrIndnatativ = infoContrato.InfocontrIndnatativ,
                           InfocontrIndreint = infoContrato.InfocontrIndreint,
                           InfocontrIndunic = infoContrato.InfocontrIndunic,
                           InfocontrMatricula = infoContrato.InfocontrMatricula,
                           InfocontrTpcontr = infoContrato.InfocontrTpcontr,
                           LogCodUsuario = infoContrato.LogCodUsuario,
                           LogDataOperacao = infoContrato.LogDataOperacao,
                           InfocomplCodcbo = infoContrato.InfocomplCodcbo,
                           InfocomplNatatividade = infoContrato.InfocomplNatatividade,
                           InfotermDtterm = infoContrato.InfotermDtterm,
                           InfotermMtvdesligtsv = infoContrato.InfotermMtvdesligtsv,
                           DescricaoTipoContrato = infoContrato.InfocontrTpcontr.HasValue ? $"{infoContrato.InfocontrTpcontr!.Value} - {tipoContrato.NomEsTipoContrato}" : string.Empty,
                           IndProprioTerceiro = result!,
                           DescricaoCategoria = infoContrato.InfocontrCodcateg.HasValue ? $"{infoContrato.InfocontrCodcateg} - {t01.DscEsTabela01}" : string.Empty
                       };

            return query;
            //return _eSocialDbContext.EsF2500Infocontrato
            //    .Where(x => x.IdF2500 == codigoFormulario).AsNoTracking().
            //    Select(x => new EsF2500InfocontratoDTO()
            //    {
            //        IdF2500 = x.IdF2500,
            //        IdEsF2500Infocontrato = x.IdEsF2500Infocontrato,
            //        InfocontrCodcateg = x.InfocontrCodcateg,
            //        InfocontrDtadmorig = x.InfocontrDtadmorig,
            //        InfocontrDtinicio = x.InfocontrDtinicio,
            //        InfocontrIndcateg = x.InfocontrIndcateg,
            //        InfocontrIndcontr = x.InfocontrIndcontr,
            //        InfocontrIndmotdeslig = x.InfocontrIndmotdeslig,
            //        InfocontrIndnatativ = x.InfocontrIndnatativ,
            //        InfocontrIndreint = x.InfocontrIndreint,
            //        InfocontrIndunic = x.InfocontrIndunic,
            //        InfocontrMatricula = x.InfocontrMatricula,
            //        InfocontrTpcontr = x.InfocontrTpcontr,
            //        LogCodUsuario = x.LogCodUsuario,
            //        LogDataOperacao = x.LogDataOperacao,
            //        InfocomplCodcbo = x.InfocomplCodcbo,
            //        InfocomplNatatividade = x.InfocomplNatatividade,
            //        InfotermDtterm = x.InfotermDtterm,
            //        InfotermMtvdesligtsv = x.InfotermMtvdesligtsv,
            //        DescricaoTipoContrato = x.InfocontrTpcontr.HasValue ? $"{x.InfocontrTpcontr.Value} - {x.InfocontrTpcontr.Value.ToEnum<ESocialTipoContratoTSVE>().Descricao()}" : string.Empty,
            //        IndProprioTerceiro = result!
            //    });
        }

        private async Task RemoveInfoContrato(long codigoContrato, CancellationToken ct)
        {
            var formulario2500InfocontratoExclusao = await _eSocialDbContext.EsF2500Infocontrato.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

            if (formulario2500InfocontratoExclusao is not null)
            {
                var formulario2500IdeperiodoExclusao = await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == codigoContrato).ToListAsync(ct);
                var formulario2500MudcategativExclusao = await _eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == codigoContrato).ToListAsync(ct);
                var formulario2500ObservacoesExclusao = await _eSocialDbContext.EsF2500Observacoes.Where(x => x.IdEsF2500Infocontrato == codigoContrato).ToListAsync(ct);
                var formulario2500RemuneracaoExclusao = await _eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == codigoContrato).ToListAsync(ct);
                var formulario2500UniccontrExclusao = await _eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == codigoContrato).ToListAsync(ct);

                if (formulario2500IdeperiodoExclusao.Any())
                {
                    foreach (var formularioIdePeriodo in formulario2500IdeperiodoExclusao)
                    {
                        _eSocialDbContext.Remove(formularioIdePeriodo);
                    }
                }

                if (formulario2500MudcategativExclusao.Any())
                {
                    foreach (var formularioMudCateg in formulario2500MudcategativExclusao)
                    {
                        _eSocialDbContext.Remove(formularioMudCateg);
                    }
                }

                if (formulario2500ObservacoesExclusao.Any())
                {
                    foreach (var formularioObservacao in formulario2500ObservacoesExclusao)
                    {
                        _eSocialDbContext.Remove(formularioObservacao);
                    }
                }

                if (formulario2500RemuneracaoExclusao.Any())
                {
                    foreach (var formularioRemuneracao in formulario2500RemuneracaoExclusao)
                    {
                        _eSocialDbContext.Remove(formularioRemuneracao);
                    }
                }

                if (formulario2500UniccontrExclusao.Any())
                {
                    foreach (var formularioUnicContr in formulario2500UniccontrExclusao)
                    {
                        _eSocialDbContext.Remove(formularioUnicContr);
                    }
                }
                _eSocialDbContext.Remove(formulario2500InfocontratoExclusao);

            }
        }

       

        #endregion
    }
}
