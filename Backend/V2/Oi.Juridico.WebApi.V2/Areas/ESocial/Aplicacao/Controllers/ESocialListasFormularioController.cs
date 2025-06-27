using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using System.Drawing;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Controllers
{
    [Route("api/esocial/v1/[controller]")]
    [ApiController]
    public class ESocialListasFormularioController : ControllerBase
    {
        #region Propriedades

        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private ESocialDbContext _eSocialDbContext;

        #endregion

        #region Construtor 

        public ESocialListasFormularioController(ParametroJuridicoContext parametroJuridico, ESocialDbContext eSocialDbContext)
        {
            _parametroJuridicoDbContext = parametroJuridico;
            _eSocialDbContext = eSocialDbContext;
        }

        #endregion Construtor

        #region métodos

        [HttpGet("lista/status-formulario")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaStatusFormulario()
        {

            var listaEnum = EnumExtension.ToList<EsocialStatusFormulario>()
                .Select(x => new RetornoListaDTO()
                {
                    Id = x.ToInt(),
                    Descricao = x.Descricao()
                }
                );

            return Ok(listaEnum);
        }

        [HttpGet("lista/status-reclamante")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaStatusEsocialReclamante()
        {
            var listaEnum = EnumExtension.ToList<EsocialStatusReclamante>()
                                   .Select(x => new RetornoListaDTO()
                                   {
                                       Id = x.ToInt(),
                                       Descricao = x.Descricao()
                                   }
                                    );

            return Ok(listaEnum);
        }

        #endregion

        #region F2500

        #region 1.2 Identificação do arquivo, processo, trabalhador e contribuinte (responsável direto).

        [HttpGet("lista/origem-processo-demanda")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaOrigemProcessoDemanda()
        {
            //var listaRetorno = _eSocialDbContext.EsOrigemProcessoDemanda.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialOrigemProcesso>()
              .Select(x => new RetornoListaDTO()
              {
                  Id = x.ToInt(),
                  Descricao = x.Descricao()
              }
              );

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.3 Informações complementares do processo ou da demanda.

        [HttpGet("lista/uf")]
        public async Task<ActionResult<IEnumerable<EstadoDTO>>> ObterListaUFVaraDeTramitacaoDeProcessoJudicial(CancellationToken ct)
        {
            var listaRetorno = _eSocialDbContext.Estado.AsNoTracking().Select(x => new EstadoDTO
            {
                Codigo = x.CodEstado,
                Descricao = x.NomEstado
            }).OrderBy(x => x.Codigo);

            return Ok(await listaRetorno.ToListAsync(ct));
        }

        [HttpGet("lista/codigo-municipio/{codEstado}")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaCodigoMunicipio([FromRoute] string codEstado, CancellationToken ct)
        {
            var listaRetorno = _eSocialDbContext.MunicipioIbge
                .Where(x => x.CodEstado == codEstado)
                .Select(x => new RetornoListaDTO
                {
                    Id = x.CodMunicipioIbge,
                    Descricao = x.NomMunicipio
                });

            return Ok(await listaRetorno.ToListAsync(ct));
        }

        [HttpGet("lista/tipo-de-ambito-acordo")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoDeAmbitoDeCelebracaoDoAcordo()
        {
            //var listaRetorno = _eSocialDbContext.EsTipoAmbitoCelebracaoAcordo.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});
            var listaRetorno = EnumExtension.ToList<ESocialTipoAmbienteAcordo>()
               .Select(x => new RetornoListaDTO()
               {
                   Id = x.ToInt(),
                   Descricao = x.Descricao()
               }
               );

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.4 Dependente       

        [HttpGet("lista/tipo-dependente")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoDependente(CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.EsTabela07
                                               .Where(x => x.DatInicio.Date <= DateTime.Now.Date && (!x.DatTermino.HasValue || x.DatTermino.HasValue && x.DatTermino.Value.Date >= DateTime.Now.Date) && x.IndAtivo == "S")
                                               .Select(x => new RetornoListaDTO()
                                               {
                                                   Id = x.CodEsTabela07,
                                                   Descricao = x.DscEsTabela07
                                               }).OrderBy(x => x.Id).ToListAsync(ct);
            return Ok(listaRetorno);
        }

        #endregion

        #region 1.5 Contrato de trabalho e término de TSVE

        [HttpGet("lista/tipo-contrato-tsve")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoContratoTsve()
        {
            var listaRetorno = _eSocialDbContext.EsTabelaTipoContratoTsve.Select(x => new RetornoListaDTO
            {
                Id = x.CodEsTipoContrato,
                Descricao = x.NomEsTipoContrato
            });
            //var listaRetorno = EnumExtension.ToList<ESocialTipoContratoTSVE>()
            // .Select(x => new RetornoListaDTO()
            // {
            //     Id = x.ToInt(),
            //     Descricao = x.Descricao()
            // }
            // );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/codigo-categoria")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaCodigoCategoria(CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.EsTabela01.AsNoTracking().Where(x => x.DatInicio.Date <= DateTime.Now.Date).Select(x => new RetornoListaDTO
            {
                Id = x.CodEsTabela01,
                Descricao = x.IndAtivo == "N" || x.DatTermino.HasValue && x.DatTermino.Value.Date <= DateTime.Now.Date ? $"{x.DscEsTabela01.ToUpper().Trim()} [INATIVO]" : x.DscEsTabela01.ToUpper().Trim()
            }).ToListAsync(ct);

            return Ok(listaRetorno);
        }

        [HttpGet("lista/natureza-atividade")]
        public ActionResult<IEnumerable<RetornoListaDTO>> ObterListaNaturezaAtividade()
        {
            //var listaRetorno = _eSocialDbContext.NaturezaAtividade.Select(x => new RetornoListaDTO
            //{
            //    Id = x.CodRepercProc,
            //    Descricao = x.DscRepercProc
            //});

            var listaRetorno = EnumExtension.ToList<ESocialNaturezaAtividade>()
            .Select(x => new RetornoListaDTO()
                {
                    Id = x.ToInt(),
                    Descricao = x.Descricao()
                }
            );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/motivo-termino-tsve")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaMotivoTerminoTSVE()
        {
            //var listaRetorno = _eSocialDbContext.EsMotivoTerminoTSVE.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialMotivoTerminoTSVE>()
            .Select(x => new RetornoListaDTO()
            {
                Id = x.ToInt(),
                Descricao = x.Descricao()
            }
            );

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.6 Vínculo e desligamento

        [HttpGet("lista/tipo-regime-trabalhista")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoRegimeTrabalhista()
        {
            //var listaRetorno = _eSocialDbContext.EsTipoRegimeTrabalhista.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialRegimeTrabalhista>()
             .Select(x => new RetornoListaDTO()
             {
                 Id = x.ToInt(),
                 Descricao = x.Descricao()
             }
             );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/tipo-regime-previdenciario")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoRegimePrevidenciario()
        {
            //var listaRetorno = _eSocialDbContext.EsTipoRegimePrevidenciario.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialRegimePrevidenciario>()
             .Select(x => new RetornoListaDTO()
             {
                 Id = x.ToInt(),
                 Descricao = x.Descricao()
             }
             );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/tipo-contrato-tempo-parcial")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoContratoTempoParcial()
        {
            //var listaRetorno = _eSocialDbContext.EsTipoContratoTempoParcial.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialContratoTempoParcial>()
             .Select(x => new RetornoListaDTO()
             {
                 Id = x.ToInt(),
                 Descricao = x.Descricao()
             }
             );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/tipo-contrato-vinculo-desligamento")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoContratoVinculoDesligamento()
        {
            //var listaRetorno = _eSocialDbContext.EsTipoContrato.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialTipoContrato>()
              .Select(x => new RetornoListaDTO()
              {
                  Id = x.ToInt(),
                  Descricao = x.Descricao()
              }
              );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/motivo-desligamento")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaMotivoDesligamento(CancellationToken ct)
        {

            var listaRetorno = await _eSocialDbContext.EsTabela19.AsNoTracking().Where(x => x.DatInicio.Date <= DateTime.Now.Date && !x.DatTermino.HasValue).Select(x => new RetornoListaDTO
            {
                Id = x.CodEsTabela19,
                Descricao = x.IndAtivo == "N" ? $"{x.DscEsTabela19} [INATIVO]" : x.DscEsTabela19
            }).ToListAsync(ct);

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.8 Renumeração

        [HttpGet("lista/unidade-pagamento")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaUnidadePagamento()
        {
            var listaRetorno = EnumExtension.ToList<ESocialUnidadePagamento>()
             .Select(x => new RetornoListaDTO()
             {
                 Id = x.ToInt(),
                 Descricao = x.Descricao().ToUpper().Trim()
             }
             );

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.11 Estabelecimento responsável pelo pagamento e informações do período e valores decorrentes do processo trabalhista

        [HttpGet("lista/repercussao-processo")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaRepercussaoProcesso()
        {
            //var listaRetorno = _eSocialDbContext.EsRepercussaoProcesso.Select(x => new RetornoListaDTO
            //{
            //    Id = x.,
            //    Descricao = x.
            //});

            var listaRetorno = EnumExtension.ToList<ESocialRepercussaoProcesso>()
              .Select(x => new RetornoListaDTO()
              {
                  Id = x.ToInt(),
                  Descricao = x.Descricao()
              }
              );

            return Ok(listaRetorno);
        }

        [HttpGet("lista/ind-repercussao-processo")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaIndRepercussaoProcesso()
        {
            var listaRetorno = _eSocialDbContext.EsTabelaRepercProc.Select(x => new RetornoListaDTO
            {
                Id = x.CodRepercProc,
                Descricao = x.DscRepercProc
            });

            return Ok(listaRetorno);
        }

        #endregion

        #region 1.12 Bases de cálculo de contribuição previdenciária e FGTS

        [HttpGet("lista/grau-exposicao")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaGrauExposicao(CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.EsTabela02
                                               .Where(x => x.IndAtivo == "S")
                                               .Select(x => new RetornoListaDTO()
                                               {
                                                   Id = x.CodEsTabela02,
                                                   Descricao = x.DscEsTabela02
                                               }).OrderBy(x => x.Id).ToListAsync(ct);


            return Ok(listaRetorno);
        }


        [HttpGet("lista/codigoCBO")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaCodigoCBO(CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.Cbo
                                                .Where(x => x.IndAtivo == "S")
                                                .Select(x => new RetornoListaDTO()
                                                {
                                                    Id = x.CodIdentificador,
                                                    Descricao = x.DscCbo
                                                }).OrderBy(x => x.Id).ToListAsync(ct);

            return Ok(listaRetorno);
        }

        #endregion

        #endregion

        #region F2501

        #region Tabela 29

        [HttpGet("lista/codigo-receita")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaCodigoReceita(CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.EsTabela29
                                               .Where(x => x.IndAtivo == "S")
                                               .Select(x => new RetornoListaDTO()
                                               {
                                                   Id = x.CodEsTabela29,
                                                   Descricao = x.DscEsTabela29
                                               }).OrderBy(x => x.Id).ToListAsync(ct);


            return Ok(listaRetorno);
        }

        [HttpGet("lista/codigo-receita-IRRF")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaCodigoReceitaIRRF(CancellationToken ct)
        {

            //var listaRetorno = EnumExtension.ToList<EsocialCodigoIrrf>()
            //             .Select(x => new RetornoListaDTO()
            //             {
            //                 Id = x.ToInt(),
            //                 Descricao = x.Descricao()
            //             }
            //             );
            var listaRetorno = _eSocialDbContext.EsTabelaCrIrrf
                                    .Select(x => new RetornoListaUFDTO
                                    {
                                        Id = x.CodCrirrf.ToString().PadLeft(6, '0'),
                                        Descricao = x.NomCrirrf
                                    });

            return Ok(listaRetorno);
        }

        [HttpGet("lista/tipo-processo-IRRF")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoProcessoIRRF(CancellationToken ct)
        {
            var listaRetorno = new List<RetornoListaDTO>()
            {
                new RetornoListaDTO() { Id = 1, Descricao = "Administrativo" },
                new RetornoListaDTO() { Id = 2, Descricao = "Judicial" },
            };

            return Ok(listaRetorno);
        }

        [HttpGet("lista/valores/ind-apuracao")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaIndApuracaoValores(CancellationToken ct)
        {
            var listaRetorno = new List<RetornoListaDTO>()
            {
                new RetornoListaDTO() { Id = 1, Descricao = "Mensal" },
                new RetornoListaDTO() { Id = 2, Descricao = "Anual (13° salário)" },
            };

            return Ok(listaRetorno);
        }

        [HttpGet("lista/deducao/tipo-deducoes")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoDeducoesValores(CancellationToken ct)
        {
            var listaRetorno = new List<RetornoListaDTO>()
            {
                new RetornoListaDTO() { Id = 1, Descricao = "1 - Previdência oficial" },
                new RetornoListaDTO() { Id = 5, Descricao = "5 - Pensão alimentícia" },
                new RetornoListaDTO() { Id = 7, Descricao = "7 - Dependentes" }
            };

            return Ok(listaRetorno);
        }

        #endregion

        #region Tipo Rendimento
        [HttpGet("lista/tipo-rendimento")]
        public async Task<ActionResult<IEnumerable<RetornoListaDTO>>> ObterListaTipoRendimento()
        {
            var listaRetorno = EnumExtension.ToList<ESocialTipoRendimento>()
             .Select(x => new RetornoListaDTO()
             {
                 Id = x.ToInt(),
                 Descricao = x.Descricao()
             }
             );

            return Ok(listaRetorno);
        }
        #endregion

        #endregion
    }
}
