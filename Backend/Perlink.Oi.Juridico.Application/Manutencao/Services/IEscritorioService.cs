using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IEscritorioService
    {
        CommandResult<Escritorio> Criar(CriarEscritorioCommand command);
        CommandResult<Escritorio> Atualizar(AtualizarEscritorioCommand command);
        CommandResult Remover(int id);
    }
}
