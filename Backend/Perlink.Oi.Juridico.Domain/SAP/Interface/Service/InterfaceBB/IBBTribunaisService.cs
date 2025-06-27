using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBTribunaisService : IBaseCrudService<BBTribunais, long> {
        Task<ICollection<BBTribunais>> RecuperarTodosEmOrdemAlfabetica();
    
        Task<bool> VerificarDuplicidadeTribunalnalBB(BBTribunais entidade);

        Task<ICollection<BBTribunais>> ObterPorFiltroPaginado(FiltrosDTO filtros);

        Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros);
    }
}