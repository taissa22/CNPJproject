using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Service;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IFornecedorasContratosService : IBaseCrudService<FornecedorasContratos, long>
    {
        Task<bool> ExisteFornecedorasContratosComEmpresaSap(long codigoFornecedor);
    }
}
