using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IOrgaoService
    {
        CommandResult Criar(CriarOrgaoCommand command);

        CommandResult Atualizar(AtualizarOrgaoCommand command);

        CommandResult Remover(int id);
    }
}
