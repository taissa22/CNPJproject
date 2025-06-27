using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusAudiencia.Dtos;
using System.Linq.Expressions;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusAudiencia.Controllers
{
    [Route("DeparaStatusAudiencia")]
    [ApiController]
    public class DeparaStatusAudienciaController : ControllerBase
    {
        #region Propriedades 

        private readonly ParametroJuridicoContext _parametroJuridico;
        private ManutencaoDbContext _manutencaoContext;
        //private readonly ILogger<RelatorioMovimentacoesCivelConsumidorController> _logger;

        #endregion Propriedades 

        #region Construtor 

        public DeparaStatusAudienciaController(ParametroJuridicoContext parametroJuridico, ManutencaoDbContext manutencaoContext)
        {
            _parametroJuridico = parametroJuridico;
            _manutencaoContext = manutencaoContext;
        }

        #endregion Construtor 

        #region Métodos HTTP

        [HttpGet("ObterDeParaStatus")]
        public async Task<IActionResult> ObterDeParaStatus(CancellationToken ct, [FromQuery] int pagina, [FromQuery] int quantidade = 8, [FromQuery] string coluna = "id", [FromQuery] string direcao = "asc", [FromQuery] string? pesquisa = null, int tipoProcesso = 0)
        {
            using var scope = await _manutencaoContext.Database.BeginTransactionAsync(ct);

            _manutencaoContext.PesquisarPorCaseInsensitive();

            var query = _manutencaoContext.DeParaStatusAppPreposto.AsNoTracking().Select(x => new DeparaStatusAudienciaResponse
            {
                Id = x.Id,
                IdStatusApp = x.StatusApp,
                DescricaoStatusApp = x.StatusAppNavigation.Descricao,
                IdSubStatusApp = x.SubstatusApp,
                DescricaoSubStatusApp = x.SubstatusAppNavigation.Descricao,
                IdStatusSisjur = x.StatusSisjur,
                DescricaoStatusSisjur = x.StatusSisjurNavigation.IndAtivo == "S" ? x.StatusSisjurNavigation.DscStatusAudiencia : x.StatusSisjurNavigation.DscStatusAudiencia + " - [INATIVO]",
                CriacaoAutomaticaNovaAudiencia = x.CriarAudiencia == "N" ? "Não" : "Sim",
                IdTipoProcesso = x.CodTipoProcesso,
                DescricaoTipoProcesso = x.CodTipoProcesso == 1 ? "CÍVEL CONSUMIDOR" : x.CodTipoProcesso == 7 ? "JUIZADO ESPECIAL CÍVEL" : x.CodTipoProcesso == 17 ? "PROCON" : ""
            });

            if (pesquisa != null)
            {
                pesquisa = pesquisa.ToUpper().Trim();

                query = query.Where(x => x.DescricaoStatusApp.Contains(pesquisa) ||
                                    x.DescricaoSubStatusApp.Contains(pesquisa) ||
                                    x.DescricaoStatusSisjur.Contains(pesquisa));
            }

            if (tipoProcesso != 0)
            {
                query = query.Where(x => x.IdTipoProcesso == tipoProcesso);
            }

            var total = await query.CountAsync(ct);

            var data = direcao == "asc" ? await query.OrderBy(AdicionaOrdenacao(coluna)).Skip((pagina - 1) * quantidade).Take(quantidade).ToListAsync(ct) :
                                          await query.OrderByDescending(AdicionaOrdenacao(coluna)).Skip((pagina - 1) * quantidade).Take(quantidade).ToListAsync(ct);

            return Ok(new { data, total });
        }
        
        [HttpGet("ObterStatusAppPrepostoEStatusSisjur")]
        public IActionResult ObterStatusAppPrepostoEStatusSisjur()
        {
            var statusAppPreposto = _manutencaoContext.StatusDeParaAppPreposto.AsNoTracking().Where(x => x.Substatus == "N" && x.TipoContato == (int)TipoContatoEnum.Pauta)
                                                                                             .OrderBy(x => x.Descricao)
                                                                                             .Select(x => new StatusResponse
                                                                                             {
                                                                                                 Id = x.Id,
                                                                                                 Descricao = x.Descricao
                                                                                             });

            var substatusAppPreposto = _manutencaoContext.StatusDeParaAppPreposto.AsNoTracking().Where(x => x.Substatus == "S" && x.TipoContato == (int)TipoContatoEnum.Pauta)
                                                                                                .OrderBy(x => x.Descricao)
                                                                                                .Select(x => new StatusResponse
                                                                                                {
                                                                                                    Id = x.Id,
                                                                                                    Descricao = x.Descricao
                                                                                                });

            var statusSisjur = _manutencaoContext.StatusAudiencia.AsNoTracking()
                                                                 .Where(x => x.IndAtivo.ToUpper().Trim() == "S")
                                                                 .OrderBy(x => x.DscStatusAudiencia)
                                                                 .Select(x => new StatusResponse
                                                                 {
                                                                     Id = x.CodStatusAudiencia,
                                                                     Descricao = x.DscStatusAudiencia
                                                                 });

            return Ok(new { statusAppPreposto, substatusAppPreposto, statusSisjur });
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, decimal id)
        {
            try
            {
                var depara = _manutencaoContext.DeParaStatusAppPreposto.FirstOrDefault(a => a.Id == id);

                if (depara is null)
                {
                    return BadRequest("DE x PARA não encontrado");
                }

                _manutencaoContext.Remove(depara);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Exportar")]
        public async Task<IActionResult> Exportar(CancellationToken ct, [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc", [FromQuery] string? pesquisa = "", [FromQuery] int tipoProcesso = 0)
        {
            using var scope = await _manutencaoContext.Database.BeginTransactionAsync(ct);

            _manutencaoContext.PesquisarPorCaseInsensitive();

            var query = _manutencaoContext.DeParaStatusAppPreposto.AsNoTracking().Select(x => new DeparaStatusAudienciaResponse
            {
                Id = x.Id,
                IdStatusApp = x.StatusApp,
                DescricaoStatusApp = x.StatusAppNavigation.Descricao,
                IdSubStatusApp = x.SubstatusApp,
                DescricaoSubStatusApp = x.SubstatusAppNavigation.Descricao,
                IdStatusSisjur = x.StatusSisjur,
                DescricaoStatusSisjur = x.StatusSisjurNavigation.IndAtivo == "S" ? x.StatusSisjurNavigation.DscStatusAudiencia : x.StatusSisjurNavigation.DscStatusAudiencia + " - [INATIVO]",
                CriacaoAutomaticaNovaAudiencia = x.CriarAudiencia == "N" ? "Não" : "Sim",
                IdTipoProcesso = x.CodTipoProcesso,
                DescricaoTipoProcesso = x.CodTipoProcesso == 1 ? "CÍVEL CONSUMIDOR" : x.CodTipoProcesso == 7 ? "JUIZADO ESPECIAL CÍVEL" : x.CodTipoProcesso == 17 ? "PROCON" : ""
            });

            if (pesquisa != null)
            {
                pesquisa = pesquisa.ToUpper().Trim();

                query = query.Where(x => x.DescricaoStatusApp.Contains(pesquisa) ||
                                    x.DescricaoSubStatusApp.Contains(pesquisa) ||
                                    x.DescricaoStatusSisjur.Contains(pesquisa));
            }

            if (tipoProcesso != 0)
            {
                query = query.Where(x => x.IdTipoProcesso == tipoProcesso);
            }

            var data = direcao == "asc" ? await query.OrderBy(AdicionaOrdenacao(coluna)).ToListAsync(ct) :
                                         await query.OrderByDescending(AdicionaOrdenacao(coluna)).ToListAsync(ct);

            StringBuilder csv = new StringBuilder();

            csv.AppendLine($"Código;Tipo de Processo;Status / Substatus APP Preposto (DE);Status Correspondente no SISJUR (PARA);Criação Automática de Nova Audiência");

            foreach (var x in data)
            {
                csv.Append($"\"{x.Id}\";");
                csv.Append($"\"{x.DescricaoTipoProcesso}\";");
                csv.Append($"\"{x.DescricaoStatusApp + @" / " + x.DescricaoSubStatusApp}\";");
                csv.Append($"\"{x.DescricaoStatusSisjur}\";");
                csv.Append($"\"{x.CriacaoAutomaticaNovaAudiencia}\";");
                csv.AppendLine("");
            }

            string nomeArquivo = $"DExPARA_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());

            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();

            return File(bytes, "text/csv", nomeArquivo);

        }

        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] IncluirDeParaRequest dto)
        {
            try
            {
                var deParaExistente = _manutencaoContext.DeParaStatusAppPreposto.Where(x => x.StatusApp == dto.StatusAPPId &&
                                                                                            x.CodTipoProcesso == dto.TipoProcesso &&
                                                                                            x.SubstatusApp == dto.SubstatusAPPId).FirstOrDefault();

                if (deParaExistente != null)
                {
                    return BadRequest("A dupla " + _manutencaoContext.StatusDeParaAppPreposto.Where(s => s.Id == deParaExistente.StatusApp).AsNoTracking().First().Descricao + " / " +
                                                   _manutencaoContext.StatusDeParaAppPreposto.Where(s => s.Id == deParaExistente.SubstatusApp).AsNoTracking().First().Descricao + " já tem uma correspondência registrada para o tipo de processo selecionado!");
                }

                var dePara = new DeParaStatusAppPreposto();

                dePara.StatusApp = (byte)dto.StatusAPPId;
                dePara.SubstatusApp = (byte)dto.SubstatusAPPId;
                dePara.StatusSisjur = (byte)dto.StatusSisjurId;
                dePara.CriarAudiencia = dto.CriarAudienciaAutomatica ? "S" : "N";
                dePara.CodTipoProcesso = (byte) dto.TipoProcesso;

                _manutencaoContext.DeParaStatusAppPreposto.Add(dePara);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut, Route("Alterar")]
        public async Task<IActionResult> AlterarAsync(CancellationToken ct, [FromBody] AlterarDeParaRequest dto)
        {
            try
            {
                var deParaExistente = _manutencaoContext.DeParaStatusAppPreposto.Where(x => x.StatusApp == dto.StatusAPPId &&
                                                                                            x.CodTipoProcesso == dto.TipoProcesso &&
                                                                                            x.SubstatusApp == dto.SubstatusAPPId &&
                                                                                            x.Id != dto.Id).FirstOrDefault();

                if (deParaExistente != null)
                {
                    return BadRequest("A dupla " + _manutencaoContext.StatusDeParaAppPreposto.Where(s => s.Id == deParaExistente.StatusApp).AsNoTracking().First().Descricao + " / " +
                                                   _manutencaoContext.StatusDeParaAppPreposto.Where(s => s.Id == deParaExistente.SubstatusApp).AsNoTracking().First().Descricao + " já tem uma correspondência registrada para o tipo de processo selecionado!");
                }

                var dePara = _manutencaoContext.DeParaStatusAppPreposto.Where(x => x.Id == dto.Id).First();

                dePara.StatusApp = (byte)dto.StatusAPPId;
                dePara.SubstatusApp = (byte)dto.SubstatusAPPId;
                dePara.StatusSisjur = (byte)dto.StatusSisjurId;
                dePara.CriarAudiencia = dto.CriarAudienciaAutomatica ? "S" : "N";
                dePara.CodTipoProcesso = (byte)dto.TipoProcesso;

                _manutencaoContext.DeParaStatusAppPreposto.Update(dePara);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Alterado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Métodos HTTP

        #region Métodos Auxiliares

        private Expression<Func<DeparaStatusAudienciaResponse, object>> AdicionaOrdenacao(string coluna)
        {
            switch (coluna)
            {
                case "id":
                    return m => m.Id;

                case "descricaoStatusApp":
                    return m => m.DescricaoStatusApp;

                case "descricaoStatusSisjur":
                    return m => m.DescricaoStatusSisjur;

                case "criacaoAutomaticaNovaAudiencia":
                    return m => m.CriacaoAutomaticaNovaAudiencia;

                case "tipoProcesso":
                     return m => m.DescricaoTipoProcesso;

                default:
                    return m => m.Id;
            }
        }

        #endregion Métodos Auxiliares
    }
}
