using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IFeriadosRepository : IBaseCrudRepository<Feriados, long> {
        Task<bool> Existe(DateTime result);
    }
}
