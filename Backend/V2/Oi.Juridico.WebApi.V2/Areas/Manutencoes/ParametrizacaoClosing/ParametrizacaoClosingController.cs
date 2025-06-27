using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Oi.Juridico.Contextos.V2.ParametrizacaoClosingContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Dtos;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Services;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Providers;
using System.Data;
using System.Runtime.InteropServices;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing;

[Route("parametrizacao-closing")]
[ApiController]
public class ParametrizacaoClosingController : ApiControllerBase
{
    private readonly ParametrizacaoClosingContext _parametrizacaoClosingContext;
    private readonly ParametrizacaoClosingService _parametrizacaoClosingService;
    private readonly ParametroJuridicoContext _parametroJuridicoContext;

    public ParametrizacaoClosingController(ParametrizacaoClosingContext parametrizacaoClosingContext, ParametroJuridicoContext parametroJuridicoContext)
    {
        _parametrizacaoClosingContext = parametrizacaoClosingContext;
        _parametroJuridicoContext = parametroJuridicoContext;
    }


    [HttpGet("obter")]
    public async Task<IActionResult> ObterAsync()
    {
        var parametrizacaoClosing = await _parametrizacaoClosingContext.ParametrizacaoClosing
            .AsNoTracking()
            .Select(x => new ObterResponse
            {
                CodTipoProcesso = x.CodTipoProcesso,
                IndClosingHibrido = x.IndClosingHibrido,
                PercResponsabilidade = x.PerDefaultResponsabilidade,
                ClassificaoClosing = x.ClassificacaoClosingDefault,
                IdEscritorioPadrao = x.IdEscritorioPadrao,
                IndClosingHibridoClientCO = x.IndClosingHibridoClientco,
                ClassificaoClosingClientCO = x.ClassfClosingDftClientco,
            })
            .ToArrayAsync();

        return Ok(parametrizacaoClosing);
    }

    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarRequest request)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest("Modelo inválido.");
        }

        string errorMessage = string.Empty;

        if (request.ClassificaoClosing == 3 && request.IndClosingHibrido == "N")
        {
            errorMessage += "- A Classificação Closing default Oi Móvel só pode ser configurada como \"Híbrido\" quando a classificação \"Híbrido\" estiver ligada.\n";
        }

        if (request.ClassificaoClosingClientCO == 3 && request.IndClosingHibridoClientCO == "N")
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage += "<br/><br/>"; // Duas linhas em branco entre as mensagens
            }
            errorMessage += "- A Classificação Closing default Oi Fibra só pode ser configurada como \"Híbrido\" quando a classificação \"Híbrido\" estiver ligada.\n";
        }

        if (request.CodTipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id || request.CodTipoProcesso == TipoProcesso.JEC.Id)
        {
            var parametrizacaoClosingService = new ParametrizacaoClosingService(new Repositories.ParametrizacaoClosingRepository(_parametrizacaoClosingContext), _parametroJuridicoContext);

            var result = await parametrizacaoClosingService.ValidarRegraClassificacaoClosing(request);

            if (result == false)
            {
                errorMessage += "- Para parametrizar qualquer um dos closing como Pré, Pós ou Híbrido, um deles deve estar parametrizado como \"A definir\" ou \"N/A\"";
            }
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            var errorCode = "ERR001"; // Código de erro específico
            return BadRequest(new { Code = errorCode, Message = errorMessage.Trim() });
        }


        var obj = await _parametrizacaoClosingContext.ParametrizacaoClosing.ToArrayAsync();

        var itemDb = obj.First(x => x.CodTipoProcesso == request.CodTipoProcesso);
        itemDb.ClassificacaoClosingDefault = request.ClassificaoClosing;
        itemDb.IndClosingHibrido = request.IndClosingHibrido;
        itemDb.PerDefaultResponsabilidade = request.PercResponsabilidade;
        itemDb.IdEscritorioPadrao = request.IdEscritorioPadrao;

        itemDb.ClassfClosingDftClientco = request.ClassificaoClosingClientCO;
        itemDb.IndClosingHibridoClientco = request.IndClosingHibridoClientCO;

        try
        {
            await _parametrizacaoClosingContext.SaveChangesAsync(User.Identity!.Name, true);
            return Ok($"OK");
        }
        catch (Exception ex)
        {
            return BadRequest($"Houve um erro ao tentar processar sua solicitação '{ex.Message}'.");
        }
        finally
        {
            _parametrizacaoClosingContext.Dispose();
        }
    }

    [HttpGet("obter-escritorio")]
    public async Task<IActionResult> ObterEscritorioAsync()
    {
        var escritorios = await _parametrizacaoClosingContext.Profissional
            .AsNoTracking()
            .Where(x => (x.IndEscritorio.ToUpper() == "S") && (x.IndCivelEstrategico.ToUpper() == "S"))
            .Select(x => new EscritorioResponse
            {
                Id = x.CodProfissional,
                Nome = (x.IndAtivo == "N" && !x.NomProfissional.Contains("[INATIVO]")) ? x.NomProfissional + " - [INATIVO]" : x.NomProfissional
            })
            .OrderBy(x => x.Nome)
            .ToArrayAsync();

        return Ok(escritorios);
    }

    [HttpGet("exportar")]
    public IActionResult Exportar()
    {
        var parametrizacaoClosing = _parametrizacaoClosingContext.ParametrizacaoClosing
            .AsNoTracking()
            .Select(x => new ObterResponse
            {
                CodTipoProcesso = x.CodTipoProcesso,
                IndClosingHibrido = x.IndClosingHibrido,
                PercResponsabilidade = x.PerDefaultResponsabilidade,
                ClassificaoClosing = x.ClassificacaoClosingDefault,
                IdEscritorioPadrao = x.IdEscritorioPadrao,
                ClassificaoClosingClientCO = x.ClassfClosingDftClientco,
                IndClosingHibridoClientCO = x.IndClosingHibridoClientco,
            })
            .ToArray();

        foreach (var i in parametrizacaoClosing)
        {
            i.Ordencacao = i.CodTipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id ? 1
                            : i.CodTipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id ? 2
                            : i.CodTipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id ? 3
                            : i.CodTipoProcesso == TipoProcesso.TRABALHISTA.Id ? 4
                            : i.CodTipoProcesso == TipoProcesso.JEC.Id ? 5
                            : 6;
        }

        StringBuilder csv = new StringBuilder();
        csv.AppendLine($"TIPO PROCESSO; CLASSIFICAÇÃO CLOSING DEFAULT Oi Móvel; " +
            $"CLASSIFICAÇÃO HÍBRIDO LIGADA OI Móvel; % DE RESPONSABILIDADE OI DEFAULT OI MÓVEL; ESCRITÓRIO PADRÃO PROCESSOS SECUNDÁRIOS OI MÓVEL;" +
            $"CLASSIFICAÇÃO CLOSING DEFAULT OI FIBRA; CLASSIFICAÇÃO HÍBRIDO LIGADA OI FIBRA");

        foreach (var x in parametrizacaoClosing.OrderBy(x => x.Ordencacao))
        {
            string tipoProcesso = x.CodTipoProcesso == TipoProcesso.CIVEL_CONSUMIDOR.Id ? "Cível Consumidor"
                                : x.CodTipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id ? "Cível Estratégico"
                                : x.CodTipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id ? "Trabalhista ADM"
                                : x.CodTipoProcesso == TipoProcesso.TRABALHISTA.Id ? "Trabalhista Judicial"
                                : x.CodTipoProcesso == TipoProcesso.JEC.Id ? "Juizado Especial"
                                : "NÃO SE APLICA";

            string classificaoClosing = ClassificacaoClosing.PorId((int)x.ClassificaoClosing!).Nome.ToUpper();
            string indClosingHibrido = x.IndClosingHibrido.ToUpper() == "S" ? "SIM" : x.IndClosingHibrido.ToUpper() == "N" ? "NÃO" : x.IndClosingHibrido;
            string percResponsabilidade = ((int)x.PercResponsabilidade!).ToString();
            string escritorio = x.IdEscritorioPadrao != null ? ObterEscritorio(x.IdEscritorioPadrao) : "NÃO SE APLICA";
            string classificaoClosingClienteCO = ClassificacaoClosing.PorId((int)x.ClassificaoClosingClientCO!).Nome.ToUpper();
            string indClosingHibridoClienteCO = x.IndClosingHibridoClientCO.ToUpper() == "S" ? "SIM" : x.IndClosingHibridoClientCO.ToUpper() == "N" ? "NÃO" : x.IndClosingHibridoClientCO;

            if (x.CodTipoProcesso == TipoProcesso.CIVEL_ESTRATEGICO.Id)
            {
                percResponsabilidade = "NÃO SE APLICA";
                classificaoClosing = "NÃO SE APLICA";
                classificaoClosingClienteCO = "NÃO SE APLICA";
            }
            else
            {
                escritorio = "NÃO SE APLICA";
            }


            csv.Append($"\"{tipoProcesso}\";");
            csv.Append($"\"{classificaoClosing}\";");
            csv.Append($"\"{indClosingHibrido}\";");
            csv.Append($"\"{percResponsabilidade}\";");
            csv.Append($"\"{escritorio}\";");

            csv.Append($"\"{classificaoClosingClienteCO}\";");
            csv.Append($"\"{indClosingHibridoClienteCO}\";");

            csv.AppendLine("");
        }

        string nomeArquivo = $"ParametrizacaoClosing_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
        byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
        bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
        return File(bytes, "text/csv", nomeArquivo);
    }

    private string ObterEscritorio(int? idEscritorio)
    {
        var escritorio = _parametrizacaoClosingContext.Profissional
            .AsNoTracking()
            .Where(x => x.CodProfissional == idEscritorio)
            .Select(x => new EscritorioResponse
            {
                Id = x.CodProfissional,
                Nome = (x.IndAtivo == "N" && !x.NomProfissional.Contains("[INATIVO]")) ? x.NomProfissional + " - [INATIVO]" : x.NomProfissional
            })
            .FirstOrDefault();

        return escritorio!.Nome;
    }

}
