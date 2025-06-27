using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IGrupoPedidoService
    {
        CommandResult Criar(CriarGrupoPedidoCommand command);

        CommandResult Atualizar(AtualizarGrupoPedidoCommand command);

        CommandResult Remover(int id);
    }
}
