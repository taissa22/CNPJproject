using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IPrepostosService
    {
        CommandResult Criar(CriarPrepostoCommand command);

        CommandResult Atualizar(AtualizarPrepostoCommand command);

        CommandResult Remover(int id);
    }
}
