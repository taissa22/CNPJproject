using Perlink.Oi.Juridico.Infra.Entities;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ICentroDeCustoRepository
    {
        CommandResult<IReadOnlyCollection<CentroCusto>> Obter();
    }
}