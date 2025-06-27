using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository
{
    public interface IGrupoEmpresaContabilSapParteRepository : IBaseCrudRepository<GrupoEmpresaContabilSapParte, long>
    {
        void ExcluirGrupoXEmpresa(long id);

        void ExcluirGrupoXEmpresa(IEnumerable<GrupoEmpresaContabilSapParte> id);
    }
}