using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IComplementoDeAreaEnvolvidaService
    {
        CommandResult Criar(CriarComplementoDeAreaEnvolvidaCommand command);

        CommandResult Atualizar(AtualizarComplementoDeAreaEnvolvidaCommand command);

        CommandResult Remover(int complementoDeAreaEnvolvidaId);
    }
}
