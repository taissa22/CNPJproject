using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// EndPoint que resolve requisições dos assuntos do tipo Civel Consumidor
    /// </summary>
    [Authorize]
    [Route("manutencao/assuntos/civel-consumidor")]
    [ApiController]
    public class AssuntosCivelConsumidorController : ApiControllerBase
    {

        /// <summary>
        /// Recuperar todos os assuntos pelo id do processo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginadoDoCivelConsumidor([FromServices] IAssuntoRepository repository, [FromQuery] int pagina, 
            [FromQuery] int quantidade = 8, [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc", 
            [FromQuery] string descricao = null)
        {
            return Result(repository.ObterPaginadoDoCivelConsumidor(pagina, quantidade, 
                EnumHelpers.ParseOrDefault(coluna, AssuntoCivelConsumidorSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), descricao));
        }
        
        /// <summary>
        /// Insere um novo assunto cível consumidor
        /// <param name="service"></param>
        /// <param name="command"></param>
        /// </summary>
        [HttpPost]
        public IActionResult Criar([FromServices] IAssuntoService service, 
            [FromBody] CriarAssuntoDoCivelConsumidorCommand command)
        {
            return Result(service.CriarDoCivelConsumidor(command));
        }

        /// <summary>
        /// Atualiza um assunto civel consumidor
        /// <param name="service"></param>      
        /// <param name="command"></param>
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IAssuntoService service, 
            [FromBody] AtualizarAssuntoDoCivelConsumidorCommand command)
        {
            return Result(service.AtualizarDoCivelConsumidor(command));
        }

        /// <summary>
        /// Exclui um assunto cível consumidor
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAssuntoService service, 
            [FromRoute] int id)
        {
            return Result(service.RemoverDoCivelConsumidor(id));
        }

        /// <summary>
        /// Exporta os assuntos do módulo cível consumidor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IAssuntoRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc", 
            [FromQuery] string descricao = null)
        {
            var resultado = repository.ObterDoCivelConsumidor(EnumHelpers.ParseOrDefault(coluna, AssuntoCivelConsumidorSort.Descricao),
            string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), descricao);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CÓDIGO;DESCRIÇÃO;PROPOSTA;NEGOCIAÇÃO;CÁLCULO DA CONTINGÊNCIA; CORRESPONDENTE CÍVEL ESTRATÉGICO(DExPARA MIGRAÇÃO DE PROCESSO); CORRESPONDENTE CÍVEL ESTRATÉGICO ATIVO");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{x.Descricao}\";");
                    csv.Append($"\"{x.Proposta}\";");
                    csv.Append($"\"{x.Negociacao}\";");
                    csv.Append($"\"{(string.IsNullOrEmpty(x.CodTipoContingencia) ? string.Empty :  (x.CodTipoContingencia.Equals("M") ? "MÉDIA" : "PROCESSO A PROCESSO"))}\";");                    
                    csv.Append($"\"{(string.IsNullOrEmpty(x.DescricaoMigracao) ? string.Empty : x.DescricaoMigracao)}\";");
                    if (x.DescricaoMigracao == null)
                    {
                        csv.Append($"\"{(x.AtivoDePara ? "Sim" : "")}\";");
                    }
                    else
                    {
                        csv.Append($"\"{(x.AtivoDePara ? "Sim" : "Não")}\";");
                    }
                    csv.AppendLine("");
                }

                string nomeArquivo = $"assuntos_cc_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        [HttpGet("ObterDescricaoDeParaCivelConsumidor")]
        public IActionResult ObterDescricaoDeParaCivelConsumidor([FromServices] IAssuntoRepository repository)
        {
            return Result(repository.ObterDescricaoDeParaCivelConsumidor());
        }
    }
}