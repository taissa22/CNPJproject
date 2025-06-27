using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ILog_LoteAppService : IBaseCrudAppService<Log_LoteHistoricoViewModel, Log_Lote, long>
    {
        //Log_Lote RecuperarLog_Lote(long Cod);
        //Task<IResultadoApplication<Log_LoteViewModel>> RecuperarLog_Lote(long id);

        Task<IResultadoApplication<ICollection<Log_LoteHistoricoViewModel>>> ObterHistorico(long codigoLote);
    }
}
