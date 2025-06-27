using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBComarcaService : IBaseCrudService<BBComarca, long>
    {
        Task<bool> UFIsValid(string uf);
        Task<bool> CodigoBBJaExiste(BBComarca bbComarca);
        Task<int> TotalBBComarca(FiltrosDTO filtroDTO);
        Task<ICollection<BBComarca>> RecuperarTodosEmOrdemAlfabetica();
        Task<ICollection<BBComarca>> ConsultarBBComarca(FiltrosDTO filtroDTO);
        Task<ICollection<BBComarca>> ExportarBBComarca(FiltrosDTO filtroDTO);
    }
}

