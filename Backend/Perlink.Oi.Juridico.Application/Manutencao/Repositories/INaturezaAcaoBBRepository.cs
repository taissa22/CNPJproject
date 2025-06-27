using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
   public interface INaturezaAcaoBBRepository
    {
        CommandResult<IReadOnlyCollection<NaturezaAcaoBB>> ObterNaturezasAcoesBB(int? naturezaId);
    }
}
