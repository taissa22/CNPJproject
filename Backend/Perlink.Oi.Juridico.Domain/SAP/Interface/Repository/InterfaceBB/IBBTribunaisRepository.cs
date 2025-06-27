using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBTribunaisRepository :  IBaseCrudRepository<BBTribunais, long>
    {
        Task<bool> VerificarDuplicidadeTribunalnalBB(BBTribunais entidade);
        Task<ICollection<BBTribunais>> ObterPorFiltroPaginado(FiltrosDTO filtros);
        Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros);
    }
}
