using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IUsuarioOperacaoRetroativaService
    {
        CommandResult Criar(CriarUsuarioOperacaoRetroativaCommand command);

        CommandResult Atualizar(AtualizarUsuarioOperacaoRetroativaCommand command);

        CommandResult Remover(string codUsuario);
    }
}
