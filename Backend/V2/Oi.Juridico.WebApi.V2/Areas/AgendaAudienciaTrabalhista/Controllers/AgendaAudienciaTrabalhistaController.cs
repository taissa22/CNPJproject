using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.AtmPexContext.Entities;
using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Services;
using Perlink.Oi.Juridico.Domain.Logs.Entity;
using System.Security.Cryptography;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Controllers
{
    [Route("api/agenda/[controller]")]
    [ApiController]
    public class AgendaAudienciaTrabalhistaController : ControllerBase
    {
        private AgendaAudienciaContext _agendaDbContext;

        public AgendaAudienciaTrabalhistaController(AgendaAudienciaContext agendaDbContext)
        {
            _agendaDbContext = agendaDbContext;
        }

        #region consultas

        [HttpPost("consulta/agenda-trabalhista/{ordenarPor}")]
        public async Task<ActionResult<RetornoPaginadoDTO<VAgendaTrabalhistaDTO>>> ConsultarAgendaTrabalhista([FromBody] AgendaTrabalhistaRequestDTO requestDTO,
                                                                                                         [FromRoute] int? ordenarPor,
                                                                                                         [FromServices] AgendaAudienciaService service,
                                                                                                         [FromQuery] int? pagina = 1,
                                                                                                         [FromQuery] int? quantidade = 50,
                                                                                                         CancellationToken ct = default)
        {

            var loginUsuario = User.Identity!.Name;

            var usuarioEmpresaDoGrupo = _agendaDbContext.AcaUsuarioEmpresaGrupo.AsNoTracking().Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte).ToListAsync(ct);
            requestDTO.Empresa = usuarioEmpresaDoGrupo.Result;

            #region preenche datas se vazias

            if (!requestDTO.DataAudienciaDe.HasValue)
            {
                DateTime hoje = DateTime.Today;

                DateTime segundaProximaSemana = hoje.AddDays(7 - (int)hoje.DayOfWeek + (int)DayOfWeek.Monday);

                DateTime sextaProximaSemana = segundaProximaSemana.AddDays(4);

                requestDTO.DataAudienciaDe = segundaProximaSemana;
                requestDTO.DataAudienciaAte = sextaProximaSemana;
            }
            #endregion

            var (dadosInvalido, listaErros) = requestDTO.Validar();

            if (dadosInvalido)
            {
                return BadRequest(listaErros);
            }

            var resultado = new RetornoPaginadoDTO<VAgendaTrabalhistaDTO>();
            
            resultado = await service.ConsultarAgenda(requestDTO, ordenarPor, pagina, quantidade, resultado, ct);

            return resultado;

        }

        [HttpPost("consulta/agenda-trabalhista-estado/{ordenarPor}")]
        public async Task<ActionResult<RetornoPaginadoDTO<VAgendaTrabalhistaDTO>>> ConsultarAgendaTrabalhistaPorEstado([FromBody] AgendaTrabalhistaRequestDTO requestDTO,
                                                                                                         [FromRoute] int? ordenarPor,                                                                                                         
                                                                                                         [FromServices] AgendaAudienciaService service,
                                                                                                         [FromQuery] int? pagina = 1,
                                                                                                         [FromQuery] int? quantidade = 50,
                                                                                                         CancellationToken ct = default)
        {

            var loginUsuario = User.Identity!.Name;

            var usuarioEmpresaDoGrupo = _agendaDbContext.AcaUsuarioEmpresaGrupo.AsNoTracking().Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte).ToListAsync(ct);
            requestDTO.Empresa = usuarioEmpresaDoGrupo.Result;

            #region preenche datas se vazias

            if (!requestDTO.DataAudienciaDe.HasValue)
            {
                DateTime hoje = DateTime.Today;

                DateTime segundaProximaSemana = hoje.AddDays(7 - (int)hoje.DayOfWeek + (int)DayOfWeek.Monday);

                DateTime sextaProximaSemana = segundaProximaSemana.AddDays(4);

                requestDTO.DataAudienciaDe = segundaProximaSemana;
                requestDTO.DataAudienciaAte = sextaProximaSemana;
            }
            #endregion

            var (dadosInvalido, listaErros) = requestDTO.Validar();

            if (dadosInvalido)
            {
                return BadRequest(listaErros);
            }

            var resultado = new RetornoPaginadoDTO<VAgendaTrabalhistaDTO>();

            resultado = await service.ConsultarAgendaPorEstado(requestDTO, ordenarPor, pagina, quantidade, resultado, ct);

            return resultado;

        }

        [HttpGet("consulta/usuario-associado")]
        public async Task<ActionResult<bool>> ObterListaClassificacaoHierarquica(CancellationToken ct)
        {
            var loginUsuario = User.Identity!.Name;

            var usuarioAssociado = await _agendaDbContext.AcaUsuarioEmpresaGrupo.AnyAsync(u => u.CodUsuario == loginUsuario, ct);

            return Ok(usuarioAssociado);
        }

        #endregion

        #region edição

        [HttpPut("alteracao/agenda-preposto")]
        public async Task<ActionResult> EditaAgendaPreposto([FromBody] IEnumerable<AgendaPrepostoRequestDTO> requestDTO, [FromServices] AgendaAudienciaService service, CancellationToken ct)
        {
            var loginUsuario = User.Identity!.Name;

            using var scope = await _agendaDbContext.Database.BeginTransactionAsync(ct);
            _agendaDbContext.PesquisarPorCaseInsensitive();

            var listaErros = new List<string>();

            foreach (var item in requestDTO)
            {
                var (dadosInvalido, listaErrosDTO) = item.Validar();

                if (dadosInvalido)
                {
                    listaErros = listaErrosDTO.ToList();
                }

                var agendaPreposto = await _agendaDbContext.ReclamadaPrepostoAudiencia.FirstOrDefaultAsync(x => x.CodProcesso == item.CodProcesso && x.SeqAudiencia == item.SeqAudiencia && x.CodParte == item.CodParte, ct);
                var novaAgenda = agendaPreposto == null;
                
                service.PreencheEntidadeAgendaPreposto(item, ref agendaPreposto, User);

                if(agendaPreposto!.CodPreposto is null)
                {
                    _agendaDbContext.Remove(agendaPreposto);

                    try
                    {
                        _agendaDbContext.ExecutarProcedureDeLog(User.Identity!.Name, true);
                        await _agendaDbContext.SaveChangesAsync(ct);
                    }
                    catch (Exception ex)
                    {
                        await scope.RollbackAsync(ct);
                        return BadRequest(new { mensagem = "Falha ao editar preposto.", erro = ex.Message });
                    }
                    
                    continue;

                }

                if (novaAgenda)
                {
                    _agendaDbContext.Add(agendaPreposto!);
                }


                try
                {
                    dadosInvalido = listaErros.Count > 0;

                    if (dadosInvalido)
                    {
                        await scope.RollbackAsync(ct);

                        return BadRequest(listaErros);
                    }

                    _agendaDbContext.ExecutarProcedureDeLog(User.Identity!.Name, true);
                    await _agendaDbContext.SaveChangesAsync(ct);

                }
                catch (Exception ex)
                {
                    await scope.RollbackAsync(ct);
                    return BadRequest(new { mensagem = "Falha ao editar preposto.", erro = ex.Message });
                }
            }

            await scope.CommitAsync(ct);
            return Ok();
        }

        #endregion

        #region Exportações
        [HttpPost("exportacao/exportar-agenda/{ordenarPor}")]
        public async Task<ActionResult> ExportarAgenda([FromBody] AgendaTrabalhistaRequestDTO requestDTO,
                                                       [FromRoute] int? ordenarPor,
                                                       [FromServices] AgendaAudienciaService service,
                                                        CancellationToken ct)
        {

            var loginUsuario = User.Identity!.Name;

            var usuarioEmpresaDoGrupo = _agendaDbContext.AcaUsuarioEmpresaGrupo.AsNoTracking().Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte).ToListAsync(ct);
            requestDTO.Empresa = usuarioEmpresaDoGrupo.Result;
            requestDTO.EstadoSelecionado = null;

            #region preenche datas se vazias

            if (!requestDTO.DataAudienciaDe.HasValue)
            {
                DateTime hoje = DateTime.Today;

                DateTime segundaProximaSemana = hoje.AddDays(7 - (int)hoje.DayOfWeek + (int)DayOfWeek.Monday);

                DateTime sextaProximaSemana = segundaProximaSemana.AddDays(4);

                requestDTO.DataAudienciaDe = segundaProximaSemana;
                requestDTO.DataAudienciaAte = sextaProximaSemana;
            }
            #endregion

            var (dadosInvalido, listaErros) = requestDTO.Validar();

            if (dadosInvalido)
            {
                return BadRequest(listaErros);
            }

            var resultado = new List<VAgendaTrabalhistaExportarDTO>();

            resultado = await service.ConsultaExportarAgenda(requestDTO, ordenarPor, resultado, ct);

            var ListaAgenda = resultado;

            var csv = ListaAgenda.ToCsvByteArray(typeof(ExportaAgendaRetornoMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "text/csv", $"AGENDA_TRABALHISTA_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");

        }


        #endregion

        
    }
}
