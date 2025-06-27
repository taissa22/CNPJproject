using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Controllers
{
    [Route("api/esocial/v1_1/ESocialF2500")]
    [ApiController]
    public class ESocialF2500ObservacoesController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500ObservacoesController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/observacao/{codigoContrato}/{codigoObservacao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500ObservacoesDTO>> ListaObservacoesF2500dAsync([FromRoute] int codigoContrato, [FromRoute] long codigoObservacao, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    var observacao = await _eSocialDbContext.EsF2500Observacoes.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Observacoes == codigoObservacao, ct);
                    if (observacao is not null)
                    {
                        EsF2500ObservacoesDTO observacaoDTO = PreencheObservacaoDTO(ref observacao);

                        return Ok(observacaoDTO);
                    }

                    return NotFound($"Nenhuma informação de observação encontrada para o id: {codigoObservacao} ");
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da observação.", erro = e.Message });
            }
        }

        #endregion

        #region Lista paginado

        [HttpGet("lista/observacao/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<IEnumerable<EsF2500ObservacoesDTO>>> ListaObservacoesF2500dAsync([FromRoute] int codigoContrato,
                                                                                                       [FromQuery] int pagina,
                                                                                                       [FromQuery] string coluna,
                                                                                                       [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);
                if (contrato is not null)
                {
                    IQueryable<EsF2500ObservacoesDTO> listaObservacoes = RecuperaListaObservacoes(codigoContrato);

                    listaObservacoes = listaObservacoes.OrderBy(x => x.IdEsF2500Observacoes);                     

                    var total = await listaObservacoes.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500ObservacoesDTO>
                    {
                        Total = total,
                        Lista = await listaObservacoes.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Contrato não encontrado para o id: {codigoContrato} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista de Observaçõe.", erro = e.Message });
            }
        }

        #endregion

        #region Alteração 

        [HttpPut("alteracao/observacao/{codigoFormulario}/{codigoContrato}/{codigoObservacao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraObservacaoF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] int codigoObservacao, [FromBody] EsF2500ObservacoesRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {


                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma observação do contrato de trabalho.");
                        }

                        var observacao = await _eSocialDbContext.EsF2500Observacoes.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Observacoes == codigoObservacao, ct);

                        if (observacao is not null)
                        {
                            PreencheObservacao(ref observacao, requestDTO);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Alteração não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para alterar uma observação do contrato de trabalho.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        if (contrato.InfocontrIndcontr == "S")
                        {
                            listaErros.Add($"Não deve ser informado o grupo \"Observações\" (Bloco F) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
                        }

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro alterado com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}");
                }

                return BadRequest($"Formulario não encontrado para o id: {codigoFormulario}");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar observação do contrato de trabalho.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusao 

        [HttpPost("inclusao/observacao/{codigoFormulario}/{codigoContrato}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraObservacoesF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromBody] EsF2500ObservacoesRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }
                var (formularioInvalido, listaErrosDTO) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {


                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de observação do contrato de trabalho.");
                        }

                        if (await _eSocialDbContext.EsF2500Observacoes.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= 99)
                        {
                            return BadRequest("O sistema só permite a inclusão de até 99 Observações de Contratos de Trabalho.");
                        };

                        var observacao = new EsF2500Observacoes();

                        PreencheObservacao(ref observacao, requestDTO, codigoContrato);

                        _eSocialDbContext.Add(observacao);

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de observação do contrato de trabalho.");
                        }

                        var listaErros = listaErrosDTO.ToList();

                        if (contrato.InfocontrIndcontr == "S")
                        {
                            listaErros.Add($"Não deve ser informado o grupo \"Observações\" (Bloco F) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
                        }

                        formularioInvalido = listaErros.Count > 0;

                        if (formularioInvalido)
                        {
                            return BadRequest(listaErros);
                        }

                        await _eSocialDbContext.SaveChangesAsync(ct);

                        return Ok("Registro incluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario}.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao incluir informações de observação do contrato de trabalho.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/observacao/{codigoFormulario}/{codigoContrato}/{codigoObservacao}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiObservacoesF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoContrato, [FromRoute] long codigoObservacao, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato);

                if (formulario is not null)
                {
                    if (contrato is not null)
                    {


                        if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma observação do contrato de trabalho.");
                        }

                        var observacao = await _eSocialDbContext.EsF2500Observacoes.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Observacoes == codigoObservacao, ct);

                        if (observacao is not null)
                        {
                            observacao.LogCodUsuario = User!.Identity!.Name;
                            observacao.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(observacao);
                        }
                        else
                        {
                            return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                        }

                        if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                        {
                            return BadRequest("O formulário deve estar com status 'Rascunho' para excluir uma observação do contrato de trabalho.");
                        }

                        await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                        return Ok("Registro excluído com sucesso.");
                    }

                    return BadRequest($"Contrato não encontrado para o id: {codigoContrato}.");
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario}.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir observação do contrato de trabalho.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos Privados

        private void PreencheObservacao(ref EsF2500Observacoes observacao, EsF2500ObservacoesRequestDTO requestDTO, int? codigoContrato = null)
        {

            if (codigoContrato.HasValue)
            {
                observacao.IdEsF2500Infocontrato = codigoContrato.Value;
            }
            observacao.ObservacoesObservacao = requestDTO.ObservacoesObservacao;

            observacao.LogCodUsuario = User!.Identity!.Name;
            observacao.LogDataOperacao = DateTime.Now;
        }

        private static EsF2500ObservacoesDTO PreencheObservacaoDTO(ref EsF2500Observacoes? observacao)
        {
            return new EsF2500ObservacoesDTO()
            {
                IdEsF2500Observacoes = observacao!.IdEsF2500Observacoes,
                IdEsF2500Infocontrato = observacao!.IdEsF2500Infocontrato,
                LogCodUsuario = observacao!.LogCodUsuario,
                LogDataOperacao = observacao!.LogDataOperacao,
                ObservacoesObservacao = observacao!.ObservacoesObservacao
            };
        }

        private IQueryable<EsF2500ObservacoesDTO> RecuperaListaObservacoes(int codigoContrato)
        {
            return _eSocialDbContext.EsF2500Observacoes
                .Where(x => x.IdEsF2500Infocontrato == codigoContrato).AsNoTracking()
                .Select(x => new EsF2500ObservacoesDTO()
                {
                    IdEsF2500Infocontrato = x.IdEsF2500Infocontrato,
                    IdEsF2500Observacoes = x.IdEsF2500Observacoes,
                    LogDataOperacao = x.LogDataOperacao,
                    LogCodUsuario = x.LogCodUsuario,
                    ObservacoesObservacao = x.ObservacoesObservacao
                });
        }

        #endregion
    }
}
