using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IAdvogadoDoEscritorioService
    {
        CommandResult Criar(CriarAdvogadoDoEscritorioCommand command);
        CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>> Atualizar(AtualizarAdvogadoDoEscritorioCommand command);
        CommandResult Remover(int Id, int escritorioId);
    }
}