using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface IProcessoAppService : IBaseCrudAppService<ProcessoViewModel, Processo, long>
    {
        Task<IResultadoApplication<string>> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, ILogger logger);
        
        Task<IResultadoApplication> ExpurgoPrePosRj(ILogger logger);

        Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, string rota);

        Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> RecuperarProcessoPeloCodigoInterno(long codigoInterno, long codigoTipoProcesso, string rota);
    }
}