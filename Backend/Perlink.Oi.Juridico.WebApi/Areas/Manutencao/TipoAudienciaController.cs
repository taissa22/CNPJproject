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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller que resolve as requisições de tipos de audiências
    /// </summary>
    [Authorize]
    [Route("manutencao/tipos-de-audiencia")]
    [ApiController]
    public class TipoAudienciaController : ApiControllerBase
    {

        /// <summary>
        /// Lista todos os tipos de audiências
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] ITipoAudienciaRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] int? tipoProcesso = null,
            [FromQuery] string pesquisa = null)
        {
            if (quantidade == 0)
            {
                return Result(repository.ObterComboboxTipoProcesso());
            }

            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoAudienciaSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
               tipoProcesso.HasValue ? TipoProcesso.PorId(tipoProcesso.Value) : TipoProcesso.NAO_DEFINIDO, pesquisa));
        }

        [HttpGet("ObterDescricaoDeParaCivelConsumidor")]
        public IActionResult ObterDescricaoDeParaCivelConsumidor([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoAudiencia>>.Valid(dbContext.TipoAudiencias.AsNoTracking().Where(x => x.TipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR).ToList()));
        }

        [HttpGet("ObterDescricaoDeParaCivelEstrategico")]
        public IActionResult ObterDescricaoDeParaCivelEstrategico([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoAudiencia>>.Valid(dbContext.TipoAudiencias.AsNoTracking().Where(x => x.TipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO).ToList()));
        }



        /// <summary>
        /// Cria um novo registro de tipos de audiências
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ITipoAudienciaService service,
            [FromBody] CriarTipoAudienciaCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de tipos de audiências
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] ITipoAudienciaService service,
            [FromBody] AtualizarTipoAudienciaCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de tipos de audiências
        /// </summary>
        /// <param name="service"></param>
        /// <param name="tipoAudienciaId"></param>
        /// <returns></returns>
        [HttpDelete("{tipoAudienciaId}")]
        public IActionResult Remover([FromServices] ITipoAudienciaService service, [FromRoute] int tipoAudienciaId)
        {
            return Result(service.Remover(tipoAudienciaId));
        }

        /// <summary>
        /// Exporta os tipos de audiências
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="tipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] ITipoAudienciaRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] int? tipoProcesso = null,
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, TipoAudienciaSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), tipoProcesso.HasValue ? TipoProcesso.PorId(tipoProcesso.Value) : TipoProcesso.NAO_DEFINIDO, pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                var dataHora = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var listaTipoProcesso = resultado.Dados.Select(x => x.TipoProcesso).Distinct().ToList();

                using (var compressedFileStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false, Encoding.UTF8))
                    {
                        foreach (var item in listaTipoProcesso)
                        {
                            var listaPorProcesso = resultado.Dados.Where(x => x.TipoProcesso == item);

                            StringBuilder csv = new StringBuilder();

                            TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                            string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcesso.PorId(item.Id).Nome.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");
                            string nomeArquivo = $"Tipo_Audiencia_{tipoProcessoNome}_{dataHora}.csv";

                            if (tipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id)
                            {
                                csv.AppendLine("Código;Sigla;Descrição;Tipo de Processo;Ativo;Link;Correspondente Cível Estratégico (DE x PARA migração de processo);Correspondente Cível Estratégico Ativo");
                                foreach (var a in listaPorProcesso)
                                {
                                    csv.Append($"\"{a.CodigoTipoAudiencia}\";");
                                    csv.Append($"\"{(!string.IsNullOrEmpty(a.Sigla) ? a.Sigla : string.Empty)}\";");
                                    csv.Append($"\"{a.Descricao}\";");
                                    csv.Append($"\"{(a.TipoProcesso != null ? a.TipoProcesso.Nome : string.Empty)}\";");
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                                    csv.Append($"\"{(a.LinkVirtual ? "Sim" : "Não")}\";");
                                    csv.Append($@"{a.DescricaoMigracao};");
                                    if (a.DescricaoMigracao == null)
                                    {
                                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                                    }
                                    else
                                    {
                                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                                    }
                                    csv.AppendLine("");
                                }
                            } 
                            else if (tipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id)
                            {
                                csv.AppendLine("Código;Sigla;Descrição;Tipo de Processo;Ativo;Link;Correspondente Cível Consumidor (DE x PARA migração de processo);Correspondente Cível Consumidor Ativo");
                                foreach (var a in listaPorProcesso)
                                {
                                    csv.Append($"\"{a.CodigoTipoAudiencia}\";");
                                    csv.Append($"\"{(!string.IsNullOrEmpty(a.Sigla) ? a.Sigla : string.Empty)}\";");
                                    csv.Append($"\"{a.Descricao}\";");
                                    csv.Append($"\"{(a.TipoProcesso != null ? a.TipoProcesso.Nome : string.Empty)}\";");
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                                    csv.Append($"\"{(a.LinkVirtual ? "Sim" : "Não")}\";");
                                    csv.Append($@"{a.DescricaoMigracao};");
                                    if(a.DescricaoMigracao == null)
                                    {
                                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                                    } else
                                    {
                                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                                    }
                                    csv.AppendLine("");
                                }
                            }
                            else {
                                csv.AppendLine("Código;Sigla;Descrição;Tipo de Processo;Ativo;Link");
                                foreach (var a in listaPorProcesso)
                                {
                                    csv.Append($"\"{a.CodigoTipoAudiencia}\";");
                                    csv.Append($"\"{(!string.IsNullOrEmpty(a.Sigla) ? a.Sigla : string.Empty)}\";");
                                    csv.Append($"\"{a.Descricao}\";");
                                    csv.Append($"\"{(a.TipoProcesso != null ? a.TipoProcesso.Nome : string.Empty)}\";");
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                                    csv.Append($"\"{(a.LinkVirtual ? "Sim" : "Não")}\";");
                                    csv.AppendLine("");
                                }
                            }

                            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();

                            //Criar uma entrada para cada anexo a ser "Zipado"
                            var zipEntry = zipArchive.CreateEntry(nomeArquivo);

                            //Pegar o stream do anexo
                            using (var originalFileStream = new MemoryStream(bytes))
                            {
                                using (var zipEntryStream = zipEntry.Open())
                                {
                                    //Copia o anexo na memória para a entrada ZIP criada
                                    originalFileStream.CopyTo(zipEntryStream);
                                }
                            }

                        }
                    }
                    return File(compressedFileStream.ToArray(), "application/zip", $"Tipo_Audiencia_{dataHora}.zip");
                }
            }

            return NotFound();
        }
    }
}
