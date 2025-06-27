using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IEmpresaDoGrupoService
    {
        CommandResult Criar(CriarEmpresaDoGrupoCommand command);

        CommandResult Atualizar(AtualizarEmpresaDoGrupoCommand command);

        CommandResult Remover(int id);
    }
}