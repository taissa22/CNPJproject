using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBNaturezasAcoesRepository : IBaseCrudRepository<BBNaturezasAcoes, long>
    {
        Task<bool> CodigoBBJaExiste(BBNaturezasAcoes obj);
        Task<BBNaturezasAcoes> RecuperarPorCodigoBB(long v);
    }
}
