using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IGruposLotesJuizadosRepository : IBaseCrudRepository<GruposLotesJuizados, long>
    {
        Task<IEnumerable<GruposLotesJuizados>> RecuperarGrupoLoteJuizadoPorFiltro(FiltrosDTO filtros);
        Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros);
        Task<long> ObterUltimoId();
    }
}
