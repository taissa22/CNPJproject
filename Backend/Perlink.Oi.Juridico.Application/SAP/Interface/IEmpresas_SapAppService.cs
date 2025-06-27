using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface IEmpresas_SapAppService : IBaseCrudAppService<Empresas_SapViewModel, Empresas_Sap, long>
    {
        Task<IResultadoApplication<byte[]>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO);
        Task<IPagingResultadoApplication<ICollection<Empresas_SapResultadoViewModel>>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO FiltroDTO);
        Task<IResultadoApplication> CadastrarEmpresa(Empresas_SapViewModel viewModel);

        Task<IResultadoApplication> AtualizarEmpresa(Empresas_SapViewModel viewModel);

        Task<IResultadoApplication> ExcluirEmpresasSap(long codigoEmpresasSap);
        Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO);
    }
}
