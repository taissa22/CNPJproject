using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.SegurancaContext.Data;
using Oi.Juridico.Contextos.SegurancaContext.Entities;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.WebApi.DTOs.Seguranca;
using System;
using System.Collections.Generic;
using System.IO;
using Arquivo = System.IO.File;
using System.Linq;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Infra.Extensions;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Seguranca.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AgendamentoRelatorioPerfilUsuarioController : ApiControllerBase
    {
        IParametroJuridicoProvider _parametroJuridico;
        SegurancaContext _segurancaContext;
        IUsuarioAtualProvider _usuarioAtual;

        public AgendamentoRelatorioPerfilUsuarioController(IParametroJuridicoProvider parametroJuridico, SegurancaContext segurancaContext, IUsuarioAtualProvider usuarioAtual)
        {
            _parametroJuridico = parametroJuridico;
            _segurancaContext = segurancaContext;
            _usuarioAtual = usuarioAtual;
        }

        [HttpPost("Agendar")]
        public async Task<IActionResult> AgendarAsync()
        {

            var agendamento = new AgendRelUsuarioPerfil();
            agendamento.DatSolicitacao = DateTime.Now;
            agendamento.CodUsrSolicitacao = _usuarioAtual.Login;
            agendamento.CodStatus = 0;


            _segurancaContext.Add(agendamento);
            await _segurancaContext.SaveChangesAsync();


            return Ok();
        }

        //[HttpGet("AgendarUsuarioPerfilPermissao")]
        //public async Task<string> AgendarUsuarioPerfilPermissao()
        //{
        //    return _usuarioAtual.Login;
        //}

        [HttpDelete("{idAgendamento}")]
        public async Task<IActionResult> DeletarAgendamento(int idAgendamento)
        {
            var agendamento = _segurancaContext.AgendRelUsuarioPerfil.Where(x => x.IdAgendamento == idAgendamento).FirstOrDefault();

            _segurancaContext.Remove(agendamento);
            await _segurancaContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("CarregarAgendamento")]
        public async Task<PaginatedQueryResult<SegurancaResponse>> CarregarAgendamento([FromQuery] int pagina)
        {
            var agendamentos = (from agendaUsuarioPerfil in _segurancaContext.AgendRelUsuarioPerfil
                                select new SegurancaResponse
                                {
                                    IdAgendamento = agendaUsuarioPerfil.IdAgendamento,
                                    DatSolicitacao = agendaUsuarioPerfil.DatSolicitacao,
                                    DatInicioExecucao = agendaUsuarioPerfil.DatInicioExecucao,
                                    DatFimExecucao = agendaUsuarioPerfil.DatFimExecucao,
                                    Status = agendaUsuarioPerfil.CodStatus,
                                    NomeUsuario = agendaUsuarioPerfil.CodUsrSolicitacaoNavigation.NomeUsuario,
                                    MensagemErro = agendaUsuarioPerfil.DscMsgErro
                                })
                                .OrderByDescending(x => x.DatSolicitacao);

            var total = await agendamentos.CountAsync();
            int skip = Pagination.PagesToSkip(5, total, pagina);

            var resultado = new PaginatedQueryResult<SegurancaResponse>()
            {
                Total = total,
                Data = agendamentos.Skip(skip).Take(5).ToList()
            };

            return resultado;
        }

        [HttpGet("download/{idAgendamento}")]
        public async Task<IActionResult> ObterArquivoUsuarioPerfilPermissao([FromRoute] int idAgendamento)
        {
            try
            {
                var agendamento = await (from agendaUsuarioPerfil in _segurancaContext.AgendRelUsuarioPerfil
                                         where agendaUsuarioPerfil.IdAgendamento == idAgendamento
                                         select agendaUsuarioPerfil)
                        .FirstOrDefaultAsync();

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var parametroDirNasRelatorioUsuarioPerfil = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_SERV_REL_USUARIO_PERFIL);

               
                var nomeArquivo = agendamento.NomeArquivo;

               
                if (parametroDirNasRelatorioUsuarioPerfil.Count <= 0)
                {
                    //mensagem de erro
                    return BadRequest("Diretório NAS não encontrado");
                }

                string caminhoDownload = string.Empty;

                foreach (var caminho in parametroDirNasRelatorioUsuarioPerfil)
                {
                 

                    if (!Directory.Exists(caminho))
                    {
                        continue;
                    }

                    var arquivo = Path.Combine(caminho, nomeArquivo);

                    // verifica se o arquivo existe
                    if (Arquivo.Exists(arquivo))
                    {
                        caminhoDownload = arquivo;
                        break;
                    }

                }
              
                if (caminhoDownload == string.Empty)
                {
                    return BadRequest("Arquivo não encontrado");
                }

                var dados = await Arquivo.ReadAllBytesAsync(caminhoDownload);
                return File(dados, "application/zip", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}