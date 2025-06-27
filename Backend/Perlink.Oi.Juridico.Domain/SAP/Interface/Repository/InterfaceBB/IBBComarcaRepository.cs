using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBComarcaRepository : IBaseCrudRepository<BBComarca, long>
    {
        Task<bool> UFIsValid(string uf);
        Task<bool> CodigoBBJaExiste(BBComarca bbComarca);
        Task<BBComarca> RecuperarPorCodigoBB(long codigo);
        Task<ICollection<BBComarca>> ConsultarBBComarca(FiltrosDTO filtroDTO);
        Task<ICollection<BBComarca>> ExportarBBComarca(FiltrosDTO filtroDTO);
        Task<int> TotalBBComarca(FiltrosDTO filtroDTO);
    }
}
