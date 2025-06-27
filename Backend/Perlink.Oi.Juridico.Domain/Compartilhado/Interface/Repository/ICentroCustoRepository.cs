using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICentroCustoRepository : IBaseCrudRepository<CentroCusto,long> {

        Task<IEnumerable<CentroCusto>> RecuperarCentroCustoParaFiltroLote();
        Task<IEnumerable<CentroCustoResultadoDTO>> ConsultarCentrosCustos(CentroCustoFiltroDTO filtros);
        Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO filtroDTO);
        Task CadastrarCentroCusto(CentroCusto entidade);
        Task<ICollection<CentroCustoResultadoDTO>> ExportarCentrosCustos(CentroCustoFiltroDTO filtros);
        Task<bool> VerificarDuplicidadeDescricaoCentroCusto(CentroCusto centroCusto);
        Task<bool> VerificarDuplicidadeCentroCustoSAP(CentroCusto centroCusto);
    }
}
