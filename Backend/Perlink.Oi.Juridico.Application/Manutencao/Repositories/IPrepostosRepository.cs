using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IPrepostosRepository
    {
        CommandResult<IReadOnlyCollection<Preposto>> Obter(PrepostoSort sort, bool direcao, string pesquisa);

        CommandResult<PaginatedQueryResult<Preposto>> ObterPaginado(int pagina, int quantidade, PrepostoSort sort, bool ascending, string pesquisa);

        CommandResult<PaginatedQueryResult<AlocacoesFuturas>> ObterAlocacoesFuturas(int pagina, int quantidade, AlocacaoPrepostoSort sort, bool ascending, int prepostoId, string tiposProcessos);

        CommandResult<IReadOnlyCollection<AlocacoesFuturas>> ObterAlocacao(AlocacaoPrepostoSort sort, bool direcao, int prepostoId, string tiposProcessos);

        CommandResult<bool> EstaAlocado(string tiposProcessos, int prepostoId);

        CommandResult<Preposto> ValidarDuplicidadeDeNomePreposto(string nomePreposto, int prepostoId);

    }
}
