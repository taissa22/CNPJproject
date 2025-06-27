using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/ESocialF2500")]
    [ApiController]
    public class ESocialF2500DependentesController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        private const int QuantidadePorPagina = 10;

        public ESocialF2500DependentesController(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        #region Consulta

        [HttpGet("consulta/dependente/{codigoFormulario}/{codigoDependente}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2500DependenteDTO>> ListaDependentesF2500dAsync([FromRoute] int codigoFormulario, [FromRoute] long codigoDependente, CancellationToken ct)
        {
            try
            {
                var dependente = await _eSocialDbContext.EsF2500Dependente.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Dependente == codigoDependente, ct);
                if (dependente is not null)
                {
                    EsF2500DependenteDTO dependenteDTO = PreencheDependenteDTO(ref dependente);

                    return Ok(dependenteDTO);
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

        [HttpGet("lista/dependente/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<RetornoPaginadoDTO<EsF2500DependenteDTO>>> ListaDependentesF2500dAsync([FromRoute] int codigoFormulario,
                                                                                                               [FromQuery] int pagina,
                                                                                                               [FromQuery] string coluna,
                                                                                                               [FromQuery] bool ascendente, CancellationToken ct)
        {
            try
            {
                var formulario2500 = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
                if (formulario2500 is not null)
                {
                    IQueryable<EsF2500DependenteDTO> listaDependentes = RecuperaListaDependentes(codigoFormulario);

                    switch (coluna.ToLower())
                    {
                        case "descricao":
                            listaDependentes = ascendente ? listaDependentes.OrderBy(x => x.DependenteDescdep != null ? 1 : 0).ThenBy(x => x.DependenteDescdep)
                                : listaDependentes.OrderByDescending(x => x.DependenteDescdep != null ? 1 : 0).ThenByDescending(x => x.DependenteDescdep);
                            break;
                        case "cpf":
                            listaDependentes = ascendente ? listaDependentes.OrderBy(x => x.DependenteCpfdep != null ? 1 : 0).ThenBy(x => x.DependenteCpfdep)
                                : listaDependentes.OrderByDescending(x => x.DependenteCpfdep != null ? 1 : 0).ThenByDescending(x => x.DependenteCpfdep);
                            break;
                        case "tipo":
                        default:
                            listaDependentes = ascendente ? listaDependentes.OrderBy(x => x.DependenteTpdep) : listaDependentes.OrderByDescending(x => x.DependenteTpdep);
                            break;
                    }

                    var total = await listaDependentes.CountAsync(ct);

                    var skip = Pagination.PagesToSkip(QuantidadePorPagina, total, pagina);

                    var resultado = new RetornoPaginadoDTO<EsF2500DependenteDTO>
                    {
                        Total = total,
                        Lista = await listaDependentes.Skip(skip).Take(QuantidadePorPagina).ToListAsync(ct)
                    };

                    return Ok(resultado);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar lista dos dependentes.", erro = e.Message });
            }
        }

        #endregion

        #region Alteração 

        [HttpPut("alteracao/dependente/{codigoFormulario}/{codigoDependente}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> AlteraDependenteF2500Async([FromRoute] int codigoFormulario, [FromRoute] int codigoDependente, [FromBody] EsF2500DependenteRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar dependentes");
                    }

                    var dependente = await _eSocialDbContext.EsF2500Dependente.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Dependente == codigoDependente, ct);

                    if (dependente is not null)
                    {
                        PreencheDependente(ref dependente, requestDTO);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar dependentes");
                    }

                    var listaErrosTemp = listaErros.ToList();

                    if (requestDTO.DependenteCpfdep == formulario.IdetrabCpftrab)
                    {
                        listaErrosTemp.Add("O CPF do dependente deve ser diferente do CPF do  trabalhador");
                    }

                    var jaExiste = await _eSocialDbContext.EsF2500Dependente.AnyAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Dependente != dependente.IdEsF2500Dependente && x.DependenteCpfdep == requestDTO.DependenteCpfdep, ct);

                    if (jaExiste)
                    {
                        listaErrosTemp.Add("Não é permitido CPFs duplicados.");
                    }

                    if (requestDTO.DependenteCpfdep == formulario.IdeempregadorNrinsc && formulario.IdeempregadorTpinsc == 1)
                    {
                        listaErrosTemp.Add("O CPF do dependente deve ser diferente do CPF do  declarante");
                    }

                    if (!await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == codigoFormulario && x.InfocontrIndcontr == "N", ct))
                    {
                        listaErrosTemp.Add($"Não deve ser informado o grupo \"Dependentes\" (Bloco C) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\" em todos os contratos.");
                    }

                    listaErros = listaErrosTemp;

                    formularioInvalido = listaErros.Any();

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
                return BadRequest(new { mensagem = "Falha ao alterar dependente.", erro = e.Message });
            }
        }

        #endregion

        #region Inclusao 

        [HttpPost("inclusao/dependente/{codigoFormulario}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> CadastraDependenteF2500Async([FromRoute] int codigoFormulario, [FromBody] EsF2500DependenteRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de dependente.");
                    }

                    if (await _eSocialDbContext.EsF2500Dependente.Where(x => x.IdF2500 == codigoFormulario).CountAsync(ct) >= 99)
                    {
                        return BadRequest("O formulário pode conter apenas 99 registros de dependentes");
                    };

                    var dependente = new EsF2500Dependente();

                    PreencheDependente(ref dependente, requestDTO, codigoFormulario);

                    _eSocialDbContext.Add(dependente);

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para incluir informações de dependente.");
                    }

                    var listaErrosTemp = listaErros.ToList();

                    if (requestDTO.DependenteCpfdep == formulario.IdetrabCpftrab)
                    {
                        listaErrosTemp.Add("O CPF do dependente deve ser diferente do CPF do  trabalhador");
                    }

                    var jaExiste = await _eSocialDbContext.EsF2500Dependente.AnyAsync(x => x.IdF2500 == codigoFormulario && x.DependenteCpfdep == requestDTO.DependenteCpfdep, ct);

                    if (jaExiste)
                    {
                        listaErrosTemp.Add("Não é permitido CPFs duplicados.");
                    }

                    if (requestDTO.DependenteCpfdep == formulario.IdeempregadorNrinsc && formulario.IdeempregadorTpinsc == 1)
                    {
                        listaErrosTemp.Add("O CPF do dependente deve ser diferente do CPF do  declarante");
                    }

                    if (!await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == codigoFormulario && x.InfocontrIndcontr == "N", ct))
                    {
                        listaErrosTemp.Add($"Não deve ser informado o grupo \"Dependentes\" (Bloco C) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\" em todos os contratos.");
                    }

                    listaErros = listaErrosTemp;

                    formularioInvalido = listaErros.Any();

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
                return BadRequest(new { mensagem = "Falha ao cadastrar informações de dependente.", erro = e.Message });
            }
        }

        #endregion

        #region Exclusão 

        [HttpDelete("exclusao/dependente/{codigoFormulario}/{codigoDependente}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult> ExcluiDependenteF2500Async([FromRoute] int codigoFormulario, [FromRoute] long codigoDependente, CancellationToken ct)
        {
            try
            {
                if (!_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
                {
                    return BadRequest("O usuário não tem permissão para alterar os blocos A, B, C, D, E, F, H e I");
                }

                var formulario = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir dependentes");
                    }

                    var dependente = await _eSocialDbContext.EsF2500Dependente.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario && x.IdEsF2500Dependente == codigoDependente, ct);

                    if (dependente is not null)
                    {
                        dependente.LogCodUsuario = User!.Identity!.Name;
                        dependente.LogDataOperacao = DateTime.Now;
                        _eSocialDbContext.Remove(dependente);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Exclusão não efetuada.");
                    }

                    if (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte(), ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para excluir dependentes");
                    }

                    await _eSocialDbContext.SaveChangesAsync(User.Identity.Name, true, ct);

                    return Ok("Registro excluído com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao excluir dependente.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos privados
        private void PreencheDependente(ref EsF2500Dependente dependente, EsF2500DependenteRequestDTO requestDTO, int? codigoFormulario = null)
        {
            if (codigoFormulario.HasValue)
            {
                dependente.IdF2500 = codigoFormulario.Value;
            }
            dependente.DependenteDescdep = requestDTO.DependenteDescdep;
            dependente.DependenteCpfdep = requestDTO.DependenteCpfdep;
            dependente.DependenteTpdep = requestDTO.DependenteTpdep;
            dependente.LogCodUsuario = User!.Identity!.Name;
            dependente.LogDataOperacao = DateTime.Now;

        }

        private static EsF2500DependenteDTO PreencheDependenteDTO(ref EsF2500Dependente? dependente)
        {
            return new EsF2500DependenteDTO()
            {
                IdEsF2500Dependente = dependente!.IdEsF2500Dependente,
                IdF2500 = dependente!.IdF2500,
                DependenteCpfdep = dependente!.DependenteCpfdep,
                DependenteDescdep = dependente!.DependenteDescdep,
                DependenteTpdep = dependente!.DependenteTpdep,
                LogCodUsuario = dependente!.LogCodUsuario,
                LogDataOperacao = dependente!.LogDataOperacao
            };
        }

        private IQueryable<EsF2500DependenteDTO> RecuperaListaDependentes(int codigoFormulario)
        {
            return from d in _eSocialDbContext.EsF2500Dependente.AsNoTracking()
                   join t7 in _eSocialDbContext.EsTabela07 on d.DependenteTpdep equals t7.CodEsTabela07.ToString()
                   where d.IdF2500 == codigoFormulario
                   select new EsF2500DependenteDTO()
                   {
                       IdF2500 = d.IdF2500,
                       IdEsF2500Dependente = d.IdEsF2500Dependente,
                       DependenteCpfdep = d.DependenteCpfdep,
                       DependenteDescdep = d.DependenteDescdep,
                       DependenteTpdep = d.DependenteTpdep,
                       LogCodUsuario = d.LogCodUsuario,
                       LogDataOperacao = d.LogDataOperacao,
                       DescricaoTipoDependente = $"{d.DependenteTpdep} - {t7.DscEsTabela07}"
                   };
        }

        #endregion

    }
}
