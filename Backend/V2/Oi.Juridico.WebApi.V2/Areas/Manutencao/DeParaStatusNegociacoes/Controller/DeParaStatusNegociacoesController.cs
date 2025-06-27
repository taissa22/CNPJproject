using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Entities;
using Oi.Juridico.Contextos.V2.ManutencaoSolicitanteContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusNegociacoes.DTO;
using System.Linq.Expressions;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusNegociacoes.Controller
{
    [Route("DeParaStatusNegociacoes")]
    [ApiController]
    public class DeParaStatusNegociacoesController : ControllerBase
    {
        #region Propriedades
        private readonly ParametroJuridicoContext _parametroJuridico;
        private readonly ManutencaoDbContext _manutencaoContext;

        #endregion Propriedades 

        #region Construtor
        public DeParaStatusNegociacoesController(ParametroJuridicoContext parametroJuridico, ManutencaoDbContext manutencaoContext)
        {
            _parametroJuridico = parametroJuridico;
            _manutencaoContext = manutencaoContext;
        }

        #endregion Construtor 

        #region Métodos HTTP

        [HttpGet("ObterDeParaStatus")]
        public async Task<IActionResult> ObterDeParaStatus(CancellationToken ct, [FromQuery] int pagina, [FromQuery] int quantidade = 8, [FromQuery] string coluna = "id", [FromQuery] string direcao = "asc", [FromQuery] string? pesquisa = null, int tipoProcesso = 0)
        {
            var query = _manutencaoContext.DeParaStatusNegAppPre.AsNoTracking().Select(x => new DeparaStatusNegociacoesResponse
            {
                Id = x.Id,
                IdStatusApp = x.StatusApp,
                IdSubStatusApp = x.SubstatusApp,
                IdStatusSisjur = x.StatusSisjur,
                DescricaoStatusApp = _manutencaoContext.StatusDeParaNegAppPre.Where(s => s.Id == x.StatusApp).First().Descricao,
                DescricaoSubStatusApp = _manutencaoContext.StatusDeParaNegAppPre.Where(s => s.Id == x.SubstatusApp).First().Descricao,
                DescricaoStatusSisjur = x.CodTipoProcesso == 17 ? _manutencaoContext.StatusAtendimento.Where(s => s.CodStatusAtendimento == x.StatusSisjur).First().DscStatusAtendimento :
                                        _manutencaoContext.StatusContato.Where(s => s.CodStatusContato == x.StatusSisjur).First().DscStatusContato,
                //CriaNegociacoes = x.CriarNegociacoes == "N" ? "Não" : "Sim",
                IdTipoProcesso = x.CodTipoProcesso,
                DescricaoTipoProcesso = GetDescricaoTipoProcesso(tipoProcesso)
            });

            if (pesquisa != null)
            {
                query = query.Where(x => x.DescricaoStatusApp.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()) ||
                                    x.DescricaoSubStatusApp.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()) ||
                                    x.DescricaoStatusSisjur.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()));
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

        [HttpGet("ObterStatusNegociacoesAppPrepostoEStatusSisjur")]
        public IActionResult ObterStatusAppPrepostoEStatusSisjur([FromQuery] int codTipoProcesso)
        {
            var statusAppPreposto = _manutencaoContext.StatusDeParaNegAppPre.Where(x => x.Substatus == "N"
                    && (x.TipoContato == (int)TipoContatoEnum.IlhaNegociacao || x.TipoContato == (int)TipoContatoEnum.PosSentenca))
                                                                                             .OrderBy(x => x.Descricao)
                                                                                             .Select(x => new StatusResponse
                                                                                             {
                                                                                                 Id = x.Id,
                                                                                                 Descricao = x.Descricao
                                                                                             }).AsNoTracking();

            var substatusAppPreposto = _manutencaoContext.StatusDeParaNegAppPre.Where(x => x.Substatus == "S"
                    && (x.TipoContato == (int)TipoContatoEnum.IlhaNegociacao || x.TipoContato == (int)TipoContatoEnum.PosSentenca))
                                                                                                .OrderBy(x => x.Descricao)
                                                                                                .Select(x => new StatusResponse
                                                                                                {
                                                                                                    Id = x.Id,
                                                                                                    Descricao = x.Descricao
                                                                                                }).AsNoTracking();

            IQueryable<StatusResponse>? statusSisjur = null;

            switch (codTipoProcesso)
            {
                case 1:
                case 7:
                    statusSisjur = _manutencaoContext.StatusContato.OrderBy(x => x.DscStatusContato)
                                                                     .Select(x => new StatusResponse
                                                                     {
                                                                         Id = x.CodStatusContato,
                                                                         Descricao = x.DscStatusContato
                                                                     }).AsNoTracking();
                    break;
                case 17:
                    statusSisjur = _manutencaoContext.StatusAtendimento.OrderBy(x => x.DscStatusAtendimento)
                                                                     .Where(x => x.IndAtivo == "S")
                                                                     .Select(x => new StatusResponse
                                                                     {
                                                                         Id = x.CodStatusAtendimento,
                                                                         Descricao = x.DscStatusAtendimento
                                                                     }).AsNoTracking();
                    break;
            }




            return Ok(new { statusAppPreposto, substatusAppPreposto, statusSisjur });
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, decimal id)
        {
            try
            {
                var depara = _manutencaoContext.DeParaStatusNegAppPre.FirstOrDefault(a => a.Id == id);

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
            var query = _manutencaoContext.DeParaStatusNegAppPre.AsNoTracking().Select(x => new DeparaStatusNegociacoesResponse
            {
                Id = x.Id,
                IdStatusApp = x.StatusApp,
                IdSubStatusApp = x.SubstatusApp,
                IdStatusSisjur = x.StatusSisjur,
                DescricaoStatusApp = _manutencaoContext.StatusDeParaNegAppPre.Where(s => s.Id == x.StatusApp).First().Descricao,
                DescricaoSubStatusApp = _manutencaoContext.StatusDeParaNegAppPre.Where(s => s.Id == x.SubstatusApp).First().Descricao,
                DescricaoStatusSisjur = x.CodTipoProcesso == 17 ? _manutencaoContext.StatusAtendimento.Where(s => s.CodStatusAtendimento == x.StatusSisjur).First().DscStatusAtendimento :
                                        _manutencaoContext.StatusContato.Where(s => s.CodStatusContato == x.StatusSisjur).First().DscStatusContato,
                //                CriaNegociacoes = x.CriarNegociacoes == "N" ? "Não" : "Sim",
                IdTipoProcesso = x.CodTipoProcesso,
                DescricaoTipoProcesso = GetDescricaoTipoProcesso(tipoProcesso)
            });

            if (pesquisa != null)
            {
                query = query.Where(x => x.DescricaoStatusApp.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()) ||
                                    x.DescricaoSubStatusApp.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()) ||
                                    x.DescricaoStatusSisjur.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()));
            }

            if (tipoProcesso != 0)
            {
                query = query.Where(x => x.IdTipoProcesso == tipoProcesso);
            }

            var data = direcao == "asc" ? await query.OrderBy(AdicionaOrdenacao(coluna)).ToListAsync(ct) :
                                         await query.OrderByDescending(AdicionaOrdenacao(coluna)).ToListAsync(ct);

            StringBuilder csv = new StringBuilder();

            csv.AppendLine($"Código;Tipo de Processo;Status / Substatus APP Preposto (DE);Status Correspondente no SISJUR (PARA)");

            foreach (var x in data)
            {
                csv.Append($"\"{x.Id}\";");
                csv.Append($"\"{x.DescricaoTipoProcesso}\";");
                csv.Append($"\"{x.DescricaoStatusApp + @" / " + x.DescricaoSubStatusApp}\";");
                csv.Append($"\"{x.DescricaoStatusSisjur}\";");
                //csv.Append($"\"{x.CriaNegociacoes}\";");
                csv.AppendLine("");
            }

            string nomeArquivo = $"DExPARA_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());

            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();

            return File(bytes, "text/csv", nomeArquivo);

        }

        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] DeParaNegociacoesDTO dto)
        {
            try
            {
                var deParaExistente = _manutencaoContext.DeParaStatusNegAppPre.Where(x => x.StatusApp == dto.StatusAPPId &&
                                                                                          x.CodTipoProcesso == dto.TipoProcesso &&
                                                                                          x.SubstatusApp == dto.SubstatusAPPId)
                                                                              .Include(x => x.SubstatusAppNavigation)
                                                                              .Include(x => x.StatusAppNavigation).FirstOrDefault();

                if (deParaExistente != null)
                {

                    return BadRequest(MensagemDuplicidade(deParaExistente));
                }

                var dePara = new DeParaStatusNegAppPre();

                dePara.StatusApp = (byte)dto.StatusAPPId;
                dePara.SubstatusApp = dto.SubstatusAPPId;
                dePara.StatusSisjur = dto.StatusSisjurId;
                //dePara.CriarNegociacoes = dto.CriaNegociacoes ? "S" : "N";
                dePara.CodTipoProcesso = dto.TipoProcesso;

                _manutencaoContext.DeParaStatusNegAppPre.Add(dePara);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private string MensagemDuplicidade(DeParaStatusNegAppPre deParaExistente)
        {
            var subStatusDescricao = deParaExistente.SubstatusAppNavigation?.Descricao;
            subStatusDescricao = subStatusDescricao == null ? " (SEM SUBSTATUS) " : subStatusDescricao;
            var mensagem = "A dupla " + deParaExistente.StatusAppNavigation.Descricao + " / " +
                                        subStatusDescricao +
                           " já tem uma correspondência registrada para o tipo de processo selecionado!";

            return mensagem;
        }


        [HttpPut, Route("Alterar")]
        public async Task<IActionResult> AlterarAsync(CancellationToken ct, [FromBody] DeParaNegociacoesDTO dto)
        {
            try
            {
                var deParaExistente = _manutencaoContext.DeParaStatusNegAppPre.Where(x => x.StatusApp == dto.StatusAPPId &&
                                                                                            x.CodTipoProcesso == dto.TipoProcesso &&
                                                                                            x.SubstatusApp == dto.SubstatusAPPId &&
                                                                                            x.Id != dto.Id)
                                                                              .Include(x => x.SubstatusAppNavigation)
                                                                              .Include(x => x.StatusAppNavigation).FirstOrDefault();

                if (deParaExistente != null)
                {
                    return BadRequest(MensagemDuplicidade(deParaExistente));
                }

                var dePara = _manutencaoContext.DeParaStatusNegAppPre.Where(x => x.Id == dto.Id).First();

                dePara.StatusApp = dto.StatusAPPId;
                dePara.SubstatusApp = dto.SubstatusAPPId;
                dePara.StatusSisjur = dto.StatusSisjurId;
                //dePara.CriarNegociacoes = dto.CriaNegociacoes ? "S" : "N";
                dePara.CodTipoProcesso = dto.TipoProcesso;

                _manutencaoContext.DeParaStatusNegAppPre.Update(dePara);

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

        private Expression<Func<DeparaStatusNegociacoesResponse, object>> AdicionaOrdenacao(string coluna)
        {
            switch (coluna)
            {
                case "id":
                    return m => m.Id;

                case "descricaoStatusApp":
                    return m => m.DescricaoStatusApp;

                case "descricaoStatusSisjur":
                    return m => m.DescricaoStatusSisjur;

                //case "criacaoAutomaticaNovaNegociacoes":
                //    return m => m.CriaNegociacoes;

                case "tipoProcesso":
                    return m => m.DescricaoTipoProcesso;

                default:
                    return m => m.Id;
            }
        }

        private string GetDescricaoTipoProcesso(int codTipoProcesso)
        {
            switch (codTipoProcesso)
            {
                case 1:
                    return "CÍVEL CONSUMIDOR";
                case 7:
                    return "JUIZADO ESPECIAL CÍVEL";
                case 17:
                    return "PROCON";
                default:
                    return string.Empty;
            }
        }

        #endregion Métodos Auxiliares
    }
}
