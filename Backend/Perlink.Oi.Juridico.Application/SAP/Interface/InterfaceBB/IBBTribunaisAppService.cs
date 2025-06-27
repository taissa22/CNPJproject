using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBTribunaisAppService : IBaseCrudAppService<BBTribunaisViewModel, BBTribunais, long>
    {
        //BBTribunais RecuperarBBTribunais(long Cod);
        Task<IPagingResultadoApplication<ICollection<BBTribunaisViewModel>>> ConsultarBBTribunais(FiltrosDTO filtros);
        Task<IResultadoApplication<byte[]>> ExportarBBTribunais(FiltrosDTO filtros);
        Task<IResultadoApplication> SalvarBBTribunais(BBTributarioInclusaoEdicaoDTO tributarioInclusaoEdicaoDTO);
        Task<IResultadoApplication> ExcluirBBTribunais(long id);
    }
}
