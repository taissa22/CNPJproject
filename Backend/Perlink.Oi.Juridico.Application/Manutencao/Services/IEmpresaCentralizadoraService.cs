using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IEmpresaCentralizadoraService
    {
        CommandResult Criar(CriarEmpresaCentralizadoraCommand command);

        CommandResult Atualizar(AtualizarEmpresaCentralizadoraCommand command);

        CommandResult Remover(int codigo);
    }
}