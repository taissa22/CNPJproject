using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IOrientacaoJuridicaService
    {
        CommandResult Criar(CriarOrientacaoJuridicaCommand command);

        CommandResult Atualizar(AtualizarOrientacaoJuridicaCommand command);

        CommandResult Remover(int id);
    }
}
