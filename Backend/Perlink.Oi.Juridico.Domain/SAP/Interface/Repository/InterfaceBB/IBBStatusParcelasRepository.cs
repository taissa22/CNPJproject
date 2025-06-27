using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBStatusParcelasRepository : IBaseCrudRepository<BBStatusParcelas, long>
    {
        Task<bool> VerificarDuplicidadeCodigoBBStatusParcela(BBStatusParcelas obj);

        Task<BBStatusParcelas> RecuperarPorCodigoBBStatusParcela(long codigo);
    }
}
