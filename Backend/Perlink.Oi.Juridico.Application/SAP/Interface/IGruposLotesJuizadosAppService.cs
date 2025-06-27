using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface IGruposLotesJuizadosAppService : IBaseCrudAppService<GruposLotesJuizadosViewModel, GruposLotesJuizados, long>
    {
        Task<IPagingResultadoApplication<ICollection<GruposLotesJuizadosViewModel>>> ConsultarGruposLotesJuizadosPorFiltroPaginado(FiltrosDTO filtros);        
        Task<IResultadoApplication> ExcluirGruposLotesJuizados(long codigo);
        Task<IResultadoApplication<byte[]>> ExportarGruposLotesJuizado(FiltrosDTO filtros);
        Task<long> ObterUltimoId();
    }
}
