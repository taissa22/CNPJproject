using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IFatoGeradorService
    {
        CommandResult Criar(CriarFatoGeradorCommand command);

        CommandResult Atualizar(AtualizarFatoGeradorCommand command);

        CommandResult Remover(int id);
    }
}
