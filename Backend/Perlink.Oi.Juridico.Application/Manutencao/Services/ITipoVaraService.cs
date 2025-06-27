using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ITipoVaraService
    {
        CommandResult Criar(CriarTipoVaraCommand command);

        CommandResult Atualizar(AtualizarTipoVaraCommand command);

        CommandResult Remover(int CodigoTipoVara);
    }
}