using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Data;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.CsvHelperConfigurations;
using Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.Controllers
{
    [ApiController]
    [Route("relatorio-contigencia-provisao-trabalhista-provavel")]
    public class RelatoriosContingenciaTrabalhistaPrController : ApiControllerBase
    {
        RelatoriosContingenciaTrabalhistaContext _relatoriosContingenciaTrabalhistaContext;
        IUsuarioAtualProvider _usuarioAtual;

        public RelatoriosContingenciaTrabalhistaPrController(RelatoriosContingenciaTrabalhistaContext relatoriosContingenciaTrabalhistaContext, IUsuarioAtualProvider usuarioAtual)
        {
            _relatoriosContingenciaTrabalhistaContext = relatoriosContingenciaTrabalhistaContext;
            _usuarioAtual = usuarioAtual;
        }

        #region ObterAsync
        [HttpGet("obter-detalhe")]
        public async Task<List<DetalheContingenciaTrabalhistaResponse>> ObterAsync([FromQuery] long idItemProvisaoPR, [FromQuery] int? pagina)
        {
            if (idItemProvisaoPR < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPR] deve ser definido");

            var query = _relatoriosContingenciaTrabalhistaContext.ItemFechProvisaoProvaveis
                .AsNoTracking()
                .Select(x => new DetalheContingenciaTrabalhistaResponse
                {
                    Id = x.Id,
                    IfptrIdItemFechProvTrab = x.IfptrIdItemFechProvTrab,
                    PedCodPedido = x.PedCodPedido,
                    NomePedido = x.NomePedido,
                    QtePedidosProvavelP = x.QtePedidosProvavelP,
                    PercPerdaProvavelP = x.PercPerdaProvavel * 100,
                    ExpectativaPerdaP = x.ExpectativaPerdaP,
                    ValorMedioDesembPrincipalP = x.ValorMedioDesembPrincipal,
                    ValorMedioDesembolsoJurosP = x.ValorMedioDesembolsoJuros,

                    ValProvContPrincipalP = x.ValProvContPrincipalP,
                    ValProvContJurosP = x.ValProvContJurosP,

                    PerResponsOi = x.PerResponsOi,
                    QtePedidosProvavelH = x.QtePedidosProvavelH,
                    PercPerdaProvavelH = x.PercPerdaProvavel * 100,
                    ExpectativaPerdaH = x.ExpectativaPerdaH,
                    ValorMedioDesembPrincipalH = x.ValorMedioDesembPrincipal,
                    ValorMedioDesembolsoJurosH = x.ValorMedioDesembolsoJuros,
                    ValProvContPrincipalH = x.ValProvContPrincipalH,
                    ValProvContJurosH = x.ValProvContJurosH,

                    ValProvContPrincipal = x.ValProvContPrincipal,
                    ValProvContJuros = x.ValProvContJuros

                })
                .OrderBy(x => x.NomePedido)
                .Where(x => x.IfptrIdItemFechProvTrab == idItemProvisaoPR);

            if (pagina != null)
            {
                int take = 15;

                int skip = (int)((pagina - 1) * take);

                query = query.Skip(skip).Take(take);
            }

            var relatoriosContingenciaTrabalhista = await query.ToListAsync();

            return relatoriosContingenciaTrabalhista;
        }
        #endregion

        #region ObterCabecalhoAsync
        [HttpGet("obter-cabecalho")]
        public async Task<CabecalhoProvisaoTrabalhistaResponse> ObterCabecalhoAsync([FromQuery] long idItemProvisaoPR)
        {
            if (idItemProvisaoPR < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPR] deve ser definido");

            var result = await _relatoriosContingenciaTrabalhistaContext.ItemFechamentoProvisaoTrab
                //.Include(x => x.FptrIdFechProvisaoTrabNavigation)
                .AsNoTracking()
                .Where(x => x.Id == idItemProvisaoPR)
                .Select(x => new CabecalhoProvisaoTrabalhistaResponse
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
        #endregion

        #region ObterTotaisAsync
        [HttpGet("obter-totais")]
        public async Task<IActionResult> ObterTotaisAsync([FromQuery] long idItemProvisaoPR)
        {
            if (idItemProvisaoPR < 1)
                throw new ArgumentException("Parametro [idItemProvisaoPR] deve ser definido");


            var relatoriosContingenciaTrabalhista = await _relatoriosContingenciaTrabalhistaContext.ItemFechProvisaoProvaveis
                .AsNoTracking()
                .Select(x => new DetalheContingenciaTrabalhistaResponse
                {
                    IfptrIdItemFechProvTrab = x.IfptrIdItemFechProvTrab,

                    QtePedidosProvavelP = x.QtePedidosProvavelP,
                    ExpectativaPerdaP = x.ExpectativaPerdaP,
                    
                    ValProvContPrincipalP = x.ValProvContPrincipalP,
                    ValProvContJurosP = x.ValProvContJurosP,

                    ExpectativaPerdaH = x.ExpectativaPerdaH,

                    QtePedidosProvavelH = x.QtePedidosProvavelH,

                    ValProvContPrincipalH = x.ValProvContPrincipalH,
                    ValProvContJurosH = x.ValProvContJurosH,
                    
                    ValProvContPrincipal = x.ValProvContPrincipal,
                    ValProvContJuros = x.ValProvContJuros
                })
                .Where(x => x.IfptrIdItemFechProvTrab == idItemProvisaoPR)
                .ToListAsync();

            var detalheProvisaoTrabalhistaResponse = new TotalizadoresProvisaoTrabalhistaResponse()
            {
                QtePedidosProvavelP = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosProvavelP),
                ExpectativaPerdaP = relatoriosContingenciaTrabalhista.Sum(x => x.ExpectativaPerdaP),

                ValProvContPrincipalP = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipalP),
                ValProvContJurosP = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJurosP),

                ExpectativaPerdaH = relatoriosContingenciaTrabalhista.Sum(x => x.ExpectativaPerdaH),

                QtePedidosProvavelH = relatoriosContingenciaTrabalhista.Sum(x => x.QtePedidosProvavelH),

                ValProvContPrincipalH = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipalH),
                ValProvContJurosH = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJurosH),

                ValProvContPrincipal = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContPrincipal),
                ValProvContJuros = relatoriosContingenciaTrabalhista.Sum(x => x.ValProvContJuros),

                QtdRegistros = relatoriosContingenciaTrabalhista.Count()
            };

            return Ok(detalheProvisaoTrabalhistaResponse);
        }
        #endregion

        #region PossuiHibridoAsync
        [HttpGet("possui-hibrido")]
        public async Task<string> PossuiHibridoAsync([FromQuery] long idItemProvisaoPR)
        {
            var indHibrido = await _relatoriosContingenciaTrabalhistaContext
                                        .ItemFechamentoProvisaoTrab
                                        .AsNoTracking()
                                        .Where(x => x.Id == idItemProvisaoPR)
                                        .Select(x => x.FptrIdFechProvisaoTrabNavigation.IndPossuiHibrido)
                                        .FirstOrDefaultAsync();

            return indHibrido;
        }
        #endregion

        #region Exportar
        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar([FromQuery] long idItemProvisaoPR)
        {
            var indPossuiHibrido = await PossuiHibridoAsync(idItemProvisaoPR);
            var lista = await ObterAsync(idItemProvisaoPR, null);
            var cabecalho = await ObterCabecalhoAsync(idItemProvisaoPR);
            var data = $"{cabecalho.DataFechamento.ToString("dd/MM/yyyy")} - {cabecalho.NumeroMeses} meses";

            foreach (var item in lista)
            {
                item.NomeEmpresaCentralizadora = cabecalho.NomeEmpresaCentralizadora;
                item.NomeEmpresaGrupo = cabecalho.NomeEmpresaGrupo;
                item.ProprioTerceiro = cabecalho.ProprioTerceiro;
                item.RiscoPerda = cabecalho.RiscoPerda;
                item.DataFormatada = data;
            }

            var nomeArquivo = $"PedidoPR_{idItemProvisaoPR}.csv";

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR")) { Delimiter = ";", SanitizeForInjection = false }))
                    {
                        var map = new DetalheContingenciaTrabalhistaPrMap(indPossuiHibrido);
                        csvWriter.Context.RegisterClassMap(map);
                        csvWriter.WriteRecords(lista);
                    }
                }

                byte[] bytes = memoryStream.ToArray();
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
        }
        #endregion

        [HttpPost("exportarExecutor/{indPossuiHibrido}")]
        public async Task<IActionResult> ExportarExecutorAsync(string indPossuiHibrido, [FromBody] long[] listaIdItemProvisaoPR)
        {
            var listaFinal = new List<DetalheContingenciaTrabalhistaResponse>();

            foreach (var idItemProvisaoPR in listaIdItemProvisaoPR)
            {
                var lista = await ObterAsync(idItemProvisaoPR, null);
                var cabecalho = await ObterCabecalhoAsync(idItemProvisaoPR);
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
                        var map = new DetalheContingenciaTrabalhistaPrMap(indPossuiHibrido);
                        csvWriter.Context.RegisterClassMap(map);
                        csvWriter.WriteRecords(listaFinal);
                    }
                }

                var nomeArquivo = $"PedidoPR_Media.csv";
                byte[] bytes = memoryStream.ToArray();
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
        }

    }
}