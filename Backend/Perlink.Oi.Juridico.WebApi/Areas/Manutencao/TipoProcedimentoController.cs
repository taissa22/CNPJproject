using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller Indice
    /// </summary>
    /// 
    [Authorize]
    [ApiController]
    [Route("manutencao/tipos-de-procedimento")]

    public class TipoProcedimentoController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os Tipos de Procedimentos
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <param name="codigoTipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("{codigoTipoProcesso}")]
        public IActionResult ObterPaginado(
            [FromServices] ITipoProcedimentoRepository repository,
            [FromRoute] int codigoTipoProcesso,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            if (codigoTipoProcesso <= 0)
            {
                return Result(repository.ObterComboboxTipoProcesso());
            }


            if (quantidade == 0)
            {
                return Result(repository.ObterTodos());
            }

            return Result(repository.ObterPaginado(TipoProcesso.PorId(codigoTipoProcesso), pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoProcedimentoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Obtem tipos de participacao para exibir no combobox
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [HttpGet("ObterComboboxTipoParticipacao")]
        public IActionResult ObterComboboxTipoParticipacao([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoDeParticipacao>>.Valid(dbContext.TiposDeParticipacoes.AsNoTracking().ToList()));
        }

        /// <summary>
        /// Cria um novo registro de Tipo de Procedimento
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ITipoProcedimentoService service,
            [FromBody] CriarTipoProcedimentoCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de índice
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] ITipoProcedimentoService service,
            [FromBody] AtualizarTipoProcedimentoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de Tipo de Procedimento
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public IActionResult Remover(
            [FromServices] ITipoProcedimentoService service,
            [FromRoute] int codigo)
        {
            return Result(service.Remover(codigo));
        }

        /// <summary>
        /// Exporta os Tipo de Procedimento
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="codigoTipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("exportar/{codigoTipoProcesso}")]
        public IActionResult Exportar(
            [FromServices] ITipoProcedimentoRepository repository,
            [FromRoute] int codigoTipoProcesso,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            TipoProcesso tipoProcesso = TipoProcesso.PorId(codigoTipoProcesso);

            var resultado = repository.Obter(tipoProcesso, EnumHelpers.ParseOrDefault(coluna, TipoProcedimentoSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                string colunas = "Código;Descrição;Ativo;Tipo de Processo";

                if (tipoProcesso == TipoProcesso.ADMINISTRATIVO)
                {
                    colunas += ";É Provisionado?;1º Tipo de Participação;É Órgão?;2º Tipo de Participação;É Órgão?;É Polo Passivo Único?";
                }

                if (tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO || tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO)
                {
                    colunas += ";1º Tipo de Participação;2º Tipo de Participação";
                }

                csv.AppendLine(colunas);

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Codigo}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    csv.Append($"\"{(a.IndAtivo ? "Sim" : "Não")}\";");
                    csv.Append($"\"{tipoProcesso.Nome}\";");

                    if (tipoProcesso == TipoProcesso.ADMINISTRATIVO)
                    {
                        colunas += ";É Provisionado?;1º Tipo de Participação;É Órgão?;2º Tipo de Participação;É Órgão?;É Polo Passivo Único?";
                        csv.Append($"\"{(a.IndProvisionado ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.TipoDeParticipacao1 != null ? a.TipoDeParticipacao1.Descricao : string.Empty)}\";");
                        csv.Append($"\"{(a.IndOrgao1.HasValue ? a.IndOrgao1.Value ? "Sim" : "Não" : string.Empty)}\";");
                        csv.Append($"\"{(a.TipoDeParticipacao2 != null ? a.TipoDeParticipacao2.Descricao : string.Empty)}\";");
                        csv.Append($"\"{(a.IndOrgao2.HasValue ? a.IndOrgao2.Value ? "Sim" : "Não" : string.Empty)}\";");
                        csv.Append($"\"{(a.IndPoloPassivoUnico ? "Sim" : "Não")}\";");
                    }

                    if (tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO || tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO)
                    {
                        csv.Append($"\"{(a.TipoDeParticipacao1 != null ? a.TipoDeParticipacao1.Descricao : string.Empty)}\";");                       
                        csv.Append($"\"{(a.TipoDeParticipacao2 != null ? a.TipoDeParticipacao2.Descricao : string.Empty)}\";");
                    }

                    csv.AppendLine("");
                }

                TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcesso.PorId(codigoTipoProcesso).Nome.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");

                string nomeArquivo = $"Tipo_Procedimento_{tipoProcessoNome}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";

                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

    }
}
