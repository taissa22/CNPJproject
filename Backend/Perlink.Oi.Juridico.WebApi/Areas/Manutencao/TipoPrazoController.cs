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
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller que resolve as requisições de ações do cível consumidor
    /// </summary>
    [Authorize]
    [Route("manutencao/tipos-de-prazo")]
    [ApiController]
    public class TipoPrazoController : ApiControllerBase
    {

        /// <summary>
        /// Lista todos os tipos de prazo
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("{tipoProcesso}")]
        public IActionResult ObterPaginado(
            [FromServices] ITipoPrazoRepository repository,
            [FromRoute] double tipoProcesso,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            if ( quantidade == 0 )
            {
                return Result(repository.ObterComboboxTipoProcesso());
            }

            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoPrazoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                TipoProcessoManutencao.PorId(tipoProcesso) , pesquisa));
        }

        [HttpGet("ObterDescricaoDeParaCivelConsumidor")]
        public IActionResult ObterDescricaoDeParaCivelConsumidor([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoPrazo>>.Valid(dbContext.TiposPrazos.AsNoTracking().Where(x => x.TipoProcesso == TipoProcessoManutencao.CIVEL_CONSUMIDOR).ToList()));
        }

        [HttpGet("ObterDescricaoDeParaCivelEstrategico")]
        public IActionResult ObterDescricaoDeParaCivelEstrategico([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoPrazo>>.Valid(dbContext.TiposPrazos.AsNoTracking().Where(x => x.TipoProcesso == TipoProcessoManutencao.CIVEL_ESTRATEGICO).ToList()));
        }


        /// <summary>
        /// Cria um novo registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ITipoPrazoService service,
            [FromBody] CriarTipoPrazoCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] ITipoPrazoService service,
            [FromBody] AtualizarTipoPrazoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] ITipoPrazoService service, [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Exporta as ações do cível consumidor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="tipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("exportar/{tipoProcesso}")]
        public IActionResult Exportar(
            [FromServices] ITipoPrazoRepository repository,
            [FromRoute] double tipoProcesso,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",            
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, TipoPrazoSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), TipoProcessoManutencao.PorId(tipoProcesso), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                var colunas = "Código;Descrição;Tipo de Processo;";

                if (new[] {TipoProcessoManutencao.CIVEL_CONSUMIDOR.Id, TipoProcessoManutencao.JEC.Id, TipoProcessoManutencao.PROCON.Id}.Contains(tipoProcesso))
                {
                    colunas += "Prazo de Serviço;";
                }

                if (new[] { TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO.Id, TipoProcessoManutencao.CRIMINAL_JUDICIAL.Id }.Contains(tipoProcesso))
                {
                   colunas += "Prazo referente a documento;";
                }

                colunas += "Ativo";

                if (new[] { TipoProcessoManutencao.CIVEL_CONSUMIDOR.Id }.Contains(tipoProcesso))
                {
                    colunas += ";Correspondente Cível Estratégico (DE x PARA migração de processo);Correspondente Cível Estratégico Ativo";
                }

                if (new[] { TipoProcessoManutencao.CIVEL_ESTRATEGICO.Id }.Contains(tipoProcesso))
                {
                    colunas += ";Correspondente Cível Consumidor (DE x PARA migração de processo);Correspondente Cível Consumidor Ativo";
                }

                csv.AppendLine(colunas);                

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{a.Descricao}\";");
                    csv.Append($"\"{TipoProcessoManutencao.PorId(tipoProcesso).Descricao}\";");
                    if (new[] { TipoProcessoManutencao.CIVEL_CONSUMIDOR.Id, TipoProcessoManutencao.JEC.Id, TipoProcessoManutencao.PROCON.Id }.Contains(tipoProcesso))
                    {
                        csv.Append($"\"{(a.Eh_Servico.GetValueOrDefault() ? "Sim" : "Não")}\";");
                    }
                    if (new[] { TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO.Id, TipoProcessoManutencao.CRIMINAL_JUDICIAL.Id }.Contains(tipoProcesso))
                    {
                        csv.Append($"\"{(a.Eh_Documento ? "Sim" : "Não")}\";");
                    }

                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");

                    if (new[] { TipoProcessoManutencao.CIVEL_CONSUMIDOR.Id }.Contains(tipoProcesso))
                    {
                        csv.Append($@"{a.DescricaoMigracao};");

                        if (a.DescricaoMigracao == null)
                        {
                            csv.Append($"\"{(a.AtivoDePara.HasValue && a.AtivoDePara.Value ? "Sim" : "")}\";");
                        }
                        else
                        {
                            csv.Append($"\"{(a.AtivoDePara.HasValue && a.AtivoDePara.Value ? "Sim" : "Não")}\";");
                        }

                    }

                    if (new[] { TipoProcessoManutencao.CIVEL_ESTRATEGICO.Id }.Contains(tipoProcesso))
                    {
                        csv.Append($@"{a.DescricaoMigracao};");
                        if(a.DescricaoMigracao == null)
                        {
                            csv.Append($"\"{(a.AtivoDePara.HasValue && a.AtivoDePara.Value ? "Sim" : "")}\";");
                        } else if (a.DescricaoMigracao == a.DescricaoMigracao)
                        {
                            csv.Append($"\"{(a.AtivoDePara.HasValue && a.AtivoDePara.Value ? "Sim" : "Não")}\";");
                        }
                    }
                    csv.AppendLine("");
                }

                TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcessoManutencao.PorId(tipoProcesso).Descricao.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");

                string nomeArquivo = $"Tipo_Prazo_{tipoProcessoNome}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
