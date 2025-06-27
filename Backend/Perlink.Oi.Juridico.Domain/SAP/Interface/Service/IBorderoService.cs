using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IBorderoService : IBaseCrudService<Bordero, long>
    {
        Task<IEnumerable<Bordero>> GetBordero(long codigoLote);
        Task CriacaoBordero(IList<BorderoDTO> borderos, Lote lote);
    }
}

