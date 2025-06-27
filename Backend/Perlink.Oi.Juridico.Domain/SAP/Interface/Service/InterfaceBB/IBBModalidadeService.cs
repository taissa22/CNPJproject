using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBModalidadeService : IBaseCrudService<BBModalidade, long>
    {
        Task<bool> CodigoBBJaExiste(BBModalidade obj);
        Task<IEnumerable<BBModalidade>> recuperarModalidadeBB(BBModalidadeFiltroDTO filtro);
        Task<ICollection<BBModalidade>> exportarModalidadeBB(BBModalidadeFiltroDTO filtro);
        Task<int> ObterQuantidadeTotalPorFiltro(BBModalidadeFiltroDTO filtroDTO);
    }
}

