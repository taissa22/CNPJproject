using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBModalidadeRepository : IBaseCrudRepository<BBModalidade, long>
    {
        Task<bool> CodigoBBJaExiste(BBModalidade obj);
        Task<IEnumerable<BBModalidade>> recuperarModalidadeBB(BBModalidadeFiltroDTO filtro);
        Task<ICollection<BBModalidade>> exportarModalidadeBB(BBModalidadeFiltroDTO filtro);
        Task<int> ObterQuantidadeTotalPorFiltro(BBModalidadeFiltroDTO filtroDTO);
    }
}
