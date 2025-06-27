using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
   public interface IRegionaisRepository
    {
        CommandResult<IReadOnlyCollection<Regional>> Obter();
    }
}
