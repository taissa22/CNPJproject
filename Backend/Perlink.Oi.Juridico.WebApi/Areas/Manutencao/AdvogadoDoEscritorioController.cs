using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Evento;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    ///<summary>
    /// Api controller "Escritorio"
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/advogadoescritorio")]
    public class AdvogadoDoEscritorioController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas as Escritorio
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="estado"></param>
        ///<param name="escritorioId"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>
        ///<param name="pesquisa"></param>
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IAdvogadoDoEscritorioRepository repository,
            [FromQuery] string pesquisa,
            [FromQuery] string estado,
            [FromQuery] int escritorioId ,
            [FromQuery] int pagina = 1,
            [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc"
            )
        {
            return Result(repository.ObterPaginado(escritorioId,estado, pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, AdvogadodoEscritorioSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }
 

        ///<sumary>
        /// Cria novo registro de Advogado do Escritorio
        ///</sumary>
        ///<param name = "dados" ></ param >
        ///< returns ></ returns >
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IAdvogadoDoEscritorioService service,
            [FromBody] CriarAdvogadoDoEscritorioCommand dados)
        {
            return Result(service.Criar(dados));
        }

        ///<summary>
        /// Atualiza registro de Advogado do Escritorio
        ///</summary>
        ///<param name = "service" ></ param >
        ///< param name="dados"></param>
        ///<returns></returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IAdvogadoDoEscritorioService service,
            [FromBody] AtualizarAdvogadoDoEscritorioCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        ///<summary>
        /// Exclui registro de Escritorio
        ///</summary>
        ///<param name="service"></param>
        ///  ///<param name="id"></param>
        ///  ///<param name="escritorioId"></param>
        ///  <returns></returns>
        [HttpDelete("{id}/{escritorioId}")]
        public IActionResult Remover(
            [FromServices] IAdvogadoDoEscritorioService service,
            [FromRoute] int id,
            [FromRoute] int escritorioId)
        {
            return Result(service.Remover(id,escritorioId));
        }

     
    
        ///<sumary>
        /// Exporta as Escritorio
        ///</sumary>
        /// <param name="repository"></param> 
        /// <param name="estado"></param>
        /// <param name="areaAtuacao"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult ExportarEscritorio(
            [FromServices] IEscritorioRepository repository,
            [FromQuery] string estado ,
            [FromQuery] int areaAtuacao,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            var resultado = repository.Obter(estado, areaAtuacao, EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                
                csv.AppendLine($"Tipo de Pessoa;Nome;CNPJ;CPF;Endereço;Bairro;Cidade;CEP;Estado;Email;Site;DDD;Telefone;DDD;Celular;DDD;Fax;Lotes de Juizado;Alerta da Agenda;Cível Consumidor;Cível Estratégico;Tributário Administrativo;Juizado;Trabalhista;Criminal Administrativo;Criminal Judicial;Cível Administrativo;Procon;Pex;Código SAP;Ativo");

                foreach (var a in resultado.Dados)
                {
                    var cnpj = a.CNPJ != null ? $"'{a.CNPJ}" : string.Empty;
                    var cpf = a.CPF != null ? $"'{a.CPF}" : string.Empty;

                    csv.Append($"\"{(a.TipoPessoaValor == "J" ? "Juridica" : "Fisica") }\";");
                    csv.Append($"\"{ a.Nome }\";");
                    csv.Append($"\"{ cnpj  }\";");
                    csv.Append($"\"{ cpf  }\";");
                    csv.Append($"\"{ a.Endereco }\";");
                    csv.Append($"\"{ a.Bairro }\";");
                    csv.Append($"\"{ a.Cidade }\";");
                    csv.Append($"\"{ a.CEP }\";");
                    csv.Append($"\"{ a.EstadoId }\";");
                    csv.Append($"\"{ a.Email }\";");
                    csv.Append($"\"{ a.Site }\";");
                    csv.Append($"\"{ a.TelefoneDDD }\";");
                    csv.Append($"\"{ a.Telefone }\";");
                    csv.Append($"\"{ a.CelularDDD }\";");
                    csv.Append($"\"{ a.Celular }\";");
                    csv.Append($"\"{ a.FaxDDD }\";");
                    csv.Append($"\"{ a.Fax }\";");
                    csv.Append($"\"{ a.LoteJuizado}\";");
                    csv.Append($"\"{ a.AlertaEm }\";");
                    csv.Append($"\"{ (a.IndAreaCivel == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ (a.CivelEstrategico == true ? "Sim" : "Não" )}\";");
                    csv.Append($"\"{ (a.IndAreaTributaria == true ? "Sim" : "Não" )}\";");
                    csv.Append($"\"{ (a.IndAreaJuizado == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ (a.IndAreaTrabalhista == true ? "Sim" : "Não" )}\";");
                    csv.Append($"\"{ (a.IndAreaCriminalAdministrativo == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ (a.IndAreaCriminalJudicial == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ (a.IndAreaCivelAdministrativo == true ? "Sim" : "Não" )}\";");
                    csv.Append($"\"{ (a.IndAreaProcon == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ (a.IndAreaPEX == true ? "Sim" : "Não" ) }\";");
                    csv.Append($"\"{ a.CodProfissionalSAP }\";");                    
                    csv.Append($"\"{(a.Ativo ? "Ativo" : "Inativo") }\";");
                    csv.AppendLine("");
                }
                string nomeArquivo = $"Escritorios_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }


        ///<sumary>
        /// Exporta as Escritorio
        ///</sumary>
        /// <param name="repository"></param> 
        /// <param name="estado"></param>
        /// <param name="areaAtuacao"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportaratuacao")]
        public IActionResult ExportarAtuacaoEscritorio(
            [FromServices] IEscritorioRepository repository,
            [FromServices] IEscritorioEstadoRepository repositoryAtuacao,
            [FromQuery] string estado,
            [FromQuery] int areaAtuacao,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            var resultado = repository.Obter(estado, areaAtuacao, EnumHelpers.ParseOrDefault(coluna, EscritorioSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                csv.AppendLine($"Nome;CNPJ;CPF;Área de Atuação - Civel Consumidor; Área de Atuação - Juizado");

                foreach (var a in resultado.Dados)
                {
                    var atuacao = repositoryAtuacao.Obter(a.Id,0);

                    foreach (var b in atuacao.Dados)
                    {
                        csv.Append($"\"{(a.Nome)}\";");
                        csv.Append($"\"{(a.CNPJ)}\";");
                        csv.Append($"\"{(a.CPF)}\";");
                        csv.Append($"\"{(b.TipoProcessoId == 1 ? b.Id : "")}\";");
                        csv.Append($"\"{(b.TipoProcessoId == 7 ? b.Id : "")}\";");
                        csv.AppendLine("");
                    }
                }
                string nomeArquivo = $"Estados_de_Atuacao_do_Escritório_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }


        ///<sumary>
        /// Exporta as Escritorio
        ///</sumary>
        /// <param name="repository"></param> 
        /// <param name="estado"></param>
        /// <param name="areaAtuacao"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportaradvogado")]
        public IActionResult ExportarAdvogadoEscritorio(
            [FromServices] IAdvogadoDoEscritorioRepository repository,
            [FromServices] IAdvogadoDoEscritorioRepository repositoryAdvogado,
            [FromQuery] string estado,
            [FromQuery] int escritorioId,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            var resultado = repository.Obter(escritorioId,estado, EnumHelpers.ParseOrDefault(coluna, AdvogadodoEscritorioSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                csv.AppendLine($"Estado OAB;Nro OAB;Nome;Telefone;Contato do Escritório;Email");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{(a.EstadoId)}\";");
                    csv.Append($"\"{(a.NumeroOAB)}\";");
                    csv.Append($"\"{(a.Nome)}\";");
                    csv.Append($"\"{("(" + a.CelularDDD + ")" + a.Celular)}\";");
                    csv.Append($"\"{(a.EhContato == true ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Email)}\";");
                    csv.AppendLine("");
                }
                string nomeArquivo = $"Advogados_do_Escritorio_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }


    }
}