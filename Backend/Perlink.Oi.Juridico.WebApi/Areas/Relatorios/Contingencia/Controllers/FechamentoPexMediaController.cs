using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Data;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.DTO;
using Perlink.Oi.Juridico.Infra.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia
{
    [Route("contingencia/fechamento-pex-media")]
    [ApiController]
    [Authorize]
    public class FechamentoPexMediaController : ApiControllerBase
    {
        private RelatorioMovimentacoesPexContext _movimentacoesPexContext;
        private IUsuarioAtualProvider _usuarioAtual;

        public FechamentoPexMediaController(RelatorioMovimentacoesPexContext movimentacoesPexContext, IUsuarioAtualProvider usuarioAtual)
        {
            _movimentacoesPexContext = movimentacoesPexContext;
            _usuarioAtual = usuarioAtual;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var dtos = await (from fpm in _movimentacoesPexContext.FechamentoPexMedia
                                   join emp in _movimentacoesPexContext.EmpresasCentralizadoras  
                                   on fpm.CodEmpresaCentralizadora equals emp.Codigo
                                   join usu in _movimentacoesPexContext.AcaUsuario
                                   on fpm.CodUsuario  equals usu.CodUsuario
                                   join sfc  in _movimentacoesPexContext.SolicFechamentoCont
                                   on fpm.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                                   select new FechamentoPexMediaResponse
                                   {
                                       Id = fpm.CodSolicFechamentoCont,
                                       DataExecucao = fpm.DatGeracao,
                                       DataFechamento = fpm.DatFechamento,
                                       PercentualHaircut = fpm.PerHaircut,
                                       NomeUsuario = usu.NomeUsuario,
                                       IndAplicarHaircut = fpm.IndAplicarHaircutProcGar,
                                       NumeroMeses = fpm.NroMesesMediaHistorica,
                                       MultDesvioPadrao = fpm.ValMultDesvioPadrao,
                                       Empresas = emp.Nome,
                                       DataAgendamento = sfc.DatUltimoAgend != null ? sfc.DatUltimoAgend : sfc.DataCadastro
                                   }).ToListAsync();

            var resultado = new List<FechamentoPexMediaResponse>();

            dtos.ForEach(d =>
            {
                var dto = resultado.FirstOrDefault(r => r.Id == d.Id && r.DataFechamento == d.DataFechamento);
                if (dto == null)
                {
                    resultado.Add(d);
                }
                else
                {
                    dto.Empresas += ", " + d.Empresas;
                    if (d.DataExecucao > dto.DataExecucao)
                    {
                        dto.DataExecucao = d.DataExecucao;
                    }
                }
            });

            return Ok(resultado.OrderByDescending(r => r.DataFechamento).ThenByDescending(r => r.DataExecucao));

        }


    }
}