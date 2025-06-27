using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.RequestDTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Controllers
{
    [Route("api/agenda/[controller]")]
    [ApiController]
    public class AgendaListasController : ControllerBase
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private AgendaAudienciaContext _agendaDbContext;

        public AgendaListasController(ParametroJuridicoContext parametroJuridicoDbContext, AgendaAudienciaContext agendaDbContext)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _agendaDbContext = agendaDbContext;
        }       

        [HttpPost("lista/uf")]
        public async Task<ActionResult<IEnumerable<RetornoListaUfDTO>>> ObterUFAsync()
        {
            try
            {
                var _listaEstado = await _agendaDbContext.Estado
                .Select(e => new RetornoListaUfDTO()
                {
                    Id = e.CodEstado,
                    Descricao = e.CodEstado,
                    IdDescricao= string.Format("{0} - {1}", e.CodEstado, e.NomEstado)
                    
                }).OrderBy(e => e.Id).ToListAsync();

                return _listaEstado;

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpGet("lista/tipo-audiencia")]
        public async Task<ActionResult<IEnumerable<RetornoAgendaDTO>>> ObterListaTipoAudiencia()
        {

            var _listaTipoAudiencia = await _agendaDbContext.TipoAudiencia
                .Select(e => new RetornoAgendaDTO()
                {
                    Id = e.CodTipoAud,
                    Descricao = e.DscTipoAudiencia

                }).OrderBy(e => e.Id).ToListAsync();

            return _listaTipoAudiencia;
        }

        [HttpPost("lista/prepostos/{permiteInativos}")]
        public async Task<ActionResult<IEnumerable<RetornoAgendaDTO>>> ObterListaPrepostos([FromBody] AgendaTrabalhistaRequestDTO requestDTO, [FromServices] AgendaAudienciaService service, CancellationToken ct, bool permiteInativos = false)
        {

            var _listaPreposto = await _agendaDbContext.Preposto
                .Where(x => x.IndPrepostoAtivo == "S" && x.IndPrepostoTrabalhista == "S")
                .Select(e => new RetornoAgendaDTO()
                {
                    Id = e.CodPreposto,
                    Descricao = e.NomPreposto

                }).OrderBy(e => e.Descricao).ToListAsync();

            if (permiteInativos)
            {
            var _listaPrepostoInativo = await _agendaDbContext.Preposto
                .Where(x => x.IndPrepostoAtivo == "N" && x.IndPrepostoTrabalhista == "S")
                .Select(e => new RetornoAgendaDTO()
                {
                    Id = e.CodPreposto,
                    Descricao = e.NomPreposto + " [INATIVO]"

                }).OrderBy(e => e.Descricao).ToListAsync();

                _listaPreposto.AddRange(_listaPrepostoInativo);
                
            }

            var _listaJaSelecionados = await service.ConsultarAgendaPorEstadoPreposto(requestDTO, ct);

            if (!permiteInativos && _listaJaSelecionados is not null && _listaJaSelecionados.Count  > 0)
            {
              var _listaPrepostoSelecionado =  await _agendaDbContext.Preposto
               .Where(x => x.IndPrepostoAtivo == "N" && _listaJaSelecionados.Contains(x.CodPreposto))
               .Select(e => new RetornoAgendaDTO()
               {
                   Id = e.CodPreposto,
                   Descricao = e.NomPreposto + " [INATIVO]"

               }).OrderBy(e => e.Descricao).ToListAsync();

                if (_listaPrepostoSelecionado is not null && _listaPrepostoSelecionado.Count > 0)
                {
                    _listaPreposto.AddRange(_listaPrepostoSelecionado);
                }

            }




            return _listaPreposto;
        }

        [HttpGet("lista/modalidade-audiencia")]
        public async Task<ActionResult<IEnumerable<RetornoAgendaDTO>>> ObterListaModalidadeAudiencia([FromServices] AgendaAudienciaService service,
                                                        CancellationToken ct)
        {

            var _listaModalidadeAudiencia = await service.ConsultaModalidadeAudiencia(ct);

            return _listaModalidadeAudiencia;
        }

        [HttpGet("lista/localidade-audiencia")]
        public async Task<ActionResult<IEnumerable<RetornoAgendaDTO>>> ObterListaLocalidadeAudiencia([FromServices] AgendaAudienciaService service,
                                                        CancellationToken ct)
        {

            var _listaLocalidadeAudiencia = await service.ConsultaLocalidaeAudiencia(ct);

            return _listaLocalidadeAudiencia;
        }
    }
}
