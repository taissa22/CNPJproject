using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Evento;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
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
    /// Api controller "Esfera"
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/evento")]
    public class EventoController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas as Evento
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="tipoProcesso"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>
        ///<param name="pesquisa"></param>
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IEventoRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "id",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            return Result(repository.ObterPaginado(TipoProcessoManutencao.PorId(tipoProcesso), pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, EventoSort.Id), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        [HttpGet("ObterDependente")]
        public IActionResult Obterdependente(
            [FromServices] IEventoRepository repository,
            [FromQuery] int eventoId,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "id",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterDependentePaginado(pagina, quantidade, string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), eventoId));
        }

        [HttpGet("ObterDisponiveis")]
        public IEnumerable<EventoDisponivelCommandResult> ObterDisponiveis(
           [FromServices] IEventoRepository repository,
           [FromQuery] int eventoId)
        {
            return repository.ObterDisponiveis(eventoId);
        }

        ///<sumary>
        /// Cria registro de nova Evento
        ///</sumary>
        ///<param name = "dados" ></ param >
        ///< returns ></ returns >
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IEventoService service,
            [FromBody] CriarEventoCommand dados)
        {
            return Result(service.Criar(dados));
        }

        ///<summary>
        /// Atualiza registro de esfera
        ///</summary>
        ///<param name = "service" ></ param >
        ///< param name="dados"></param>
        ///<returns></returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEventoService service,
            [FromBody] AtualizarEventoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        ///<summary>
        /// Atualiza registro de esfera
        ///</summary>
        ///<param name = "service" ></ param >
        ///< param name="dados"></param>
        ///<returns></returns>
        [HttpPut("atualizar-dependentes")]
        public IActionResult AtualizarDependetes(
            [FromServices] IEventoService service,
            [FromBody] AtualizarEventoDependenteCommand dados)
        {
            return Result(service.AtualizarDependente(dados));
        }

        ///<summary>
        /// Exclui registro de esfera
        ///</summary>
        ///<param name="service"></param>
        ///  ///<param name="codigoEvento"></param>
        ///  <returns></returns>
        [HttpDelete("{codigoEvento}/{tipoProcesso}")]
        public IActionResult Remover(
            [FromServices] IEventoService service,
            [FromRoute] int codigoEvento,
            [FromRoute] int tipoProcesso)
        {
            return Result(service.Remover(tipoProcesso, codigoEvento));
        }

        ///<sumary>
        /// Exporta as Evento
        ///</sumary>
        /// <param name="repository"></param>
        /// <param name="decisaorepository"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEventoRepository repository,
            [FromServices] IDecisaoEventoRepository decisaorepository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(TipoProcessoManutencao.PorId(tipoProcesso), EnumHelpers.ParseOrDefault(coluna, EventoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                switch (tipoProcesso)
                {
                    //CIVEL_CONSUMIDOR
                    case 1:
                        csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Cumprimento de Prazo;Notificar Via E-mail;Ativo;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial;Correspondente Cível Consumidor (DExPARA migração de processo;Correspondente Cível Consumidor Ativo)");
                        break;

                    //Trabalhista
                    case 2:
                        csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Cumprimento de Prazo;Rever Cálculo;Finalização Escritório;Finalização Contábil;Altera e Exclui;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Portencial;Rever Cálculo da Decisão;Decisão Default");
                        break;

                    //ADMINSTRATIVO
                    case 3:
                        csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Cumprimento de Prazo;Preenche Multa;Instância;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial");
                        break;

                    //TRIBUTARIO ADMINSTRATIVO
                    case 4:
                        csv.AppendLine($"Código;Descrição do Evento;Ativo;Possui Decisão;Cumprimento de Prazo;Ocultar Evento Web;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial");
                        break;

                    //TRIBUTARIO JUDICIAL
                    case 5:
                        csv.AppendLine($"Código;Descrição do Evento;Ativo;Possui Decisão;Cumprimento de Prazo;Ocultar Evento Web;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial");
                        break;

                    //TRABALHISTA_ADM
                    case 6:
                        csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial");
                        break;

                    //CIVEL_ESTRATEGICO
                    case 9:
                        csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Cumprimento de Prazo;Notificar Via E-mail;Ativo;Descrição da Decisão;Alteração do Risco de Perda;Risco de Perda Potencial;Correspondente Cível Consumidor (DExPARA migração de processo;Correspondente Cível Consumidor Ativo)");
                        break;

                    default:
                        csv.AppendLine($"");
                        break;
                }

                foreach (var a in resultado.Dados)
                {
                    var resultadoDecisao = decisaorepository.Obter(a.Id, EnumHelpers.ParseOrDefault(coluna, DecisaoEventoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

                    if (resultadoDecisao.Dados.Count == 0)
                    {
                        csv.Append($"\"{a.Id}\";");
                        csv.Append($"\"{(a.Nome)}\";");

                        switch (tipoProcesso)
                        {
                            //CIVEL_CONSUMIDOR
                            case 1:
                                {                                   
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.NotificarViaEmail.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                                    csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoEstrategico) ? string.Empty : a.DescricaoEstrategico)}\";");
                                    csv.Append($"\"{(a.AtivoEstrategico.HasValue && a.AtivoEstrategico.Value == true ? "Sim"  : string.IsNullOrEmpty(a.DescricaoEstrategico) ? " " : "Não") }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;

                            //Trabalhista
                            case 2:
                                {                                    
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.ReverCalculo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.FinalizacaoEscritorio.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.FinalizacaoContabil ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.AlterarExcluir ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }

                                break;

                            //ADMINSTRATIVO
                            case 3:
                                {   
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.PreencheMulta ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.InstanciaId != null ? a.InstanciaId == 1 ? "Primeira" : a.InstanciaId == 2 ? "Segunda" : "Terceira" : string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;     
                            //Tributario ADMINSTRATIVO
                            case 4:
                                {
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.AtualizaEscritorio ? "Sim" : "Não") }\";");                                   
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;
                            //Tributario Judicial
                            case 5:
                                {
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.AtualizaEscritorio ? "Sim" : "Não") }\";");                                   
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;
                                
                            //TRABALHISTA_ADM
                            case 6:
                                {                                   
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;

                            //CIVEL_ESTRATEGICO
                            case 9:
                                {    
                                    csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.NotificarViaEmail.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                    csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoConsumidor) ? string.Empty : a.DescricaoConsumidor)}\";");
                                    csv.Append($"\"{(a.AtivoConsumidor.HasValue && a.AtivoConsumidor.Value == true ? "Sim" : string.IsNullOrEmpty(a.DescricaoEstrategico) ? " " : "Não") }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                    csv.Append($"\"{(string.Empty) }\";");
                                }
                                break;
                        }                        
                        csv.AppendLine("");
                    }
                    else
                        foreach (var b in resultadoDecisao.Dados)
                        {
                            csv.Append($"\"{a.Id}\";");
                            csv.Append($"\"{(a.Nome)}\";");                           

                            switch (tipoProcesso)
                            {
                                //CIVEL_CONSUMIDOR
                                case 1:
                                    {                                       
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.NotificarViaEmail.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : "")))}\";");
                                        csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoEstrategico) ? string.Empty : a.DescricaoEstrategico)}\";");
                                        csv.Append($"\"{(a.AtivoEstrategico.HasValue && a.AtivoEstrategico.Value == true ? "Sim" : string.IsNullOrEmpty(a.DescricaoEstrategico) ? " " : "Não") }\";");
                                    }
                                    break;

                                //Trabalhista
                                case 2:
                                    {                                        
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.ReverCalculo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.FinalizacaoEscritorio.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.FinalizacaoContabil ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.AlterarExcluir ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                        csv.Append($"\"{(b.ReverCalculo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.DecisaoDefault ? "Sim" : "Não") }\";");
                                    }

                                    break;

                                //ADMINSTRATIVO
                                case 3:
                                    {    
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.PreencheMulta ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.InstanciaId != null ? a.InstanciaId == 1 ? "Primeira" : a.InstanciaId == 2 ? "Segunda" : "Terceira" : string.Empty) }\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                    }
                                    break;

                                //TRIBUTARIO ADMINSTRATIVO
                                case 4:
                                    {
                                        csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");                                         
                                        csv.Append($"\"{(a.AtualizaEscritorio ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.Descricao ) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                    }
                                    break;

                                //TRIBUTARIO JUDICIAL
                                case 5:
                                    {
                                        csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.AtualizaEscritorio ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                    }
                                    break;

                                //TRABALHISTA_ADM
                                case 6:
                                    {                                       
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                    }
                                    break;

                                //CIVEL_ESTRATEGICO
                                case 9:
                                    {     
                                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.NotificarViaEmail.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(a.Ativo ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.Descricao) }\";");
                                        csv.Append($"\"{(b.RiscoPerda.GetValueOrDefault() ? "Sim" : "Não") }\";");
                                        csv.Append($"\"{(b.PerdaPotencial == "PO" ? "Possível" : (b.PerdaPotencial == "PR" ? "Provável" : (b.PerdaPotencial == "RE" ? "Remoto" : ""))) }\";");
                                        csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoConsumidor) ? string.Empty : a.DescricaoConsumidor)}\";");
                                        csv.Append($"\"{(a.AtivoConsumidor.HasValue && a.AtivoConsumidor.Value == true ? "Sim" : string.IsNullOrEmpty(a.DescricaoEstrategico) ? " " : "Não") }\";");

                                    }
                                    break;
                            }
                            csv.AppendLine("");
                        }
                }

                TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcessoManutencao.PorId(tipoProcesso).Descricao.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");

                string nomeArquivo = $"Evento_Decisao_{tipoProcessoNome}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }

        ///<sumary>
        /// Exporta as Evento
        ///</sumary>
        /// <param name="repository"></param>
        /// <param name="decisaorepository"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportar-dependentes")]
        public IActionResult ExportarDecisoes(
            [FromServices] IEventoRepository repository,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            var resultado = repository.Obter(TipoProcessoManutencao.TRABALHISTA, EnumHelpers.ParseOrDefault(coluna, EventoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                csv.AppendLine($"Código;Descrição do Evento;Possui Decisão;Cumprimento de Prazo;Rever Cálculo;Finalização Escritório;Finalização Contábil;Altera e Exclui;Descrição do Evento Dependente ");

                foreach (var a in resultado.Dados)
                {
                    var resultadoDependente = repository.ObterDependente(a.Id , string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

                    if (resultadoDependente.Dados.Count == 0)
                    {
                        csv.Append($"\"{a.Id}\";");
                        csv.Append($"\"{(a.Nome)}\";");
                        csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.ReverCalculo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.FinalizacaoEscritorio.GetValueOrDefault() ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.FinalizacaoContabil ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.AlterarExcluir ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(string.Empty) }\";");
                        csv.AppendLine("");
                    }
                    else
                        foreach (var b in resultadoDependente.Dados)
                        {
                            csv.Append($"\"{a.Id}\";");
                            csv.Append($"\"{(a.Nome)}\";");
                            csv.Append($"\"{(a.PossuiDecisao.GetValueOrDefault() ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(a.EhPrazo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(a.ReverCalculo.GetValueOrDefault() ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(a.FinalizacaoEscritorio.GetValueOrDefault() ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(a.FinalizacaoContabil ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(a.AlterarExcluir ? "Sim" : "Não") }\";");
                            csv.Append($"\"{(b.Nome)}\";");
                            csv.AppendLine("");
                        }
                }
                string nomeArquivo = $"Evento_Dependentes_Trabalhista_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }

        [HttpGet("ObterDescricaoDeParaEstrategico")]
        public IActionResult ObterDescricaoEstrategico([FromServices] IEventoRepository repository)
        {
            return Ok(repository.ObterDescricaoEstrategico());
        }

        [HttpGet("ObterDescricaoDeParaConsumidor")]
        public IActionResult ObterDescricaoDeParaConsumidor([FromServices] IEventoRepository repository)
        {
            return Ok(repository.ObterDescricaoConsumidor());
        }
    }
}