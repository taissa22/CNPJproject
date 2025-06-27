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
    /// Controller que resolve as requisições de ações do cível consumidor
    /// </summary>
    [Authorize]    
    [Route("manutencao/tipos-de-documentos")]
    [ApiController]
    public class TipoDocumentoController : ApiControllerBase
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
            [FromServices] ITipoDocumentoRepository repository,
            [FromRoute] int tipoProcesso,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",            
            [FromQuery] string pesquisa = null)
        {
            if (tipoProcesso <= 0)
            {
                return Result(repository.ObterComboboxTipoProcesso());
            }

            return Result(repository.ObterPaginado(TipoProcesso.PorId(tipoProcesso), pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoDocumentoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                pesquisa));
        }

        /// <summary>
        /// Obtem tipos de processo que o usuário tem permissão para exibir no combobox
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet("ObterComboboxTiposPrazo")]
        public IActionResult ObterComboboxTiposPrazo([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoPrazo>>.Valid(dbContext.TiposPrazos.AsNoTracking().ToList()));
        }

        [HttpGet("ObterDescricaoDeParaCivelEstrategico")]
        public IActionResult ObterDescricaoDeParaCivelEstrategico([FromServices] IDatabaseContext dbContext)
        {            
            return Result(CommandResult<IEnumerable<TipoDocumento>>.Valid(dbContext.TiposDocumentos.AsNoTracking().Where(x=> x.CodTipoProcesso ==9).ToList()));
        }

        [HttpGet("ObterDescricaoDeParaCivelConsumidor")]
        public IActionResult ObterDescricaoDeParaCivelConsumidor([FromServices] IDatabaseContext dbContext)
        {
            return Result(CommandResult<IEnumerable<TipoDocumento>>.Valid(dbContext.TiposDocumentos.AsNoTracking().Where(x => x.CodTipoProcesso == 1).ToList()));
        }

        /// <summary>
        /// Cria um novo registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ITipoDocumentoService service,
            [FromBody] CriarTipoDocumentoCommand dados)
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
        public IActionResult Atualizar([FromServices] ITipoDocumentoService service,
            [FromBody] AtualizarTipoDocumentoCommand dados)
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
        public IActionResult Remover([FromServices] ITipoDocumentoService service, [FromRoute] int id)
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
            [FromServices] ITipoDocumentoRepository repository,
            [FromRoute] int tipoProcesso,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(TipoProcesso.PorId(tipoProcesso), EnumHelpers.ParseOrDefault(coluna, TipoDocumentoSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                string colunas = "Código;Descrição;Ativo;Tipo de Processo";


                if (tipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id)
                {
                    colunas += ";Cadastra Processo;Prioritário na fila de cadastro de processo;Documento de Apuração?;Enviar App Preposto?;Correspondente Cível Estratégico (DE x PARA migração de processo);Correspondente Cível Estratégico Ativo";                   
                }

                if (tipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id)
                {
                    colunas += ";Correspondente Cível Consumidor (DE x PARA migração de processo);Correspondente Cível Consumidor Ativo";
                }

                if (tipoProcesso == TipoProcesso.JEC.Id)
                {
                    colunas += ";Cadastra Processo;Prioritário na fila de cadastro de processo;Requer Data de Audiência / Prazo;Documento de Apuração?;Enviar App Preposto?";
                }

                if (tipoProcesso == TipoProcesso.PROCON.Id)
                {
                    colunas += ";Cadastra Processo;Utilizado em Protocolo;Documento de Apuração?;Enviar App Preposto?";
                }

                if (tipoProcesso == TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id || tipoProcesso == TipoProcesso.CRIMINAL_JUDICIAL.Id)
                {
                    colunas += ";Tipo de Prazo";
                }

                csv.AppendLine(colunas);

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{a.Descricao}\";");
                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.TipoProcesso != null ? a.TipoProcesso.Nome : string.Empty)}\";");                    

                    if (tipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id)
                    {
                        csv.Append($"\"{(a.MarcadoCriacaoProcesso? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndPrioritarioFila ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndDocumentoApuracao ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndEnviarAppPreposto ? "Sim" : "Não")}\";");
                        csv.Append($@"{a.DescricaoMigracao};");
                        if (a.DescricaoMigracao == null)
                        {
                            csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                        }
                        else
                        {
                            csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                        }
                    }

                    if (tipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id)
                    {
                        csv.Append($@"{a.DescricaoMigracao};");
                        if (a.DescricaoMigracao == null)
                        {
                            csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                        }
                        else
                        {
                            csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                        }
                    }

                    if (tipoProcesso == TipoProcesso.JEC.Id)
                    {
                        csv.Append($"\"{(a.MarcadoCriacaoProcesso ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndPrioritarioFila ? "Sim" : "Não")}\";");   
                        csv.Append($"\"{(a.IndRequerDatAudiencia ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndDocumentoApuracao ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndEnviarAppPreposto ? "Sim" : "Não")}\";");
                    }

                    if (tipoProcesso == TipoProcesso.PROCON.Id)
                    {
                        csv.Append($"\"{(a.MarcadoCriacaoProcesso ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndDocumentoProtocolo ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndDocumentoApuracao ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.IndEnviarAppPreposto ? "Sim" : "Não")}\";");
                    }

                    if (tipoProcesso == TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id || tipoProcesso == TipoProcesso.CRIMINAL_JUDICIAL.Id)
                    {
                        csv.Append($"\"{(a.CodTipoPrazo.HasValue ? a.TipoPrazo.Descricao : string.Empty)}\";");
                    }

                    csv.AppendLine("");
                }
                
                TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcesso.PorId(tipoProcesso).Nome.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");

                string nomeArquivo = $"TipoDocumento_{tipoProcessoNome}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
