using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IMunicipioRepository
    {
        CommandResult<PaginatedQueryResult<Municipio>> ObterPaginado(int pagina, int quantidade, string estadoId, int? municipioId, MunicipioSort estado, bool direcao);
        CommandResult<IReadOnlyCollection<Municipio>> ObterTodos(string estadoId);
    }
}
