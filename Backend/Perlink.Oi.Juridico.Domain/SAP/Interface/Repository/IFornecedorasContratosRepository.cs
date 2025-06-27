using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IFornecedorasContratosRepository : IBaseCrudRepository<FornecedorasContratos, long>
    {
        Task<bool> ExisteFornecedorasContratosComEmpresaSap(long codigoEmpresaSap);
    }
}
