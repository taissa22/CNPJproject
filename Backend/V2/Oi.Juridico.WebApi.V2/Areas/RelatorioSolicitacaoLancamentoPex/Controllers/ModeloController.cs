using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ModeloController : ControllerBase
    {

        private RelatorioSolicitacaoLancamentoPexContext _relSolicitacaoLancamentoPexContext;

        public ModeloController(RelatorioSolicitacaoLancamentoPexContext relSolicitacaoLancamentoPexContext)
        {
            _relSolicitacaoLancamentoPexContext = relSolicitacaoLancamentoPexContext;
        }
        [HttpGet("Listar")]
        public IActionResult Listar(string tipoPesquisa = "OrdemAlfabetica", string pesquisa = "", int skip = 0) // tipoPesquisa "OrdemAlfabetica" ou "DataAtualizacao"
        { 
            var resposta = new List<ModeloModel>();

            if (pesquisa == "null") pesquisa = String.Empty;

            IQueryable<AgPexRelSolicModelo> query;

            if (tipoPesquisa == "OrdemAlfabetica")
                query = _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo
                    .OrderBy(m => m.Nome);
            else
                query = _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.OrderByDescending(m => m.DatAlteracao);


            query =  query.Skip(skip).Take(10);

            if (String.IsNullOrEmpty(pesquisa))
                query.ToList().ForEach(m => resposta.Add(new ModeloModel(m)));
            else
                query.ToList().Where(m =>
                m.Nome.ToLower().Trim().Contains(pesquisa.Trim().ToLower()) ||
                (m.DatAlteracao != null && m.DatAlteracao.Value.ToString("dd/MM/yyyy").Contains(pesquisa.Trim().ToLower()))
                ).ToList().ForEach(m => resposta.Add(new ModeloModel(m)));

            return Ok(resposta);
        }

        [HttpPost("Salvar")]
        public IActionResult Salvar([FromBody] ModeloModel model)
        {
            try
            {
                if (_relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Any(a => a.Nome.Trim().ToLower() == model.Nome.Trim().ToLower()))
                {

                    throw new Exception("Já existe um modelo cadastrado com o mesmo nome");
                }

                _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Add(model.retornaEntity());
                _relSolicitacaoLancamentoPexContext.SaveChanges();
                return Ok("Modelo salvo com Sucesso");

            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] ModeloModel model)
        {
            try
            {
                if (_relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Any(a => a.Nome.Trim().ToLower() == model.Nome.Trim().ToLower() && model.Id != a.Id) )
                {

                    throw new Exception("Já existe um modelo cadastrado com o mesmo nome");
                }

                var modelo = _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Find(model.Id);

                if (modelo == null)
                {
                    return BadRequest("Modelo não encontrado");
                }
 
                modelo.DatIniSolcitacao = !String.IsNullOrEmpty(model.DataIniSolicitacao) ? DateTime.Parse(model.DataIniSolicitacao) : null;
                modelo.DatFimSolcitacao = !String.IsNullOrEmpty(model.DataFimSolicitacao) ? DateTime.Parse(model.DataFimSolicitacao) : null;
                modelo.DatIniVencimento = !String.IsNullOrEmpty(model.DataIniVencimento) ? DateTime.Parse(model.DataIniVencimento) : null;
                modelo.DatFimVencimento = !String.IsNullOrEmpty(model.DataFimVencimento) ? DateTime.Parse(model.DataFimVencimento) : null;
                modelo.ColunasRelSelecionadas = JsonConvert.SerializeObject(model.ColunasRelatorioSelecionadas);
                modelo.EscritoriosRelSelecionados = JsonConvert.SerializeObject(model.EscritoriosSelecionados);
                modelo.TiposLancRelSelecionados = JsonConvert.SerializeObject(model.TiposDeLancamentoSelecionados);
                modelo.UfsSelecionadas = JsonConvert.SerializeObject(model.UfsSelecionadas);
                modelo.UsrCodUsuario = "Perlink";
                modelo.StatusSolRelSelecionados = JsonConvert.SerializeObject(model.StatusSolicitacoesSelecionados);
                modelo.DatAlteracao = DateTime.Now;

                _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Update(modelo);
                _relSolicitacaoLancamentoPexContext.SaveChanges();
                return Ok("Modelo salvo com Sucesso");

            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

        [HttpGet("Deletar")]
        public IActionResult Deletar(decimal id)
        {
            try
            {  
                var modelo = _relSolicitacaoLancamentoPexContext.AgPexRelSolicModelo.Find(id);

                if (modelo == null)
                {
                    return BadRequest("Modelo não encontrado");
                }
                _relSolicitacaoLancamentoPexContext.Remove(modelo);
                _relSolicitacaoLancamentoPexContext.SaveChanges();
                return Ok("Modelo deletado com Sucesso");

            }
            catch (Exception ex)
            {
                var mensagem = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(mensagem);
            }
        }

    }
}
