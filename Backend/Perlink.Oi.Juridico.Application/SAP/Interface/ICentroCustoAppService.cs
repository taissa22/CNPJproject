using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ICentroCustoAppService : IBaseCrudAppService<CentroCustoViewModel, CentroCusto, long>
    {
        //CentroCusto RecuperarCentroCusto(long Cod);

        Task<IPagingResultadoApplication<ICollection<CentroCustoViewModel>>> ConsultarCentrosCustos(CentroCustoFiltroDTO CentroCustoFiltroDTO);
        Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO loteFiltroDTO);
        Task<IResultadoApplication> CriarAlterarCentroCusto(CentroCustoViewModel centroCustoViewModel);
        Task<IResultadoApplication<byte[]>> ExportarCentroCusto(CentroCustoFiltroDTO CentroCustoFiltroDTO);
        Task<IResultadoApplication> ExcluirCentroCusto(long codigo);
    }
}
