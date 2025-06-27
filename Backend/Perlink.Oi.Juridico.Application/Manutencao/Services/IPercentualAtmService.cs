using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IPercentualAtmService
    {
        CommandResult AtualizarPercentualAtmCC(IFormFile upload, DateTime dataVigencia, int codigoTipoProcesso);
    }
}