using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBComarcaAppService : IBaseCrudAppService<BBComarcaViewModel, BBComarca, long>
    {
        Task<IPagingResultadoApplication<ICollection<BBComarcaViewModel>>> ConsultarBBComarca(FiltrosDTO filtroDTO);
        Task<IResultadoApplication<byte[]>> ExportarBBComarca(FiltrosDTO filtroDTO);
        Task<IResultadoApplication> CadastrarBBComarca(BBComarcaViewModel objeto);
        Task<IResultadoApplication> AlterarBBComarca(BBComarcaViewModel objeto);
        Task<IResultadoApplication> ExcluirBBComarca(long codigo);
        Task<IResultadoApplication<ICollection<ComboboxViewModel<int>>>> ConsultarComarcaPorEstado(string codigoEstado);
    }
}
