using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface {
    public interface IComarcaAppService : IBaseCrudAppService<ComarcaViewModel, Comarca, long>
    {
        //Comarca RecuperarComarca(long Cod);
        Task<IResultadoApplication<ComarcaViewModel>> RecuperarComarca(long id);
        Task<IResultadoApplication<ICollection<ComarcaComboViewModel>>> RecuperarComarcaPorEstado(string estado);

    }
}
