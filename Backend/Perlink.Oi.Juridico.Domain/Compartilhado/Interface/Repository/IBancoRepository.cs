using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IBancoRepository : IBaseCrudRepository<Banco, long>
    {
        Task<IEnumerable<Banco>> RecuperarNomeBanco();
        Task<IEnumerable<BancoListaDTO>> RecuperarListaBancos();
    }
}
