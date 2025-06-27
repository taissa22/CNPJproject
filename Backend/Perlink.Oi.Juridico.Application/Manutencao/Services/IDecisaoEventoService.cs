using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IDecisaoEventoService
    {
        CommandResult Criar(CriarDecisaoEventoCommand command);
        CommandResult Atualizar(AtualizarDecisaoEventoCommand command);
        CommandResult Remover(int decisaoId, int eventoId);
    }
}
