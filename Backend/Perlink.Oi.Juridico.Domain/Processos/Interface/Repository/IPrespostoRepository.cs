

using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos
{
    public interface IPrepostoRepository : IBaseCrudRepository<Preposto, long>
    {
        Task<IEnumerable<PrepostoDTO>> RecuperarPrepostoTrabalhista();
        Task<IEnumerable<PrepostoDTO>> ListarPreposto(long? tipoProcesso);
    }
}
