using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IComarcaService
    {
        CommandResult Criar(CriarComarcaCommand command);

        CommandResult Atualizar(AtualizarComarcaCommand command);

        CommandResult Remover(int IdComarca);
    }
}
