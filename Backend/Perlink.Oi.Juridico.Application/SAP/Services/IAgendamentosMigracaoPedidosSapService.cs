using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.Services
{
    public interface IAgendamentosMigracaoPedidosSapService
    {
        CommandResult Remover(int agendamentoMigracaoPedidosSapId);

        CommandResult Criar(IFormFile documentos);

    }
}
