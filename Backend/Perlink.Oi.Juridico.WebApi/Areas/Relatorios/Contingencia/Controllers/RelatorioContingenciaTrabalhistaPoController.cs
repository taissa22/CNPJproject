using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Data;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.CsvHelperConfigurations;
using Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.Controllers
{
    [ApiController]
    [Route("relatorio-contigencia-provisao-trabalhista-possivel")]
    public class RelatorioContingenciaTrabalhistaPoController : Controller
    {
        RelatoriosContingenciaTrabalhistaContext _relatoriosContingenciaTrabalhistaContext;
        IUsuarioAtualProvider _usuarioAtual;

        public RelatorioContingenciaTrabalhistaPoController(RelatoriosContingenciaTrabalhistaContext relatoriosContingenciaTrabalhistaContext, IUsuarioAtualProvider usuarioAtual)
        {
            _relatoriosContingenciaTrabalhistaContext = relatoriosContingenciaTrabalhistaContext;
            _usuarioAtual = usuarioAtual;
        }

        #region ObterAsync
        [HttpGet("obter-detalhe")]
        public async Task<List<DetalheContingenciaTrabalhistaPoResponse>> ObterAsync([FromQuery] long idItemProvisaoPO, [FromQuery] int? pagina)
        {
            if (idItemProvisaoPO < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPO] deve ser definido");

            var query = _relatoriosContingenciaTrabalhistaContext.ItemFechProvisaoPossiveis
                .AsNoTracking()
                .Select(x => new DetalheContingenciaTrabalhistaPoResponse
                {
                    Id = x.Id,
                    IfptrIdItemFechProvTrab = x.IfptrIdItemFechProvTrab,
                    PedCodPedido = x.PedCodPedido,
                    NomePedido = x.NomePedido,
                    ValorMedioDesemPrincPr = x.ValorMedioDesemPrincipalPr,
                    ValorMedioDesemJurosPr = x.ValorMedioDesemJurosPr,
                    QtePedidosProvavel = x.QtePedidosProvavel,
                    QtePedidosPossivelP = x.QtePedidosPossivelP,
                    QtePedidosRemoto = x.QtePedidosRemoto,
                    PercPerdaPossivel = x.PercPerdaPossivel * 100,
                    PercPerdaProvavel = x.PercPerdaProvavel * 100,
                    ValProvContPrincipalP = x.ValProvContPrincipalP,
                    ValProvContJurosP = x.ValProvContJurosP,
                    QtePedidosPossivelH = x.QtePedidosPossivelH,
                    PerResponsOi = x.PerResponsOi,
                    ValProvContPrincipalH = x.ValProvContPrincipalH,
                    ValProvContJurosH = x.ValProvContJurosH,
                    ValProvContPrincipal = x.ValProvContPrincipal,
                    ValProvContJuros = x.ValProvContJuros

                })
                .OrderBy(x => x.NomePedido)
                .Where(x => x.IfptrIdItemFechProvTrab == idItemProvisaoPO);

            if (pagina != null)
            {
                int take = 15;

                int skip = (int)((pagina - 1) * take);

                query = query.Skip(skip).Take(take);
            };

            var relatoriosContingenciaTrabalhista = await query.ToListAsync();

            return relatoriosContingenciaTrabalhista;
        }
        #endregion

        #region ObterCabecalhoAsync
        [HttpGet("obter-cabecalho")]
        public async Task<CabecalhoProvisaoTrabalhistaPoResponse> ObterCabecalhoAsync([FromQuery] long idItemProvisaoPO)
        {
            if (idItemProvisaoPO < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPO] deve ser definido");

            var result = await _relatoriosContingenciaTrabalhistaContext.ItemFechamentoProvisaoTrab
                .AsNoTracking()
                .Where(x => x.Id == idItemProvisaoPO)
                .Select(x => new CabecalhoProvisaoTrabalhistaPoResponse
                {
                    NomeEmpresaCentralizadora = x.NomeEmpresaCentralizadora,
                    NomeEmpresaGrupo = x.NomeEmpresaGrupo,
                    ProprioTerceiro = ProprioTerceiro.PorId(x.IndProprioTerceiro).Valor,
                    RiscoPerda = RiscoPerda.PorId(x.CodRiscoPerda).Descricao,
                    DataFechamento = x.FptrIdFechProvisaoTrabNavigation.DataFechamento,
                    NumeroMeses = x.FptrIdFechProvisaoTrabNavigation.NumMesesMediaHistorica,
                    TipoDeOutliers = TipoDeOutliers.PorId(x.FptrIdFechProvisaoTrabNavigation.CodTipoOutlier).Descricao
                })
                .FirstOrDefaultAsync();

            return result;
        }
        #endregion Cabecalho

        #region ObterTotaisAsync
        [HttpGet("obter-totais")]
        public async Task<IActionResult> ObterTotaisAsync([FromQuery] long idItemProvisaoPO)
        {
            if (idItemProvisaoPO < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPO] deve ser definido");


            var relatoriosContingenciaTrabalhista = await _relatoriosContingenciaTrabalhistaContext.ItemFechProvisaoPossiveis
                .AsNoTracking()
                .Select(x => new DetalheContingenciaTrabalhistaPoResponse
                {
                    IfptrIdItemFechProvTrab = x.IfptrIdItemFechProvTrab,
                    QtePedidosProvavel = x.QtePedidosProvavel,
                    QtePedidosPossivelP = x.QtePedidosPossivelP,
                    QtePedidosRemoto = x.QtePedidosRemoto,
                    ValProvContPrincipalP = x.ValProvContPrincipalP,
                    ValProvContJurosP = x.ValProvContJurosP,

                    QtePedidosPossivelH = x.QtePedidosPossivelH,
                    ValProvContPrincipalH = x.ValProvContPrincipalH,
                    ValProvContJurosH = x.ValProvContJurosH,
                    ValProvContPrincipal = x.ValProvContPrincipal,
                    ValProvContJuros = x.ValProvContJuros
                })
                .Where(x => x.IfptrIdItemFechProvTrab == idItemProvisaoPO)
                .ToListAsync();

            var detalheProvisaoTrabalhistaResponse = new TotalizadoresProvisaoTrabalhistaPoResponse()
            {
                QtePedidosProvavelP = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosProvavel),
                QtePedidosPossivelP = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosPossivelP),
                QtePedidosRemotoP = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosRemoto),
                ValorPrincipalPedidosPo = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipalP),
                ValorCorrecaoJurosPo = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJurosP),

                ValPriProvContPedNConH = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipalH),
                ValJurProvContPedNConH = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJurosH),
                TotPrincProvContPedSDes = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipal),
                TotJurProvContPedSDes = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJuros),

                QtePedidosPossivelH = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosPossivelH),

                QtdRegistros = relatoriosContingenciaTrabalhista.Count()
            };

            return Ok(detalheProvisaoTrabalhistaResponse);
        }
        #endregion Totais

        #region PossuiHibridoAsync
        [HttpGet("possui-hibrido")]
        public async Task<string> PossuiHibridoAsync([FromQuery] long idItemProvisaoPO)
        {
            var indHibrido = await _relatoriosContingenciaTrabalhistaContext
                                        .ItemFechamentoProvisaoTrab
                                        .AsNoTracking()
                                        .Where(x => x.Id == idItemProvisaoPO)
                                        .Select(x => x.FptrIdFechProvisaoTrabNavigation.IndPossuiHibrido)
                                        .FirstOrDefaultAsync();

            return indHibrido;
        }
        #endregion Hibrido

        #region Exportar
        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar([FromQuery] long idItemProvisaoPO)
        {
            var indPossuiHibrido = await PossuiHibridoAsync(idItemProvisaoPO);
            var lista = await ObterAsync(idItemProvisaoPO, null);
            var cabecalho = await ObterCabecalhoAsync(idItemProvisaoPO);
            var nomeArquivo = $"PedidoPO_{idItemProvisaoPO}.csv";
            var data = $"{cabecalho.DataFechamento.ToString("dd/MM/yyyy")} - {cabecalho.NumeroMeses} meses";

            foreach (var item in lista)
            {
                item.NomeEmpresaCentralizadora = cabecalho.NomeEmpresaCentralizadora;
                item.NomeEmpresaGrupo = cabecalho.NomeEmpresaGrupo;
                item.ProprioTerceiro = cabecalho.ProprioTerceiro;
                item.RiscoPerda = cabecalho.RiscoPerda;
                item.DataFormatada = data;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR")) { Delimiter = ";", SanitizeForInjection = false }))
                    {
                        var map = new DetalheContingenciaTrabalhistaPoMap(indPossuiHibrido);
                        csvWriter.Context.RegisterClassMap(map);
                        csvWriter.WriteRecords(lista);
                    }
                }

                byte[] bytes = memoryStream.ToArray();
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
        }

        [HttpPost("exportarExecutor/{indPossuiHibrido}")]
        public async Task<IActionResult> ExportarExecutorAsync(string indPossuiHibrido, [FromBody]long[] listaIdItemProvisaoPO)
        {
            var listaFinal = new List<DetalheContingenciaTrabalhistaPoResponse>();

            foreach (var idItemProvisaoPO in listaIdItemProvisaoPO)
            {
                var lista = await ObterAsync(idItemProvisaoPO, null);
                var cabecalho = await ObterCabecalhoAsync(idItemProvisaoPO);
                var data = $"{cabecalho.DataFechamento.ToString("dd/MM/yyyy")} - {cabecalho.NumeroMeses} meses";

                foreach (var item in lista)
                {
                    item.NomeEmpresaCentralizadora = cabecalho.NomeEmpresaCentralizadora;
                    item.NomeEmpresaGrupo = cabecalho.NomeEmpresaGrupo;
                    item.ProprioTerceiro = cabecalho.ProprioTerceiro;
                    item.RiscoPerda = cabecalho.RiscoPerda;
                    item.DataFormatada = data;
                }

                listaFinal.AddRange(lista);
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR")) { Delimiter = ";", SanitizeForInjection = false }))
                    {
                        var map = new DetalheContingenciaTrabalhistaPoMap(indPossuiHibrido);
                        csvWriter.Context.RegisterClassMap(map);
                        csvWriter.WriteRecords(listaFinal);
                    }
                }

                var nomeArquivo = $"PedidoPO_Media.csv";
                byte[] bytes = memoryStream.ToArray();
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
        }

        #endregion
    }
}
