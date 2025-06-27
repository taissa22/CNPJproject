using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IIndiceService
    {
        CommandResult Criar(CriarIndiceCommand command);

        CommandResult Atualizar(AtualizarIndiceCommand command);

        CommandResult Remover(int CodigoIndice);
    }
}