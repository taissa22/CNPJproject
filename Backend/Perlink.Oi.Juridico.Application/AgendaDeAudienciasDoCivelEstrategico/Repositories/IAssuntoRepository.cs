using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories
{

    public interface IAssuntoRepository
    {
        CommandResult<PaginatedQueryResult<Assunto>> ObterParaDropdown(int pagina, int quantidade, int assuntoId = 0);
    }
}