using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IGruposLotesJuizadosService : IBaseCrudService<GruposLotesJuizados,long>
    {
        Task<IEnumerable<GruposLotesJuizados>> RecuperarGrupoLoteJuizadoPorFiltro(FiltrosDTO filtros);
        Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros);
        Task<long> ObterUltimoId();
    }
}
