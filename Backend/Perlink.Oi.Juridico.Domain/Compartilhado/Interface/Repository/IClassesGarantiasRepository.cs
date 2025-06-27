using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IClassesGarantiasRepository : IBaseCrudRepository<ClassesGarantias, long>
    {
        Task<IEnumerable<ComboboxDTO>> RecuperarClassesGarantias(long tipoLancamento);
    }
}
