using Perlink.Oi.Juridico.Application.Manutencao.Results.Evento;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEventoRepository
    {
        CommandResult<IReadOnlyCollection<Evento>> Obter(TipoProcessoManutencao tipoProcesso, EventoSort sort, bool ascending, string pesquisa);
        CommandResult<PaginatedQueryResult<Evento>> ObterPaginado(TipoProcessoManutencao tipoProcesso, int pagina, int quantidade, EventoSort sort, bool ascending, string pesquisa);
        IEnumerable<EventoDisponivelCommandResult> ObterDisponiveis(int eventoId);
        CommandResult<PaginatedQueryResult<Evento>> ObterDependentePaginado(int pagina, int quantidade, bool ascending, int eventoId);
        CommandResult<IReadOnlyCollection<Evento>> ObterDependente(int eventoId, bool ascending);
        IEnumerable<Evento> ObterDescricaoEstrategico();
        IEnumerable<Evento> ObterDescricaoConsumidor();

    }
}
