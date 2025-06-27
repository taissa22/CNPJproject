using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IEsferaService
    {
        CommandResult Criar(CriarEsferaCommand command);
        CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Atualizar(AtualizarEsferaCommand command);
        CommandResult Remover(int codigoEsfera);
    }
}