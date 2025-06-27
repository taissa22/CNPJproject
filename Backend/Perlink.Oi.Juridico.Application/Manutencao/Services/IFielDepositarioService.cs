using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IFielDepositarioService {
        CommandResult CriarFielDepositario(CriarFielDepositarioCommand command);
        CommandResult ExcluirFielDepositario(int fielDepositarioId);
        CommandResult AtualizarFielDepositario(AtualizarFielDepositarioCommand command);
    }
}