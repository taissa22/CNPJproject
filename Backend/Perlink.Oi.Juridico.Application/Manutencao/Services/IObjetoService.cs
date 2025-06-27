using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IObjetoService
    {
        CommandResult Criar(CriarObjetoCommand command);

        CommandResult Atualizar(AtualizarObjetoCommand command);

        CommandResult Remover(int id);
    }
}
