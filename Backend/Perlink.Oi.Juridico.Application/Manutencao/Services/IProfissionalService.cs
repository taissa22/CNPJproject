using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IProfissionalService
    {
        CommandResult Criar(CriarProfissionalCommand command);

        CommandResult Atualizar(AtualizarProfissionalCommand command);

        CommandResult Remover(int id);
    }
}
