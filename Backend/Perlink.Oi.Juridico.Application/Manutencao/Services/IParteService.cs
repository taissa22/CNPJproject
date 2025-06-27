using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IParteService
    {
        CommandResult Atualizar(AtualizarParteCommand command);

        CommandResult Criar(CriarParteCommand command);

        CommandResult Remover(int id);
    }
}
