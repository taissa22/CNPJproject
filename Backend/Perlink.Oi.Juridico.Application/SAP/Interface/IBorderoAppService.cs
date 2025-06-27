using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface {
    public interface IBorderoAppService : IBaseCrudAppService<BorderoViewModel, Bordero, long>
    {
        //Bordero RecuperarBordero(long Cod);
        //Task<IResultadoApplication<BorderoViewModel>> RecuperarBordero(long id);
        Task<IResultadoApplication<ICollection<BorderoViewModel>>> GetBordero(long codigoLote);
        Task<IResultadoApplication<byte[]>> ExportarBorderoDoLote(long codigoLote, long codigoTipoProcesso);
    }
}
