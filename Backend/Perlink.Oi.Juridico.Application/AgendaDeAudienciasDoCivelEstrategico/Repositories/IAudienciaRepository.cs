using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Filters;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Queries;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories
{
    public interface IAudienciaRepository
    {
        CommandResult<IReadOnlyCollection<AudienciaDoProcesso>> ObterAudienciasPorProcesso(AgendaDeAudienciaDoCivelEstrategicoFilter filtros);

        CommandResult<PaginatedQueryResult<AudienciaDoProcesso>> ObterAudienciasPorProcessoPaginado(int processoId, int pagina, int quantidade);

        CommandResult<PaginatedQueryResult<AudienciaDoProcesso>> ObterAudienciasPorUsuarioLogado(AgendaDeAudienciaDoCivelEstrategicoFilter filtros);
    }
}