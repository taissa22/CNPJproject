using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public  interface IEstabelecimentoService
    {
      CommandResult Criar(CriarEstabelecimentoCommand command);
      CommandResult Remover(int estabelecimentoId);
      CommandResult Atualizar(AtualizarEstabelecimentoCommand command);
    }
}
