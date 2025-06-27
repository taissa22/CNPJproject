using Microsoft.AspNetCore.Authorization; 
using Oi.Juridico.Contextos.V2.AtmCCContext.Data;
using Oi.Juridico.Contextos.V2.AtmCCContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data; 
using Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.DTOs;
using Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.Services; 
using SixLabors.ImageSharp;
using System.IO;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AgendamentoRelatorioATMController : ControllerBase
    {
        private readonly AtmCCContext _db;
        private readonly ParametroJuridicoContext _dbParametroJuridico;
        public AgendamentoRelatorioATMController(AtmCCContext db, ParametroJuridicoContext dbParametroJuridico)
        {
            _db = db;
            _dbParametroJuridico = dbParametroJuridico;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult> Listar(int pagina , [FromServices] AgendamentoRelatorioATMService service, CancellationToken ct)
        {
            try
            {
                var (agendamentos, total, mensagemErro) = await service.Listar(pagina,30, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(new PaginatedQueryResult<AgendRelatorioAtmCc>()
                {
                    Data = agendamentos,
                    Total = total
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
              
        }


        [HttpPost("Criar")] 
        public async Task<ActionResult> IncluirAgendamentoAsync(AgendamentoDTO requestDTO, [FromServices] AgendamentoRelatorioATMService service, CancellationToken ct)
        {
            try
            {
                var mensagem = await service.SalvarAgendamentoAsync(requestDTO, User.Identity!.Name!, ct);

                if (mensagem != "OK")
                {
                    return BadRequest(mensagem);
                }
                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BaixarRelatorio/{id}")]
        public async Task<ActionResult> BaixarRelatorio(decimal id)
        {
            try
            {
                var agendamento = _db.AgendRelatorioAtmCc.Find(id);
                if (agendamento == null) return BadRequest("Agendamento não encontrado.");


                var caminhosUpload = await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_RELATORIO_ATM_CC", $"{agendamento.NomArquivoRelatorio}");

                foreach (var caminhoUpload in caminhosUpload)
                {
                    if (System.IO.File.Exists(caminhoUpload))
                    {

                        var bytes = System.IO.File.ReadAllBytes(caminhoUpload);
                        return File(bytes, "application/zip", agendamento.NomArquivoRelatorio);
                    }
                }

                return BadRequest("Arquivo não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BaixarBase/{id}")]
        public async Task<ActionResult> BaixarArquivoZip(decimal id)
        {
            try
            {
                var agendamento = _db.AgendRelatorioAtmCc.Find(id);
                if (agendamento == null) return BadRequest("Agendamento não encontrado.");

                var caminhosUpload = await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_RELATORIO_ATM_CC", $"{agendamento.NomArquivoBase}");


                foreach (var caminhoUpload in caminhosUpload)
                {
                    if (System.IO.File.Exists(caminhoUpload))
                    {

                        var bytes = System.IO.File.ReadAllBytes(caminhoUpload);
                        return File(bytes, "application/zip", agendamento.NomArquivoBase);
                    }
                }

                return BadRequest("Arquivo não encontrado.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarFechamentos")]
        public async Task<ActionResult> ListarFechamentos([FromQuery] int pagina )
        { 

            var dtos = await (from fpm in _db.FechCivelConsMedia
                              join emp in _db.EmpresasCentralizadoras
                              on fpm.EmpceCodEmpCentralizadora equals emp.Codigo
                              join usu in _db.AcaUsuario
                              on fpm.UsrCodUsuario equals usu.CodUsuario
                              join sfc in _db.SolicFechamentoCont
                              on fpm.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                              select new RetornoFechamentoDTO
                              {
                                  Id = fpm.Id,
                                  CodSolicFechamentoCont = fpm.CodSolicFechamentoCont,
                                  DataExecucao = fpm.DataGeracao,
                                  MesAnoFechamento = fpm.MesAnoFechamento,
                                  DataFechamento = fpm.DataFechamento, 
                                  NomeUsuario = usu.NomeUsuario, 
                                  NumeroMeses = fpm.NumMesesMediaHistorica, 
                                  Empresas = emp.Nome,
                                  DataAgendamento = sfc.DatUltimoAgend != null ? sfc.DatUltimoAgend : sfc.DataCadastro
                              }).ToListAsync();

            var resultado = new List<RetornoFechamentoDTO>();

            dtos.ForEach(d =>
            {
                var dto = resultado.FirstOrDefault(r => r.CodSolicFechamentoCont == d.CodSolicFechamentoCont && r.DataFechamento == d.DataFechamento);
                if (dto == null)
                {
                    resultado.Add(d);
                }
                else
                {
                    dto.Empresas += ", " + d.Empresas;
                    if (d.DataExecucao > dto.DataExecucao)
                    {
                        dto.DataExecucao = d.DataExecucao;
                    }
                }
            });

            return Ok(resultado.OrderByDescending(r => r.DataFechamento).ThenByDescending(r => r.DataExecucao));
        }

        [HttpGet("ObterEstados")]
        public async Task<ActionResult> ObterEstados()
        {
            try
            {
                var lista = _db.Estado.Select(x => new { x.CodEstado, x.NomEstado } ).ToList();

                return Ok(lista.OrderBy(x => x.CodEstado));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fechamentos/indices")]
        public IActionResult ObterIndicesDoFechamentoAsync()
        {
            var agendamento = _db.AgendRelatorioAtmCc.OrderByDescending(x => x.DatSolicitacao).FirstOrDefault();

            if (agendamento != null)
            {
                var indices = _db.AgendRelatAtmCcIndiceUf.Where(x => x.CodAgendRelatorioAtm == agendamento.CodAgendRelatorioAtm)
                .AsNoTracking()
                .Select(x => new ObterIndicesDoFechamentoResponse
                {
                    CodEstado = x.Uf,
                    CodIndice = x.CodIndice,
                })
                .ToArray();
                 
                if (indices is null)
                {
                    var lista = ObterIndicesDoFechamentoResponse.GerarListaEstadosComIndiceZero();
                    return Ok(lista);
                }

                return Ok(indices);
            }
            else
            {
                var lista = ObterIndicesDoFechamentoResponse.GerarListaEstadosComIndiceZero();
                return Ok(lista);
            }
          
        }
        [HttpGet("ObterIndices")]
        public async Task<ActionResult> ObterIndices()
        {
            try
            {
                var lista = _db.Indice.Select(x => new {x.NomIndice, x.CodIndice, x.IndAcumulado}).ToList();

                return Ok(lista.OrderBy(x => x.NomIndice));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("excluir-agendamento")] 
        public async Task<ActionResult> ExcluirAgendamentoAsync([FromQuery] int codigo, [FromServices] AgendamentoRelatorioATMService service, CancellationToken ct)
        {
            try
            {
                var (excluido, mensagem) = await service.RemoveAgendamentoAsync(codigo, ct);

                if (!excluido)
                {
                    return BadRequest(mensagem);
                }

                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
