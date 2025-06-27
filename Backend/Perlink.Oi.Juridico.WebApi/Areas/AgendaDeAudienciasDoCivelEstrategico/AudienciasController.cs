using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Commands;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Filters;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Services;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller responsável pelas requisições de audiencias.
    /// </summary>
    [Authorize]
    [Route("agenda-de-audiencias-do-civel-estrategico/audiencias")]
    [ApiController]
    public class AudienciasController : ApiControllerBase {

        /// <summary>
        /// Lista todas as audiências cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpPost("Obter")]
        public IActionResult ObterPaginado([FromServices] IAudienciaRepository repository, [FromBody] AgendaDeAudienciaDoCivelEstrategicoFilter filter) {
            return Result(repository.ObterAudienciasPorUsuarioLogado(filter));
        }

        /// <summary>
        /// Lista todas as audiências cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpPost("ObterPorProcesso")]
        public IActionResult ObterPorProcessoPaginado([FromServices] IAudienciaRepository repository, [FromBody] AgendaDeAudienciaDoCivelEstrategicoFilter filter) {
            return Result(repository.ObterAudienciasPorProcessoPaginado(filter.ProcessoId ?? 0, filter.Pagina, filter.Quantidade));
        }

        /// <summary>
        /// Atualiza um novo orgão
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IAudienciaService service, [FromBody] AtualizarAudienciaCommand command) {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exporta audiencias
        /// </summary>
        [HttpPost("exportar")]
        public IActionResult Exportar([FromServices] IAudienciaRepository repository,
            [FromBody] AgendaDeAudienciaDoCivelEstrategicoFilter filter) {
            CommandResult<IReadOnlyCollection<AudienciaDoProcesso>> resultado = repository.ObterAudienciasPorProcesso(filter);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("ESTADO; COMARCA; VARA; DATA DA AUDIÊNCIA; HORA DA AUDIÊNCIA; TIPO AUDIENCIA; PREPOSTO; " +
                    "ESCRITÓRIO AUDIÊNCIA; ADVOGADO ESCRITÓRIO AUDIÊNCIA; ESTRATÉGICO; NÚMERO PROCESSO; ENDEREÇO; " +
                    "ESCRITÓRIO PROCESSO; CLASSIFICAÇÃO CLOSING MÓVEL; CLASSIFICAÇÃO CLOSING FIBRA; CLASSIFICAÇÃO HIERÁRQUICA");
                                                                               
                foreach (var x in resultado.Dados) {
                    csv.Append($"\"{(x.Processo.Comarca != null ? x.Processo.Comarca.EstadoId : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.Comarca != null ? x.Processo.Comarca.Nome : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.Vara != null ? x.Processo.Vara.VaraId.ToString() : string.Empty)}\";");
                    csv.Append($"\"{(x.DataAudiencia != null ? x.DataAudiencia.ToShortDateString() : string.Empty)}\";");
                    csv.Append($"\"{(x.HoraAudiencia != null ? x.HoraAudiencia.ToString("HH:mm") : string.Empty)}\";");
                    csv.Append($"\"{(x.TipoAudiencia != null ? x.TipoAudiencia.Descricao : string.Empty)}\";");
                    csv.Append($"\"{(x.Preposto != null ? x.Preposto.Nome : string.Empty)}\";");
                    csv.Append($"\"{(x.Escritorio != null ? x.Escritorio.Nome : string.Empty)}\";");
                    csv.Append($"\"{(x.AdvogadoEscritorio != null ? x.AdvogadoEscritorio.Nome : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.TipoProcessoId == Infra.Enums.TipoProcesso.CIVEL_ESTRATEGICO.Id ? "SIM" : "NÃO")}\";");
                    csv.Append($"=\"{(!string.IsNullOrEmpty(x.Processo.NumeroProcesso) ? x.Processo.NumeroProcesso : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.Escritorio != null ? x.Processo.Escritorio.Endereco : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.Escritorio != null ? x.Processo.Escritorio.Nome : string.Empty)}\";");
                    csv.Append($"\"{(x.Processo.Closing == 1 ? "PRÉ" : x.Processo.Closing == 2 ? "PÓS" : x.Processo.Closing == 3 ? "HÍBRIDO" : x.Processo.Closing == 4 ? "N/A" : x.Processo.Closing == 0 ? "A DEFINIR" : " ")}\";");
                    csv.Append($"\"{(x.Processo.ClosingClientCo == 1 ? "PRÉ" : x.Processo.ClosingClientCo == 2 ? "PÓS" : x.Processo.ClosingClientCo == 3 ? "HÍBRIDO" : x.Processo.ClosingClientCo == 4 ? "N/A" : x.Processo.ClosingClientCo == 0 ? "A DEFINIR" : " ")}\";");
                    csv.Append($"\"{(x.Processo.ClassificacaoProcessoId.Equals("U") ? "ÚNICO" : x.Processo.ClassificacaoProcessoId.Equals("P") ? "PRIMÁRIO" : x.Processo.ClassificacaoProcessoId.Equals("S") ? "SECUNDÁRIO" : string.IsNullOrEmpty(x.Processo.ClassificacaoProcessoId) ? "NÃO DEFINICO" : " ")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Audiencias_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}