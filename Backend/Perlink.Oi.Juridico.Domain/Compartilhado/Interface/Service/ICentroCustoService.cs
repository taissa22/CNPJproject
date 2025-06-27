using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ICentroCustoService : IBaseCrudService<CentroCusto, long>{

        Task<IEnumerable<CentroCusto>> RecuperarCentroCustoParaFiltroLote();
        Task<IEnumerable<CentroCustoResultadoDTO>> ConsultarCentrosCustos(CentroCustoFiltroDTO filtros);
        // Task ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO loteFiltroDTO);
        Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO centroCustoFiltroDTO);
        Task CadastrarCentroCusto(CentroCusto entidade);
        Task<ICollection<CentroCustoResultadoDTO>> ExportarCentrosCustos(CentroCustoFiltroDTO centroCustoFiltroDTO);
        Task<bool> VerificarDuplicidadeDescricaoCentroCusto(CentroCusto centroCusto);
        Task<bool> VerificarDuplicidadeCentroCustoSAP(CentroCusto centroCusto);
    }
}
