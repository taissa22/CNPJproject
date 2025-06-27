using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{

    public interface IEmpresasSapFornecedorasService : IBaseCrudService<EmpresasSapFornecedoras, long>
    {
        Task<bool> ExisteEmpresaSapFornecedorasComEmpresaSap(long codigoEmpresasSap);
    }
}
