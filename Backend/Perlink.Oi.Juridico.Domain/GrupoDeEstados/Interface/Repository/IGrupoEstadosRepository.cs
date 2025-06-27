using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository
{
    public interface IGrupoEstadosRepository : IBaseCrudRepository<GrupoEstados, long>
    {
        void ExcluirGrupoEstado(long id);
        void ExcluirGrupoEstado(IEnumerable<GrupoEstados> listaEntity);

    }
}
