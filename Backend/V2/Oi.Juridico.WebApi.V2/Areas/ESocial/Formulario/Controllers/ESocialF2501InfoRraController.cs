using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Controllers
{
    [Route("api/esocial/formulario/ESocialF2501")]
    [ApiController]
    public class ESocialF2501InfoRraController : ControllerBase
    {
        private readonly ILogger? _logger;

        #region Consulta

        [HttpGet("consulta/inforra/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ACESSAR_CADASTRO_ESOCIAL)]
        public async Task<ActionResult<EsF2501InfocrirrfDTO>> RetornaInfoRraF2501Async([FromRoute] int codigoFormulario,
                                                                                       [FromRoute] int codigoInfocrirrf,
                                                                                       [FromServices] ESocialF2501InfoCrIrrfService service,
                                                                                       CancellationToken ct)
        {
            try
            {
                var infoCrIrrf = await service.RetornaInfoCrIrrfPorIdAsync(codigoInfocrirrf, ct);
                if (infoCrIrrf is not null)
                {
                    EsF2501InfoRraDTO infoRraDTO = PreencheInfoRraDTO(ref infoCrIrrf);

                    return Ok(infoRraDTO);
                }

                return BadRequest($"Formulário não encontrado para o id: {codigoFormulario} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados da informação de Rendimentos Recebidos Acumuladamente.", erro = e.Message });
            }
        }

        #endregion

        #region Alteração

        [HttpPut("alteracao/inforra/{codigoFormulario}/{codigoInfocrirrf}")]
        [TemPermissao(Permissoes.ESOCIAL_BLOCO_CDE_2501)]
        public async Task<ActionResult> AlteraInfoRraF2501Async([FromRoute] int codigoFormulario,
                                                                [FromRoute] int codigoInfocrirrf,
                                                                [FromBody] EsF2501InfoRraRequestDTO requestDTO,
                                                                [FromServices] ESocialF2501InfoCrIrrfService service,
                                                                [FromServices] ESocialF2501Service service2501,
                                                                [FromServices] DBContextService serviceDbContext,
                                                                CancellationToken ct)
        {
            try
            {
                var (formularioInvalido, listaErros) = requestDTO.Validar();

                var formulario = await service2501.RetornaFormularioPorIdAsync(codigoFormulario, ct);

                if (formulario is not null)
                {
                    if (formulario!.StatusFormulario != EsocialStatusFormulario.Rascunho.ToByte())
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de Rendimentos Recebidos Acumuladamente.");
                    }

                    var infoCrIrrf = await service.RetornaInfoCrIrrfEditavelPorIdAsync(codigoInfocrirrf, ct);

                    if (infoCrIrrf is not null)
                    {
                        PreencheInfoRra(ref infoCrIrrf, requestDTO);
                    }
                    else
                    {
                        return BadRequest("Registro não encontrado. Alteração não efetuada.");
                    }

                    if (!await service2501.FormularioPodeSerSalvoPorIdAsync(codigoFormulario, ct))
                    {
                        return BadRequest("O formulário deve estar com status 'Rascunho' para alterar informações de Rendimentos Recebidos Acumuladamente.");
                    }

                    if (formularioInvalido)
                    {
                        return BadRequest(listaErros);
                    }

                    await serviceDbContext.SalvaAlteracoesAsync(ct);

                    return Ok("Registro alterado com sucesso.");
                }

                return BadRequest("O formulário informado não foi encontrado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao alterar informações de Rendimentos Recebidos Acumuladamente.", erro = e.Message });
            }
        }

        #endregion

        #region Métodos privados

        private void PreencheInfoRra(ref EsF2501Infocrirrf infoCrIrrf, EsF2501InfoRraRequestDTO requestDTO)
        {

            infoCrIrrf.InforraDescrra = requestDTO.InforraDescrra;
            infoCrIrrf.InforraQtdmesesrra = requestDTO.InforraQtdmesesrra;
            infoCrIrrf.DespprocjudVlrdespcustas = requestDTO.DespprocjudVlrdespcustas;
            infoCrIrrf.DespprocjudVlrdespadvogados = requestDTO.DespprocjudVlrdespadvogados;

            infoCrIrrf.LogCodUsuario = User!.Identity!.Name;
            infoCrIrrf.LogDataOperacao = DateTime.Now;

        }

        private static EsF2501InfoRraDTO PreencheInfoRraDTO(ref EsF2501Infocrirrf? infoContrato)
        {
            return new EsF2501InfoRraDTO()
            {
                IdEsF2501Infocrirrf = infoContrato!.IdEsF2501Infocrirrf,
                IdF2501 = infoContrato!.IdF2501,
                LogDataOperacao = infoContrato!.LogDataOperacao,
                LogCodUsuario = infoContrato!.LogCodUsuario,
                InforraDescrra = infoContrato!.InforraDescrra,
                InforraQtdmesesrra = infoContrato!.InforraQtdmesesrra,
                DespprocjudVlrdespcustas = infoContrato!.DespprocjudVlrdespcustas,
                DespprocjudVlrdespadvogados = infoContrato!.DespprocjudVlrdespadvogados
            };
        }

        #endregion
    }
}
