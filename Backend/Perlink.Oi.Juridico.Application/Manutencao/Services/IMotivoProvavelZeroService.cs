using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IMotivoProvavelZeroService
    {
        CommandResult Criar(CriarMotivoProvavelZeroCommand command);

        CommandResult Atualizar(AtualizarMotivoProvavelZeroCommand command);

        CommandResult Remover(int Id);
    }
}
