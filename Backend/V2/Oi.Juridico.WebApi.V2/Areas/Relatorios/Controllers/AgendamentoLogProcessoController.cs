using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Entities;
using Oi.Juridico.Contextos.V2.Extensions;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Relatorios.DTOs;
using Oi.Juridico.WebApi.V2.Areas.Relatorios.Models;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios
{
    [Route("[controller]")]
    [ApiController]
    public class AgendamentoLogProcessoController : ControllerBase
    {
        private readonly AgendamentoLogProcessoContext _db;
        private readonly ParametroJuridicoContext _dbParametroJuridico;
        public AgendamentoLogProcessoController(AgendamentoLogProcessoContext db, ParametroJuridicoContext dbParametroJuridico)
        {
            _db = db;
            _dbParametroJuridico = dbParametroJuridico;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult> Listar(
        int pagina,
        int quantidade,
        DateTime? dataIni,
        DateTime? dataFim)
        {
            //TODO: Implementar controle de permissões quando juntar com a branch que faz o controle via middleware
            var resposta = new List<AgendamentoLogProcessoResponse>();
            var query = _db.AgendLogProcesso
                .OrderByDescending(x => x.DatCadastro)
                .Where(a => (a.DatCadastro.Date >= dataIni || dataIni == null) && (a.DatCadastro.Date <= dataFim || dataFim == null));

            var total = query.Count();
            var agendamentos = await query
                .Skip(Pagination.PagesToSkip(quantidade, total, pagina))
                .Take(quantidade)
                .ToListAsync();

            agendamentos.ForEach(a => resposta.Add(new AgendamentoLogProcessoResponse(a)));

            return Ok(new PaginatedQueryResult<AgendamentoLogProcessoResponse>()
            {
                Data = resposta,
                Total = total
            });
        }


        [HttpPost("Criar")]
        public async Task<ActionResult> Criar(AgendamentoLogProcessoModel model)
        {
            try
            {

                if (String.IsNullOrEmpty(model.ArquivoBase64))
                {
                    BadRequest("Arquivo não encontrado.");
                }

                var arquivo = Convert.FromBase64String(model.ArquivoBase64.Split(",")[1]);
                model.NomeDoArquivo = $"Codigo_Interno_Carregado_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";

                var caminhoUpload = (await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_AGEND_LOG_PROC"))[0];

                if (!Directory.Exists(caminhoUpload))
                {
                    Directory.CreateDirectory(caminhoUpload);
                }

                FileStream Stream = new FileStream(caminhoUpload + @"\" + model.NomeDoArquivo, FileMode.Create);
                Stream.Write(arquivo, 0, arquivo.Length);
                Stream.Close();
                var linhas = System.IO.File.ReadLines(caminhoUpload + @"\" + model.NomeDoArquivo);
                var numeroDelinhas = linhas.Where(l => !String.IsNullOrEmpty(l.Trim())).Count();

                if (numeroDelinhas < 3)
                {
                    return BadRequest("O arquivo .CSV importado não possui registros para carga.");
                }

                _db.AgendLogProcesso.Add(model.retornaEntity(User.Identity!.Name));
                _db.SaveChanges();
                return Ok("Salvo com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Deletar")]
        public async Task<ActionResult> Deletar(decimal id)
        {
            try
            {
                var agendamento = _db.AgendLogProcesso.Find(id);
                if (agendamento == null) return BadRequest("Agendamento não encontrado.");
                _db.AgendLogProcesso.Remove(agendamento);
             

                if (!String.IsNullOrEmpty(agendamento.NomArquivo))
                { 
                    var caminhosUpload = await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_AGEND_LOG_PROC", agendamento.NomArquivo);

                    foreach (var caminhoUpload in caminhosUpload)
                    {
                        if (System.IO.File.Exists(caminhoUpload))
                        {
                            System.IO.File.Delete(caminhoUpload);
                            break;
                        }
                    }
                }
                _db.SaveChanges();
                return Ok("Excluído com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BaixarArquivo")]
        public async Task<ActionResult> BaixarArquivo(decimal id)
        {
            try
            {
                var agendamento = _db.AgendLogProcesso.Find(id);
                if (agendamento == null) return BadRequest("Agendamento não encontrado.");

                var caminhosUpload = await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_AGEND_LOG_PROC", agendamento.NomArquivo);

                foreach (var caminhoUpload in caminhosUpload)
                {
                    if (System.IO.File.Exists(caminhoUpload))
                    {
                        var bytes = System.IO.File.ReadAllBytes(caminhoUpload);
                        bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                        return File(bytes, "text/csv", agendamento.NomArquivo);
                    }
                }

                return BadRequest("Arquivo não encontrado.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BaixarArquivoZip")]
        public async Task<ActionResult> BaixarArquivoZip(decimal id)
        {
            try
            {
                var agendamento = _db.AgendLogProcesso.Find(id);
                if (agendamento == null) return BadRequest("Agendamento não encontrado.");

                var caminhosUpload = await _dbParametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_AGEND_LOG_PROC", $"Resultado\\{agendamento.NomArquivoResultado}");

                foreach (var caminhoUpload in caminhosUpload)
                {
                    if (System.IO.File.Exists(caminhoUpload))
                    {

                        var bytes = System.IO.File.ReadAllBytes(caminhoUpload);
                        return File(bytes, "application/zip", agendamento.NomArquivoResultado);
                    }
                }

                return BadRequest("Arquivo não encontrado.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BaixarArquivoModelo")]
        public async Task<ActionResult> BaixarArquivoModelo()
        {
            try
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código Interno;");

                string nomeArquivo = $"Codigo_Interno.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ParametroJuridico")]
        public async Task<ActionResult> ParametroJuridico()
        {
            return Ok(_dbParametroJuridico.ParametroJuridico.Where(p => p.CodParametro == "TAM_MAX_CARGA_DOCUMENTOS").FirstOrDefault());
        }


    }
}
