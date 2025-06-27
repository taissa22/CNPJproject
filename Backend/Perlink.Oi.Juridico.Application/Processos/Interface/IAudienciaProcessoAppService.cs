using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Processos.Interface
{
    public interface IAudienciaProcessoAppService : IBaseCrudAppService<AudienciaProcessoUpdateVM, AudienciaProcesso, long>
    {
        Task<IResultadoApplication<AgendaAudienciaFiltrosViewModel>> CarregarFiltros();

        Task<IResultadoApplication<AgendaAudienciaPartesViewModel>> RecuperarPartes(long codigoProcesso);

        Task<IResultadoApplication<AgendaAudienciaPedidosViewModel>> RecuperarPedidos(long codigoProcesso);

        Task<IResultadoApplication<AgendaAudienciaComboEdicaoViewModel>> RecuperarCombosEdicao();

        Task<IResultadoApplication<AgendaAudienciaAdvogadoViewModel>> RecuperarAdvogadoEscritorio(long codigoEscritorio);

        AudienciaProcessoBuscaResultadoVM ObterAudienciasPorFiltro(IEnumerable<FilterVM> lstFilters, int pageNumber, int pageSize, IEnumerable<SortOrderVM> orders, bool IsExportMethod);

        IResultadoApplication<byte[]> Exportar(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders, bool IsExportMethod);

        IResultadoApplication<byte[]> ExportarCompleto(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders, bool IsExportMethod);

        Task<IResultadoApplication<byte[]>> Download(IEnumerable<FilterVM> lstFilters, IEnumerable<SortOrderVM> orders);
    }
}
