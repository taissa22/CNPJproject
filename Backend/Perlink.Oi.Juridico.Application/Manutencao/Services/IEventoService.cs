using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IEventoService
    {
        CommandResult Criar(CriarEventoCommand command);
        CommandResult Atualizar(AtualizarEventoCommand command);

        CommandResult AtualizarDependente(AtualizarEventoDependenteCommand command);

        CommandResult Remover(int tipoProcesso,int id);
    }
}
