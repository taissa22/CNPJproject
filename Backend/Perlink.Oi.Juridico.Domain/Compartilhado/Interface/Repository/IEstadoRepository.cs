using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IEstadoRepository : IBaseCrudRepository<Estado, string> {
        Task<IEnumerable<EstadoDTO>> RecuperarListaEstados();

        Task<IList<Estado>> ListarEstadosSemGrupo();
    }
}
